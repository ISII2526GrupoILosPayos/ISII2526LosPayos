namespace AppForSEII2526.API.Models
{
    public class BanReport
    {
        public BanReport()
        {
        }

        public BanReport(int id, string detailedDescription, DateTime endDate, string reason, DateTime startDate)
        {
            Id = id;
            DetailedDescription = detailedDescription;
            EndDate = endDate;
            Reason = reason;
            StartDate = startDate;
        }

        public int Id { get; set; }

        [StringLength(150, ErrorMessage = "Description can be neither longer than 150 characters nor shorter than 20.", MinimumLength = 20)]
        public string DetailedDescription { get; set; }

        public DateTime EndDate { get; set; }

        [StringLength(50, ErrorMessage = "Reason can be neither longer than 50 characters nor shorter than 10.", MinimumLength=10)]
        public string Reason { get; set; }

        public DateTime StartDate { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is BanReport report &&
                   Id == report.Id &&
                   DetailedDescription == report.DetailedDescription &&
                   EndDate == report.EndDate &&
                   Reason == report.Reason &&
                   StartDate == report.StartDate;
        }
    }
}
