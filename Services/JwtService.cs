using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ResourceBookingAPI.Configuration;
using ResourceBookingAPI.DTOs;
using ResourceBookingAPI.Interfaces.Repositories;
using ResourceBookingAPI.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ResourceBookingAPI.Services
{
    /// <summary>
    /// Service for handling JWT token generation and validation.
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly ILoginRepos _userRepo;

        /// <summary>
        /// Initializes the JwtService with configuration and user repository.
        /// </summary>
        /// <param name="jwtConfig">The JWT configuration settings.</param>
        /// <param name="userRepo">The repository for user data and validation.</param>
        public JwtService(IOptions<JwtConfig> jwtConfig, ILoginRepos userRepo)
        {
            _jwtConfig = jwtConfig.Value;
            _userRepo = userRepo;
        }

        /// <summary>
        /// Generates a JWT token for a user based on provided credentials.
        /// </summary>
        /// <param name="request">The login request containing username and password.</param>
        /// <returns>A LoginResponseDto containing user details and access token if successful, otherwise null.</returns>
        public async Task<LoginResponseDto?> GenerateToken(LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var user = await _userRepo.FetchUser(request.Username);
            if (user == null || !_userRepo.ValidatePassword(user, request.Password))
                return null;

            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(_jwtConfig.TokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, user.Role ?? "User")
                }),
                Expires = tokenExpiryTimeStamp,
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Key)),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new LoginResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                InstitutionId = user.InstitutionId,
                UserRole = user.Role,
                AccessToken = accessToken,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            };
        }
    }
}