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
            seiyuu.PictureUrl.Should().Be("https://myanimelist.cdn-dena.com/r/46x64/images/voiceactors/3/29853.jpg?s=e70f662e2eeb4bc8e1e7b7b96d46d1f1");
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
            character.CharacterPicture.Should().Be("https://myanimelist.cdn-dena.com/r/46x64/images/characters/7/204821.webp?s=7674741b4bd4173d6832e1bb5d0ac2b1");
            character.CharacterType.Should().Be("Main");
            character.CharacterUrl.Should().Be("https://myanimelist.net/character/36765/Kazuto_Kirigaya");
            character.Seiyuu.Count.Should().Be(7);
        }

        #endregion

        private class CharacterInformationScrapingExtensionsFixture
        {
            #region Constructor

            public CharacterInformationScrapingExtensionsFixture()
            {
                Document = new HtmlDocument();
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var examplePath = Path.Combine(path, "PageExamples", "11757CharacterInfo.html");
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