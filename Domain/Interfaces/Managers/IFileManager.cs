using Domain.Models;
using System.IO;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IFileManager
    {
        Task<FileProcessResult> ProcessFileAsync(string content, string fileExtension);
    }
}
