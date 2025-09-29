namespace AppForSEII2526.API.Models
{
    public class Brand
    {
        public Brand() { }

        public Brand(string name, string location)
        {
            Name = name;
            Location = location;
        }

        [Key] // PK por defecto
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string Location { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Brand other)
                return this.Id == other.Id;
            return false;
        }
    }
}
