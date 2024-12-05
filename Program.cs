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
            var builder = WebApplication.CreateBuilder(args);

            //Configure appsettings
            builder.Configuration.AddEnvironmentVariables();
            builder.Configuration["MongoDbSettings:ConnectionString"] = Environment.GetEnvironmentVariable("MONGO_CON_STR");
            builder.Configuration["MongoDbSettings:DatabaseName"] = Environment.GetEnvironmentVariable("MONGO_DB_NAME");
            builder.Configuration["GitHubCdnConfig:PAT"] = Environment.GetEnvironmentVariable("GH_CDN_PAT");
            builder.Configuration["GitHubCdnConfig:ApiURL"] = Environment.GetEnvironmentVariable("GH_API_URL");
            builder.Configuration["GitHubCdnConfig:PagesURL"] = Environment.GetEnvironmentVariable("GH_PAGES_URL");
            builder.Configuration["JwtConfig:Key"] = Environment.GetEnvironmentVariable("JWT_KEY");

            builder.Services.AddHttpClient();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Configure MongoDB
            builder.Services.Configure<MongoConfig>(
                builder.Configuration.GetSection("MongoDbSettings"));
            builder.Services.AddSingleton<IMongoService, MongoService>();

            //Configure GithubCDN
            builder.Services.Configure<GitHubCdnConfig>(
                builder.Configuration.GetSection("GitHubCdnConfig"));
            builder.Services.AddSingleton<ICdnService, GitHubCdnService>();

            //Configure JWT Authentication
            builder.Services.Configure<JwtConfig>(
                builder.Configuration.GetSection("JwtConfig"));
            builder.Services.AddSingleton<IJwtService, JwtService>();

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

            //Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()       
                          .AllowAnyMethod()       
                          .AllowAnyHeader();      
                });
            });

            //Configure repositories
            builder.Services.AddSingleton<IBookingRepos, BookingMongoRepos>();
            builder.Services.AddSingleton<ICrudRepos<ErrorReport, string>, ErrorReportMongoRepos>();
            builder.Services.AddSingleton<IInstitutionRepos, InstitutionMongoRepos>();
            builder.Services.AddSingleton<ICrudRepos<Resource, string>, ResourceMongoRepos>();
            builder.Services.AddSingleton<ICrudRepos<User, string>, UserMongoRepos>();
            builder.Services.AddSingleton<ILoginRepos, UserMongoRepos>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Configure middleware
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
