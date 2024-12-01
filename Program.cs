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

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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

            //Configure React
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.AllowAnyOrigin()       
                          .AllowAnyMethod()       
                          .AllowAnyHeader();      
                });
            });

            //Configure MongoDB
            builder.Services.Configure<MongoConfig>(
                builder.Configuration.GetSection("MongoDbSettings"));
            builder.Services.AddSingleton<IMongoService, MongoService>();

            //Register repositories
            builder.Services.AddSingleton<ICrudRepos<Booking, string>, BookingMongoRepos>();
            builder.Services.AddSingleton<ICrudRepos<ErrorReport, string>, ErrorReportMongoRepos>();
            builder.Services.AddSingleton<IInstitutionRepos<Institution, string>, InstitutionMongoRepos>();
            builder.Services.AddSingleton<ICrudRepos<Resource, string>, ResourceMongoRepos>();
            builder.Services.AddSingleton<ICrudRepos<User, string>, UserMongoRepos>();
            builder.Services.AddSingleton<ILoginRepos, LoginMongoRepos>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Configure middleware
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
