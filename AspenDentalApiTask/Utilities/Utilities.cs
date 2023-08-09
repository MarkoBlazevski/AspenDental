namespace AspenDentalApiTask.Utilities
{
    public static class Utilities
    {
        public static string GenerateRandomRepoName()
        {
            string prefix = "AspenDental";
            string dateTimeSuffix = DateTime.Now.ToString("yyyyMMddHHmmss");
            string randomRepoName = $"{prefix}-{dateTimeSuffix}";

            return randomRepoName;
        }
    }
}
