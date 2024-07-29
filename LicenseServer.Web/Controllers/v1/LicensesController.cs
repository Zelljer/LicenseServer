using LicenseServer.Database.Dependencies;
using LicenseServer.Domain.Methods;
using LicenseServer.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LicenseServer.Web.Controllers.v1
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class LicensesController : ControllerBase
	{
		private readonly LicensService _licensService;
		private readonly ILogger<LicensesController> _logger;
		public LicensesController(ILogger<LicensesController> logger)
		{
			_licensService = new LicensService();
			_logger = logger;
		}

		[HttpGet("licensesOrg")] // 3. GET Метод получения активных лицензий по id организации 
		public async Task<ActionResult> GetLicensesByOrg(int orgId) 
		{
			try
			{
				var licenses = await _licensService.GetLicensesByOrg(orgId);
				return Ok(licenses);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new { Status = "Fail", Data = "Произошла ошибка при выполнении запроса" });
			}
		}

		[HttpGet("licensesOrgProg")] // 4. GET Метод получения активных лицензий по id организации по конкретной программе
		public async Task<ActionResult> GetLicensesByOrgWithProg(int orgId, ProgramType programId )
		{
			try
			{
				var licenses = await _licensService.GetLicensesByOrgWithProg(orgId, programId);
				return Ok(licenses);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new { Status = "Fail", Data = "Произошла ошибка при выполнении запроса" });
			}
		}

		[HttpPost("create")] // 6. POST Метод добавления лицензии для организации
		public async Task<ActionResult> CreateLicense(LicenseAPI.LicenseRequest licenseData)
		{
			try
			{
				var createdLicense = await _licensService.CreateLicense(licenseData);
				return CreatedAtAction(nameof(GetLicensesByOrg), createdLicense);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new { Status = "Fail", Data = "Произошла ошибка при выполнении запроса" });
			}
		}

		[HttpDelete("delete")] // 7. POST Метод удаления лицензии для организации
		public async Task<IActionResult> DeleteLicense(int id) 
		{
			try
			{
				var deleteLicense = await _licensService.DeleteLicense(id);
				return Ok(deleteLicense);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new { Status = "Fail", Data = "Произошла ошибка при выполнении запроса" });
			}
		}
	}
}