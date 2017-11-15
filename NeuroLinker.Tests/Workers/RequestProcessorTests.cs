using FluentAssertions;
using HtmlAgilityPack;
using Moq;
using NeuroLinker.Helpers;
using NeuroLinker.Interfaces.Helpers;
using NeuroLinker.Workers;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using NeuroLinker.ResponseWrappers;

namespace NeuroLinker.Tests.Workers
{
    public class RequestProcessorTests
    {
        #region Public Methods

        [TestCase(true, HttpStatusCode.OK)]
        [TestCase(false, HttpStatusCode.Unauthorized)]
        public void CredentialVerificationRespondsCorrectly(bool validResult, HttpStatusCode statusCode)
        {
            // arrange
            const string user = "testUser";
            const string pass = "testPass";
            var fixture = new RequestProcessorFixture();

            var path = AppDomain.CurrentDomain.BaseDirectory;
            var examplePath = Path.Combine(path, "PageExamples", "ValidUser.xml");
            var data = File.ReadAllText(examplePath);

            if (validResult)
            {
                fixture.PageRetrieverMock.Setup(
                        t => t.RetrieveDocumentAsStringAsync(MalRouteBuilder.VerifyCredentialsUrl(), user, pass))
                    .ReturnsAsync(new StringRetrievalWrapper(HttpStatusCode.OK, true, data));
            }
            else
            {
                fixture.PageRetrieverMock.Setup(
                        t => t.RetrieveDocumentAsStringAsync(MalRouteBuilder.VerifyCredentialsUrl(), user, pass))
                    .ReturnsAsync(new StringRetrievalWrapper(HttpStatusCode.Unauthorized, false,
                        "Invalid credentials"));
            }

            var sut = fixture.Instance;

            // act
            var result = sut.VerifyCredentials(user, pass).Result;

            // assert
            result.ResponseStatusCode.Should().Be(statusCode);
            result.Success.Should().Be(validResult);
        }

        [Test]
        public void FailedHttpRetrievalDoesNotCauseAnException()
        {
            // arrange
            const int malId = 13127;
            var fixture = new RequestProcessorFixture();

            var document = new HtmlDocument();
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var examplePath = Path.Combine(path, "PageExamples", $"{malId}.html");
            using (var htmlFile = File.Open(examplePath, FileMode.Open))
            {
                document.Load(htmlFile);
            }

            fixture.PageRetrieverMock
                .Setup(t => t.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeUrl(malId)))
                .ReturnsAsync(new HtmlDocumentRetrievalWrapper(HttpStatusCode.NotFound, true, document));

            var sut = fixture.Instance;
            var act = new Action(() => sut.GetAnime(malId).Wait());

            // act
            // assert
            act.ShouldNotThrow<Exception>();
        }

        [Test]
        public async Task IfAnimeDoesNotExistAnErrorIsPassedBackToTheCaller()
        {
            // arrange
            const int malId = 13127;
            var fixture = new RequestProcessorFixture();

            var document = new HtmlDocument();
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var examplePath = Path.Combine(path, "PageExamples", $"{malId}.html");
            using (var htmlFile = File.Open(examplePath, FileMode.Open))
            {
                document.Load(htmlFile);
            }

            fixture.PageRetrieverMock
                .Setup(t => t.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeUrl(malId)))
                .ReturnsAsync(new HtmlDocumentRetrievalWrapper(HttpStatusCode.NotFound, true, document));

            var sut = fixture.Instance;

            // act
            var result = await sut.GetAnime(malId);

            // assert
            result.ResponseStatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Success.Should().BeFalse();
        }

        [Test]
        public void LoadingCharacterInformationWorksCorrectly()
        {
            // arrange
            const int characterId = 36828;
            var fixture = new RequestProcessorFixture();

            var document = new HtmlDocument();
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var examplePath = Path.Combine(path, "PageExamples", $"{characterId}.html");
            using (var htmlFile = File.Open(examplePath, FileMode.Open))
            {
                document.Load(htmlFile);
            }

            fixture.PageRetrieverMock
                .Setup(t => t.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeCharacterUrl(characterId)))
                .ReturnsAsync(new HtmlDocumentRetrievalWrapper(HttpStatusCode.OK, true, document));

            var sut = fixture.Instance;

            // act
            var retrievalWrapper = sut.DoCharacterRetrieval(characterId).Result;

            // assert
            retrievalWrapper.Success.Should().BeTrue();
            retrievalWrapper.ResponseStatusCode.Should().Be(HttpStatusCode.OK);
            retrievalWrapper.ResponseData.Name.Should().Be("Asuna Yuuki (結城 明日奈 / アスナ)");
            retrievalWrapper.ResponseData.Id.Should().Be(characterId);
        }

        [Test]
        public void LoadingCharacterWithAnErrorCorrectlyMarksItAsBroken()
        {
            // arrange
            const int characterId = 36828;
            var fixture = new RequestProcessorFixture();

            fixture.PageRetrieverMock
                .Setup(t => t.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeCharacterUrl(characterId)))
                .Throws(new Exception("Cannot load"));

            var sut = fixture.Instance;

            // act
            var retrievalWrapper = sut.DoCharacterRetrieval(characterId).Result;

            // assert
            retrievalWrapper.Exception.Should().NotBeNull();
            retrievalWrapper.ResponseData.ErrorOccured.Should().BeTrue();
            retrievalWrapper.ResponseData.ErrorMessage.Should().Be("Cannot load");
        }

        [Test]
        public void LoadingInvalidDataCorrectlyMarksTheItemAsError()
        {
            // arrange
            const int animeId = 11757;
            var fixture = new RequestProcessorFixture(animeId);

            fixture.PageRetrieverMock
                .Setup(t => t.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeUrl(animeId)))
                .Throws(new Exception("Cannot load"));

            var sut = fixture.Instance;

            // act
            var result = sut.GetAnime(animeId).Result;

            // assert
            result.Exception.Should().NotBeNull();
            result.ResponseData.ErrorOccured.Should().BeTrue();
            result.ResponseData.ErrorMessage.Should().Be("Cannot load");
        }

        [Test]
        public void LoadingSeiyuuWithErrorCorrectlyMarksAsBroken()
        {
            // arrange
            const int seiyuuId = 40;
            var fixture = new RequestProcessorFixture();

            fixture.PageRetrieverMock
                .Setup(t => t.RetrieveHtmlPageAsync(MalRouteBuilder.SeiyuuUrl(seiyuuId)))
                .Throws(new Exception("Cannot load"));

            var sut = fixture.Instance;

            // act
            var retrievalWrapper = sut.DoSeiyuuRetrieval(seiyuuId).Result;

            // assert
            retrievalWrapper.Exception.Should().NotBeNull();
            retrievalWrapper.ResponseData.ErrorOccured.Should().BeTrue();
            retrievalWrapper.ResponseData.ErrorMessage.Should().Be("Cannot load");
        }

        [Test]
        public void RetrievingAnimeWithUsernameAndPasswordDoesNotPopulateUserFields()
        {
            // arrange
            const int animeId = 11757;
            var fixture = new RequestProcessorFixture(animeId);
            var document = new HtmlDocument();
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var examplePath = Path.Combine(path, "PageExamples", $"{animeId}LoggedIn.html");
            using (var htmlFile = File.Open(examplePath, FileMode.Open))
            {
                document.Load(htmlFile);
            }

            fixture.PageRetrieverMock
                .Setup(t => t.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeUrl(animeId)))
                .ReturnsAsync(new HtmlDocumentRetrievalWrapper(HttpStatusCode.OK, true, document));

            var sut = fixture.Instance;

            // act
            var result = sut.GetAnime(animeId).Result;

            // assert
            fixture.PageRetrieverMock.Verify(t => t.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeCharacterUrl(animeId)),
                Times.Once);
            result.ResponseStatusCode.Should().Be(HttpStatusCode.OK);
            result.Success.Should().BeTrue();
            result.ResponseData.UserScore.Should().Be(0);
            result.ResponseData.UserWatchedEpisodes.Should().Be(0);
            result.ResponseData.UserWatchedStatus.Should().Be(null);
        }

        [Test]
        public void RetrievingAnimeWithUsernameAndPasswordPopulatesTheUserFields()
        {
            // arrange
            const int animeId = 11757;
            var fixture = new RequestProcessorFixture(animeId);
            const string user = "testUser";
            const string pass = "testPass";
            var document = new HtmlDocument();
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var examplePath = Path.Combine(path, "PageExamples", $"{animeId}LoggedIn.html");
            using (var htmlFile = File.Open(examplePath, FileMode.Open))
            {
                document.Load(htmlFile);
            }

            fixture.PageRetrieverMock
                .Setup(t => t.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeUrl(animeId), user, pass))
                .ReturnsAsync(new HtmlDocumentRetrievalWrapper(HttpStatusCode.OK, true, document));

            var sut = fixture.Instance;

            // act
            var result = sut.GetAnime(animeId, user, pass).Result;

            // assert
            fixture.PageRetrieverMock.Verify(t => t.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeCharacterUrl(animeId)),
                Times.Once);
            result.ResponseData.UserScore.Should().Be(10);
            result.ResponseData.UserWatchedEpisodes.Should().Be(25);
            result.ResponseData.UserWatchedStatus.Should().Be("Completed");
        }

        [Test]
        public void RetrievingSeiyuuInformationWorksCorrectly()
        {
            // arrange
            const int seiyuuId = 40;
            var fixture = new RequestProcessorFixture();

            var document = new HtmlDocument();
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var examplePath = Path.Combine(path, "PageExamples", $"{seiyuuId}.html");
            using (var htmlFile = File.Open(examplePath, FileMode.Open))
            {
                document.Load(htmlFile);
            }

            fixture.PageRetrieverMock
                .Setup(t => t.RetrieveHtmlPageAsync(MalRouteBuilder.SeiyuuUrl(seiyuuId)))
                .ReturnsAsync(new HtmlDocumentRetrievalWrapper(HttpStatusCode.OK, true, document));

            var sut = fixture.Instance;

            // act
            var retrievalWrapper = sut.DoSeiyuuRetrieval(seiyuuId).Result;

            // assert
            retrievalWrapper.ResponseStatusCode.Should().Be(HttpStatusCode.OK);
            retrievalWrapper.Success.Should().BeTrue();
            retrievalWrapper.ResponseData.Id.Should().Be(seiyuuId);
            retrievalWrapper.ResponseData.Name.Should().Be("Mamiko Noto");
            retrievalWrapper.ResponseData.ErrorOccured.Should().BeFalse();
        }

        #endregion

        private class RequestProcessorFixture
        {
            #region Constructor

            public RequestProcessorFixture(int malId)
                : this()
            {
                var characterDocument = new HtmlDocument();
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var examplePath = Path.Combine(path, "PageExamples", $"{malId}CharacterInfo.html");
                using (var htmlFile = File.Open(examplePath, FileMode.Open))
                {
                    characterDocument.Load(htmlFile);
                }

                PageRetrieverMock
                    .Setup(t => t.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeCharacterUrl(malId)))
                    .ReturnsAsync(new HtmlDocumentRetrievalWrapper(HttpStatusCode.OK, true, characterDocument));
            }

            public RequestProcessorFixture()
            {
                Instance = new RequestProcessor(PageRetriever);
            }

            #endregion

            #region Properties

            public RequestProcessor Instance { get; }
            public IPageRetriever PageRetriever => PageRetrieverMock.Object;
            public Mock<IPageRetriever> PageRetrieverMock { get; } = new Mock<IPageRetriever>();

            #endregion
        }
    }
}