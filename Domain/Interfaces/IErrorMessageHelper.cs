namespace Domain.Interfaces
{
    public interface IErrorMessageHelper
    {
        void AppendErrorMessage(string message);
        string GetErrorMessage();
    }
}
