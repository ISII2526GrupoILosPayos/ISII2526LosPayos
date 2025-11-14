using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.UserDTOs;
using AppForSEII2526.UT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

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

        var result = await controller.GetUsers(null, null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var usersActual = Assert.IsType<List<UserForBanDTO>>(okResult.Value);
        Assert.Equal(expectedResults, usersActual);
    }

    public static IEnumerable<object[]> TestCasesFor_GetUsers_OK()
    {
        return new List<object[]>
        {
            // surname = null, type = null  → 1 usuario (Pau)
            new object[] { null, null, 1 },

            // surname = "Femenia" → Pau → 1
            new object[] { "Femenia", null, 1 },

            // surname = "Mbappe" → Mbappé no tiene complaints → 0
            new object[] { "Mbappe", null, 0 },

            // complaintType = "Queja" → Pau tiene 2 quejas (pero 1 usuario) → 1
            new object[] { null, "Queja", 1 },

            // complaintType = "Denuncia" → Pau tiene 1 → 1
            new object[] { null, "Denuncia", 1 },

            // complaintType = "Mejora" → no hay ninguna sin procesar → 0
            new object[] { null, "Mejora", 0 },

            // surname = "Femenia", type = "Queja" → 1
            new object[] { "Femenia", "Queja", 1 },

            // surname = "Femenia", type = "XXXXX" → 0
            new object[] { "Femenia", "XXXXX", 0 },

            // surname = "" (vacío) → Contains("") = true → Pau tiene complaints válidas → 1
            new object[] { "", null, 1 }
        };
    }

    // ---------------------------------------------------------------------

    [Theory]
    [MemberData(nameof(TestCasesFor_GetUsers_OK))]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task GetUsers_OK_test(string? surname, string? complaintType, int expectedCount)
    {
        var controller = new UsersController(_context, null);

        var result = await controller.GetUsers(surname, complaintType);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actual = Assert.IsType<List<UserForBanDTO>>(okResult.Value);

        Assert.Equal(expectedCount, actual.Count);
    }
}
