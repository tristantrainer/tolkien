using System.Security.Claims;
using System.Text;
using HotChocolate.Resolvers;
using KiriathSolutions.Tolkien.Api.Auth;
using KiriathSolutions.Tolkien.Api.Contexts;
using KiriathSolutions.Tolkien.Api.DataLoaders;
using KiriathSolutions.Tolkien.Api.EndpointDefinitions;
using KiriathSolutions.Tolkien.Api.Extensions;
using KiriathSolutions.Tolkien.Api.Models;
using KiriathSolutions.Tolkien.Api.Options;
using KiriathSolutions.Tolkien.Api.Repositories;
using KiriathSolutions.Tolkien.Api.Types;
using KiriathSolutions.Tolkien.Api.Types.ObjectTypes;
using KiriathSolutions.Tolkien.Api.Types.Scalars;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var MyAllowSpecificOrigins = "_mySpecificAllowOrigins";

var builder = WebApplication.CreateBuilder(args);

var apiKey = builder.Configuration.GetValue<string>("Authentication:ApiKey");

if (apiKey == null)
    throw new ArgumentException("ApiKey setting missing from appsettings");

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiKey));

builder
    .ConfigureOptions(typeof(AuthenticationOptions));

builder.Services
    .AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, (policy) =>
        {
            policy
                .WithOrigins("https://localhost:3000", "http://localhost:3000", "http://localhost:6006")
                .WithHeaders("x-wooster-request", "content-type", "authorization");
        });
});

builder
    .Services
    .AddPooledDbContextFactory<AbacusContext>(options => options.UseNpgsql("server=localhost;port=5432;database=financial;user id=postgres;password=Secure.Mars.Cup;"))
    .AddPooledDbContextFactory<AuthContext>(options => options.UseNpgsql("server=localhost;port=5432;database=financial;user id=postgres;password=Secure.Mars.Cup;"))
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(20)
            };
    });

builder
    .Services
    .AddTransient<IUnitOfWork, UnitOfWork>();

builder
    .Services
    .AddTransient<ITolkienUser, TolkienUser>();

builder
    .Services
    .AddAuthorization();

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .RegisterDbContext<AbacusContext>(DbContextKind.Pooled)
    .AddDataLoader<AccountBatchDataLoader>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<MutationType>()
    .AddType<QueryType>()
    .AddType<AccountType>()
    .AddType<AccountHistoryItemType>()
    .AddType<IndividualType>()
    .AddType<AccountIdType>()
    .AddType<AccountCategoryIdType>()
    .AddType<AccountHistoryIdType>()
    .AddType<CollectiveIdType>()
    .AddType<IndividualIdType>()
    .AddType<TransactionIdType>()
    .AddType<TransactionCategoryIdType>()
    // .TryAddTypeInterceptor<ValidationTypeInterceptor>()
    .AddResolvers(typeof(BaseResolvers<>));

builder.Services
    .AddEndpointDefinitions(typeof(AdminEndpointDefinition));

builder.Services
    .AddSingleton<IJwtAuthenticationManager, JwtAuthenticationManager>();

builder.Services
    .AddSingleton<IPasswordHasher<IJwtBearer>, PasswordHasher<IJwtBearer>>();

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.MapEndpointDefinitions();

app.Run();
