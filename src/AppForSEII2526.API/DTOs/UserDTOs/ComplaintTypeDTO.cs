namespace AppForSEII2526.API.DTOs.UserDTOs
{
    public class ComplaintTypeDTO
    {
        public ComplaintTypeDTO(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public string Name { get; set; }
        public int Count { get; set; }
    }
}
