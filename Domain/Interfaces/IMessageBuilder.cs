namespace Domain.Interfaces
{
    public interface IMessageBuilder
    {
        void AppendMessage(string message);
        string GetMessage();
    }
}
