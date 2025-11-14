using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.BanReportDTO;
using AppForSEII2526.API.Models;
using AppForSEII2526.UT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppForSEII2526.UT.BanReportController_test
{
    public class GetBanReport_test : AppForSEII25264SqliteUT
    {
        private readonly int _existingReportId = 100;

        public GetBanReport_test()
        {
            var complaintTypes = new List<ComplaintType>()
            {
                new ComplaintType { Id = 1, Name = "Queja" },
                new ComplaintType { Id = 2, Name = "Denuncia" }
            };

            var users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "1",
                    Name = "Pau",
                    Surname = "Femenia",
                    Address = "Campus",
                    AccountCreationDate = new DateTime(2004, 11, 12)
                },
                new ApplicationUser
                {
                    Id = "2",
                    Name = "Kylian",
                    Surname = "Mbappe",
                    Address = "La Finca",
                    AccountCreationDate = new DateTime(2025, 10, 10)
                },
                new ApplicationUser
                {
                    Id = "3",
                    Name = "Leo",
                    Surname = "Messi",
                    Address = "Miami",
                    AccountCreationDate = new DateTime(1987, 6, 24)
                }
            };

            var complaints = new List<Complaint>()
            {
                new Complaint
                {
                    Id = 1,
                    Description = "Queja usuario 1 - A",
                    ComplaintDate = new DateTime(2025, 10, 01),
                    Processed = false,
                    Customer = users[0],
                    Type = complaintTypes[0]
                },
                new Complaint
                {
                    Id = 2,
                    Description = "Queja usuario 1 - B",
                    ComplaintDate = new DateTime(2025, 10, 02),
                    Processed = false,
                    Customer = users[0],
                    Type = complaintTypes[1]
                },

                new Complaint
                {
                    Id = 3,
                    Description = "Queja usuario 2",
                    ComplaintDate = new DateTime(2025, 10, 03),
                    Processed = false,
                    Customer = users[1],
                    Type = complaintTypes[0]
                },

                new Complaint
                {
                    Id = 4,
                    Description = "Queja usuario 3",
                    ComplaintDate = new DateTime(2025, 10, 04),
                    Processed = false,
                    Customer = users[2],
                    Type = complaintTypes[1]
                }
            };

            var report = new BanReport
            {
                Id = _existingReportId,
                Reason = "Uso indebido de la plataforma",
                DetailedDescription = "Varias quejas relacionadas con el uso inadecuado de la plataforma por parte de los clientes.",
                StartDate = new DateTime(2025, 10, 01),
                EndDate = new DateTime(2025, 12, 31),
                ReportCustomers = new List<ReportCustomer>()
            };

            var reportCustomers = new List<ReportCustomer>()
            {
                new ReportCustomer
                {
                    BanReportId = report.Id,
                    BanReport = report,
                    CustomerId = users[0].Id,
                    Customer = users[0],
                    Message = "Comportamiento inadecuado en varias ocasiones.",
                    State = ReportState.Completed   
                },
                new ReportCustomer
                {
                    BanReportId = report.Id,
                    BanReport = report,
                    CustomerId = users[1].Id,
                    Customer = users[1],
                    Message = "Comentarios ofensivos hacia otros clientes.",
                    State = ReportState.Completed   
                }
            };

            report.ReportCustomers = reportCustomers;

            _context.ComplaintTypes.AddRange(complaintTypes);
            _context.ApplicationUsers.AddRange(users);
            _context.Complaints.AddRange(complaints);
            _context.BanReports.Add(report);
            _context.ReportCustomers.AddRange(reportCustomers);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetBanReport_ReportDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var mock = new Mock<ILogger<UsersController>>();
            ILogger<UsersController> logger = mock.Object;
            var controller = new BanReportController(_context, logger);

            int nonExistingId = _existingReportId + 999;

            // Act
            var result = await controller.GetBanReport(nonExistingId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var message = Assert.IsType<string>(notFoundResult.Value);
            Assert.Equal("Report not found", message);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetBanReport_ExistingReport_UpdatesComplaintsAndReturnsDTO()
        {
            // Arrange
            var controller = new BanReportController(_context, null);

            // Act
            var result = await controller.GetBanReport(_existingReportId);

            // Assert resultado HTTP
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ReportOperationResultDTO>(okResult.Value);

            // Recuperamos el informe desde la BD para verificar cambios
            var reportInDb = _context.BanReports
                .Where(r => r.Id == _existingReportId)
                .Select(r => new
                {
                    Report = r,
                    ReportCustomers = r.ReportCustomers,
                })
                .First();

            // 1) Comprobamos que todos los ReportCustomer del informe están en estado InProgress
            foreach (var rc in reportInDb.ReportCustomers)
            {
                Assert.Equal(ReportState.InProgress, rc.State);

                // Todas las quejas de los clientes incluidos en el informe deben estar Processed = true
                Assert.All(rc.Customer.Complaint, c => Assert.True(c.Processed));
            }

            // 2) Comprobamos que el usuario NO incluido en el informe mantiene sus quejas como estaban
            var userNotInReport = _context.ApplicationUsers
                .First(u => u.Id == "3");
            Assert.Contains(userNotInReport.Complaint, c => c.Processed == false);

            // 3) Comprobamos contenido del DTO devuelto
            //    Coinciden campos del informe
            Assert.Equal("Uso indebido de la plataforma", dto.Reason);
            Assert.Equal(
                "Varias quejas relacionadas con el uso inadecuado de la plataforma por parte de los clientes.",
                dto.Description);
            Assert.Equal(new DateTime(2025, 10, 01), dto.StartDate);
            Assert.Equal(new DateTime(2025, 12, 31), dto.EndDate);

            //    Coinciden usuarios devueltos (en el mismo orden en que se añadieron al informe)
            Assert.Equal(2, dto.Users.Count);

            Assert.Equal("Pau", dto.Users[0].Name);
            Assert.Equal("Femenia", dto.Users[0].Surname);
            Assert.Equal("Comportamiento inadecuado en varias ocasiones.", dto.Users[0].PersonalMessage);

            Assert.Equal("Kylian", dto.Users[1].Name);
            Assert.Equal("Mbappe", dto.Users[1].Surname);
            Assert.Equal("Comentarios ofensivos hacia otros clientes.", dto.Users[1].PersonalMessage);
        }
    }
}
