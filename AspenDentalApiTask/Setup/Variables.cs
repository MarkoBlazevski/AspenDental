using System.Text.Json;

namespace AspenDentalApiTask.Setup
{
    public class Variables
    {
        public string ReportPath { get; set; }

        // BaseUrl
        public string GitHubUrl { get; set; }
        public string TriangleApiUrl { get; set; }

        // Headers
        public string BearerToken { get; set; }
        public string AcceptHeader { get; set; }

        // Endpoints
        public string CreateRepoEndpoint { get; set; }
        public string GetRepoEndpoint { get; set; }
        public string UpdateRepoEndpoint { get; set; }
        public string DeleteRepoEndpoint { get; set; }
        public string LoginName { get; set; }
        public string TriangleEndpoint { get; set; }

        //Triangle sides
        public List<float> Equilateral { get; set; }
        public List<float> Isosceles { get; set; }
        public List<float> Scalene { get; set; }
        public List<float> InvalidTriangleSideName { get; set; }
        public List<float> ZeroAsSideValue { get; set; }
        public List<float> NegativeNumAsSideValue { get; set; }
        public List<float> FloatNumAsSideValue { get; set; }
        public List<float> BaseIsTooBig { get; set; }
        public string UnexistingRepoName { get; set; }

        private static string PathToDirectory()
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return path;
        }

        public static Variables InitValues()
        {
            string filename = @"appsettings.json";

            string configFile = Path.Combine(PathToDirectory(), filename);
            string jsonString = File.ReadAllText(configFile);
            Variables variables = JsonSerializer.Deserialize<Variables>(jsonString);

            return variables;
        }
    }
}
