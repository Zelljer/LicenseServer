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
	public class LicensesController(ILogger<LicensesController> logger) : ControllerBase
	{
		private readonly LicensService _licensService = new LicensService();
		private readonly ILogger<LicensesController> _logger = logger;

		[HttpGet("licensesOrg")] // 3. GET Метод получения активных лицензий по id организации 
		public async Task<ActionResult> GetLicensesByOrg(int orgId)
		{
			try
			{
                if (!ModelState.IsValid)
                    return ResponseResults.ErrorOkResult("Введите корректные данные");

                var licenses = await _licensService.GetLicensesByOrgId(orgId);
				return Ok(licenses);
			}
			catch (Exception ex)
			{
                return ResponseResults.ErrorBadResult("Произошла ошибка при выполнении запроса", logger, ex);
            }
		}

		[HttpGet("licensesOrgProg")] // 4. GET Метод получения активных лицензий по id организации по конкретной программе
		public async Task<ActionResult> GetLicensesByOrgWithProg(int orgId, ProgramType programId)
		{
			try
			{
                if (!ModelState.IsValid)
                    return ResponseResults.ErrorOkResult("Введите корректные данные" );

                var licenses = await _licensService.GetLicensesByOrgIdWithProgId(orgId, programId);
				return Ok(licenses);
			}
			catch (Exception ex)
			{
                return ResponseResults.ErrorBadResult("Произошла ошибка при выполнении запроса", logger, ex);
            }
		}

		[Authorize(Roles = $"{nameof(RoleType.Admin)},{nameof(RoleType.Manager)}")]
		[HttpPost("create")] // 6. POST Метод добавления лицензии для организации
		public async Task<ActionResult> CreateLicense(LicenseAPI.LicenseRequest licenseData)
		{
			try
			{
                if (!ModelState.IsValid)
                    return ResponseResults.ErrorOkResult("Введите корректные данные");

                var createdLicense = await _licensService.CreateLicense(licenseData);
				return CreatedAtAction(nameof(GetLicensesByOrg), createdLicense);
			}
			catch (Exception ex)
			{
                return ResponseResults.ErrorBadResult("Произошла ошибка при выполнении запроса",logger,ex);
            }
		}

		[Authorize(Roles = nameof(RoleType.Admin))]
		[HttpDelete("delete")] // 7. POST Метод удаления лицензии для организации
		public async Task<IActionResult> DeleteLicense(int licenseId) 
		{
			try
			{
                if (!ModelState.IsValid)
                    return ResponseResults.ErrorOkResult("Введите корректные данные");

                var deleteLicense = await _licensService.DeleteLicenseById(licenseId);
				return Ok(deleteLicense);
			}
			catch (Exception ex)
			{
                return ResponseResults.ErrorBadResult("Произошла ошибка при выполнении запроса", logger, ex);
            }
		}
	}
}