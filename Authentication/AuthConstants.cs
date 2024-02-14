namespace api_with_auth.Authentication
{
    public static class AuthConstans
    {

        public const string ApiKeySectionName = "Authentication:ApiKey";
        public const string ApiKeyHeaderName = "X-Api-Key";
        public const string messageForInvalidKey = "Invalid ApiKey!";
        public const string messageForMissingKey = "ApiKey missing!";
    }
}