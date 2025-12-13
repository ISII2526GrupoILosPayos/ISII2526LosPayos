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
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
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
                report.Id,
                report.Reason,
                report.DetailedDescription,
                report.StartDate,
                report.EndDate,
                usersDTO
            );

            return Ok(resultDTO);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReportOperationResultDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> CreateBanReport([FromBody] BanReportForCreateDTO reportDto)

        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            if (reportDto.Users == null || reportDto.Users.Count == 0)
            {
                ModelState.AddModelError("Users", "Error! You must select at least one user to report.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            if (reportDto.StartDate > reportDto.EndDate)
            {
                ModelState.AddModelError("Dates", "Error! The start date cannot be after the end date.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var userIds = reportDto.Users
                .Select(u => u.CustomerId)
                .Distinct()
                .ToList();

            var users = await _context.ApplicationUsers
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            if (users.Count != userIds.Count)
            {
                var existingIds = users.Select(u => u.Id).ToHashSet();
                var missingIds = userIds.Where(id => !existingIds.Contains(id));

                ModelState.AddModelError("Users",
                    "Error! One or more selected users do not exist: " + string.Join(", ", missingIds));
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var report = new BanReport
            {
                Reason = reportDto.Reason,
                DetailedDescription = reportDto.DetailedDescription,
                StartDate = reportDto.StartDate,
                EndDate = reportDto.EndDate,
                ReportCustomers = new List<ReportCustomer>()
            };

            foreach (var userEntry in reportDto.Users)
            {
                var customer = users.First(u => u.Id == userEntry.CustomerId);

                var rc = new ReportCustomer
                {
                    CustomerId = customer.Id,
                    Customer = customer,
                    BanReport = report,
                    Message = userEntry.PersonalMessage,
                    State = ReportState.InProgress 
                };

                report.ReportCustomers.Add(rc);
            }

            _context.BanReports.Add(report);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving BanReport");
                return Conflict("Error while saving the report. Please try again later.");
            }

            // Construir el DTO de salida
            IList<ReportUserDTO> usersDTO = report.ReportCustomers
                .Select(rc => new ReportUserDTO(
                    rc.Customer.Name,
                    rc.Customer.Surname,
                    rc.Message
                ))
                .ToList();

            var resultDTO = new ReportOperationResultDTO(
                report.Id,
                report.Reason,
                report.DetailedDescription,
                report.StartDate,
                report.EndDate,
                usersDTO
            );

            return CreatedAtAction(nameof(GetBanReport),
                new { reportId = report.Id },
                resultDTO);
        }
    }
}
