using System;
using System.Text;
using FluentAssertions;
using NeuroLinker.Factories;
using NUnit.Framework;

namespace NeuroLinker.Tests.Factories
{
    public class HttpClientFactoryTest
    {
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
    }
}