using AspenDentalApiTask.Clients;
using AspenDentalApiTask.Common;
using AspenDentalApiTask.Services;
using AspenDentalApiTask.Setup;
using BoDi;
using NUnit.Framework;
using System.Net;

namespace AspenDentalApiTask.Tests
{
    public class TriangleApiTest : BaseTest
    {
        private readonly IObjectContainer _objectContainer;
        private readonly TriangleService _triangleService;
        private readonly TriangleApiClient _triangleApiClient;

        public TriangleApiTest()
        {
            _objectContainer = new ObjectContainer();
            _variables = new Variables();
            // Initialize Variables from appsettings.json
            _variables = Variables.InitValues();
            _objectContainer.RegisterInstanceAs(_variables);
            _triangleApiClient = new TriangleApiClient(_objectContainer);
            _triangleService = new TriangleService(_triangleApiClient, _objectContainer);
        }

        [Test]
        [Parallelizable]
        public async Task GetTriangleTypeAsync_ValidTriangleRequest_ShouldReturn_TriangleTypeEquilateral()
        {
            var body = JsonBody.CreateTriangleRequestBody(_variables.Equilateral);

            var response = await _triangleService.GetTriangleTypeAsync(body);

            Assert.That(response.ResponseHttp, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.ResponseBody, Is.Not.Null);
            Assert.That(response.ResponseBody.Type, Is.Not.Empty);
            Assert.That(response.ResponseBody.Type, Is.EqualTo(TriangleMessages.TriangleIsEquilateral));
        }

        [Test]
        [Parallelizable]
        public async Task GetTriangleType_ValidTriangleRequest_ShouldReturn_TriangleTypeIsoscalene()
        {
            var body = JsonBody.CreateTriangleRequestBody(_variables.Isosceles);

            var response = await _triangleService.GetTriangleTypeAsync(body);

            Assert.That(response.ResponseHttp, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.ResponseBody, Is.Not.Null);
            Assert.That(response.ResponseBody.Type, Is.Not.Empty);
            Assert.That(response.ResponseBody.Type, Is.EqualTo(TriangleMessages.TriangleIsIsosceles));
        }

        [Test]
        [Parallelizable]
        public async Task GetTriangleType_ValidTriangleRequest_ShouldReturn_TriangleTypeScalene()
        {
            var body = JsonBody.CreateTriangleRequestBody(_variables.Scalene);

            var response = await _triangleService.GetTriangleTypeAsync(body);

            Assert.That(response.ResponseHttp, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.ResponseBody, Is.Not.Null);
            Assert.That(response.ResponseBody.Type, Is.Not.Empty);
            Assert.That(response.ResponseBody.Type, Is.EqualTo(TriangleMessages.TriangleIsScalene));
        }

        [Test]
        [Parallelizable]
        public async Task GetTriangleType_InvalidTriangleRequest_InvalidTriangleSideName_ShouldReturn_BadRequest()
        {
            var body = new
                { 
                    S = 5,
                    B = 5,
                    C = 5
                };

            var (ResponseHttp, ResponseBody) = await _triangleService.GetTriangleTypeAsync(body);

            Assert.That(ResponseHttp, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(ResponseBody, Is.Not.Null);
            Assert.That(ResponseBody.Status, Is.EqualTo(400));
            Assert.That(ResponseBody.Type, Is.Not.Empty);
            Assert.That(ResponseBody.Error.A[0], Is.EqualTo(TriangleMessages.AIsRequired));
        }

        [Test]
        [Parallelizable]
        public async Task GetTriangleType_InvalidTriangleRequest_WithZero_ShouldReturn_BadRequest()
        {
            var body = JsonBody.CreateTriangleRequestBody(_variables.ZeroAsSideValue);

            var (ResponseHttp, ResponseBody) = await _triangleService.GetTriangleTypeAsync(body);

            Assert.That(ResponseHttp, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(ResponseBody.Message, Is.Not.Empty);
            Assert.That(ResponseBody.Message, Is.EqualTo(TriangleMessages.NotATriangle));
        }

        [Test]
        [Parallelizable]
        public async Task GetTriangleType_InvalidTriangleRequest_WithNegativeNum_ShouldReturn_BadRequest()
        {
            var body = JsonBody.CreateTriangleRequestBody(_variables.NegativeNumAsSideValue);

            var (ResponseHttp, ResponseBody) = await _triangleService.GetTriangleTypeAsync(body);

            Assert.That(ResponseHttp, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(ResponseBody.Message, Is.Not.Empty);
            Assert.That(ResponseBody.Message, Is.EqualTo(TriangleMessages.NotATriangle));
        }

        [Test]
        [Parallelizable]
        public async Task GetTriangleType_InvalidTriangleRequest_WithFloatNum_ShouldReturn_BadRequest()
        {
            var body = JsonBody.CreateTriangleRequestBody(_variables.FloatNumAsSideValue);

            var (ResponseHttp, ResponseBody) = await _triangleService.GetTriangleTypeAsync(body);

            Assert.That(ResponseHttp, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(ResponseBody.Message, Is.Not.Empty);
            Assert.That(ResponseBody.Message, Is.EqualTo(TriangleMessages.UnableToFormValidTriangle));
        }

        [Test]
        [Parallelizable]
        public async Task GetTriangleType_InvalidTriangleRequest_BaseIsTooBig_ShouldReturn_BadRequest()
        {
            var body = JsonBody.CreateTriangleRequestBody(_variables.BaseIsTooBig);

            var (ResponseHttp, ResponseBody) = await _triangleService.GetTriangleTypeAsync(body);

            Assert.That(ResponseHttp, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(ResponseBody.Message, Is.Not.Empty);
            Assert.That(ResponseBody.Message, Is.EqualTo(TriangleMessages.NotATriangle));
        }

        [Test]
        [Parallelizable]
        public async Task GetTriangleType_InvalidTriangleRequest_ExtraTriangleSide_ShouldReturn_BadRequest()
        {
            var body = new
            {
                A = 5,
                B = 5,
                C = 5,
                D = 5
            };

            var (ResponseHttp, _) = await _triangleService.GetTriangleTypeAsync(body);

            // Check if the response HTTP status code is in the 200s or 300s range
            Assert.That((int)ResponseHttp, Is.GreaterThanOrEqualTo(400), "Unexpected success response");
        }

        [Test]
        [Parallelizable]
        public async Task GetTriangleType_InvalidTriangleRequest_InputFieldIsRequired_ShouldReturn_BadRequest()
        {
            var (ResponseHttp, ResponseBody) = await _triangleService.GetTriangleTypeAsync(_variables.LoginName);

            Assert.That(ResponseHttp, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(ResponseBody.Error.Input[0], Is.EqualTo(TriangleMessages.InputFieldIsRequired));
        }
    }
}
