using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using KiriathSolutions.Tolkien.Api.Commands;
using KiriathSolutions.Tolkien.Api.Contexts;
using KiriathSolutions.Tolkien.Api.Entities;
using KiriathSolutions.Tolkien.Api.Options;
using KiriathSolutions.Tolkien.Api.Extensions;

namespace KiriathSolutions.Tolkien.Api.Auth
{
    public record AuthenticationResponse(string Token, string RefreshToken, DateTime RefreshTokenExpires);

    public enum RefreshTokenExpiryOption
    {
        Preserve,
        Reset
    }

    public enum RefreshTokenUpdateOption
    {
        Preserve,
        Update
    }

    public interface IUserManager
    {
        AuthenticationResponse? LogIn(LogInCommand command);
        AuthenticationResponse? RefreshToken(string refreshToken);
        AuthenticationResponse? ResetPassword(ResetPasswordCommand command, IJwtBearer authUser);
        AuthenticationResponse SignUp(SignUpCommand command);
        AuthenticationResponse? VerifyToken(string refreshToken);
    }

    public class TolkienUserManager : IUserManager
    {
        private readonly IDbContextFactory<AuthContext> _authContextFactory;
        private readonly AuthenticationOptions _authOptions;
        private readonly IPasswordHasher<IJwtBearer> _passwordHasher;

        public TolkienUserManager(IDbContextFactory<AuthContext> authContextFactory, IOptions<AuthenticationOptions> authOptions, IPasswordHasher<IJwtBearer> passwordHasher)
        {
            _authContextFactory = authContextFactory;
            _authOptions = authOptions.Value;
            _passwordHasher = passwordHasher;
        }

        public AuthenticationResponse? LogIn(LogInCommand command)
        {
            using var context = _authContextFactory.CreateDbContext();

            var individual = context
                .Individuals
                .FirstOrDefault((record) => record.EmailAddress == command.EmailAddress);

            if (individual == null)
                return null;

            var authenticated = _passwordHasher.VerifyHashedPassword(individual, individual.PasswordHash, command.Password);

            if (authenticated == PasswordVerificationResult.Failed)
                return null;

            return Authenticate(individual, RefreshTokenExpiryOption.Reset);
        }

        public AuthenticationResponse? RefreshToken(string refreshToken)
        {
            using var context = _authContextFactory.CreateDbContext();

            var individual = context
                .Individuals
                .FirstOrDefault((record) => record.RefreshToken == refreshToken);

            if (individual == null)
                return null;

            if (individual.RefreshTokenExpires < DateTime.UtcNow)
                return null;

            return Authenticate(individual);
        }

        public AuthenticationResponse? ResetPassword(ResetPasswordCommand command, IJwtBearer authUser)
        {
            using var context = _authContextFactory.CreateDbContext();

            var individual = context
                .Individuals
                .FirstOrDefault((record) => record.PublicId == authUser.PublicId);

            if (individual == null)
                return null;

            var authenticated = _passwordHasher.VerifyHashedPassword(individual, individual.PasswordHash, command.OldPassword);

            if (authenticated == PasswordVerificationResult.Failed)
                return null;

            var newPasswordHash = _passwordHasher.HashPassword(authUser, command.NewPassword);
            individual.PasswordHash = newPasswordHash;

            context.SaveChanges();

            return Authenticate(individual);
        }

        public AuthenticationResponse SignUp(SignUpCommand command)
        {
            var passwordHash = _passwordHasher.HashPassword(JwtBearer.Empty, command.Password);

            var individual = new Individual
            {
                PublicId = Guid.NewGuid(),
                EmailAddress = command.EmailAddress,
                FirstName = command.FirstName,
                LastName = command.LastName,
                PasswordHash = passwordHash,
                Created = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
            };

            using var context = _authContextFactory.CreateDbContext();

            context.Individuals.Add(individual);
            context.SaveChanges();

            return Authenticate(individual, RefreshTokenExpiryOption.Reset);
        }

        public AuthenticationResponse? VerifyToken(string refreshToken)
        {
            using var context = _authContextFactory.CreateDbContext();

            var individual = context
                .Individuals
                .FirstOrDefault((record) => record.RefreshToken == refreshToken);

            if (individual == null)
                return null;

            if (individual.RefreshTokenExpires < DateTime.UtcNow)
                return null;

            var token = CreateToken(individual);
            return new AuthenticationResponse(token, refreshToken, individual.RefreshTokenExpires ?? new DateTime());
        }


        private AuthenticationResponse Authenticate(Individual individual, RefreshTokenExpiryOption refreshTokenExpiryAction = RefreshTokenExpiryOption.Preserve)
        {
            var token = CreateToken(individual);
            var refreshToken = GenerateRefreshToken();

            var refreshTokenExpires = UpdateRefreshToken(individual.Id, refreshToken, refreshTokenExpiryAction);

            return new AuthenticationResponse(token, refreshToken, refreshTokenExpires);
        }

        private string CreateToken(IJwtBearer jwtBearer)
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

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private DateTime UpdateRefreshToken(int individualId, string refreshToken, RefreshTokenExpiryOption refreshTokenExpiryAction)
        {
            using var context = _authContextFactory.CreateDbContext();

            var individual = context
                .Individuals
                .FirstOrDefault((record) => record.Id == individualId);

            if (individual == null)
                throw new ArgumentException("IndividualId does not match a user in the database!");

            individual.RefreshToken = refreshToken;

            if (refreshTokenExpiryAction == RefreshTokenExpiryOption.Reset)
                individual.RefreshTokenExpires = DateTime.UtcNow.AddDays(_authOptions.RefreshTokenExpiryInDays);

            context.SaveChanges();

            return individual.RefreshTokenExpires ?? DateTime.UtcNow;
        }
    }
}