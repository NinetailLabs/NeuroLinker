using FluentAssertions;
using HttpMock;
using Moq;
using NeuroLinker.Helpers;
using NeuroLinker.Interfaces.Factories;
using NeuroLinker.Interfaces.Helpers;
using NeuroLinker.Interfaces.Workers;
using NeuroLinker.Models;
using NeuroLinker.Workers;
using NUnit.Framework;
using System.Net;
using System.Net.Http;

namespace NeuroLinker.Tests.Workers
{
    public class DataPushWorkerTests
    {
        #region Public Methods

        [Test]
        public void AddingNewAnimeWithErrorResponseWorksCorrectly()
        {
            // arrange
            const string url = "http://localhost:8654";
            const string path = "/api/animelist/add/0.xml";
            MalRouteBuilder.AdjustRoot(url);
            var httpMock = HttpMockRepository.At(url);
            httpMock.Stub(t => t.Post(path))
                .WithStatus(HttpStatusCode.BadRequest);

            var animeDummy = new Mock<AnimeUpdate>();
            const string user = "User";
            const string pass = "Pass";
            var fixture = new DataPushWorkerFixture();
            var userListDummy = new Mock<UserList>();

            fixture.ListRetrievalWorkerMock.Setup(t => t.RetrieveUserListAsync(user))
                .ReturnsAsync(userListDummy.Object);

            var sut = fixture.Instance;

            // act
            var result = sut.PushAnimeDetailsToMal(animeDummy.Object, user, pass).Result;

            // assert
            result.Success.Should().BeFalse();
            result.ResponseStatusCode.Should().Be(HttpStatusCode.BadRequest);
            httpMock.AssertWasCalled(x => x.Post(path));
        }

        [Test]
        public void AddingNewAnimeWorksCorrectly()
        {
            // arrange
            const string url = "http://localhost:8654";
            const string path = "/api/animelist/add/0.xml";
            MalRouteBuilder.AdjustRoot(url);
            var httpMock = HttpMockRepository.At(url);
            httpMock.Stub(t => t.Post(path))
                .OK();

            var animeDummy = new Mock<AnimeUpdate>();
            const string user = "User";
            const string pass = "Pass";
            var fixture = new DataPushWorkerFixture();
            var userListDummy = new Mock<UserList>();

            fixture.ListRetrievalWorkerMock.Setup(t => t.RetrieveUserListAsync(user))
                .ReturnsAsync(userListDummy.Object);

            var sut = fixture.Instance;

            // act
            var result = sut.PushAnimeDetailsToMal(animeDummy.Object, user, pass).Result;

            // assert
            result.Success.Should().BeTrue();
            result.ResponseStatusCode.Should().Be(HttpStatusCode.OK);
            httpMock.AssertWasCalled(x => x.Post(path));
        }

        [Test]
        public void UpdatingExistingAnimeWorksCorrectly()
        {
            // arrange
            const string url = "http://localhost:8654";
            const string path = "/api/animelist/update/0.xml";
            MalRouteBuilder.AdjustRoot(url);
            var httpMock = HttpMockRepository.At(url);
            httpMock.Stub(t => t.Post(path))
                .OK();

            var animeDummy = new Mock<AnimeUpdate>();
            var userAnimeDummy = new UserListAnime();
            const string user = "User";
            const string pass = "Pass";
            var fixture = new DataPushWorkerFixture();
            var userListDummy = new UserList();

            fixture.ListRetrievalWorkerMock.Setup(t => t.RetrieveUserListAsync(user))
                .ReturnsAsync(userListDummy);
            userAnimeDummy.SeriesId = 0;
            userListDummy.Anime.Add(userAnimeDummy);

            var sut = fixture.Instance;

            // act
            var result = sut.PushAnimeDetailsToMal(animeDummy.Object, user, pass).Result;

            // assert
            result.Success.Should().BeTrue();
            result.ResponseStatusCode.Should().Be(HttpStatusCode.OK);
            httpMock.AssertWasCalled(x => x.Post(path));
        }

        #endregion

        private class DataPushWorkerFixture
        {
            #region Constructor

            public DataPushWorkerFixture()
            {
                HttpClientFactoryMock
                    .Setup(t => t.GetHttpClient(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(new HttpClient());

                Instance = new DataPushWorker(HttpClientFactory, ListRetrievalWorker, XmlHelper);
            }

            #endregion

            #region Properties

            public IHttpClientFactory HttpClientFactory => HttpClientFactoryMock.Object;
            public Mock<IHttpClientFactory> HttpClientFactoryMock { get; } = new Mock<IHttpClientFactory>();

            public DataPushWorker Instance { get; }
            public IListRetrievalWorker ListRetrievalWorker => ListRetrievalWorkerMock.Object;
            public Mock<IListRetrievalWorker> ListRetrievalWorkerMock { get; } = new Mock<IListRetrievalWorker>();
            public IXmlHelper XmlHelper => XmlHelperMock.Object;
            public Mock<IXmlHelper> XmlHelperMock { get; } = new Mock<IXmlHelper>();

            #endregion
        }
    }
}