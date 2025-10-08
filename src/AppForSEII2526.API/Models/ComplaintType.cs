namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class ComplaintType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
