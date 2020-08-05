using Bll.Helpers;
using NUnit.Framework;

namespace BllTests
{
    public class MessageBuilderTests
    {
        [Test]
        public void TestGetErrorMessage_Succeed()
        {
            var errorHelper = new MessageBuilder();
            var testMessage1 = "this is a first test message";
            var testMessage2 = "this is a second test message";

            errorHelper.AppendMessage(testMessage1);
            errorHelper.AppendMessage(testMessage2);

            Assert.AreEqual($"{testMessage1}\r\n{testMessage2}\r\n", errorHelper.GetMessage());
        }
    }
}
