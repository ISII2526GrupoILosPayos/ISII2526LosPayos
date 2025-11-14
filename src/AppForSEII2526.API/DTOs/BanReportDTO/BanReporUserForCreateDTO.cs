namespace AppForSEII2526.API.DTOs.UserDTOs
{
    public class BanReportUserForCreateDTO
    {
        [Required]
        public string CustomerId { get; set; }
        public string? PersonalMessage { get; set; }
    }
}
