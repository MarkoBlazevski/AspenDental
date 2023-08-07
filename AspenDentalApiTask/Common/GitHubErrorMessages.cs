namespace AspenDentalApiTask.Common
{
    public static class GitHubErrorMessages
    {
        public const string NameIsTooShort = "name is too short (minimum is 1 character)";
        public const string NameAlreadyExists = "name already exists on this account";
        public const string NotFound = "Not Found";
        public const string ProblemsParsingJSON = "Problems parsing JSON";
        public const string UnexpectedStatusCode = "Unexpected status code: NotFound";
    }
}
