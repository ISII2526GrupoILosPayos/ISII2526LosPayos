namespace AppForSEII2526.API.DTOs.UserDTOs
{
    public class UserForBanDTO
    {
        public UserForBanDTO(string id, string name)
        {
            Id = id;
            Name = name;
        }

        //public UserForBanDTO(string id, string name, string surname, DateTime accountCreationDate, int numberOfComplaints, IList<ComplaintType> complaintTypes, bool complaintProcessed)
        //{
        //    Id = id;
        //    Name = name;
        //    Surname = surname;
        //    AccountCreationDate = accountCreationDate;
        //    NumberOfComplaints = numberOfComplaints;
        //    ComplaintTypes = complaintTypes;
        //    ComplaintProcessed = complaintProcessed;
        //}

        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        //public DateTime AccountCreationDate { get; set; }
        //public int NumberOfComplaints { get; set; }
        //public IList<ComplaintType> ComplaintTypes { get; set; }
        //public bool ComplaintProcessed { get; set; }

    }
}
