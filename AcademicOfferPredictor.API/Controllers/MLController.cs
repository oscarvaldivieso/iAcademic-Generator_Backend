using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AcademicOfferPredictor.API.Models;
using AcademicOfferPredictor.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AcademicOfferPredictor.API.Controllers
{
    [ApiController]
    [Route("api/ml")]
    public class MlController : ControllerBase
    {
        private readonly IOfferPredictorService _service;
        private readonly IConfiguration _config;
        private readonly ILogger<MlController> _logger;

        public MlController(
            IOfferPredictorService service,
            IConfiguration config,
            ILogger<MlController> logger)
        {
            _service = service;
            _config = config;
            _logger = logger;
        }

        public class MlRunRequest
        {
            public string Period { get; set; } = "";
        }

        // ====== POST /api/ml/run  ======
        [HttpPost("run")]
        public async Task<IActionResult> Run(
            [FromBody] MlRunRequest request,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request?.Period))
                return BadRequest("Period is required.");

            var result = await _service.RunAsync(request.Period, ct);
            return Ok(result);
        }

        // ====== GET /api/ml/resultados/{periodo}  ======
        [HttpGet("resultados/{periodo}")]
        public async Task<IActionResult> GetResultados(string periodo, CancellationToken ct)
        {
            try
            {
                var connString = _config.GetConnectionString("AcademicDb")!;
                var list = new List<object>();

                await using var cn = new SqlConnection(connString);
                await cn.OpenAsync(ct);

                var sql = @"
                    SELECT per_codigo, mod_codigo, nombre_materia, cam_codigo,
                           sec_codigo, doc_codigo, dias_pattern, bloque_horario,
                           hora_inicio, matric_pred, abrir_pred, prob, run_tag
                    FROM ML.pred_ofertas_resultados
                    WHERE per_codigo = @per
                    ORDER BY matric_pred DESC";

                await using var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@per", periodo);

                await using var rd = await cmd.ExecuteReaderAsync(ct);
                while (await rd.ReadAsync(ct))
                {
                    list.Add(new
                    {
                        perCodigo = rd["per_codigo"].ToString(),
                        modCodigo = rd["mod_codigo"].ToString(),
                        nombreMateria = rd["nombre_materia"].ToString(),
                        camCodigo = rd["cam_codigo"].ToString(),
                        secCodigo = rd["sec_codigo"].ToString(),
                        docCodigo = rd["doc_codigo"].ToString(),
                        diasPattern = rd["dias_pattern"].ToString(),
                        bloqueHorario = rd["bloque_horario"].ToString(),
                        horaInicio = rd["hora_inicio"].ToString(),
                        matricPred = Convert.ToDouble(rd["matric_pred"]),
                        abrirPred = Convert.ToInt32(rd["abrir_pred"]),
                        prob = Convert.ToDouble(rd["prob"]),
                        runTag = rd["run_tag"].ToString()
                    });
                }

                return Ok(new
                {
                    periodo,
                    totalResultados = list.Count,
                    resultados = list
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando resultados para periodo {Periodo}", periodo);
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
