
namespace AppForSEII2526.API.Models
{
    public class ReportCustomer
    {
        public ReportCustomer()
        {
        }
        [Key]
        public int BanReportId { get; set; }
        [Key]
        public int CustomerId { get; set; }
        public string Message { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReportCustomer customer &&
                   BanReportId == customer.BanReportId &&
                   CustomerId == customer.CustomerId &&
                   Message == customer.Message;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BanReportId, CustomerId, Message);
        }
    }
}
