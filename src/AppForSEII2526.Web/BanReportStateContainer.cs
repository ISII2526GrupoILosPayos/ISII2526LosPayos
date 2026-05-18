using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class BanUserProcessStateContainer
    {
        public BanReportForCreateDTO BanReport { get; private set; } = new BanReportForCreateDTO()
        {
            Reason = "",
            DetailedDescription = "",
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(1),
            Users = new List<BanReportUserForCreateDTO>()
        };

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();


        public void AddUserToBanReport(BanReportUserForCreateDTO user)
        {
            if (!BanReport.Users.Any(u => u.CustomerId == user.CustomerId))
                BanReport.Users.Add(new BanReportUserForCreateDTO()
                {
                    CustomerId = user.CustomerId,
                    PersonalMessage = user.PersonalMessage
                });

            //NotifyStateChanged();
        }

        public void RemoveUserFromBanReport(BanReportUserForCreateDTO user)
        {
            BanReport.Users.Remove(user);

            //NotifyStateChanged();
        }

        public void ClearBanUserCart()
        {
            BanReport.Users.Clear();

            //NotifyStateChanged();
        }

        public void ReportProcessed()
        {
            BanReport = new BanReportForCreateDTO()
            {
                Reason = "",
                DetailedDescription = "",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                Users = new List<BanReportUserForCreateDTO>()
            };

            //NotifyStateChanged();
        }
    }
}

