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
        [ProducesResponseType(typeof(IList<UserForBanDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsers(string? surname, string? complaintType, DateTime? creationDate)
        {
            var users = await _context.ApplicationUsers
                .Where(m =>
                    (surname == null || m.Surname.Contains(surname)) &&
                    (creationDate == null || m.AccountCreationDate > creationDate) &&
                    m.Complaint.Any(n =>
                        !n.Processed &&
                        (complaintType == null || n.Type.Name.Contains(complaintType))))
                .Include(u => u.Complaint)        
                .ThenInclude(c => c.Type)           
                .ToListAsync();

            var result = users.Select(u =>
                new UserForBanDTO(
                    u.Id,
                    u.Name,
                    u.Surname,
                    u.AccountCreationDate,
                    u.Complaint
                        .Where(o =>
                            !o.Processed &&
                            (complaintType == null || o.Type.Name.Contains(complaintType)))
                        .GroupBy(o => o.Type.Name)
                        .Select(g => new ComplaintTypeDTO(g.Key, g.Count()))
                        .OrderBy(ct => ct.Name)
                        .ToList()
                )
            ).ToList();

            return Ok(result);
        }
    }
}
