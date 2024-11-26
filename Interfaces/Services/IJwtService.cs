using ResourceBookingAPI.DTOs;

namespace ResourceBookingAPI.Interfaces.Services
{
    public interface IJwtService
    {
        Task<LoginResponseDto?> GenerateToken(LoginRequestDto request);
    }
}
