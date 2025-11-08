using System;
using System.Collections.Generic;

namespace UploadData.API.Models
{
    /// <summary>
    /// Resultado de una carga de archivo Excel a STG.ofertas_raw.
    /// </summary>
    public sealed class UploadResult
    {
        public Guid BatchId { get; set; }
        public int Inserted { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string? Worksheet { get; set; }
        public List<string> Warnings { get; set; } = new();
        public string? Message { get; set; }
    }
}
