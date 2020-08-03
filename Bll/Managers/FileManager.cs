using Bll.Parsers;
using Domain.Interfaces;
using Domain.Models;
using System.IO;

namespace Bll.Managers
{
    public class FileManager : IFileManager
    {
        private IFileParserFactory _fileParserFactory;
        public FileManager(IFileParserFactory fileParserFactory)
        {
            _fileParserFactory = fileParserFactory;
        }
        public FileProcessResult ProcessFile(FileStream fileContent, string fileExtension)
        {
            var parser = _fileParserFactory.GetParser(fileExtension);
            var parseResult = parser.ParseFile(fileContent);

            if (parseResult.IsSucceed)
            {
                //write parse result to the db

            }
            else
            { 
                //write log about validation errors
            }

            return new FileProcessResult
            {
                IsSucceed = parseResult.IsSucceed,
                ErrorMessage = parseResult.ErrorMessage
            };
        }
    }
}
