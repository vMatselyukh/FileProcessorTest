﻿using Domain.Interfaces;
using System.Text;

namespace Bll.Helpers
{
    /// <summary>
    /// Create an instance of this class each time before using it.
    /// Don't reused old instance because of accumulation of error
    /// messages
    /// </summary>
    public class MessageBuilder : IMessageBuilder
    {
        private StringBuilder _stringBuilder;

        public MessageBuilder()
        {
            _stringBuilder = new StringBuilder();
        }

        public void AppendMessage(string message)
        {
            _stringBuilder.Append(message);
            _stringBuilder.AppendLine();
        }

        public string GetMessage()
        {
            return _stringBuilder.ToString();
        }
    }
}
