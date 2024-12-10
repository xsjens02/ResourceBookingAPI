using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ResourceBookingAPI.Configuration;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Interfaces.Services;
using ResourceBookingAPI.Models;
using ResourceBookingAPI.Repositories.Mongo;
using ResourceBookingAPI.Services;
using System.Text;

namespace ResourceBookingAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create and configure the web application builder
            var builder = WebApplication.CreateBuilder(args);

            // Configure appsettings from environment variables
            // These variables are used for MongoDB, GitHub CDN, and JWT configuration
            builder.Configuration.AddEnvironmentVariables();
            builder.Configuration["MongoDbSettings:ConnectionString"] = Environment.GetEnvironmentVariable("MONGO_CON_STR");
            builder.Configuration["MongoDbSettings:DatabaseName"] = Environment.GetEnvironmentVariable("MONGO_DB_NAME");
            builder.Configuration["GitHubCdnConfig:PAT"] = Environment.GetEnvironmentVariable("GH_CDN_PAT");
            builder.Configuration["GitHubCdnConfig:ApiURL"] = Environment.GetEnvironmentVariable("GH_API_URL");
            builder.Configuration["GitHubCdnConfig:PagesURL"] = Environment.GetEnvironmentVariable("GH_PAGES_URL");
            builder.Configuration["JwtConfig:Key"] = Environment.GetEnvironmentVariable("JWT_KEY");

            // Add required services for the application
            builder.Services.AddHttpClient(); 
            builder.Services.AddControllers(); 
            builder.Services.AddEndpointsApiExplorer(); 
            builder.Services.AddSwaggerGen(); 

            // Configure MongoDB settings from the configuration file
            builder.Services.Configure<MongoConfig>(
                builder.Configuration.GetSection("MongoDbSettings"));
            builder.Services.AddSingleton<IMongoService, MongoService>(); // Register MongoService as singleton

            // Configure GitHub CDN settings and register the service
            builder.Services.Configure<GitHubCdnConfig>(
                builder.Configuration.GetSection("GitHubCdnConfig"));
            builder.Services.AddSingleton<ICdnService, GitHubCdnService>(); // Register GitHubCdnService for file uploads and deletions

            // Configure JWT Authentication settings
            builder.Services.Configure<JwtConfig>(
                builder.Configuration.GetSection("JwtConfig"));
            builder.Services.AddSingleton<IJwtService, JwtService>(); // Register JwtService for token generation

            // Set up JWT Authentication 
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["JwtConfig:Issuer"], 
                    ValidAudience = builder.Configuration["JwtConfig:Audience"], 
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)), 
                    ValidateIssuer = true, 
                    ValidateAudience = true, 
                    ValidateLifetime = true, 
                    ValidateIssuerSigningKey = true 
                };
            });

            // Configure CORS 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin() 
                          .AllowAnyMethod() 
                          .AllowAnyHeader(); 
                });
            });

            // Register repositories for accessing the MongoDB collections
            builder.Services.AddSingleton<IBookingRepos, BookingMongoRepos>();
            builder.Services.AddSingleton<IErrorReportRepos, ErrorReportMongoRepos>();
            builder.Services.AddSingleton<IInstitutionRepos, InstitutionMongoRepos>();
            builder.Services.AddSingleton<ICrudRepos<Resource, string>, ResourceMongoRepos>();
            builder.Services.AddSingleton<ICrudRepos<User, string>, UserMongoRepos>();
            builder.Services.AddSingleton<ILoginRepos, UserMongoRepos>();

            // Build the application
            var app = builder.Build();

            // Configure middleware 
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(); 
                app.UseSwaggerUI(); 
            }

            // Set up middleware for HTTPS redirection, CORS, authentication, and authorization
            app.UseHttpsRedirection(); 
            app.UseCors("AllowAll"); // Enable CORS with the "AllowAll" policy
            app.UseAuthentication(); 
            app.UseAuthorization(); 
            app.MapControllers(); 

            app.Run();
        }
    }
}