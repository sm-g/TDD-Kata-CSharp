using System;
using NUnit.Framework;

namespace TDDKata
{
    [TestFixture]
    public class CalculatorTests
    {
        [Test]
        public void Add_EmptyString_ShouldReturnZero()
        {
            var result = Calculator.Add("");
            Assert.AreEqual(0, result);
        }

        [Test]
        public void Add_OneNumber_ShouldReturnThatNumber()
        {
            var result = Calculator.Add("3");
            Assert.AreEqual(3, result);
        }

        [TestCase("3,4", 7)]
        [TestCase("3,5,4,100", 112)]
        public void Add_AnyCountOfNumbers_ShouldReturnSum(string input, int expected)
        {
            var result = Calculator.Add(input);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Add_WithNewLinesBetweenNumbers_ShouldReturnSum()
        {
            var result = Calculator.Add("3\n4,5,1");
            Assert.AreEqual(13, result);
        }

        [Test]
        public void Add_WithDelimeter_ShouldReturnSum()
        {
            var result = Calculator.Add("//;\n4;1");
            Assert.AreEqual(5, result);
        }

        [Test]
        public void Add_WithDelimeterAndNewLinesBetweenNumbers_ShouldReturnSum()
        {
            var result = Calculator.Add("//;\n4;1\n3;1");
            Assert.AreEqual(9, result);
        }

        [Test]
        public void Add_Negative_ShouldThrow()
        {
            var ex = Assert.Throws<ArgumentException>(() => Calculator.Add("-1"));
            Assert.That(ex.Message, Contains.Substring("negatives not allowed").IgnoreCase);
        }

        [Test]
        public void Add_WithNegativeNumbers_ShouldReturnThrowAndListNegatives()
        {
            var ex = Assert.Throws<ArgumentException>(() => Calculator.Add("-1,5,-1"));
            Assert.That(ex.Message, Contains.Substring("negatives not allowed -1,-1").IgnoreCase);
        }

        [TestCase("3,999", 1002)]
        [TestCase("3,1000", 1003)]
        [TestCase("3,1001", 3)]
        [TestCase("1001", 0)]
        [TestCase("1003,800,3000", 800)]
        public void Add_ShouldNotAccountNumbersBiggerThan1000(string unput, int expected)
        {
            var result = Calculator.Add(unput);
            Assert.AreEqual(expected, result);
        }

        [TestCase("//[***]\n1***2***3", 6)]
        [TestCase("//[]]]\n1]]2]]4", 7)]
        [TestCase("//[]]]\n1]]2\n5", 8)]
        [TestCase("//[*]\n1*2*6", 9)]
        public void Add_WithMultiCharDelimeter_ShouldReturnSum(string unput, int expected)
        {
            var result = Calculator.Add(unput);
            Assert.AreEqual(expected, result);
        }

        [TestCase("//[*][%]\n1*2%3", 6)]
        [TestCase("//[**][%]\n1**2%4", 7)]
        [TestCase("//[**][,,,]\n1**2,,,5", 8)]
        [TestCase("//[[]][,]\n1[]2,6", 9)]
        [TestCase("//[]]][,]\n1]]2,7", 10)]
        public void Add_WithMultipleDelimeters_ShouldReturnSum(string unput, int expected)
        {
            var result = Calculator.Add(unput);
            Assert.AreEqual(expected, result);
        }
    }
}