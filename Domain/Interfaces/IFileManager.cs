using Domain.Models;
using System.IO;

namespace Domain.Interfaces
{
    public interface IFileManager
    {
        FileProcessResult ProcessFile(FileStream fileContent, string fileExtension);
    }
}
