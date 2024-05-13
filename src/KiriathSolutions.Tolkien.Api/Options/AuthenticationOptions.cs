namespace KiriathSolutions.Tolkien.Api.Options
{
    public class AuthenticationOptions : BaseOptions<AuthenticationOptions>
    {
        public string ApiKey { get; set; } = default!;
        public int TokenExpiryInMinutes { get; set; }
        public int RefreshTokenExpiryInDays { get; set; }

        protected override string Section => "Authentication";
    }
}