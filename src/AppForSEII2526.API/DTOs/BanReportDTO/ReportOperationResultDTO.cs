namespace AppForSEII2526.API.DTOs.BanReportDTO
{
    public class ReportOperationResultDTO
    {
        public ReportOperationResultDTO(string reason, string description, DateTime startDate, DateTime endDate, IList<ReportUserDTO> users)
        {
            Reason = reason;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Users = users;
        }

        public string Reason { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IList<ReportUserDTO> Users { get; set; }
    }

}
