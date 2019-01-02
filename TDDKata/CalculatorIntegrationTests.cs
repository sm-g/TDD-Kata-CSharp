using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace TDDKata
{
    [TestFixture]
    public class CalculatorIntegrationTests
    {
        [Test]
        public void Add_ShouldWriteResultToLogger()
        {
            var logger = new LoggerMock();
            var calculator = new CalculatorService(logger);

            calculator.Add("1,2,3");
            calculator.Add("5");

            Assert.AreEqual(new[] { "6", "5" }, logger.Writes);
        }

        [Test]
        public void Add_WhenLoggerThrows_ShouldSendMessageFromExceptionToWebService()
        {
            var logger = new FailingLoggerStub()
            {
                ExceptionToThrow = new DivideByZeroException("message from ex")
            };
            var webService = new WebServiceMock();
            var calculator = new CalculatorService(logger, webService);

            calculator.Add("1,2,3");

            Assert.AreEqual(new[] { "message from ex" }, webService.Messages);
        }

        [Test]
        public void Add_ShouldSendResultToConsole()
        {
            var sb = new StringBuilder();
            var textWriter = new StringWriter(sb);
            Console.SetOut(textWriter);
            var calculator = new CalculatorService();

            calculator.Add("1,2,3");

            Assert.AreEqual("6" + Environment.NewLine, sb.ToString());
        }

        [Test]
        public void Add_ShouldWriteNewLineWithResultToConsole()
        {
            var sb = new StringBuilder();
            var textWriter = new StringWriter(sb);
            Console.SetOut(textWriter);
            var calculator = new CalculatorService();

            calculator.Add("1,2,3");
            calculator.Add("7");

            Assert.AreEqual("6" + Environment.NewLine + "7" + Environment.NewLine, sb.ToString());
        }

        private class LoggerMock : ILogger
        {
            public List<string> Writes { get; } = new List<string>();

            public void Write(string value)
            {
                Writes.Add(value);
            }
        }

        private class FailingLoggerStub : ILogger
        {
            public Exception ExceptionToThrow { get; set; }

            public void Write(string value)
            {
                throw ExceptionToThrow;
            }
        }

        private class WebServiceMock : IWebService
        {
            public List<string> Messages { get; } = new List<string>();

            public void Notify(string message)
            {
                Messages.Add(message);
            }
        }
    }
}