namespace BlogApi
{
    public static class Configuration
    {
        public static string JwtKey { get; set; } = "3d65ae3b-fd0e-447e-9079-a2a1ca3eb70a";

        public static string ApiKeyName = "api_key";
        public static string ApiKey = "curso_api_!@#%%%$#!!@#%(98123";

        public static SmtpConfiguration Smtp = new();

        public class SmtpConfiguration
        {
            public string Host { get; set; }
            public int Port { get; set; } = 25;
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
