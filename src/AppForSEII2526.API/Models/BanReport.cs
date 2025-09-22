namespace AppForSEII2526.API.Models
{
    public class BanReport
    {
        public int Id { get; set; }

        [StringLength(150, ErrorMessage = "Description can be neither longer than 150 characters nor shorter than 20.", MinimumLength = 20)]
        public string DetailedDescription { get; set; }

        public DateTime EndDate { get; set; }

        [StringLength(50, ErrorMessage = "Reason can be neither longer than 50 characters nor shorter than 10.", MinimumLength=10)]
        public string Reason { get; set; }

        public DateTime StartDate { get; set; }
    }
}
