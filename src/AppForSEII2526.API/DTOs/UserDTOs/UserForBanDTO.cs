
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
            if (obj is not UserForBanDTO other)
                return false;

            if (Id != other.Id ||
                Name != other.Name ||
                Surname != other.Surname ||
                AccountCreationDate != other.AccountCreationDate)
                return false;

            if (ComplaintTypes.Count != other.ComplaintTypes.Count)
                return false;

            for (int i = 0; i < ComplaintTypes.Count; i++)
            {
                if (!ComplaintTypes[i].Equals(other.ComplaintTypes[i]))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = HashCode.Combine(Id, Name, Surname, AccountCreationDate);

            foreach (var ct in ComplaintTypes)
                hash = HashCode.Combine(hash, ct.GetHashCode());

            return hash;
        }

    }
}
