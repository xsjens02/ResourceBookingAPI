﻿namespace ResourceBookingAPI.DTOs
{
    public class LoginResponseDto
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? InstitutionId { get; set; }
        public string? UserRole { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
