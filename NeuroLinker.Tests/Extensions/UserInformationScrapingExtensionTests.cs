using System;
using System.IO;
using FluentAssertions;
using HtmlAgilityPack;
using NeuroLinker.Extensions;
using NeuroLinker.Models;
using NUnit.Framework;

namespace NeuroLinker.Tests.Extensions
{
    public class UserInformationScrapingExtensionTests
    {
        #region Public Methods

        [Test]
        public void UserScoreIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new UserInformationScrapingExtensionsFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveUserScore(fixture.Document);

            // assert
            sut.UserScore.Should().Be(10);
        }

        [Test]
        public void UserWatchedEpisodeIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new UserInformationScrapingExtensionsFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveUserEpisode(fixture.Document);

            // assert
            sut.UserWatchedEpisodes.Should().Be(25);
        }

        [Test]
        public void UserWatchStatusIsCorrectlyRetrieved()
        {
            // arrange
            var fixture = new UserInformationScrapingExtensionsFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveUserStatus(fixture.Document);

            // assert
            sut.UserWatchedStatus.Should().Be("Completed");
        }

        #endregion

        private class UserInformationScrapingExtensionsFixture
        {
            #region Constructor

            public UserInformationScrapingExtensionsFixture()
            {
                Document = new HtmlDocument();
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var examplePath = Path.Combine(path, "PageExamples", "11757LoggedIn.html");
                using (var htmlFile = File.Open(examplePath, FileMode.Open))
                {
                    Document.Load(htmlFile);
                }

                Instance = new Anime();
            }

            #endregion

            #region Properties

            public HtmlDocument Document { get; }

            public Anime Instance { get; }

            #endregion
        }
    }
}