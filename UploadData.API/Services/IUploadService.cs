using System.Data;
using System.Threading;
using System.Threading.Tasks;
using UploadData.API.Models;

namespace UploadData.API.Services
{
    public interface IUploadService
    {
        Task<int> CargarCsvAsync(string csvPath, bool replacePeriod = false);
        Task<int> CargarDataTableAsync(DataTable dt, bool replacePeriod = false);
        Task<UploadResult> ProcessExcelToStagingAsync(string filePath, string? sheet, int? semestre, CancellationToken ct);
        Task RunPostLoadAsync(CancellationToken ct);
    }
}
