namespace AppForSEII2526.API.DTOs.BanReportDTO
{
    public class BanReportResultDTO
    {

        public BanReportResultDTO(string name, string surname, string message, string reason, string description, DateTime startDate, DateTime endDate){
            this.name = name;
            this.surname = surname;
            this.message = message;
            this.reason = reason;
            this.description = description;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public string name { get; set; }
        public string surname { get; set; }
        public string message { get; set; }
        public string reason { get; set; }
        public string description { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}