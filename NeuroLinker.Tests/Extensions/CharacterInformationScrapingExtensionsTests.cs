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
    public class CharacterInformationScrapingExtensionsTests
    {
        #region Public Methods

        [Test]
        public void ScrapingCharactersCorrectlyCleansCharacterNames()
        {
            // arrange
            var fixture = new CharacterInformationScrapingExtensionsFixture("35790CharacterInfo.html");
            var sut = fixture.Instance;

            // act
            sut.PopulateCharacterAndSeiyuuInformation(fixture.Document);

            // assert
            sut.CharacterInformation.Last().CharacterName.Should().Be("Village Doctor's Assistant");
        }

        [Test]
        public void ScrapingCharacterInformationReturnsTheCorrectNumberOfCharacters()
        {
            // arrange
            var fixture = new CharacterInformationScrapingExtensionsFixture();
            var sut = fixture.Instance;

            // act
            sut.PopulateCharacterAndSeiyuuInformation(fixture.Document);

            // assert
            sut.CharacterInformation.Count.Should().Be(48);
        }

        [Test]
        public void ScrapingCharacterReturnsTheCorrectSeiyuuInformation()
        {
            // arrange
            var fixture = new CharacterInformationScrapingExtensionsFixture();
            var sut = fixture.Instance;

            // act
            sut.PopulateCharacterAndSeiyuuInformation(fixture.Document);

            // assert
            var seiyuu = sut.CharacterInformation.First().Seiyuu.First();
            seiyuu.Id.Should().Be(732);
            seiyuu.Language.Should().Be("English");
            seiyuu.Name.Should().Be("Papenbrook, Bryce");
            seiyuu.PictureUrl.Should().Be("https://cdn.myanimelist.net/r/42x62/images/voiceactors/3/29853.jpg?s=c0dded60545804ead1a1300c69236bce");
            seiyuu.Url.Should().Be("https://myanimelist.net/people/732/Bryce_Papenbrook");
        }

        [Test]
        public void ScrapingCharactersReturnsTheCorrectInformation()
        {
            // arrange
            var fixture = new CharacterInformationScrapingExtensionsFixture();
            var sut = fixture.Instance;

            // act
            sut.PopulateCharacterAndSeiyuuInformation(fixture.Document);

            // assert
            var character = sut.CharacterInformation.First();
            character.Id.Should().Be(36765);
            character.CharacterName.Should().Be("Kirigaya, Kazuto");
            character.CharacterPicture.Should().Be("https://cdn.myanimelist.net/r/42x62/images/characters/7/204821.webp?s=dba48aec3bbfb248505160925c39a655");
            character.CharacterType.Should().Be("Main");
            character.CharacterUrl.Should().Be("https://myanimelist.net/character/36765/Kazuto_Kirigaya");
            character.Seiyuu.Count.Should().Be(8);
        }

        #endregion

        private class CharacterInformationScrapingExtensionsFixture
        {
            #region Constructor

            public CharacterInformationScrapingExtensionsFixture(string pageName = "11757CharacterInfo.html")
            {
                Document = new HtmlDocument();
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var examplePath = Path.Combine(path, "PageExamples", pageName);
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