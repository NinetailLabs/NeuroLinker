using FluentAssertions;
using NeuroLinker.Extensions;
using NUnit.Framework;

namespace NeuroLinker.Tests.Extensions
{
    public class StringExtensionsTests
    {
        #region Public Methods

        [Test]
        public void HtmlEncodedStringIsCorrectlyDecoded()
        {
            // arrange
            const string htmlEncodedString = "Tokyo Ghoul: &quot;Jack&quot;";

            // act
            var result = htmlEncodedString.HtmlDecode();

            // assert
            result.Should().Be("Tokyo Ghoul: \"Jack\"");
        }

        #endregion
    }
}