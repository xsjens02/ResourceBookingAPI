namespace ResourceBookingAPI.DTOs
{
    public class BookingStatRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }
        public string? InstitutionId { get; set; }
    }
}
