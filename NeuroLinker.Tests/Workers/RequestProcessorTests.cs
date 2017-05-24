using System;
using System.IO;
using FluentAssertions;
using HtmlAgilityPack;
using Moq;
using NeuroLinker.Helpers;
using NeuroLinker.Interfaces;
using NeuroLinker.Interfaces.Helpers;
using NeuroLinker.Workers;
using NUnit.Framework;

namespace NeuroLinker.Tests.Workers
{
    public class RequestProcessorTests
    {
        #region Public Methods

        [TestCase(true)]
        [TestCase(false)]
        public void CredentialVerificationRespondsCorrectly(bool validResult)
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
                    .ReturnsAsync(data);
            }
            else
            {
                fixture.PageRetrieverMock.Setup(
                        t => t.RetrieveDocumentAsStringAsync(MalRouteBuilder.VerifyCredentialsUrl(), user, pass))
                    .ReturnsAsync("Invalid credentials");
            }

            var sut = fixture.Instance;

            // act
            var result = sut.VerifyCredentials(user, pass).Result;

            // assert
            result.Should().Be(validResult);
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
               .ReturnsAsync(document);

            var sut = fixture.Instance;

            // act
            var character = sut.DoCharacterRetrieval(characterId).Result;

            // assert
            character.Name.Should().Be("Asuna Yuuki (結城 明日奈 / アスナ)");
            character.Id.Should().Be(characterId);
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
            var character = sut.DoCharacterRetrieval(characterId).Result;

            // assert
            character.ErrorOccured.Should().BeTrue();
            character.ErrorMessage.Should().Be("Cannot load");
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
            result.ErrorOccured.Should().BeTrue();
            result.ErrorMessage.Should().Be("Cannot load");
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
            var seiyuu = sut.DoSeiyuuRetrieval(seiyuuId).Result;

            // assert
            seiyuu.ErrorOccured.Should().BeTrue();
            seiyuu.ErrorMessage.Should().Be("Cannot load");
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
                .ReturnsAsync(document);

            var sut = fixture.Instance;

            // act
            var result = sut.GetAnime(animeId).Result;

            // assert
            fixture.PageRetrieverMock.Verify(t => t.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeCharacterUrl(animeId)), Times.Once);
            result.UserScore.Should().Be(0);
            result.UserWatchedEpisodes.Should().Be(0);
            result.UserWatchedStatus.Should().Be(null);
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
                .ReturnsAsync(document);

            var sut = fixture.Instance;

            // act
            var result = sut.GetAnime(animeId, user, pass).Result;

            // assert
            fixture.PageRetrieverMock.Verify(t => t.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeCharacterUrl(animeId)), Times.Once);
            result.UserScore.Should().Be(10);
            result.UserWatchedEpisodes.Should().Be(25);
            result.UserWatchedStatus.Should().Be("Completed");
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
               .ReturnsAsync(document);

            var sut = fixture.Instance;

            // act
            var seiyuu = sut.DoSeiyuuRetrieval(seiyuuId).Result;

            // assert
            seiyuu.Id.Should().Be(seiyuuId);
            seiyuu.Name.Should().Be("Mamiko Noto");
            seiyuu.ErrorOccured.Should().BeFalse();
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
                    .ReturnsAsync(characterDocument);
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