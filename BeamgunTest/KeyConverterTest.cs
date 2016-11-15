using System;
using System.Windows.Forms;
using BeamgunApp.Models;
using NUnit.Framework;

namespace BeamgunTest
{
    [TestFixture]
    public class KeyConverterTest
    {
        [Test]
        public void ConvertsSingleKeys()
        {
            var converter = new KeyConverter();

            Assert.AreEqual("a", converter.Convert(Keys.A));
            Assert.AreEqual("z", converter.Convert(Keys.Z));
            Assert.AreEqual("\n", converter.Convert(Keys.Return));
            Assert.AreEqual(" ", converter.Convert(Keys.Space));
        }

        [Test]
        public void ConvertsEnumerablesOfKeys()
        {
            var converter = new KeyConverter();

            var result = converter.Convert(new[] {Keys.A, Keys.B, Keys.C, Keys.Space, Keys.Return});

            Assert.AreEqual("abc \n", result);
        }
    }
}
