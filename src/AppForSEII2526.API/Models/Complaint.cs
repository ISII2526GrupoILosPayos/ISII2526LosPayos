namespace AppForSEII2526.API.Models
{
    public class Complaint
    {
        public Complaint() { }

        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime ComplaintDate { get; set; }
        public bool Processed { get; set; }
        public ApplicationUser Customer { get; set; }
        public ComplaintType Type { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Complaint complaint &&
                   Id == complaint.Id &&
                   Description == complaint.Description &&
                   ComplaintDate == complaint.ComplaintDate &&
                   Processed == complaint.Processed;
        }
    }
}
