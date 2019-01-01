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
        private const char Comma = ',';
        private const char NewLine = '\n';

        public static object Add(string input)
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
                if (input[2] == '[')
                {
                    // multi char delimiter
                    var endOfDelimSequence = input.IndexOf("]\n");
                    var delimiterSequence = input.Substring(3, endOfDelimSequence - 3);
                    return (input.Substring(endOfDelimSequence + 2), new[] { delimiterSequence, NewLine.ToString() });
                }
                return (input.Substring(4), new[] { input[2].ToString(), NewLine.ToString() });
            }
            return (input, new[] { Comma.ToString(), NewLine.ToString() });
        }
    }
}