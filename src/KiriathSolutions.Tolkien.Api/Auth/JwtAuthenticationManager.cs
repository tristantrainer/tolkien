using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using KiriathSolutions.Tolkien.Api.Options;
using KiriathSolutions.Tolkien.Api.Contexts;

namespace KiriathSolutions.Tolkien.Api.Auth
{
    public interface IJwtAuthenticationManager
    {
        string? Authenticate(IJwtBearer jwtBearer);
    }

    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly AuthenticationOptions _authOptions;
        private readonly IDbContextFactory<AbacusContext> _dbContextFactory;

        public JwtAuthenticationManager(IDbContextFactory<AbacusContext> dbContextFactory, IOptions<AuthenticationOptions> authOptions)
        {
            _authOptions = authOptions.Value;
            _dbContextFactory = dbContextFactory;
        }

        public string? Authenticate(IJwtBearer jwtBearer)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_authOptions.ApiKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim("UserId", jwtBearer.PublicId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(_authOptions.TokenExpiryInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}