using FluentAssertions;
using NeuroLinker.Configuration;
using NeuroLinker.Enumerations;
using NeuroLinker.Tests.TestHelpers;
using NUnit.Framework;

namespace NeuroLinker.Tests.Configuration
{
    public class HttpClientConfigurationTests
    {
        #region Public Methods

        [Test]
        public void ConfigurationIsLoadedCorrectly()
        {
            // arrange
            const string agentString = "Kuroyukihime";

            ApplicationConfigurationHelper.AdjustKeys(ConfigurationKeys.UserAgent, agentString);

            // act
            var sut = new HttpClientConfiguration();

            // assert
            sut.UserAgent.Should().Be(agentString);
        }

        #endregion
    }
}