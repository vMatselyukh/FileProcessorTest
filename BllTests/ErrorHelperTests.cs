using Bll.Helpers;
using NUnit.Framework;

namespace BllTests
{
    public class ErrorHelperTests
    {
        [Test]
        public void TestGetErrorMessageSucceed()
        {
            var errorHelper = new ErrorMessageHelper();
            var testMessage1 = "this is a first test message";
            var testMessage2 = "this is a second test message";

            errorHelper.AppendErrorMessage(testMessage1);
            errorHelper.AppendErrorMessage(testMessage2);

            Assert.AreEqual($"{testMessage1}\r\n{testMessage2}\r\n", errorHelper.GetErrorMessage());
        }
    }
}
