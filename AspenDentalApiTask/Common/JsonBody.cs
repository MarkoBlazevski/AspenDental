using AspenDentalApiTask.Models.Requests;

namespace AspenDentalApiTask.Common
{
    public class JsonBody
    {
        public static CreateGitHubRepoRequest CreateGitHubRepoRequestBody(string name) => new CreateGitHubRepoRequest()
        {
            Name = name
        };

        public static UpdateGitHubRepoRequest UpdateGitHubRepoRequestBody(bool status, bool siteAdmin) => new UpdateGitHubRepoRequest()
        {
            Private = status,
            SiteAdmin = siteAdmin
        };

        public static TriangleRequest CreateTriangleRequestBody(List<float> sides)
        {
            if (sides.Count != 3)
            {
                throw new ArgumentException("Triangle requires exactly three sides.", nameof(sides));
            }

            return new TriangleRequest()
            {
                A = sides[0],
                B = sides[1],
                C = sides[2]
            };
        }
    }
}
