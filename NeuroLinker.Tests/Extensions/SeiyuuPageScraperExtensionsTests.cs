﻿using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using HtmlAgilityPack;
using NeuroLinker.Extensions;
using NeuroLinker.Models;
using NUnit.Framework;

namespace NeuroLinker.Tests.Extensions
{
    public class SeiyuuPageScraperExtensionsTests
    {
        #region Public Methods

        [Test]
        public void RetrievingRolesRetrievesTheCorrectNumberOfRoles()
        {
            // arrange
            var fixture = new SeiyuuPageScraperExtensionsFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveRoles(fixture.Document);

            // assert
            sut.Roles.Count.Should().Be(430);
        }

        [Test]
        public void RoleInformationIsCorrectlyPopulated()
        {
            // arrange
            var fixture = new SeiyuuPageScraperExtensionsFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveRoles(fixture.Document);

            // assert
            var ichinose = sut.Roles.Where(x => x.CharacterId == 4602).ToList();
            ichinose.Count.Should().Be(3);

            var clannad = ichinose.First();
            clannad.AnimeId.Should().Be(2167);
            clannad.AnimePicUrl.Should()
                .Be(
                    "https://myanimelist.cdn-dena.com/r/46x64/images/anime/1804/95033.webp?s=76a99a68702cf1785cc8f227745d333b");
            clannad.AnimeTitle.Should().Be("Clannad");
            clannad.CharacterName.Should().Be("Ichinose, Kotomi");
            clannad.CharacterPic.Should()
                .Be(
                    "https://myanimelist.cdn-dena.com/r/46x64/images/characters/15/33494.webp?s=1f9e52d93a5b3ef542e5d48e74afe787");
            clannad.CharacterUrl.Should().Be("https://myanimelist.net/character/4602/Kotomi_Ichinose");
            clannad.RoleType.Should().Be("Main");
        }

        [Test]
        public void SeiyuuAdditionalInformationIsCorrectlyRetrieved()
        {
            // arrange
            var fixture = new SeiyuuPageScraperExtensionsFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveAdditionalInformation(fixture.Document);

            // assert
            sut.More.Count.Should().Be(7);
            sut.More.First().Should().Be("Birthplace: Kanazawa, Ishikawa Prefecture, Japan");
        }

        [Test]
        public void SeiyuuBirthdayIsCorrectlyRetrieved()
        {
            // arrange
            var fixture = new SeiyuuPageScraperExtensionsFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveBirthday(fixture.Document);

            // assert
            sut.BirthDay.Should().Be(new DateTime(1980, 02, 06));
        }

        [Test]
        public void SeiyuuFamilyNameIsCorrectlyRetrieved()
        {
            // arrange
            var fixture = new SeiyuuPageScraperExtensionsFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveFamilyName(fixture.Document);

            // assert
            sut.FamilyName.Should().Be("能登");
        }

        [Test]
        public void SeiyuuGivenNameIsCorrectlyRetrieved()
        {
            // arrange
            var fixture = new SeiyuuPageScraperExtensionsFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveGivenName(fixture.Document);

            // assert
            sut.GivenName.Should().Be("麻美子");
        }

        [Test]
        public void SeiyuuImageIsCorrectlyRetrieved()
        {
            // arrange
            var fixture = new SeiyuuPageScraperExtensionsFixture();

            var sut = fixture.Instance;

            // act
            sut.RetrieveSeiyuuImage(fixture.Document);

            // assert
            sut.ImageUrl.Should().Be("https://myanimelist.cdn-dena.com/images/voiceactors/2/44269.jpg");
        }

        [Test]
        public void SeiyuuNameIsCorrectlyRetrieved()
        {
            // arrange
            var fixture = new SeiyuuPageScraperExtensionsFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveName(fixture.Document);

            // assert
            sut.Name.Should().Be("Mamiko Noto");
        }

        [Test]
        public void SeiyuuWebsiteIsCorectlyRetrieved()
        {
            // arrange
            var fixture = new SeiyuuPageScraperExtensionsFixture();
            var sut = fixture.Instance;

            // act
            sut.RetrieveWebsite(fixture.Document);

            // assert
            sut.Website.Should().BeEmpty();
        }

        #endregion

        private class SeiyuuPageScraperExtensionsFixture
        {
            #region Constructor

            public SeiyuuPageScraperExtensionsFixture(int pageNumber = 40)
            {
                Document = new HtmlDocument();
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var examplePath = Path.Combine(path, "PageExamples", $"{pageNumber}.html");
                using (var htmlFile = File.Open(examplePath, FileMode.Open))
                {
                    Document.Load(htmlFile);
                }

                Instance = new Seiyuu();
            }

            #endregion

            #region Properties

            public HtmlDocument Document { get; }

            public Seiyuu Instance { get; }

            #endregion
        }
    }
}