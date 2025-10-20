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
                .Where(m=> ((m.Surname.Contains(surname)) || (surname==null))
                
                &&( m.Complaint.Where(n=> n.Processed==false)))
                .Select(m=> new UserForBanDTO(m.Id,
                    m.Name,
                    m.Surname,
                    m.AccountCreationDate,
                    m.ComplaintTypes.Select(n=> new ComplaintTypeDTO(
                        n.Type.Id,
                        n.Type.Name
                    )).ToList()
                ))
                .ToListAsync();
            return Ok(usersDTO);
        }
    }
}
