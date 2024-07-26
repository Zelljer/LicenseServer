using LicenseServer.Domain.Methods;
using LicenseServer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LicenseServer.Web.Controllers.v1
{
	[ApiController]
	[Route("api/[controller]")]
	public class OrganizationsController : ControllerBase
	{
		private readonly OrganizationService _organizationService;
		private readonly ILogger<OrganizationsController> _logger;

		public OrganizationsController(ILogger<OrganizationsController> logger)
		{
			_organizationService = new OrganizationService();
			_logger = logger;
		}

		[HttpGet("organizations")] // 5. GET Метод получения списка всех организаций из БД со списком всех лицензий (постраничный вывод)
		public async Task<ActionResult> GetOrganizations(int page, int pageSize)
		{
			try
			{
				var licenses = await _organizationService.GetOrganizations(page, pageSize);
				return Ok(licenses);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new { Status = "Fail", Data = "Произошла ошибка при выполнении запроса" });
			}
		}
			
		[HttpPost("create")] // 8. POST Метод добавления организации
		public async Task<ActionResult> CreateOrganization(OrganizationAPI.OrganizationRequest organization)
		{
			try
			{
				var createdOrganization = await _organizationService.CreateOrganization(organization);
				return CreatedAtAction(nameof(GetOrganizations), createdOrganization);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new { Status = "Fail", Data = "Произошла ошибка при выполнении запроса" });
			}
		}
	}
}
