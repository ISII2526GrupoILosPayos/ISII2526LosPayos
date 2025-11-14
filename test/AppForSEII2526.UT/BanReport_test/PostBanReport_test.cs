using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.BanReportDTO;
using AppForSEII2526.API.DTOs.UserDTOs;
using AppForSEII2526.API.Models;
using AppForSEII2526.UT;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.UT.BanReportController_test
{
    public class CreateBanReport_test : AppForSEII25264SqliteUT
    {
        private readonly List<ApplicationUser> _users;

        public CreateBanReport_test()
        {
            _users = new List<ApplicationUser>()
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
                }
            };

            _context.ApplicationUsers.AddRange(_users);
            _context.SaveChanges();
        }

        // TEST 1 — Lista de usuarios vacía
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateBanReport_EmptyUserList_ReturnsBadRequest()
        {
            var dto = new BanReportForCreateDTO
            {
                Reason = "Bad behaviour",
                DetailedDescription = "Something happened",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(10),
                Users = new List<BanReportUserForCreateDTO>() // vacío
            };

            var controller = new BanReportController(_context, null);

            var result = await controller.CreateBanReport(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<ValidationProblemDetails>(badRequest.Value);
            Assert.True(errors.Errors.ContainsKey("Users"));
        }

        // TEST 2 — Fechas inválidas
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateBanReport_InvalidDates_ReturnsBadRequest()
        {
            var dto = new BanReportForCreateDTO
            {
                Reason = "Some reason",
                DetailedDescription = "Some detail",
                StartDate = new DateTime(2025, 12, 1),
                EndDate = new DateTime(2025, 11, 1), // inválido
                Users = new List<BanReportUserForCreateDTO>()
                {
                    new BanReportUserForCreateDTO { CustomerId = "1" }
                }
            };

            var controller = new BanReportController(_context, null);

            var result = await controller.CreateBanReport(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            Assert.True(errors.Errors.ContainsKey("Dates"));
        }

        // TEST 3 — Usuario no existente
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateBanReport_UserDoesNotExist_ReturnsBadRequest()
        {
            var dto = new BanReportForCreateDTO
            {
                Reason = "Reason",
                DetailedDescription = "A proper detailed description",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(5),
                Users = new List<BanReportUserForCreateDTO>()
                {
                    new BanReportUserForCreateDTO { CustomerId = "999" } // no existe
                }
            };

            var controller = new BanReportController(_context, null);

            var result = await controller.CreateBanReport(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            Assert.True(errors.Errors.ContainsKey("Users"));
        }

        // TEST 4 — Creación correcta
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateBanReport_ValidData_CreatesReportAndReturnsDTO()
        {
            var dto = new BanReportForCreateDTO
            {
                Reason = "Uso indebido",
                DetailedDescription = "Comportamiento inaceptable repetido.",
                StartDate = new DateTime(2025, 10, 1),
                EndDate = new DateTime(2025, 12, 31),
                Users = new List<BanReportUserForCreateDTO>()
                {
                    new BanReportUserForCreateDTO
                    {
                        CustomerId = "1",
                        PersonalMessage = "Mensaje usuario 1"
                    },
                    new BanReportUserForCreateDTO
                    {
                        CustomerId = "2",
                        PersonalMessage = "Mensaje usuario 2"
                    }
                }
            };

            var controller = new BanReportController(_context, null);

            var result = await controller.CreateBanReport(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var returnDto = Assert.IsType<ReportOperationResultDTO>(created.Value);

            Assert.Equal(dto.Reason, returnDto.Reason);
            Assert.Equal(dto.DetailedDescription, returnDto.Description);
            Assert.Equal(dto.StartDate, returnDto.StartDate);
            Assert.Equal(dto.EndDate, returnDto.EndDate);

            Assert.Equal(2, returnDto.Users.Count);

            Assert.Equal("Pau", returnDto.Users[0].Name);
            Assert.Equal("Femenia", returnDto.Users[0].Surname);
            Assert.Equal("Mensaje usuario 1", returnDto.Users[0].PersonalMessage);

            Assert.Equal("Kylian", returnDto.Users[1].Name);
            Assert.Equal("Mbappe", returnDto.Users[1].Surname);
            Assert.Equal("Mensaje usuario 2", returnDto.Users[1].PersonalMessage);
        }
    }
}

