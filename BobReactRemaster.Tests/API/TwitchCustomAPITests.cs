using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BobReactRemaster.APIs;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace BobReactRemaster.Tests.API
{
    class TwitchCustomAPITests
    {

        [Test]
        public async Task GetTwitchGameIDFromName_ValidGame_ReturnsPredefinedID()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"data\":[{\"id\":\"510218\",\"name\":\"Among Us\",\"box_art_url\":\"https://static-cdn.jtvnw.net/ttv-boxart/Among%20Us-{width}x{height}.jpg\"}]}")
                }).Verifiable();
            var actual = await TwitchCustomAPI.GetTwitchGameIDFromName("Among Us", new HttpClient(handlerMock.Object));
            ClassicAssert.AreEqual("510218",actual);
        }
        [Test]
        public async Task GetTwitchGameIDFromName_InValidGame_ReturnsPredefinedID()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"data\":[{\"id\":\"510218\",\"name\":\"Among Us\",\"box_art_url\":\"https://static-cdn.jtvnw.net/ttv-boxart/Among%20Us-{width}x{height}.jpg\"}]}")
                }).Verifiable();
            var actual = await TwitchCustomAPI.GetTwitchGameIDFromName("Among Us", new HttpClient(handlerMock.Object));
            ClassicAssert.AreNotEqual("5102138", actual);
        }
        [Test]
        public async Task TryToSetTwitchGame_InValidGameID_ReturnsFalse()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }).Verifiable();
            var actual = await TwitchCustomAPI.TryToSetTwitchGame("5","Among Us", new HttpClient(handlerMock.Object));
            ClassicAssert.AreEqual(false, actual);
        }
        [Test]
        public async Task TryToSetTwitchGame_ValidGameID_ReturnsTrue()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                }).Verifiable();
            var actual = await TwitchCustomAPI.TryToSetTwitchGame("5", "Among Us", new HttpClient(handlerMock.Object));
            ClassicAssert.AreEqual(true, actual);
        }
        [Test]
        public async Task TryToSetTwitchTitle_InValidTitle_ReturnsFalse()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest
                }).Verifiable();
            var actual = await TwitchCustomAPI.TryToSetTwitchTitle("5", null, new HttpClient(handlerMock.Object));
            ClassicAssert.AreEqual(false, actual);
        }
        [Test]
        public async Task TryToSetTwitchTitle_ValidTitle_ReturnsTrue()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                }).Verifiable();
            var actual = await TwitchCustomAPI.TryToSetTwitchTitle("5", "Among Us", new HttpClient(handlerMock.Object));
            ClassicAssert.AreEqual(true, actual);
        }
    }
}
