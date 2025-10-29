using AppForSEII2526.API.DTOs.BanReportDTO;
using AppForSEII2526.API.DTOs.UserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanReportController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public BanReportController(ApplicationDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<BanReportResultDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetBanReport(int id)
        {
            IList<BanReportResultDTO> banReportsDTO = await _context.BanReports
                .Where(m => m.Id == id)
                .Include(n => n.ReportCustomers)
                .ThenInclude(o => o.Customer)
                .ThenInclude(p => p.Complaint)
                .Select(q => new BanReportResultDTO(
                    q.ReportCustomers.Custo

                ))
                .ToListAsync();
            return Ok(banReportsDTO);
        }

    }
}
