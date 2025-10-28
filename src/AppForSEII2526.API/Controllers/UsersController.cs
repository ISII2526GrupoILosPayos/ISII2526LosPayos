using AppForSEII2526.API.DTOs.UserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ApplicationDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<UserForBanDTO>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsers(string? surname, string? complaintType)
        {
            IList<UserForBanDTO> usersDTO = await _context.ApplicationUsers
                .Where(m =>
                ((m.Surname.Contains(surname)) || (surname == null)) &&
                m.Complaint.Any(n =>
                !n.Processed &&
                (complaintType == null || n.Type.Name.Contains(complaintType))))
                .Select(n => new UserForBanDTO(
                    n.Id,
                    n.Name,
                    n.Surname,
                    n.AccountCreationDate,
                    n.Complaint
                    .Where(o =>
                        !o.Processed &&
                        (complaintType == null || o.Type.Name.Contains(complaintType)))
                .GroupBy(q => q.Type.Name)
                .Select(g => new ComplaintTypeDTO(
                    g.Key,
                    g.Count()))
                .ToList()))
                .ToListAsync();
            return Ok(usersDTO);
        }
    }
}
