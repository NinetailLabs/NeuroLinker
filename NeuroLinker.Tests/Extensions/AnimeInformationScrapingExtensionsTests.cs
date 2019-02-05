using FluentAssertions;
using HtmlAgilityPack;
using NeuroLinker.Extensions;
using NeuroLinker.Models;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

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
            sut.YearOnlyDate.Should().BeFalse();
        }

        [Test]
        public void YearOnlyAirDateIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new PageScrapingLogicFixture("1190.html");
            var sut = fixture.Instance;

            // act
            sut.RetrieveAirDates(fixture.Document);

            // assert
            sut.StartDate.Should().Be(new DateTime(2003, 01, 01));
            sut.EndDate.Should().Be(new DateTime(2003, 01, 01));
            sut.YearOnlyDate.Should().BeTrue();
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

        [TestCase("11757.html", SaoSynopsis)]
        [TestCase("34973.html", LoveLiveSynopsis)]
        [TestCase("32901.html", EromangaSynopsis)]
        [TestCase("3467.html", NogizakaSynopsis)]
        public void AnimeSynopsisIsRetrievedCorrectly(string page, string synopsis)
        {
            // arrange
            var fixture = new PageScrapingLogicFixture(page);
            var sut = fixture.Instance;

            // act
            sut.RetrieveSynopsis(fixture.Document);

            // assert
            sut.Synopsis.Should().Be(synopsis.Replace("\r\n", Environment.NewLine));
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

        [TestCase("11757.html", "TV")]
        [TestCase("23365.html", "Music")]
        public void AnimeTypeIsRetrievedCorrectly(string page, string expectedType)
        {
            // arrange
            var fixture = new PageScrapingLogicFixture(page);
            var sut = fixture.Instance;

            // act
            sut.RetrieveType(fixture.Document);

            // assert
            sut.Type.Should().Be(expectedType);
        }

        [TestCase("11757.html", "PG-13 - Teens 13 or older")]
        [TestCase("34599.html", "R - 17+ (violence & profanity)")]
        public void ClassificationIsRetrievedCorrectly(string page, string expectedValue)
        {
            // arrange
            var fixture = new PageScrapingLogicFixture(page);
            var sut = fixture.Instance;

            // act
            sut.RetrieveRating(fixture.Document);

            // assert
            sut.Classification.Should().Be(expectedValue);
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
            sut.FavoriteCount.Should().Be(50373);
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
            sut.MemberCount.Should().Be(1318097);
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
            sut.Rank.Should().Be(1283);
        }

        [TestCase("11757.html")]
        [TestCase("11757_old.html")]
        public void RelatedInformationIsParsedCorrectly(string page)
        {
            // arrange
            var fixture = new PageScrapingLogicFixture(page);
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

        // Issue-13 - Some anime would crash with a null reference exception
        [TestCase("83.html")]
        [TestCase("84.html")]
        [TestCase("116.html")]
        public void RelatedInformationShouldNotCauseANullReferenceCrash(string page)
        {
            // arrange
            var fixture = new PageScrapingLogicFixture(page);
            var sut = fixture.Instance;

            var act = new Action(() => sut.RetrieveRelatedAnime(fixture.Document));

            // act
            // assert
            act.ShouldNotThrow<NullReferenceException>();
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
            sut.MemberScore.Should().Be(7.63);
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

        private const string SaoSynopsis =
            "In the year 2022, virtual reality has progressed by leaps and bounds, and a massive online role-playing game called Sword Art Online (SAO) is launched. With the aid of \"NerveGear\" technology, players can control their avatars within the game using nothing but their own thoughts.\r\n                                                    \r\n                                                    Kazuto Kirigaya, nicknamed \"Kirito,\" is among the lucky few enthusiasts who get their hands on the first shipment of the game. He logs in to find himself, with ten-thousand others, in the scenic and elaborate world of Aincrad, one full of fantastic medieval weapons and gruesome monsters. However, in a cruel turn of events, the players soon realize they cannot log out; the game's creator has trapped them in his new world until they complete all one hundred levels of the game.\r\n                                                    \r\n                                                    In order to escape Aincrad, Kirito will now have to interact and cooperate with his fellow players. Some are allies, while others are foes, like Asuna Yuuki, who commands the leading group attempting to escape from the ruthless game. To make matters worse, Sword Art Online is not all fun and games: if they die in Aincrad, they die in real life. Kirito must adapt to his new reality, fight for his survival, and hopefully break free from his virtual hell.\r\n                                                    \r\n                                                    [Written by MAL Rewrite]";

        private const string LoveLiveSynopsis =
            "The second season of the Love Live! spinoff series, Love Live! Sunshine!!";

        private const string EromangaSynopsis =
            "One year ago, Sagiri Izumi became step-siblings with Masamune Izumi. But the sudden death of their parents tears their new family apart, resulting in Sagiri becoming a shut-in which cut her off from her brother and society.\r\n                                                    \r\n                                                    While caring for what's left of his family, Masamune earns a living as a published light novel author with one small problem: he's never actually met his acclaimed illustrator, Eromanga-sensei, infamous for drawing the most lewd erotica. Through an embarrassing chain of events, he learns that his very own little sister was his partner the whole time!\r\n                                                    \r\n                                                    As new characters and challenges appear, Masamune and Sagiri must now face the light novel industry together. Eromanga-Sensei follows the development of their relationship and their struggle to become successful; and as Sagiri slowly grows out of her shell, just how long will she be able to hide her true persona from the rest of the world?\r\n                                                    \r\n                                                    [Written by MAL Rewrite]";

        private const string NogizakaSynopsis =
            "Hakujo Academy is a private high school with a student body filled with elite students. At the top of this list is the beautiful Haruka Nogizaka, an intelligent and wealthy girl who comes from a prestigious family, and is by far the most idolized girl at school. Her popularity is so great that her classmates occasionally give her French nicknames to further express their infatuation.\r\n                                                    \r\n                                                    Yuuto Ayase is a timid boy who has the unbelievable luck of sitting in class with Haruka every day. Like many others, he admires her from a distance, never even dreaming of approaching her. But with a sudden twist of fate, Yuuto's random visit to the school library brings Haruka's darkest secret to light, one which could potentially destroy her current reputation of elegance. As it turns out, the school's most beloved princess is actually a huge otaku. This revelation marks the beginning of a beautiful friendship between these two classmates.";

        #endregion

        private class PageScrapingLogicFixture
        {
            #region Constructor

            public PageScrapingLogicFixture(string htmlPage = "11757.html")
            {
                Document = new HtmlDocument();
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var examplePath = Path.Combine(path, "PageExamples", htmlPage);
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