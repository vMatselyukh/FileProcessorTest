using Domain.Interfaces;
using System.Text;

namespace Bll.Helpers
{
    /// <summary>
    /// Create an instance of this class each time before using it.
    /// Don't reused old instance because of accumulation of error
    /// messages
    /// </summary>
    public class ErrorMessageHelper : IErrorMessageHelper
    {
        private StringBuilder _stringBuilder;

        public ErrorMessageHelper()
        {
            _stringBuilder = new StringBuilder();
        }

        public void AppendErrorMessage(string message)
        {
            _stringBuilder.Append(message);
            _stringBuilder.AppendLine();
        }

        public string GetErrorMessage()
        {
            return _stringBuilder.ToString();
        }
    }
}
