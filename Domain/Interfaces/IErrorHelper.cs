namespace Domain.Interfaces
{
    public interface IErrorHelper
    {
        void AppendErrorMessage(string message);
        string GetErrorMessage();
    }
}
