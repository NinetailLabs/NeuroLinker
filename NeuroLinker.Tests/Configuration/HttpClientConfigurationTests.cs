using System.Reflection;
using FluentAssertions;
using NeuroLinker.Configuration;
using NeuroLinker.Enumerations;
using NeuroLinker.Tests.TestHelpers;
using NUnit.Framework;
using VaraniumSharp.Extensions;

namespace NeuroLinker.Tests.Configuration
{
    public class HttpClientConfigurationTests
    {
        #region Public Methods

        [Test]
        public void ConfigurationIsLoadedCorrectly()
        {
            // arrange
            StringExtensions.ConfigurationLocation = Assembly.GetExecutingAssembly().Location;
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