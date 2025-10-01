
namespace AppForSEII2526.API.Models
{
    public class ReportCustomer
    {
        public ReportCustomer()
        {
        }

        public int BanReportId { get; set; }
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
