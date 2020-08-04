using Domain.Interfaces;
using System;

namespace Bll.Parsers
{
    public class FileParserFactory : IFileParserFactory
    {
        private IServiceProvider _serviceProvider;
        public FileParserFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IFileParser GetParser(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension))
            {
                throw new ArgumentNullException("File extension shouldn't be empty");
            }

            fileExtension = fileExtension.ToLower().Trim();

            switch (fileExtension) {
                case ".csv":
                    return (CsvParser)_serviceProvider.GetService(typeof(CsvParser));
                case ".xml":
                    return (XmlParser)_serviceProvider.GetService(typeof(XmlParser));
                default:
                    throw new FormatException("Unknown file format");
            }
        }
    }
}
