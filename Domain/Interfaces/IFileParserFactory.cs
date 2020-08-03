namespace Domain.Interfaces
{
    public interface IFileParserFactory
    {
        IFileParser GetParser(string fileExtension);
    }
}
