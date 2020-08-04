namespace Domain.Interfaces
{
    public interface IMessageErrorHelper
    {
        void AppendErrorMessage(string message);
        string GetErrorMessage();
    }
}
