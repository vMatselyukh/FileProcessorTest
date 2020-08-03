using Domain.Interfaces;
using System;

namespace Bll.Parsers
{
    public class FileParserFactory
    {
        public static IFileParser GetParser(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension))
            {
                throw new ArgumentNullException("File extension shouldn't be empty");
            }

            fileExtension = fileExtension.ToLower().Trim();

            switch (fileExtension) {
                case ".csv":
                    return new CsvParser();
                case ".xml":
                    return new XmlParser();
                default:
                    throw new FormatException("Unknown file format");
            }
        }
    }
}
