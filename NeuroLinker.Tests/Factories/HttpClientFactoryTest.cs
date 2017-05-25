using System;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using NeuroLinker.Factories;
using NeuroLinker.Interfaces.Configuration;
using NUnit.Framework;

namespace NeuroLinker.Tests.Factories
{
    public class HttpClientFactoryTest
    {
        #region Public Methods

        [Test]
        public void CreatingAnHttpClientWhenConfigurationProvidesABlankStringLeavesOutTheUserAgentHeader()
        {
            // arrange
            var configurationMock = new Mock<IHttpClientConfiguration>();
            configurationMock
                .Setup(t => t.UserAgent)
                .Returns(string.Empty);

            var sut = new HttpClientFactory(configurationMock.Object);

            // act
            var client = sut.GetHttpClient(null, null);

            // assert
            client.DefaultRequestHeaders.UserAgent.Count.Should().Be(0);
        }

        [Test]
        public void CreatingHttClientWhenConfigurationIsProvidedWorksCorrectly()
        {
            // arrange
            const string agentString = "Kuroyukihime";
            var configurationMock = new Mock<IHttpClientConfiguration>();
            configurationMock
                .Setup(t => t.UserAgent)
                .Returns(agentString);

            var sut = new HttpClientFactory(configurationMock.Object);

            // act
            var client = sut.GetHttpClient(null, null);

            // assert
            client.DefaultRequestHeaders.UserAgent.Count.Should().Be(1);
            client.DefaultRequestHeaders.UserAgent.First().ToString().Should().Be(agentString);
        }

        [Test]
        public void CreatingHttpClientWithAuthenticationWorksCorrectly()
        {
            // arrange
            const string user = "User";
            const string pass = "Pass";
            var sut = new HttpClientFactory();

            // act
            var client = sut.GetHttpClient(user, pass);

            // assert
            var authValue = $"Basic {Convert.ToBase64String(Encoding.Default.GetBytes($"{user}:{pass}"))}";
            client.DefaultRequestHeaders.Authorization.ToString().Should().Be(authValue);
        }

        [Test]
        public void CreatingHttpClientWithoutAuthenticationWorksCorrectly()
        {
            // arrange
            var sut = new HttpClientFactory();

            // act
            var client = sut.GetHttpClient(null, null);

            // assert
            client.DefaultRequestHeaders.Authorization.Should().BeNull();
        }

        #endregion
    }
}