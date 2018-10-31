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
    public class CharacterPageScrapingExtensionsTests
    {
        #region Public Methods

        [Test]
        public void AnimeOgraphyAnimeDetailsAreRetrievedCorrectly()
        {
            // arrange
            var fixture = new CharacterPageScrapingExtensionFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveAnimeography(fixture.Document);

            // assert
            var sao = sut.Animeography.First();
            sao.Id.Should().Be(11757);
            sao.ImageUrl.Should()
                .Be(
                    "https://myanimelist.cdn-dena.com/r/23x32/images/anime/11/39717.webp?s=9a77a024d5915d53e853920dd437c37b");
            sao.Name.Should().Be("Sword Art Online");
            sao.Url.Should().Be("https://myanimelist.net/anime/11757/Sword_Art_Online");
        }

        [Test]
        public void CharacterBioIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new CharacterPageScrapingExtensionFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveBiography(fixture.Document);

            // assert
            sut.Biography.Should().Be(Biography.HtmlDecode());
        }

        [Test]
        public void CharacterImageIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new CharacterPageScrapingExtensionFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveCharacterImage(fixture.Document);

            // assert
            sut.ImageUrl.Should().Be("https://myanimelist.cdn-dena.com/images/characters/15/262053.jpg");
        }

        [Test]
        public void CharacterNameIsCorrectlyRetrieved()
        {
            // arrange
            var fixture = new CharacterPageScrapingExtensionFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveCharacterName(fixture.Document);

            // assert
            sut.Name.Should().Be("Asuna Yuuki (結城 明日奈 / アスナ)");
        }

        [Test]
        public void CorrectNumberOfAnimeIsRetrievedForAnimeography()
        {
            // arrange
            var fixture = new CharacterPageScrapingExtensionFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveAnimeography(fixture.Document);

            // assert
            sut.Animeography.Count.Should().Be(10);
        }

        [Test]
        public void CorrectNumberOfMangaIsRetrievedForMangaography()
        {
            // arrange
            var fixture = new CharacterPageScrapingExtensionFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveMangaograhy(fixture.Document);

            // assert
            sut.Mangaography.Count.Should().Be(16);
        }

        [Test]
        public void CorrectNumberOfSeiyuuIsRetrieved()
        {
            // arrange
            var fixture = new CharacterPageScrapingExtensionFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveSeiyuu(fixture.Document);

            // assert
            sut.Seiyuu.Count.Should().Be(10);
        }

        [Test]
        public void FavoritesAreCorrectlyRetrieved()
        {
            // arrange
            var fixture = new CharacterPageScrapingExtensionFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveFavoriteCount(fixture.Document);

            // assert
            sut.FavoriteCount.Should().Be(18885);
        }

        [Test]
        public void MangaographyDetailsAreRetrievedCorrectly()
        {
            // arrange
            var fixture = new CharacterPageScrapingExtensionFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveMangaograhy(fixture.Document);

            // assert
            var sao = sut.Mangaography.First();
            sao.Id.Should().Be(21479);
            sao.ImageUrl.Should()
                .Be(
                    "https://myanimelist.cdn-dena.com/r/23x32/images/manga/1/34697.webp?s=96f6203aa05d6a2742fc7ea02107f9af");
            sao.Name.Should().Be("Sword Art Online");
            sao.Url.Should().Be("https://myanimelist.net/manga/21479/Sword_Art_Online");
        }

        [Test]
        public void SeiyuuInformationIsRetrievedCorrectly()
        {
            // arrange
            var fixture = new CharacterPageScrapingExtensionFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveSeiyuu(fixture.Document);

            // assert
            var seiyuu = sut.Seiyuu.First();
            seiyuu.Id.Should().Be(890);
            seiyuu.Name.Should().Be("Tomatsu, Haruka");
            seiyuu.Url.Should().Be("https://myanimelist.net/people/890/Haruka_Tomatsu");
            seiyuu.PictureUrl.Should().Be("https://myanimelist.cdn-dena.com/images/voiceactors/3/44285v.jpg");
        }

        #endregion

        #region Variables

        private const string Biography =
            "Birthdate: September 30, 2007 Age: 15 (Beginning of Aincrad arc); 17 (End of Aincrad arc, Fairy Dance arc); 18 (Phantom Bullet arc, Alicization Arc) Height: 168 cm Weight: 55 kg Three Sizes: 82-60-83 Weapons of choice: Wind Fleuret (1st Floor), Lambent Light (Forged by Lisbeth)  Asuna is a friend of Kirito and is a sub-leader of the guild Knights of the Blood (KoB), a medium-sized guild of about thirty players, also called the strongest guild in Aincrad. Being one of the few girls that are in SAO, and even more so that she&#039;s extremely pretty, she receives many invitations and proposals. She is a skilled player earning the title &quot;Flash&quot; for her extraordinary sword skill that is lightning fast. Her game alias is the same as her real world name.                  <SPOILER>She is married to Kirito in SAO. They decide to live a peaceful life and to buy a house on the lower part of Aincrad and take a break from the guild Knights of the Blood Oath. But later they need to return to the front lines, abandoning their peaceful life. After the SAO incident, all the players that survived should have awoken, but Asuna is still trapped within her NerveGear. She is actually abducted by her betrothed, Sugou Naboyuki, who transfers her to Alfheim Online (ALO). In ALO, She is known as Queen Titania, yet she is only a character who is trapped within a cage that is programmed to be a unbreakable object. She was rescued by Kirito who became a Game Master by accessing Heathcliff&#039;s account, and defeated Sugou. Kirito logged Asuna off from ALO and rejoins her in the hospital after he ended his fight with Sugou in the real world. </SPOILER>";

        #endregion

        private class CharacterPageScrapingExtensionFixture
        {
            #region Constructor

            public CharacterPageScrapingExtensionFixture()
            {
                Document = new HtmlDocument();
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var examplePath = Path.Combine(path, "PageExamples", "36828.html");
                Document.Load(examplePath);
                //using (var htmlFile = File.Open(examplePath, FileMode.Open))
                //{
                //    Document.Load();
                //}

                Instance = new Character();
            }

            #endregion

            #region Properties

            public HtmlDocument Document { get; }

            public Character Instance { get; }

            #endregion
        }
    }
}