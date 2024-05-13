using KiriathSolutions.Tolkien.Api.Auth;
using KiriathSolutions.Tolkien.Api.Commands;
using Microsoft.AspNetCore.Mvc;

namespace KiriathSolutions.Tolkien.Api.EndpointDefinitions
{
    public class AdminEndpointDefinition : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            app
                .MapGet("/admin/ping", () => "Successful!")
                .RequireAuthorization();

            app
                .MapPost("/admin/reset-password", ResetPassword)
                .RequireAuthorization();

            app.MapPost("/admin/log-in", LogIn);
            app.MapPost("/admin/sign-up", SignUp);
            app.MapPost("/admin/refresh-token", RefreshToken);
            app.MapPost("/admin/verify", VerifyToken);
        }

        internal IResult SignUp([FromBody] SignUpCommand command, [Service] IUserManager userManager)
        {
            var authenticationResponse = userManager.SignUp(command);
            return Results.Json(authenticationResponse);
        }

        internal IResult LogIn([FromBody] LogInCommand command, [Service] IUserManager userManager)
        {
            var authenticationResponse = userManager.LogIn(command);
            return authenticationResponse == null ? Results.Unauthorized() : Results.Json(authenticationResponse);
        }

        internal IResult ResetPassword([FromBody] ResetPasswordCommand command, [Service] IJwtBearer authUser, [Service] IUserManager userManager)
        {
            var authenticationResponse = userManager.ResetPassword(command, authUser);
            return authenticationResponse == null ? Results.Unauthorized() : Results.Json(authenticationResponse);
        }

        internal IResult RefreshToken([FromBody] RefreshTokenCommand command, [Service] IUserManager userManager)
        {
            if (command.RefreshToken == null)
                return Results.Unauthorized();

            var authenticationResponse = userManager.RefreshToken(command.RefreshToken);
            return authenticationResponse == null ? Results.Unauthorized() : Results.Json(authenticationResponse);
        }

        internal IResult VerifyToken([FromBody] VerifyTokenCommand command, [Service] IUserManager userManager)
        {
            if (command.RefreshToken == null)
                return Results.Unauthorized();

            var authenticationResponse = userManager.VerifyToken(command.RefreshToken);
            return authenticationResponse == null ? Results.Unauthorized() : Results.Json(authenticationResponse);
        }

        public void DefineServices(IServiceCollection services)
        {
            services
                .AddSingleton<IUserManager, TolkienUserManager>()
                .AddScoped<IJwtBearer, TolkienUser>();
        }
    }
}