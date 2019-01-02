using System;
using System.Collections.Generic;
using System.Linq;

namespace TDDKata
{
    /// <summary>
    /// http://osherove.com/tdd-kata-1/
    /// </summary>
    public class Calculator
    {
        private static readonly string Comma = ",";
        private static readonly string NewLine = "\n";

        public static int Add(string input)
        {
            var (separatedNumbers, delimiters) = ParseDelimiters(input);

            var numbers = separatedNumbers
                .Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x))
                .Where(x => x <= 1000)
                .ToList();

            CheckNegatives(numbers);

            return numbers.Sum();
        }

        private static void CheckNegatives(IEnumerable<int> numbers)
        {
            var negatives = numbers.Where(x => x < 0);
            if (negatives.Any())
                throw new ArgumentException("negatives not allowed " + string.Join(",", negatives));
        }

        private static (string separatedNumbers, string[] delimiters) ParseDelimiters(string input)
        {
            if (input.StartsWith("//"))
            {
                // //[delim1][delim2]\n1,2,3
                if (input[2] == '[')
                {
                    var endOfDelimetersDescription = input.IndexOf("]\n");
                    var delimitersDescription = input.Substring(3, endOfDelimetersDescription - 3);
                    var delimiters = delimitersDescription.Split(new[] { "][" }, StringSplitOptions.RemoveEmptyEntries);

                    return (input.Substring(endOfDelimetersDescription + 2), delimiters.Concat(new[] { NewLine }).ToArray());
                }

                // //;\n1;2;3
                return (input.Substring(4), new[] { input[2].ToString(), NewLine });
            }
            // 1,2\n3
            return (input, new[] { Comma, NewLine });
        }
    }

    public class CalculatorService
    {
        private ILogger _logger;
        private IWebService _webService;

        public CalculatorService(ILogger logger = null, IWebService webService = null)
        {
            _logger = logger;
            _webService = webService;
        }

        public int Add(string input)
        {
            var result = Calculator.Add(input);
            try
            {
                _logger?.Write(result.ToString());
            }
            catch (Exception ex)
            {
                _webService?.Notify(ex.Message);
            }
            Console.WriteLine(result);
            return result;
        }
    }

    public interface ILogger
    {
        void Write(string value);
    }

    public interface IWebService
    {
        void Notify(string message);
    }
}