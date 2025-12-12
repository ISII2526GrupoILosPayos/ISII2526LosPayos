using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.UserDTOs;
using AppForSEII2526.UT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace AppForSEII2526.UT.UsersController_test
{
    public class GetUsers_test : AppForSEII25264SqliteUT
    {
        public GetUsers_test()
        {
            var complaintTypes = new List<ComplaintType>()
            {
                new ComplaintType() { Id = 1, Name = "Queja" },
                new ComplaintType() { Id = 2, Name = "Denuncia" },
                new ComplaintType() { Id = 3, Name = "Mejora" }
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
                }
            };

            var complaints = new List<Complaint>()
            {
                new Complaint
                {
                    Id = 3,
                    Description = "Hola",
                    ComplaintDate = new DateTime(2025, 10, 26),
                    Processed = false,
                    Customer = users[0],          // u1
                    Type = complaintTypes[0]      // ct1
                },
                new Complaint
                {
                    Id = 6,
                    Description = "Adios",
                    ComplaintDate = new DateTime(2024, 10, 25),
                    Processed = false,
                    Customer = users[0],          // u1
                    Type = complaintTypes[1]      // ct2
                },
                new Complaint
                {
                    Id = 8,
                    Description = "hasta",
                    ComplaintDate = new DateTime(2000, 01, 01),
                    Processed = true,
                    Customer = users[0],          // u1
                    Type = complaintTypes[0]      // ct1
                },
                new Complaint
                {
                    Id = 9,
                    Description = "Luego",
                    ComplaintDate = new DateTime(2001, 01, 01),
                    Processed = false,
                    Customer = users[0],          // u1
                    Type = complaintTypes[0]      // ct1
                }
            };



            _context.ComplaintTypes.AddRange(complaintTypes);
            _context.ApplicationUsers.AddRange(users);
            _context.Complaints.AddRange(complaints);
            _context.SaveChanges();
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetUsers_null_surname_type()
        {
            var complaintTypeDTOs = new List<ComplaintTypeDTO>()
            {
                new ComplaintTypeDTO("Denuncia", 1),
                new ComplaintTypeDTO("Queja", 2)
            };


            var expectedResults = new List<UserForBanDTO> {
                new UserForBanDTO(
                 id: "1",
                 name: "Pau",
                 surname: "Femenia",
                 accountCreationDate: new DateTime(2004, 11, 12),
                 complaintTypes: complaintTypeDTOs
                 )
             };

            var mock = new Mock<ILogger<UsersController>>();
            ILogger<UsersController> logger = mock.Object;
            UsersController controller = new UsersController(_context, logger);

            var result = await controller.GetUsers(null, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var usersActual = Assert.IsType<List<UserForBanDTO>>(okResult.Value);
            Assert.Equal(expectedResults, usersActual);
        }

        public static IEnumerable<object[]> TestCasesFor_GetUsersForBan_OK()
        {
            var userDTOs = new List<UserForBanDTO>()
            {
                new UserForBanDTO(
                    id: "1",
                    name: "Pau",
                    surname: "Femenia",
                    accountCreationDate: new DateTime(2004, 11, 12),
                    complaintTypes: new List<ComplaintTypeDTO>()
                    {
                        new ComplaintTypeDTO("Denuncia", 1),
                        new ComplaintTypeDTO("Queja", 2)
                    }
                ),
                new UserForBanDTO(
                    id: "1",
                    name: "Pau",
                    surname: "Femenia",
                    accountCreationDate: new DateTime(2004, 11, 12),
                    complaintTypes: new List<ComplaintTypeDTO>()
                    {
                        new ComplaintTypeDTO("Queja", 2)
                    }
                ),
                new UserForBanDTO(
                    id: "1",
                    name: "Pau",
                    surname: "Femenia",
                    accountCreationDate: new DateTime(2004, 11, 12),
                    complaintTypes: new List<ComplaintTypeDTO>()
                    {
                        new ComplaintTypeDTO("Denuncia", 1)
                    }
                ),
                new UserForBanDTO(
                    id: "2",
                    name: "Kylian",
                    surname: "Mbappe",
                    accountCreationDate: new DateTime(2025, 10, 10),
                    complaintTypes: new List<ComplaintTypeDTO>()
                    {
                        new ComplaintTypeDTO("Denuncia", 1)
                    }
                )
            };
            var allTests = new List<object[]>
            {
                new object[] { "Femen", null, null, new List<UserForBanDTO> { userDTOs[0] } },
                new object[] { null, "Queja", null, new List<UserForBanDTO> { userDTOs[1] } },
                new object[] { "Femen", "Denuncia", null, new List<UserForBanDTO> { userDTOs[2] } },
                new object[] { null, null, null, new List<UserForBanDTO> { userDTOs[0] } },
                new object[] { "Mbapp", null, null, new List<UserForBanDTO>() },
                new object[] {null, null, new DateTime(2003, 11, 12), new List<UserForBanDTO>{ userDTOs[0] } },
                new object[] {null, null, new DateTime(2005, 11, 12), new List<UserForBanDTO>() }
            };  
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetUsersForBan_OK))]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetUsersForBan_OK_test(string? surname, string? type, DateTime? fechaCreacion, List<UserForBanDTO> expectedUsers)
        {
            // Arrange
            var controller = new UsersController(_context, null);

            // Act
            var result = await controller.GetUsers(surname, type, fechaCreacion);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var usersActual = Assert.IsType<List<UserForBanDTO>>(okResult.Value);

            Assert.Equal(expectedUsers, usersActual);
        }
    }
}
