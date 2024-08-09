using LicenseServer.Database.Dependencies;
using LicenseServer.Domain.Methods;
using LicenseServer.Domain.Models;
using LicenseServer.Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LicenseServer.Web.Controllers.v1
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class OrganizationsController(ILogger<OrganizationsController> logger) : ControllerBase
	{
		private readonly OrganizationService _organizationService = new OrganizationService();
		private readonly ILogger<OrganizationsController> _logger = logger;

        [HttpGet("organizationId")] // 5. GET Метод получения списка всех организаций из БД со списком всех лицензий (постраничный вывод)
        public async Task<ActionResult> GetOrganizationById(int organizationId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ResponseResults.ErrorOkResult("Введите корректные данные");

                var licenses = await _organizationService.GetOrganizationById(organizationId);
                return Ok(licenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResponseResults.ErrorBadResult("Произошла ошибка при выполнении запроса");
            }
        }

        [HttpGet("organizations")] // 5. GET Метод получения списка всех организаций из БД со списком всех лицензий (постраничный вывод)
		public async Task<ActionResult> GetOrganizationsByPages(int page, int pageSize)
		{
			try
			{
                if (!ModelState.IsValid)
                    return ResponseResults.ErrorOkResult("Введите корректные данные");

                var licenses = await _organizationService.GetOrganizationsByPages(page, pageSize);
				return Ok(licenses);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
                return ResponseResults.ErrorBadResult("Произошла ошибка при выполнении запроса");
            }
		}

		[Authorize(Roles = $"{nameof(RoleType.Admin)},{nameof(RoleType.Manager)}")]
		[HttpPost("create")] // 8. POST Метод добавления организации
		public async Task<ActionResult> CreateOrganization(OrganizationAPI.OrganizationRequest organization)
		{
			try
			{
                if (!ModelState.IsValid)
                    return ResponseResults.ErrorOkResult("Введите корректные данные");

                var createdOrganization = await _organizationService.CreateOrganization(organization);
				return CreatedAtAction(nameof(GetOrganizationsByPages), createdOrganization);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
                return ResponseResults.ErrorBadResult("Произошла ошибка при выполнении запроса");
            }
		}
	}
}
