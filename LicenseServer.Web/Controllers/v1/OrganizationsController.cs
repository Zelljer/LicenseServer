using LicenseServer.Database.Models;
using LicenseServer.Domain.Methods;
using Microsoft.AspNetCore.Mvc;

namespace LicenseServer.Controllers.v1
{
	[ApiController]
	[Route("api/[controller]")]
	public class OrganizationsController(OrganizationService organizationService) : ControllerBase
	{
		private readonly OrganizationService _organizationService = organizationService;

		[HttpGet("organizations")] // 5. GET Метод получения списка всех организаций из БД со списком всех лицензий (постраничный вывод)
		public async Task<ActionResult> GetOrganizations(int page, int pageSize)
		{
			try
			{
				var licenses = await _organizationService.GetOrganizations(page, pageSize);
				return Ok(licenses);
			}
			catch //(Exception ex)
			{
				return null;
				//_logger.LogError(ex.Message);
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
			catch //(Exception ex)
			{
				return null;
				//_logger.LogError(ex.Message);
			}
		}
	}
}
