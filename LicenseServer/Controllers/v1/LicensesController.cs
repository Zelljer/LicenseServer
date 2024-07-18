using LicenseServer.Database;
using LicenseServer.Models.API;
using LicenseServer.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using System.Security.Cryptography;
using License = LicenseServer.Models.Database.License;

namespace LicenseServer.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class LicensesController : ControllerBase
	{
		private readonly ApplicationContext _context;
		public LicensesController(ApplicationContext context) => _context = context;

		[HttpGet("licensesOrg")] // 3. GET Метод получения активных лицензий по id организации 
		public async Task<ActionResult<IEnumerable<License>>> GetLicensesByOrg(int orgId) 
		{
			try
			{
				if (orgId == 0)
					return BadRequest(new Result.Fail() { Data = { "Укажите корректный Id лицензии" } });

				var licenses = await _context.Licenses.Include(l => l.Organization).Include(l => l.Tarif)
					.Where(l => l.Organization.Id == orgId && l.EndDate > DateTime.Now).ToListAsync();

				return Ok(new Result.Success<IEnumerable<License>> { Status = "Success", Data = licenses }); 
			}
			catch { return BadRequest(new Result.Fail() { Data = { "Ошибка при выпролнении запроса" } }); }
		}

		[HttpGet("licensesOrgProg")] // 4. GET Метод получения активных лицензий по id организации по конкретной программе
		public async Task<ActionResult<IEnumerable<License>>> GetLicensesByOrgWithProg(int orgId, ProgramType programId )
		{
			try
			{
				var errorResult = new Result.Fail();
				if (orgId == 0)
					errorResult.Data.Add("Указан не корректный Id организации");
				if (programId == 0)
					errorResult.Data.Add("Укажите корректную программу");
				if (errorResult.Data.Count > 0)
					return BadRequest(errorResult);

				var licenses = await _context.Licenses.Include(l => l.Organization).Include(l => l.Tarif)
					.Where(l => l.Organization.Id == orgId && l.Tarif.Program == programId && l.EndDate > DateTime.Now).ToListAsync();

				return Ok(new Result.Success<IEnumerable<License>> { Status = "Success", Data = licenses });
			}
			catch { return BadRequest(new Result.Fail() { Data = { "Ошибка при выпролнении запроса" } }); }
		}

		[HttpPost("create")] // 6. POST Метод добавления лицензии для организации
		public async Task<ActionResult<License>> CreateLicense(LicenseAPI licenseData)
		{
			try
			{
				Organization neededOrganization = await _context.Organizations.FindAsync(licenseData.OrganizationId);
				Tarif neededTarif = await _context.Tarifs.FindAsync(licenseData.TarifId);

				var errorResult = new Result.Fail();
				if (neededOrganization == null)
					errorResult.Data.Add("Указана не существующая организация или не корректный Id организации");
				if (neededTarif == null)
					errorResult.Data.Add("Указан не существующий тариф или не корректный Id тарифа");
				if (errorResult.Data.Count > 0)
					return BadRequest(errorResult);

				License currentLicense = new License()
				{
					Organization = neededOrganization,
					Tarif = neededTarif,
					DateCreated = DateTime.Now,
					StartDate = licenseData.DateStart,
					EndDate = licenseData.DateStart + TimeSpan.FromDays(neededTarif.DaysCount),
				};

				_context.Licenses.Add(currentLicense);
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(GetLicensesByOrg), new { id = currentLicense.Id }, new Result.Success<License> { Data = currentLicense });

			}
			catch { return BadRequest(new Result.Fail() { Data = { "Ошибка при выпролнении запроса" } }); }
		}

		[HttpDelete("delete/{id}")] // 7. POST Метод удаления лицензии для организации
		public async Task<IActionResult> DeleteLicense(int id) 
		{
			var license = await _context.Licenses.FindAsync(id);

			if (license == null)
				return BadRequest(new Result.Fail() { Data = { "Указана не существующая лицензия или не корректный Id лицензии" } });

			_context.Licenses.Remove(license);
			await _context.SaveChangesAsync();

			return Ok(new Result.Success<Array> { Data = null });
		}
	}
}