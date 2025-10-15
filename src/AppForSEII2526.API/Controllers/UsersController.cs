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
        public async Task<IActionResult> GetUsers(string? surname, ComplaintType? complaintType)
        {
            IList<UserForBanDTO> usersDTO = await _context.ApplicationUsers
                .Where(m=> m.Surname.Contains(surname) || (surname==null))
                .Select(m=> new UserForBanDTO(m.Id,
                    m.Name
                ))
                .ToListAsync();
            return Ok(usersDTO);
        }
    }
}
