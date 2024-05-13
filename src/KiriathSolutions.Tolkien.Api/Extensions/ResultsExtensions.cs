using System.Net.Mime;
using System.Text;
using KiriathSolutions.Tolkien.Api.Auth;

namespace KiriathSolutions.Tolkien.Api.Extensions
{
    static class ResultsExtensions
    {
        public static IResult Authenticated(this IResultExtensions resultExtensions, AuthenticationResponse response)
        {
            ArgumentNullException.ThrowIfNull(resultExtensions);
            return new AuthenticationResult(response);
        }
    }

    class AuthenticationResult : IResult
    {
        private readonly AuthenticationResponse _authResponse;

        public AuthenticationResult(AuthenticationResponse authResponse)
        {
            _authResponse = authResponse;
        }

        public Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = MediaTypeNames.Text.Plain;
            httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_authResponse.Token);
            httpContext.Response.Headers.AccessControlAllowOrigin = "http://localhost:3000/";
            httpContext.Response.Cookies.Append("refresh-token", _authResponse.RefreshToken, new CookieOptions
            {
                Expires = _authResponse.RefreshTokenExpires,
                HttpOnly = true,
                Domain = "192.168.1.11",
                SameSite = SameSiteMode.Strict
            });

            return httpContext.Response.WriteAsync(_authResponse.Token);
        }
    }
}