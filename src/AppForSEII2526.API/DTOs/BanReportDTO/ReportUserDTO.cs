namespace AppForSEII2526.API.DTOs.BanReportDTO
{
    public class ReportUserDTO
    {
        public ReportUserDTO(string name, string surname, string personalMessage)
        {
            Name = name;
            Surname = surname;
            PersonalMessage = personalMessage;
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PersonalMessage { get; set; }
    }

}
