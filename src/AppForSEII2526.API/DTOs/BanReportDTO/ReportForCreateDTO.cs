using AppForSEII2526.API.DTOs.UserDTOs;

namespace AppForSEII2526.API.DTOs.BanReportDTO
{
    public class BanReportForCreateDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 10,
            ErrorMessage = "Reason can be neither longer than 50 characters nor shorter than 10.")]
        public string Reason { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 20,
            ErrorMessage = "Description can be neither longer than 150 characters nor shorter than 20.")]
        public string DetailedDescription { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public IList<BanReportUserForCreateDTO> Users { get; set; }
    }
}
