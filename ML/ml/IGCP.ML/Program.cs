// Program.cs - Fuente: STG.vw_ofertas_enriched + vistas ML.vw_ofe_predict_* por modo
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace AcademicOfferPredictor
{
    public class Program
    {
        private const string CONN =
            "Server=iAcademicGenerator.mssql.somee.com;Database=iAcademicGenerator;User Id=oscarvaldivieso_SQLLogin_1;Password=admin123;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

        // Vista base para TRAIN (histórico completo)
        public const string TRAIN_VIEW = "STG.vw_ofertas_enriched";

        // Vistas para PRED (según modo)
        private const string VIEW_PREDICT_BASE = "ML.vw_ofe_predict_base";
        private const string VIEW_PREDICT_WITH_REQUESTS = "ML.vw_ofe_predict_with_solicitudes";

        // Decisión base
        private const float OPEN_THRESHOLD = 12f;
        private const float SIGMA = 5f;

        // ML
        private const int NUMBER_OF_LEAVES = 64;
        private const int MIN_EXAMPLE_COUNT = 10;
        private const int NUMBER_OF_BITS = 15;

        // Guardado
        private const int BULK_BATCH_SIZE = 1000;
        private const string RESULT_TABLE = "ML.pred_ofertas_resultados";
        private const bool SAVE_MODEL = true;

        public static async Task<int> Main(string[] args)
        {
            try
            {
                // Defaults
                string targetPer = "20254";
                string mode = "base";

                // args[0] como periodo si no empieza con --
                if (args.Length > 0 && !args[0].StartsWith("--", StringComparison.OrdinalIgnoreCase))
                {
                    targetPer = args[0];
                }

                // Buscar --mode <valor>
                for (int i = 0; i < args.Length; i++)
                {
                    if (string.Equals(args[i], "--mode", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                    {
                        mode = args[i + 1];
                    }
                }

                // Elegir vista de predicción según modo
                string predictView = mode.Equals("solicitudes", StringComparison.OrdinalIgnoreCase)
                    ? VIEW_PREDICT_WITH_REQUESTS
                    : VIEW_PREDICT_BASE;

                Console.WriteLine($"[INIT] Periodo={targetPer}  Modo={mode}  View={predictView}");

                var predictor = new OfferPredictor(
                    CONN, predictView, OPEN_THRESHOLD, SIGMA,
                    NUMBER_OF_LEAVES, MIN_EXAMPLE_COUNT, NUMBER_OF_BITS,
                    BULK_BATCH_SIZE, RESULT_TABLE, SAVE_MODEL);

                await predictor.RunAutoAsync(targetPer);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FATAL ERROR] {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                if (ex.InnerException != null) Console.WriteLine("Inner: " + ex.InnerException.Message);
                return 1;
            }
        }

        public class OfferPredictor
        {
            // Evitar ejecuciones simultáneas (API o consola)
            private static readonly SemaphoreSlim _runLock = new(1, 1);

            private readonly string _conn;
            private readonly string _sourceView; // aquí guardamos la vista de PREDICCIÓN
            private readonly float _openThreshold;
            private readonly float _sigma;
            private readonly int _numberOfLeaves;
            private readonly int _minExampleCount;
            private readonly int _numberOfBits;
            private readonly int _bulkBatchSize;
            private readonly string _resultTable;
            private readonly bool _saveModel;

            private readonly string _outputDir;
            private readonly MLContext _ml;

            public OfferPredictor(
                string connectionString,
                string sourceView,
                float openThreshold,
                float sigma,
                int numberOfLeaves,
                int minExampleCount,
                int numberOfBits,
                int bulkBatchSize,
                string resultTable,
                bool saveModel)
            {
                _conn = connectionString;
                _sourceView = sourceView; // ahora es la vista de PRED
                _openThreshold = openThreshold;
                _sigma = sigma;
                _numberOfLeaves = numberOfLeaves;
                _minExampleCount = minExampleCount;
                _numberOfBits = numberOfBits;
                _bulkBatchSize = bulkBatchSize;
                _resultTable = resultTable;
                _saveModel = saveModel;

                _outputDir = Path.Combine(AppContext.BaseDirectory, "out");
                Directory.CreateDirectory(_outputDir);
                _ml = new MLContext(seed: 42);
            }

            // AHORA RECIBE EL PERIODO
            public async Task RunAutoAsync(string targetPer)
            {
                // Evita que se ejecute en paralelo
                if (!await _runLock.WaitAsync(0))
                {
                    throw new InvalidOperationException("Ya hay una ejecución en progreso. Intenta más tarde.");
                }

                try
                {
                    var runTag = $"{targetPer}-Q4-{DateTime.Now:yyyyMMdd-HHmmss}";

                    Console.WriteLine($"Fuente TRAIN: {Program.TRAIN_VIEW}  Fuente PRED: {_sourceView}  Periodo objetivo: {targetPer}");

                    var trainRows = await LoadTrainingDataAsync();
                    var predRows = await LoadPredictionDataAsync(targetPer);
                    ValidateData(trainRows, predRows);

                    var trainFeatures = trainRows.Select(MapToTrainFeatures).ToList();
                    var predFeatures = predRows.Select(MapToPredictFeatures).ToList();

                    var trainData = _ml.Data.LoadFromEnumerable(trainFeatures);
                    var predData = _ml.Data.LoadFromEnumerable(predFeatures);

                    var model = TrainRegressionModel(trainData);
                    EvaluateModel(model, trainData);

                    var predictions = PredictEnrollments(model, predData);

                    var outFile = Path.Combine(_outputDir, $"pred_ofertas_{targetPer}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
                    await ExportResultsAsync(predFeatures, predictions, outFile);
                    await SaveResultsToDatabaseAsync(runTag, predFeatures, predictions);

                    if (_saveModel) SaveModel(model, trainData);

                    Console.WriteLine($"OK → {predictions.Count} filas. CSV: {outFile}  RunTag: {runTag}");
                }
                finally
                {
                    _runLock.Release();
                }
            }

            // ===================== CARGA TRAIN (ENRICHED + SOLICITUDES) =====================
            private async Task<List<TrainingRow>> LoadTrainingDataAsync()
            {
                string sql = $@"
SELECT
    o.per_codigo,
    o.mod_codigo,
    o.cam_codigo,
    o.sec_codigo,
    o.doc_codigo,
    o.nombre_materia,

    o.ofe_modulo,
    o.ofe_semestre,
    o.ofe_anio,
    o.ofe_nivel,

    0 AS ofe_presencialidad_obligatoria,          -- no viene, valor seguro
    o.ofe_duracion_clase,
    0 AS ofe_es_core,                              -- no viene, valor seguro

    CAST(o.dias_pattern AS varchar(10)) AS dias_pattern,
    COALESCE(NULLIF(LTRIM(RTRIM(o.bloque_horario)), ''), 'NA') AS bloque_horario,

    COALESCE(o.is_virtual, CASE WHEN o.dias_pattern = 0 THEN 1 ELSE 0 END) AS is_virtual,

    (o.dia_L + o.dia_MA + o.dia_MI + o.dia_J + o.dia_V + o.dia_S + o.dia_D) AS dias_count,
    o.dia_L, o.dia_MA, o.dia_MI, o.dia_J, o.dia_V, o.dia_S, o.dia_D,
    CASE WHEN o.dia_S = 1 OR o.dia_D = 1 THEN 1 ELSE 0 END AS has_weekend,

    ISNULL(s.NumSolicitudes, 0) AS pre_solicitudes,

    CASE WHEN COALESCE(o.is_virtual, CASE WHEN o.dias_pattern=0 THEN 1 ELSE 0 END) = 1
         THEN 'VIRTUAL' ELSE 'PRESENCIAL' END      AS ofe_modalidad_programa,

    o.ofe_matriculados,

    CAST(CONVERT(varchar(8), o.hora_inicio, 108) AS varchar(20)) AS hora_inicio
FROM {Program.TRAIN_VIEW} AS o
LEFT JOIN ML.vw_solicitudes_agg AS s
    ON  s.per_codigo = o.per_codigo
    AND s.cam_codigo = o.cam_codigo
    AND s.mod_codigo = o.mod_codigo
    AND s.sec_codigo = o.sec_codigo;";

                var list = new List<TrainingRow>();
                await using var cn = new SqlConnection(_conn);
                await cn.OpenAsync();
                await using var cmd = new SqlCommand(sql, cn) { CommandTimeout = 120 };
                await using var rd = await cmd.ExecuteReaderAsync();
                while (await rd.ReadAsync())
                {
                    list.Add(new TrainingRow
                    {
                        PerCodigo = GetString(rd, "per_codigo"),
                        ModCodigo = GetString(rd, "mod_codigo"),
                        CamCodigo = GetString(rd, "cam_codigo"),
                        SecCodigo = GetString(rd, "sec_codigo"),
                        DocCodigo = GetString(rd, "doc_codigo"),
                        NombreMateria = GetString(rd, "nombre_materia"),

                        OfeModulo = GetInt(rd, "ofe_modulo"),
                        OfeSemestre = GetInt(rd, "ofe_semestre"),
                        OfeAnio = GetInt(rd, "ofe_anio"),
                        OfeNivel = GetInt(rd, "ofe_nivel"),

                        OfePresencialidadObligatoria = GetInt(rd, "ofe_presencialidad_obligatoria"),
                        OfeDuracionClase = GetInt(rd, "ofe_duracion_clase"),
                        OfeEsCore = GetInt(rd, "ofe_es_core"),

                        DiasPattern = GetString(rd, "dias_pattern", "NA"),
                        BloqueHorario = GetString(rd, "bloque_horario", "NA"),
                        IsVirtual = GetInt(rd, "is_virtual"),

                        DiasCount = GetInt(rd, "dias_count"),
                        Dia_L = GetInt(rd, "dia_L"),
                        Dia_MA = GetInt(rd, "dia_MA"),
                        Dia_MI = GetInt(rd, "dia_MI"),
                        Dia_J = GetInt(rd, "dia_J"),
                        Dia_V = GetInt(rd, "dia_V"),
                        Dia_S = GetInt(rd, "dia_S"),
                        Dia_D = GetInt(rd, "dia_D"),
                        HasWeekend = GetInt(rd, "has_weekend"),

                        PreSolicitudes = GetInt(rd, "pre_solicitudes"),
                        OfeModalidadPrograma = GetString(rd, "ofe_modalidad_programa", "PRESENCIAL"),

                        OfeMatriculados = GetInt(rd, "ofe_matriculados"),
                        HoraInicio = GetString(rd, "hora_inicio", "")
                    });
                }
                Console.WriteLine($"   → Train: {list.Count}");
                return list;
            }

            // ===================== CARGA PRED (vista ML.vw_ofe_predict_* + OUTER APPLY) =====================
            private async Task<List<PredictionRow>> LoadPredictionDataAsync(string periodoObjetivo)
            {
                string sql = $@"
;WITH ultimos AS (
  SELECT TOP (6) per_codigo
  FROM {_sourceView}
  GROUP BY per_codigo
  ORDER BY per_codigo DESC
),
base AS (
  SELECT
    CAST(per_codigo AS varchar(20)) AS per_codigo,
    mod_codigo, cam_codigo, sec_codigo, doc_codigo,
    nombre_materia,
    ofe_modulo, ofe_semestre, ofe_anio, ofe_nivel,
    0 AS ofe_presencialidad_obligatoria,
    ofe_duracion_clase,
    0 AS ofe_es_core,

    CAST(dias_pattern AS varchar(10)) AS dias_pattern,
    NULLIF(LTRIM(RTRIM(bloque_horario)), '') AS bloque_horario,

    COALESCE(is_virtual, CASE WHEN dias_pattern = 0 THEN 1 ELSE 0 END) AS is_virtual,

    (dia_L + dia_MA + dia_MI + dia_J + dia_V + dia_S + dia_D) AS dias_count,
    dia_L, dia_MA, dia_MI, dia_J, dia_V, dia_S, dia_D,
    CASE WHEN dia_S = 1 OR dia_D = 1 THEN 1 ELSE 0 END AS has_weekend,

    pre_solicitudes,
    CASE WHEN COALESCE(is_virtual, CASE WHEN dias_pattern=0 THEN 1 ELSE 0 END) = 1
         THEN 'VIRTUAL' ELSE 'PRESENCIAL' END AS ofe_modalidad_programa,

    CAST(CONVERT(varchar(8), hora_inicio, 108) AS varchar(20)) AS hora_inicio
  FROM {_sourceView}
  WHERE per_codigo IN (SELECT per_codigo FROM ultimos)
),
latest AS (
  SELECT b.*,
         ROW_NUMBER() OVER (
           PARTITION BY b.mod_codigo, b.cam_codigo,
                        b.dias_pattern, b.bloque_horario,
                        b.ofe_duracion_clase, ISNULL(b.hora_inicio,'')
           ORDER BY TRY_CONVERT(int, b.per_codigo) DESC
         ) AS rn
  FROM base b
)
SELECT
  l.mod_codigo, l.cam_codigo, '' AS sec_codigo, l.doc_codigo, l.nombre_materia,
  l.ofe_modulo, l.ofe_semestre, l.ofe_anio, l.ofe_nivel,
  l.ofe_presencialidad_obligatoria, l.ofe_duracion_clase, l.ofe_es_core,

  COALESCE(l.hora_inicio,   mh.hora_inicio)          AS hora_inicio,
  COALESCE(l.dias_pattern,  md.dias_pattern, 'NA')   As dias_pattern,
  COALESCE(l.bloque_horario,mb.bloque_horario, 'NA') AS bloque_horario,

  l.is_virtual,
  l.dias_count, l.dia_L, l.dia_MA, l.dia_MI, l.dia_J, l.dia_V, l.dia_S, l.dia_D, l.has_weekend,
  l.pre_solicitudes, l.ofe_modalidad_programa
FROM latest l
OUTER APPLY (
  SELECT TOP (1) d.dias_pattern
  FROM (
    SELECT dias_pattern, COUNT(*) AS cnt, MAX(TRY_CONVERT(int, per_codigo)) AS last_per
    FROM base b2
    WHERE b2.mod_codigo = l.mod_codigo AND b2.cam_codigo = l.cam_codigo
          AND b2.dias_pattern IS NOT NULL
    GROUP BY dias_pattern
  ) d
  ORDER BY d.cnt DESC, d.last_per DESC
) md
OUTER APPLY (
  SELECT TOP (1) bq.bloque_horario
  FROM (
    SELECT bloque_horario, COUNT(*) As cnt, MAX(TRY_CONVERT(int, per_codigo)) AS last_per
    FROM base b2
    WHERE b2.mod_codigo = l.mod_codigo AND b2.cam_codigo = l.cam_codigo
          AND b2.bloque_horario IS NOT NULL
    GROUP BY bloque_horario
  ) bq
  ORDER BY bq.cnt DESC, bq.last_per DESC
) mb
OUTER APPLY (
  SELECT TOP (1) h.hora_inicio
  FROM (
    SELECT hora_inicio, COUNT(*) AS cnt, MAX(TRY_CONVERT(int, per_codigo)) AS last_per
    FROM base b2
    WHERE b2.mod_codigo = l.mod_codigo AND b2.cam_codigo = l.cam_codigo
          AND b2.hora_inicio IS NOT NULL
    GROUP BY hora_inicio
  ) h
  ORDER BY h.cnt DESC, h.last_per DESC
) mh
WHERE l.rn = 1
ORDER BY l.mod_codigo, l.cam_codigo,
         COALESCE(l.bloque_horario, mb.bloque_horario),
         COALESCE(l.hora_inicio,   mh.hora_inicio);";

                var list = new List<PredictionRow>();
                await using var cn = new SqlConnection(_conn);
                await cn.OpenAsync();
                await using var cmd = new SqlCommand(sql, cn) { CommandTimeout = 120 };
                await using var rd = await cmd.ExecuteReaderAsync();
                while (await rd.ReadAsync())
                {
                    list.Add(new PredictionRow
                    {
                        PerCodigo = periodoObjetivo,
                        ModCodigo = GetString(rd, "mod_codigo"),
                        CamCodigo = GetString(rd, "cam_codigo"),
                        SecCodigo = GetString(rd, "sec_codigo"),
                        DocCodigo = GetString(rd, "doc_codigo"),
                        NombreMateria = GetString(rd, "nombre_materia"),

                        OfeModulo = GetInt(rd, "ofe_modulo"),
                        OfeSemestre = GetInt(rd, "ofe_semestre"),
                        OfeAnio = GetInt(rd, "ofe_anio"),
                        OfeNivel = GetInt(rd, "ofe_nivel"),

                        OfePresencialidadObligatoria = GetInt(rd, "ofe_presencialidad_obligatoria"),
                        OfeDuracionClase = GetInt(rd, "ofe_duracion_clase"),
                        OfeEsCore = GetInt(rd, "ofe_es_core"),

                        HoraInicio = GetString(rd, "hora_inicio", ""),
                        DiasPattern = GetString(rd, "dias_pattern", "NA"),
                        BloqueHorario = GetString(rd, "bloque_horario", "NA"),
                        IsVirtual = GetInt(rd, "is_virtual"),

                        DiasCount = GetInt(rd, "dias_count"),
                        Dia_L = GetInt(rd, "dia_L"),
                        Dia_MA = GetInt(rd, "dia_MA"),
                        Dia_MI = GetInt(rd, "dia_MI"),
                        Dia_J = GetInt(rd, "dia_J"),
                        Dia_V = GetInt(rd, "dia_V"),
                        Dia_S = GetInt(rd, "dia_S"),
                        Dia_D = GetInt(rd, "dia_D"),
                        HasWeekend = GetInt(rd, "has_weekend"),

                        PreSolicitudes = GetInt(rd, "pre_solicitudes"),
                        OfeModalidadPrograma = GetString(rd, "ofe_modalidad_programa", "PRESENCIAL")
                    });
                }
                Console.WriteLine($"   → Predict candidates: {list.Count}");
                return list;
            }

            private void ValidateData(List<TrainingRow> trainRows, List<PredictionRow> predRows)
            {
                if (trainRows.Count == 0) throw new InvalidOperationException("No hay datos de entrenamiento");
                if (predRows.Count == 0) throw new InvalidOperationException("No hay datos para predicción");
            }

            // ===================== PIPELINE (LightGBM) =====================
            private ITransformer TrainRegressionModel(IDataView trainData)
            {
                string[] cat = {
                    nameof(MLFeatures.PerCodigo),
                    nameof(MLFeatures.ModCodigo),
                    nameof(MLFeatures.CamCodigo),
                    nameof(MLFeatures.SecCodigo),
                    nameof(MLFeatures.DocCodigo),
                    nameof(MLFeatures.OfeModalidadPrograma),
                    nameof(MLFeatures.DiasPattern),
                    nameof(MLFeatures.BloqueHorario)
                };

                string[] num = {
                    nameof(MLFeatures.OfeModulo),
                    nameof(MLFeatures.OfeSemestre),
                    nameof(MLFeatures.OfeAnio),
                    nameof(MLFeatures.OfeNivel),
                    nameof(MLFeatures.OfePresencialidadObligatoria),
                    nameof(MLFeatures.OfeDuracionClase),
                    nameof(MLFeatures.OfeEsCore),
                    nameof(MLFeatures.IsVirtual),
                    nameof(MLFeatures.DiasCount),
                    nameof(MLFeatures.Dia_L),
                    nameof(MLFeatures.Dia_MA),
                    nameof(MLFeatures.Dia_MI),
                    nameof(MLFeatures.Dia_J),
                    nameof(MLFeatures.Dia_V),
                    nameof(MLFeatures.Dia_S),
                    nameof(MLFeatures.Dia_D),
                    nameof(MLFeatures.HasWeekend),
                    nameof(MLFeatures.PreSolicitudes)
                };

                var preprocess =
                    _ml.Transforms.Categorical.OneHotHashEncoding(
                            cat.Select(c => new InputOutputColumnPair(c, c)).ToArray(),
                            numberOfBits: _numberOfBits,
                            outputKind: OneHotEncodingEstimator.OutputKind.Indicator)
                        .Append(_ml.Transforms.Concatenate("Features", cat.Concat(num).ToArray()))
                        .Append(_ml.Transforms.NormalizeMinMax("Features"))
                        .Append(_ml.Transforms.CopyColumns("Label", nameof(MLFeatures.OfeMatriculados)));

                var trainer = _ml.Regression.Trainers.LightGbm(
                    labelColumnName: "Label",
                    featureColumnName: "Features",
                    numberOfLeaves: _numberOfLeaves,
                    minimumExampleCountPerLeaf: _minExampleCount,
                    numberOfIterations: 300,
                    learningRate: 0.1);

                var model = (preprocess.Append(trainer)).Fit(trainData);
                Console.WriteLine("   → Modelo LightGBM entrenado");
                return model;
            }

            private void EvaluateModel(ITransformer model, IDataView trainData)
            {
                var pred = model.Transform(trainData);
                var m = _ml.Regression.Evaluate(pred, "Label", "Score");
                Console.WriteLine($"   Métricas (train): R²={m.RSquared:F4}  MAE={m.MeanAbsoluteError:F2}  RMSE={m.RootMeanSquaredError:F2}");
            }

            private List<EnrollmentPrediction> PredictEnrollments(ITransformer model, IDataView predData)
            {
                var predictions = model.Transform(predData);
                return _ml.Data.CreateEnumerable<EnrollmentPrediction>(predictions, reuseRowObject: false).ToList();
            }

            // ===================== EXPORT / BD =====================
            private async Task ExportResultsAsync(List<MLFeatures> features, List<EnrollmentPrediction> predictions, string outputPath)
            {
                await using var sw = new StreamWriter(outputPath);
                await sw.WriteLineAsync("per_codigo,mod_codigo,nombre_materia,cam_codigo,sec_codigo,doc_codigo,dias_pattern,bloque_horario,hora_inicio,matric_pred,abrir_pred,prob");

                int n = Math.Min(features.Count, predictions.Count);
                for (int i = 0; i < n; i++)
                {
                    var f = features[i];
                    var y = Math.Max(0f, predictions[i].Score);
                    int abrir = y >= _openThreshold ? 1 : 0;
                    float prob = 1f / (1f + MathF.Exp(-(y - _openThreshold) / _sigma));

                    await sw.WriteLineAsync(string.Join(",",
                        Escape(f.PerCodigo), Escape(f.ModCodigo), Escape(f.NombreMateria),
                        Escape(f.CamCodigo), Escape(f.SecCodigo), Escape(f.DocCodigo),
                        Escape(f.DiasPattern), Escape(f.BloqueHorario), Escape(f.HoraInicio),
                        y.ToString("0", CultureInfo.InvariantCulture),
                        abrir.ToString(CultureInfo.InvariantCulture),
                        prob.ToString("0.000", CultureInfo.InvariantCulture)
                    ));
                }
                Console.WriteLine($"   → CSV: {n} filas");
            }

            private async Task SaveResultsToDatabaseAsync(string runTag, List<MLFeatures> features, List<EnrollmentPrediction> predictions)
            {
                var dt = new DataTable();
                dt.Columns.Add("per_codigo", typeof(string));
                dt.Columns.Add("mod_codigo", typeof(string));
                dt.Columns.Add("nombre_materia", typeof(string));
                dt.Columns.Add("cam_codigo", typeof(string));
                dt.Columns.Add("sec_codigo", typeof(string));
                dt.Columns.Add("doc_codigo", typeof(string));
                dt.Columns.Add("dias_pattern", typeof(string));
                dt.Columns.Add("bloque_horario", typeof(string));
                dt.Columns.Add("hora_inicio", typeof(string));
                dt.Columns.Add("matric_pred", typeof(double));
                dt.Columns.Add("abrir_pred", typeof(int));
                dt.Columns.Add("prob", typeof(double));
                dt.Columns.Add("run_tag", typeof(string));

                int n = Math.Min(features.Count, predictions.Count);
                for (int i = 0; i < n; i++)
                {
                    var f = features[i];
                    var y = Math.Max(0f, predictions[i].Score);
                    int abrir = y >= _openThreshold ? 1 : 0;
                    float prob = 1f / (1f + MathF.Exp(-(y - _openThreshold) / _sigma));

                    dt.Rows.Add(
                        f.PerCodigo, f.ModCodigo, f.NombreMateria, f.CamCodigo, f.SecCodigo, f.DocCodigo,
                        f.DiasPattern, f.BloqueHorario, f.HoraInicio,
                        (double)y, abrir, (double)prob, runTag);
                }

                await using var cn = new SqlConnection(_conn);
                await cn.OpenAsync();
                var tx = await cn.BeginTransactionAsync();
                try
                {
                    string periodo = features.First().PerCodigo;
                    await using (var del = new SqlCommand("DELETE FROM ML.pred_ofertas_resultados WHERE per_codigo=@p", cn, (SqlTransaction)tx))
                    {
                        del.Parameters.AddWithValue("@p", periodo);
                        await del.ExecuteNonQueryAsync();
                    }

                    using var bulk = new SqlBulkCopy(cn, SqlBulkCopyOptions.Default, (SqlTransaction)tx)
                    {
                        DestinationTableName = _resultTable,
                        BatchSize = _bulkBatchSize,
                        BulkCopyTimeout = 300
                    };
                    MapBulkCopyColumns(bulk);
                    await bulk.WriteToServerAsync(dt);

                    await tx.CommitAsync();
                    Console.WriteLine($"   → Guardados {dt.Rows.Count} en {_resultTable}");
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }

            private void MapBulkCopyColumns(SqlBulkCopy bulk)
            {
                bulk.ColumnMappings.Add("per_codigo", "per_codigo");
                bulk.ColumnMappings.Add("mod_codigo", "mod_codigo");
                bulk.ColumnMappings.Add("nombre_materia", "nombre_materia");
                bulk.ColumnMappings.Add("cam_codigo", "cam_codigo");
                bulk.ColumnMappings.Add("sec_codigo", "sec_codigo");
                bulk.ColumnMappings.Add("doc_codigo", "doc_codigo");
                bulk.ColumnMappings.Add("dias_pattern", "dias_pattern");
                bulk.ColumnMappings.Add("bloque_horario", "bloque_horario");
                bulk.ColumnMappings.Add("hora_inicio", "hora_inicio");
                bulk.ColumnMappings.Add("matric_pred", "matric_pred");
                bulk.ColumnMappings.Add("abrir_pred", "abrir_pred");
                bulk.ColumnMappings.Add("prob", "prob");
                bulk.ColumnMappings.Add("run_tag", "run_tag");
            }

            private void SaveModel(ITransformer model, IDataView trainData)
            {
                var modelPath = Path.Combine(_outputDir, $"modelo_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
                _ml.Model.Save(model, trainData.Schema, modelPath);
                Console.WriteLine($"   → Modelo guardado: {modelPath}");
            }

            // ===================== HELPERS =====================
            private static string GetString(SqlDataReader reader, string columnName, string def = "")
                => reader[columnName] is null or DBNull ? def : reader[columnName].ToString()?.Trim() ?? def;

            private static int GetInt(SqlDataReader reader, string columnName)
            {
                var value = reader[columnName];
                if (value is null or DBNull) return 0;
                return value switch
                {
                    int i => i,
                    long l => (int)l,
                    decimal d => (int)d,
                    double db => (int)db,
                    float f => (int)f,
                    _ => int.TryParse(value.ToString(), out var parsed) ? parsed : 0
                };
            }

            private static string Escape(object? value)
            {
                var text = value?.ToString() ?? "";
                return (text.Contains(',') || text.Contains('"') || text.Contains('\n'))
                    ? $"\"{text.Replace("\"", "\"\"")}\""
                    : text;
            }

            private static MLFeatures MapToTrainFeatures(TrainingRow r) => new()
            {
                PerCodigo = r.PerCodigo,
                ModCodigo = r.ModCodigo,
                CamCodigo = r.CamCodigo,
                SecCodigo = r.SecCodigo,
                DocCodigo = r.DocCodigo,
                NombreMateria = r.NombreMateria,
                OfeModalidadPrograma = r.OfeModalidadPrograma,
                DiasPattern = r.DiasPattern,
                BloqueHorario = r.BloqueHorario,
                HoraInicio = r.HoraInicio,

                OfeModulo = r.OfeModulo,
                OfeSemestre = r.OfeSemestre,
                OfeAnio = r.OfeAnio,
                OfeNivel = r.OfeNivel,
                OfePresencialidadObligatoria = r.OfePresencialidadObligatoria,
                OfeDuracionClase = r.OfeDuracionClase,
                OfeEsCore = r.OfeEsCore,
                IsVirtual = r.IsVirtual,
                DiasCount = r.DiasCount,
                Dia_L = r.Dia_L,
                Dia_MA = r.Dia_MA,
                Dia_MI = r.Dia_MI,
                Dia_J = r.Dia_J,
                Dia_V = r.Dia_V,
                Dia_S = r.Dia_S,
                Dia_D = r.Dia_D,
                HasWeekend = r.HasWeekend,
                PreSolicitudes = r.PreSolicitudes,
                OfeMatriculados = r.OfeMatriculados
            };

            private static MLFeatures MapToPredictFeatures(PredictionRow r) => new()
            {
                PerCodigo = r.PerCodigo,
                ModCodigo = r.ModCodigo,
                CamCodigo = r.CamCodigo,
                SecCodigo = r.SecCodigo,
                DocCodigo = r.DocCodigo,
                NombreMateria = r.NombreMateria,
                OfeModalidadPrograma = r.OfeModalidadPrograma,
                DiasPattern = r.DiasPattern,
                BloqueHorario = r.BloqueHorario,
                HoraInicio = r.HoraInicio,

                OfeModulo = r.OfeModulo,
                OfeSemestre = r.OfeSemestre,
                OfeAnio = r.OfeAnio,
                OfeNivel = r.OfeNivel,
                OfePresencialidadObligatoria = r.OfePresencialidadObligatoria,
                OfeDuracionClase = r.OfeDuracionClase,
                OfeEsCore = r.OfeEsCore,
                IsVirtual = r.IsVirtual,
                DiasCount = r.DiasCount,
                Dia_L = r.Dia_L,
                Dia_MA = r.Dia_MA,
                Dia_MI = r.Dia_MI,
                Dia_J = r.Dia_J,
                Dia_V = r.Dia_V,
                Dia_S = r.Dia_S,
                Dia_D = r.Dia_D,
                HasWeekend = r.HasWeekend,
                PreSolicitudes = r.PreSolicitudes
            };
        }

        // ===================== MODELOS =====================
        public record TrainingRow
        {
            public string PerCodigo { get; init; } = "";
            public string ModCodigo { get; init; } = "";
            public string CamCodigo { get; init; } = "";
            public string SecCodigo { get; init; } = "";
            public string DocCodigo { get; init; } = "";
            public string NombreMateria { get; init; } = "";
            public int OfeModulo { get; init; }
            public int OfeSemestre { get; init; }
            public int OfeAnio { get; init; }
            public int OfeNivel { get; init; }
            public int OfePresencialidadObligatoria { get; init; }
            public int OfeDuracionClase { get; init; }
            public int OfeEsCore { get; init; }
            public string DiasPattern { get; init; } = "NA";
            public string BloqueHorario { get; init; } = "NA";
            public int IsVirtual { get; init; }
            public int DiasCount { get; init; }
            public int Dia_L { get; init; }
            public int Dia_MA { get; init; }
            public int Dia_MI { get; init; }
            public int Dia_J { get; init; }
            public int Dia_V { get; init; }
            public int Dia_S { get; init; }
            public int Dia_D { get; init; }
            public int HasWeekend { get; init; }
            public int PreSolicitudes { get; init; }
            public string OfeModalidadPrograma { get; init; } = "";
            public int OfeMatriculados { get; init; }
            public string HoraInicio { get; init; } = "";
        }

        public record PredictionRow
        {
            public string PerCodigo { get; init; } = "";
            public string ModCodigo { get; init; } = "";
            public string CamCodigo { get; init; } = "";
            public string SecCodigo { get; init; } = "";
            public string DocCodigo { get; init; } = "";
            public string NombreMateria { get; init; } = "";
            public int OfeModulo { get; init; }
            public int OfeSemestre { get; init; }
            public int OfeAnio { get; init; }
            public int OfeNivel { get; init; }
            public int OfePresencialidadObligatoria { get; init; }
            public int OfeDuracionClase { get; init; }
            public int OfeEsCore { get; init; }
            public string HoraInicio { get; init; } = "";
            public string DiasPattern { get; init; } = "NA";
            public string BloqueHorario { get; init; } = "NA";
            public int IsVirtual { get; init; }
            public int DiasCount { get; init; }
            public int Dia_L { get; init; }
            public int Dia_MA { get; init; }
            public int Dia_MI { get; init; }
            public int Dia_J { get; init; }
            public int Dia_V { get; init; }
            public int Dia_S { get; init; }
            public int Dia_D { get; init; }
            public int HasWeekend { get; init; }
            public int PreSolicitudes { get; init; }
            public string OfeModalidadPrograma { get; init; } = "";
        }

        public class MLFeatures
        {
            public string PerCodigo { get; set; } = "";
            public string ModCodigo { get; set; } = "";
            public string CamCodigo { get; set; } = "";
            public string SecCodigo { get; set; } = "";
            public string DocCodigo { get; set; } = "";
            public string NombreMateria { get; set; } = "";
            public string OfeModalidadPrograma { get; set; } = "";
            public string DiasPattern { get; set; } = "NA";
            public string BloqueHorario { get; set; } = "NA";
            public string HoraInicio { get; set; } = "";

            public float OfeModulo { get; set; }
            public float OfeSemestre { get; set; }
            public float OfeAnio { get; set; }
            public float OfeNivel { get; set; }
            public float OfePresencialidadObligatoria { get; set; }
            public float OfeDuracionClase { get; set; }
            public float OfeEsCore { get; set; }
            public float IsVirtual { get; set; }
            public float DiasCount { get; set; }
            public float Dia_L { get; set; }
            public float Dia_MA { get; set; }
            public float Dia_MI { get; set; }
            public float Dia_J { get; set; }
            public float Dia_V { get; set; }
            public float Dia_S { get; set; }
            public float Dia_D { get; set; }
            public float HasWeekend { get; set; }
            public float PreSolicitudes { get; set; }
            public float OfeMatriculados { get; set; }
        }

        public class EnrollmentPrediction
        {
            [ColumnName("Score")]
            public float Score { get; set; }
        }
    }
}
