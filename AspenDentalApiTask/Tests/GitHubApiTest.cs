using AspenDentalApiTask.Clients;
using AspenDentalApiTask.Common;
using AspenDentalApiTask.Models.Responses;
using AspenDentalApiTask.Services;
using AspenDentalApiTask.Setup;
using BoDi;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;

namespace AspenDentalApiTask.Tests
{
    [TestFixture]
    public class GitHubApiTest : BaseTest
    {
        private readonly GitHubApiClient _gitHubApiClient;
        private readonly GitHubService _gitHubService;
        private readonly string _repoName;
        private readonly IObjectContainer _objectContainer;
        private readonly List<string> createdRepos = new List<string>();

        public GitHubApiTest()
        {
            _objectContainer = new ObjectContainer();
            // Initialize Variables from appsettings.json
            _variables = Variables.InitValues();
            _objectContainer.RegisterInstanceAs(_variables, typeof(Variables));
            _gitHubApiClient = new GitHubApiClient(_objectContainer);
            _gitHubService = new GitHubService(_gitHubApiClient, _objectContainer);
            _repoName = Utilities.Utilities.GenerateRandomRepoName();
        }

        [Test]
        [Parallelizable]
        public async Task CreateRepo_WithValidName_ReturnsRepoResponse()
        {
            var body = JsonBody.CreateGitHubRepoRequestBody(_repoName);
            var response = await _gitHubService.CreateRepoAsync(body);
            createdRepos.Add(_repoName);

            Assert.That(response.ResponseBody, Is.Not.Null);
            Assert.That(response.ResponseHttp, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(response.ResponseBody.Name, Is.EqualTo(_repoName));
        }

        [Test]
        [Parallelizable]
        public async Task CreateRepo_With_InvalidName_UnprocessableEntity()
        {
            var repoName = string.Empty; // Invalid name
            var body = JsonBody.CreateGitHubRepoRequestBody(repoName);

            try
            {
                var response = await _gitHubService.CreateRepoAsync(body);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                var errorResponse = JsonConvert.DeserializeObject<GitHubErrorResponse>(ex.Message);
                Assert.That(errorResponse.Errors[1].Message, Is.EqualTo(GitHubErrorMessages.NameIsTooShort));
                return;
            }

            Assert.Fail("Expected HttpStatus code and message was not thrown.");
        }

        [Test]
        public async Task CreateRepo_With_ExistingName_UnprocessableEntity()
        {
            var body = JsonBody.CreateGitHubRepoRequestBody(_repoName);

            // First Creation - Should be successful
            var firstResponse = await _gitHubService.CreateRepoAsync(body);
            createdRepos.Add(_repoName);

            Assert.That(firstResponse.ResponseBody, Is.Not.Null);
            Assert.That(firstResponse.ResponseBody.Name, Is.EqualTo(_repoName));

            // Second Creation - Should throw an exception
            try
            {
                var secondResponse = await _gitHubService.CreateRepoAsync(body);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                var errorResponse = JsonConvert.DeserializeObject<GitHubErrorResponse>(ex.Message);
                Assert.That(errorResponse.Errors[0].Message, Is.EqualTo(GitHubErrorMessages.NameAlreadyExists));
                return;
            }

            Assert.Fail("Expected HttpStatus code and message was not thrown.");
        }

        [Test]
        [Parallelizable]
        public async Task CreateRepo_WithInvalidJson_BadRequest()
        {
            // Create a JSON string with an extra comma at the end
            var body = JsonBody.CreateGitHubRepoRequestBody(_repoName) + ",";

            try
            {
                var response = await _gitHubService.CreateRepoAsync(body);
                Assert.Fail("Exception was not thrown");
            }
            catch (HttpRequestException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(ex.Message, Does.Contain(GitHubErrorMessages.ProblemsParsingJSON));
            }
        }

        [Test]
        public async Task Get_Repo()
        {
            var body = JsonBody.CreateGitHubRepoRequestBody(_repoName);
            var createResponse = await _gitHubService.CreateRepoAsync(body);
            createdRepos.Add(_repoName);

            // Verify repo is created correctly
            Assert.That(createResponse.ResponseBody, Is.Not.Null);
            Assert.That(createResponse.ResponseBody.Name, Is.EqualTo(_repoName));

            // Step 2: Retrieve the repo
            var getResponse = await _gitHubService.GetRepoAsync(createResponse.ResponseBody.Owners.Login, createResponse.ResponseBody.Name);

            // Verify retrieved repo details are correct
            Assert.That(getResponse.ResponseBody, Is.Not.Null);
            Assert.That(getResponse.ResponseHttp, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getResponse.ResponseBody.Name, Is.EqualTo(_repoName));
            Assert.That(getResponse.ResponseBody.Owners.Login, Is.EqualTo(createResponse.ResponseBody.Owners.Login));
        }

        [Test]
        [Parallelizable]
        public async Task GetRepo_NonExistingRepo_ReturnsNotFound()
        {
            var ownerLogin = _variables.LoginName; // Replace this with your actual login or a variable

            try
            {
                var response = await _gitHubService.GetRepoAsync(ownerLogin, _repoName);
            }
            catch (HttpRequestException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                // Assert that the exception message contains "Not Found"
                Assert.That(ex.Message, Is.EqualTo(GitHubErrorMessages.NotFound));
            }
        }

        [Test]
        public async Task UpdateRepo_WithValidNameAndBody_ReturnsRepoResponse()
        {
            //Arrange
            var createRepoBody = JsonBody.CreateGitHubRepoRequestBody(_repoName);

            // First create a repo
            var createRepoResponse = await _gitHubService.CreateRepoAsync(createRepoBody);
            createdRepos.Add(_repoName);

            var body = JsonBody.UpdateGitHubRepoRequestBody(true, true);
            // Then update the repo
            var updateRepoResponse = await _gitHubService.UpdateRepoAsync(createRepoResponse.ResponseBody.Owners.Login,
                                                                          createRepoResponse.ResponseBody.Name, body);

            Assert.That(updateRepoResponse.ResponseBody, Is.Not.Null);
            Assert.That(updateRepoResponse.ResponseBody.Private, Is.EqualTo(true));
        }

        [Test]
        public async Task UpdateRepo_WithValidNameAnd_WithoutBody_ReturnsBadRequest()
        {
            //Arrange
            var createRepoBody = JsonBody.CreateGitHubRepoRequestBody(_repoName);

            // First create a repo
            var createRepoResponse = await _gitHubService.CreateRepoAsync(createRepoBody);
            createdRepos.Add(_repoName);

            // Then update the repo with no body
            var updateRepoResponse = await _gitHubService.UpdateRepoAsync(createRepoResponse.ResponseBody.Owners.Login,
                                                                          createRepoResponse.ResponseBody.Name, string.Empty);

            Assert.That(updateRepoResponse.ResponseHttp, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task DeleteRepo_WithValidName_NoContentResponse()
        {
            var createRepoBody = JsonBody.CreateGitHubRepoRequestBody(_repoName);

            // First create a repo
            var createRepoResponse = await _gitHubService.CreateRepoAsync(createRepoBody);

            // Then delete the repo
            await _gitHubService.DeleteRepoAsync(createRepoResponse.ResponseBody.Owners.Login,
                                                 createRepoResponse.ResponseBody.Name);

            // Try to get the deleted repo, expect a not found error
            try
            {
                await _gitHubService.GetRepoAsync(createRepoResponse.ResponseBody.Owners.Login,
                                                  createRepoResponse.ResponseBody.Name);
            }
            catch (Exception e)
            {
                Assert.That(e.Message, Is.EqualTo(GitHubErrorMessages.NotFound));
            }
        }

        [Test]
        public async Task DeleteRepo_UnexistingRepo_NoContentResponse()
        {

            // Then delete the repo that do not exist
            var response = await _gitHubService.DeleteRepoAsync(_variables.LoginName, _variables.UnexistingRepoName);

            // Try to get the deleted repo, expect a not found error
            Assert.That(response.ResponseHttp, Is.EqualTo(HttpStatusCode.NotFound));

        }

        [TearDown]
        public async Task GitHubTearDown()
        {
            // Iterates over all the created repos during the test.
            foreach (var repoName in createdRepos)
            {
                // Deletes the repo given its name. Ensures that the testing environment is cleaned up.
                await _gitHubService.DeleteRepoAsync(_variables.LoginName, repoName);
            }

            // Clears the list of created repos, preparing for the next test.
            createdRepos.Clear();
        }
    }
}
