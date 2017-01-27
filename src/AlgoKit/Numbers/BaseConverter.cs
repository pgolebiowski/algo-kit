using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoKit.Extensions;

namespace AlgoKit.Numbers
{
    /// <summary>
    /// Allows to convert decimal numbers to non-standard bases,
    /// given an arbitrary collection of digits.
    /// </summary>
    public class BaseConverter
    {
        private readonly char[] digits;
        private readonly Dictionary<char, int> indicesByDigits;

        private int Base => this.digits.Length;

        public BaseConverter(IEnumerable<char> digits)
        {
            if (digits == null)
                throw new ArgumentNullException(nameof(digits));

            this.digits = digits.ToArray();

            if (this.Base < 2)
                throw new ArgumentException("Base must be at least 2.");

            if (this.digits.Distinct().Count() != this.digits.Length)
                throw new ArgumentException("All digits must be unique.");

            this.indicesByDigits = this.digits
                .Select((d, i) => new {Digit = d, Index = i})
                .ToDictionary(x => x.Digit, x => x.Index);
        }

        public string FromBase10(int number)
        {
            var resultLength = Math.Max((int) Math.Ceiling(Math.Log(number + 1, this.Base)), 1);
            var result = new char[resultLength];

            var i = resultLength;
            do
            {
                var remainder = number % this.Base;
                number /= this.Base;

                result[--i] = this.digits[remainder];
            } while (number > 0);

            return new string(result);
        }

        public int ToBase10(string number)
        {
            if (number == "")
                throw new ArgumentException("Empty string.");

            var multiplier = 1;
            var result = 0;

            foreach (var digit in number.Reverse())
            {
                int index;
                if (!this.indicesByDigits.TryGetValue(digit, out index))
                    throw new ArgumentException($"Invalid digit: {digit}");

                result += index * multiplier;
                multiplier *= this.Base;
            }

            return result;
        }
    }
}
