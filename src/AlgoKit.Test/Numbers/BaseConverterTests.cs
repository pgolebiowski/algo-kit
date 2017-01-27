using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoKit.Numbers;
using NUnit.Framework;

namespace AlgoKit.Test.Numbers
{
    [TestFixture]
    public class BaseConverterTests
    {
        [TestCase("01")]
        [TestCase("01234567")]
        [TestCase("0123456789")]
        [TestCase("0123456789abcdef")]
        public void ShouldProperlyHandleCommonBases(string digits)
        {
            // Arrange
            var converter = new BaseConverter(digits);

            for (var i = 0; i < 5000; ++i)
            {
                var correctInTargetBase = Convert.ToString(i, digits.Length);

                // Act
                var inTargetBase = converter.FromBase10(i);
                var convertedBack = converter.ToBase10(inTargetBase);

                // Assert
                Assert.AreEqual(correctInTargetBase, inTargetBase);
                Assert.AreEqual(i, convertedBack);
            }            
        }

        [TestCase("ab", "baabbaaabaababbabaaaaaaa")]
        [TestCase("abc", "caacbbaabbacbab")]
        [TestCase("abcd", "cbcacbbccaaa")]
        [TestCase("abcde", "baadaaaaaaa")]
        [TestCase("abcdef", "ffecaabee")]
        [TestCase("abcdefg", "bfagggded")]
        [TestCase("abcdefgh", "egbbdcaa")]
        [TestCase("abcdefghi", "cahdbdhb")]
        [TestCase("abcdefghij", "baaaaaaa")]
        [TestCase("abcdefghijk", "fhbabgk")]
        [TestCase("abcdefghijkl", "decdafe")]
        [TestCase("abcdefghijklm", "cambihk")]
        [TestCase("abcdefghijklmn", "beieefk")]
        [TestCase("abcdefghijklmno", "nchogk")]
        [TestCase("abcdefghijklmnop", "jijgia")]
        [TestCase("abcdefghijklmnopqrst", "dckaaa")]
        public void ShouldHandleTenMillionInVariousBases(string digits, string expected)
        {
            // Arrange
            var converter = new BaseConverter(digits);
            var number = 10000000;

            // Act
            var inTargetBase = converter.FromBase10(number);
            var convertedBack = converter.ToBase10(inTargetBase);

            // Assert
            Assert.AreEqual(expected, inTargetBase);
            Assert.AreEqual(number, convertedBack);
        }
    }
}
