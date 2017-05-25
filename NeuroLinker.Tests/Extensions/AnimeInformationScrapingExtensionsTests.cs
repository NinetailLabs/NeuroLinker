using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using HtmlAgilityPack;
using NeuroLinker.Extensions;
using NeuroLinker.Models;
using NUnit.Framework;

namespace NeuroLinker.Tests.Extensions
{
    public class AnimeInformationScrapingExtensionsTests
    {
        #region Public Methods

        [Test]
        public void AirDatesAreRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveAirDates(fixture.Document);

            // assert
            sut.StartDate.Should().Be(new DateTime(2012, 07, 08));
            sut.EndDate.Should().Be(new DateTime(2012, 12, 23));
        }

        [Test]
        public void AlternativeTitlesAreRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveAlternativeTitles(fixture.Document);

            // assert
            sut.OtherTitles["Japanese"].Count.Should().Be(1);
            sut.OtherTitles["English"].Count.Should().Be(1);
            sut.OtherTitles["Synonyms"].Count.Should().Be(2);
        }

        [Test]
        public void AnimeIdIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveAnimeId(fixture.Document);

            // assert
            sut.Id.Should().Be(11757);
        }

        [Test]
        public void AnimeImageIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveImage(fixture.Document);

            // assert
            sut.ImageUrl.Should().Be("https://myanimelist.cdn-dena.com/images/anime/11/39717.jpg");
            sut.HighResImageUrl.Should().Be("https://myanimelist.cdn-dena.com/images/anime/11/39717l.jpg");
        }

        [Test]
        public void AnimeSynopsisIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveSynopsis(fixture.Document);

            // assert
            sut.Synopsis.Should().Be(_synopsis);
        }

        [Test]
        public void AnimeTitleIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveAnimeTitle(fixture.Document);

            // assert
            sut.Title.Should().Be("Sword Art Online");
        }

        [Test]
        public void AnimeTypeIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveType(fixture.Document);

            // assert
            sut.Type.Should().Be("TV");
        }

        [Test]
        public void ClassificationIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveRating(fixture.Document);

            // assert
            sut.Classification.Should().Be("PG-13 - Teens 13 or older");
        }

        [Test]
        public void EpisodeCountIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveEpisodes(fixture.Document);

            // assert
            sut.Episodes.Should().Be(25);
        }

        [Test]
        public void FavoriteCountIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveFavotireCount(fixture.Document);

            // assert
            sut.FavoriteCount.Should().Be(42922);
        }

        [Test]
        public void GenresAreRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveGenres(fixture.Document);

            // assert
            sut.Genres.Count.Should().Be(5);
            sut.Genres.Contains("Action").Should().BeTrue();
        }

        [Test]
        public void InfoUrlsAreCorrectlyRetrieved()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveInfoUrls(fixture.Document);

            // assert
            sut.AdditionalInfoUrls.Episodes.Should().Be("https://myanimelist.net/anime/11757/Sword_Art_Online/episode");
            sut.AdditionalInfoUrls.Reviews.Should().Be("https://myanimelist.net/anime/11757/Sword_Art_Online/reviews");
            sut.AdditionalInfoUrls.Recommendation.Should()
                .Be("https://myanimelist.net/anime/11757/Sword_Art_Online/userrecs");
            sut.AdditionalInfoUrls.Stats.Should().Be("https://myanimelist.net/anime/11757/Sword_Art_Online/stats");
            sut.AdditionalInfoUrls.CharactersAndStaff.Should()
                .Be("https://myanimelist.net/anime/11757/Sword_Art_Online/characters");
            sut.AdditionalInfoUrls.News.Should().Be("https://myanimelist.net/anime/11757/Sword_Art_Online/news");
            sut.AdditionalInfoUrls.Forum.Should().Be("https://myanimelist.net/anime/11757/Sword_Art_Online/forum");
            sut.AdditionalInfoUrls.Featured.Should()
                .Be("https://myanimelist.net/anime/11757/Sword_Art_Online/featured");
            sut.AdditionalInfoUrls.Clubs.Should().Be("https://myanimelist.net/anime/11757/Sword_Art_Online/clubs");
            sut.AdditionalInfoUrls.Pictures.Should().Be("https://myanimelist.net/anime/11757/Sword_Art_Online/pics");
        }

        [Test]
        public void MemberCountIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveMemberCount(fixture.Document);

            // assert
            sut.MemberCount.Should().Be(973483);
        }

        [Test]
        public void PopularityRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrievePopularity(fixture.Document);

            // assert
            sut.Popularity.Should().Be(3);
        }

        [Test]
        public void RankIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveRank(fixture.Document);

            // assert
            sut.Rank.Should().Be(904);
        }

        [Test]
        public void RelatedInformationIsParsedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveRelatedAnime(fixture.Document);

            // assert
            sut.MangaAdaptation.Count.Should().Be(2);
            sut.Sequels.Count.Should().Be(1);
            sut.Others.Count.Should().Be(1);

            sut.Sequels.First().Id.Should().Be(20021);
            sut.Sequels.First().Title.Should().Be("Sword Art Online: Extra Edition");
            sut.Sequels.First().Url.Should().Be(@"https://myanimelist.net/anime/20021/Sword_Art_Online__Extra_Edition");
        }

        [Test]
        public void ScoreIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveScore(fixture.Document);

            // assert
            sut.MemberScore.Should().Be(7.78);
        }

        [Test]
        public void StatusIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveStatus(fixture.Document);

            // assert
            sut.Status.Should().Be("Finished Airing");
        }

        #endregion

        #region Variables

        private readonly string _synopsis =
                $"In the year 2022, virtual reality has progressed by leaps and bounds, and a massive online role-playing game called Sword Art Online (SAO) is launched. With the aid of \"NerveGear\" technology, players can control their avatars within the game using nothing but their own thoughts.{Environment.NewLine}{Environment.NewLine}Kazuto Kirigaya, nicknamed \"Kirito,\" is among the lucky few enthusiasts who get their hands on the first shipment of the game. He logs in to find himself, with ten-thousand others, in the scenic and elaborate world of Aincrad, one full of fantastic medieval weapons and gruesome monsters. However, in a cruel turn of events, the players soon realize they cannot log out; the game's creator has trapped them in his new world until they complete all one hundred levels of the game.{Environment.NewLine}{Environment.NewLine}In order to escape Aincrad, Kirito will now have to interact and cooperate with his fellow players. Some are allies, while others are foes, like Asuna Yuuki, who commands the leading group attempting to escape from the ruthless game. To make matters worse, Sword Art Online is not all fun and games: if they die in Aincrad, they die in real life. Kirito must adapt to his new reality, fight for his survival, and hopefully break free from his virtual hell.{Environment.NewLine}{Environment.NewLine}[Written by MAL Rewrite]"
            ;

        #endregion

        private class PageScrapingLogicFixture
        {
            #region Constructor

            public PageScrapingLogicFixture()
            {
                Document = new HtmlDocument();
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var examplePath = Path.Combine(path, "PageExamples", "11757.html");
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