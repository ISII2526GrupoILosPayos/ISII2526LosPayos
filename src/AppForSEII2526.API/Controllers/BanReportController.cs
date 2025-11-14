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
        [ProducesResponseType(typeof(ReportOperationResultDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBanReport(int reportId)
        {
            var report = await _context.BanReports
                .Include(r => r.ReportCustomers)
                    .ThenInclude(rc => rc.Customer)
                    .ThenInclude(c => c.Complaint)
                .FirstOrDefaultAsync(r => r.Id == reportId);

            if (report == null)
            {
                return NotFound("Report not found");
            }

            foreach (var rc in report.ReportCustomers)
            {
                if (rc.Customer?.Complaint != null)
                {
                    foreach (var complaint in rc.Customer.Complaint)
                    {
                        if (!complaint.Processed)
                        {
                            complaint.Processed = true;
                        }
                    }
                }
                rc.State = ReportState.InProgress;
            }

            await _context.SaveChangesAsync();

            IList<ReportUserDTO> usersDTO = report.ReportCustomers
                .Select(rc => new ReportUserDTO(
                    rc.Customer.Name,
                    rc.Customer.Surname,
                    rc.Message
                ))
                .ToList();

            var resultDTO = new ReportOperationResultDTO(
                report.Reason,
                report.DetailedDescription,
                report.StartDate,
                report.EndDate,
                usersDTO
            );

            return Ok(resultDTO);
        }
    }
}
