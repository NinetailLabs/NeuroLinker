using System;
using System.Text;
using FluentAssertions;
using HttpMock;
using NeuroLinker.Factories;
using NeuroLinker.Helpers;
using NUnit.Framework;

namespace NeuroLinker.Tests.Helpers
{
    // TODO - Update to use proper fixtures!
    public class PageRetrieverTests
    {
        #region Public Methods

        [Test]
        public void BasicPageRetrievalWorks()
        {
            // arrange
            const string testPath = "/test";
            const string url = "http://localhost:8654";
            var fullUrl = $"{url}{testPath}";
            var httpMock = HttpMockRepository.At(url);
            httpMock.Stub(t => t.Get(testPath))
                .Return("")
                .OK();

            var sut = new PageRetriever(new HttpClientFactory());

            // act
            var result = sut.RetrieveHtmlPageAsync(fullUrl).Result;

            // assert
            result.Should().NotBeNull();
        }

        [Test]
        public void PageRetrievalWithAuthenticationWorks()
        {
            // arrange
            const string username = "testuser";
            const string password = "testpass";
            var authValue = $"Basic {Convert.ToBase64String(Encoding.Default.GetBytes($"{username}:{password}"))}";
            const string testPath = "/test";
            const string url = "http://localhost:8654";
            var fullUrl = $"{url}{testPath}";
            var httpMock = HttpMockRepository.At(url);
            httpMock.Stub(t => t.Get(testPath))
                .Return("")
                .OK();

            var sut = new PageRetriever(new HttpClientFactory());

            // act
            var result = sut.RetrieveHtmlPageAsync(fullUrl, username, password).Result;

            // assert
            result.Should().NotBeNull();
            var headers = httpMock.AssertWasCalled(t => t.Get(testPath))
                .LastRequest()
                .RequestHead
                .Headers
                .Should()
                .Contain("Authorization", authValue);
        }

        [Test]
        public void RetrievePageAsString()
        {
            // arrange
            const string testPath = "/test";
            const string url = "http://localhost:8654";
            var fullUrl = $"{url}{testPath}";
            var httpMock = HttpMockRepository.At(url);
            httpMock.Stub(t => t.Get(testPath))
                .Return("Hello World")
                .OK();

            var sut = new PageRetriever(new HttpClientFactory());

            // act
            var result = sut.RetrieveDocumentAsStringAsync(fullUrl, null, null).Result;

            // assert
            result.Should().Be("Hello World");
        }

        #endregion
    }
}