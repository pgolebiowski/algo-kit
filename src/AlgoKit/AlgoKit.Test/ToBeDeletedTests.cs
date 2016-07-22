using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AlgoKit.Test
{
    [TestFixture]
    public class ToBeDeletedTests
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReturnSameValue(bool input)
        {
            var instance = new ToBeDeleted();
            Assert.AreEqual(input, instance.Fixed(input));
        }
    }
}
