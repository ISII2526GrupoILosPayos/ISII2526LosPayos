
namespace AppForSEII2526.API.DTOs.UserDTOs
{
    public class UserForBanDTO
    {
        public UserForBanDTO(string id, string name, string surname, DateTime accountCreationDate, IList<ComplaintTypeDTO> complaintTypes)
        {
            Id = id;
            Name = name;
            Surname = surname;
            AccountCreationDate = accountCreationDate;
            ComplaintTypes = complaintTypes;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime AccountCreationDate { get; set; }
        public IList<ComplaintTypeDTO> ComplaintTypes { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is UserForBanDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Surname == dTO.Surname &&
                   AccountCreationDate == dTO.AccountCreationDate &&
                   EqualityComparer<IList<ComplaintTypeDTO>>.Default.Equals(ComplaintTypes, dTO.ComplaintTypes);
        }
    }
}
