using System;
using AlgoKit.Numbers;
using Xunit;

namespace AlgoKit.Test.Numbers
{
    public class BaseConverterTests
    {
        [Theory]
        [InlineData("01")]
        [InlineData("01234567")]
        [InlineData("0123456789")]
        [InlineData("0123456789abcdef")]
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
                Assert.Equal(correctInTargetBase, inTargetBase);
                Assert.Equal(i, convertedBack);
            }            
        }

        [Theory]
        [InlineData("ab", "baabbaaabaababbabaaaaaaa")]
        [InlineData("abc", "caacbbaabbacbab")]
        [InlineData("abcd", "cbcacbbccaaa")]
        [InlineData("abcde", "baadaaaaaaa")]
        [InlineData("abcdef", "ffecaabee")]
        [InlineData("abcdefg", "bfagggded")]
        [InlineData("abcdefgh", "egbbdcaa")]
        [InlineData("abcdefghi", "cahdbdhb")]
        [InlineData("abcdefghij", "baaaaaaa")]
        [InlineData("abcdefghijk", "fhbabgk")]
        [InlineData("abcdefghijkl", "decdafe")]
        [InlineData("abcdefghijklm", "cambihk")]
        [InlineData("abcdefghijklmn", "beieefk")]
        [InlineData("abcdefghijklmno", "nchogk")]
        [InlineData("abcdefghijklmnop", "jijgia")]
        [InlineData("abcdefghijklmnopqrst", "dckaaa")]
        public void ShouldHandleTenMillionInVariousBases(string digits, string expected)
        {
            // Arrange
            var converter = new BaseConverter(digits);
            var number = 10000000;

            // Act
            var inTargetBase = converter.FromBase10(number);
            var convertedBack = converter.ToBase10(inTargetBase);

            // Assert
            Assert.Equal(expected, inTargetBase);
            Assert.Equal(number, convertedBack);
        }
    }
}
