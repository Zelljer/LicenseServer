using LicenseServer.Database.Models;
using LicenseServer.Domain.Methods;
using LicenseServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace LicenseServer.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class LicensesController(LicensService licensService) : ControllerBase
	{
		private readonly LicensService _licensService = licensService;

		[HttpGet("licensesOrg")] // 3. GET Метод получения активных лицензий по id организации 
		public async Task<ActionResult> GetLicensesByOrg(int orgId) 
		{
			try
			{
				var licenses = await _licensService.GetLicensesByOrg(orgId);
				return Ok(licenses);
			}
			catch //(Exception ex)
			{
				return null;
				//_logger.LogError(ex.Message);
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
			catch //(Exception ex)
			{
				return null;
				//_logger.LogError(ex.Message);
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
			catch //(Exception ex)
			{
				return null;
				//_logger.LogError(ex.Message);
			}
		}

		[HttpDelete("delete")] // 7. POST Метод удаления лицензии для организации
		public async Task<IActionResult> DeleteLicense(int id) 
		{
			try
			{
				var createdLicense = await _licensService.DeleteLicense(id);
				return Ok(createdLicense);

			}
			catch //(Exception ex)
			{
				return null;
				//_logger.LogError(ex.Message);
			}
		}
	}
}