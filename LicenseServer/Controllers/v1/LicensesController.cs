using LicenseServer.Database;
using LicenseServer.Models.API;
using LicenseServer.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using System.Globalization;
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
		public async Task<ActionResult<IEnumerable<LicenseAPI.LicenseResponse>>> GetLicensesByOrg(int orgId) 
		{

				if (orgId <= 0)
					return BadRequest(new Result.Fail() { Data = { "Укажите корректный Id лицензии" } });

				var licenses = await _context.Licenses.Where(l => l.Organization.Id == orgId).Select(t => new LicenseAPI.LicenseResponse
				{
					Id = t.Id,
					OrganizationId = t.Organization.Id,
					TarifId = t.Tarif.Id,
					DateCreated = t.DateCreated,
					StartDate = t.StartDate,
					EndDate = t.EndDate,
				}).ToArrayAsync();

			if (!licenses.Any())
					return Ok(new Result.Success<string> { Data = "В базе нет данных" });

			return Ok(new Result.Success<IEnumerable<LicenseAPI.LicenseResponse>> { Data = licenses }); 
			
		}

		[HttpGet("licensesOrgProg")] // 4. GET Метод получения активных лицензий по id организации по конкретной программе
		public async Task<ActionResult<IEnumerable<LicenseAPI.LicenseResponse>>> GetLicensesByOrgWithProg(int orgId, ProgramType programId )
		{
			try
			{
				var errorResult = new Result.Fail();
				if (orgId == 0)
					errorResult.Data.Add("Указан не корректный Id организации");
				else if (_context.Organizations.FindAsync(orgId).Result == null)
					errorResult.Data.Add("Нет организации с таким Id");
				if (programId < 0)
					errorResult.Data.Add("Укажите корректную программу");
				if (!Enum.IsDefined(typeof(ProgramType), programId))
					errorResult.Data.Add("Указана не существующая программа");
				if (errorResult.Data.Any())
					return BadRequest(errorResult);

				var licenses = await _context.Licenses.Include(l => l.Organization).Include(l => l.Tarif)
					.Where(l => l.Organization.Id == orgId && l.Tarif.Program == programId && l.EndDate > DateTime.Now).Select(t => new LicenseAPI.LicenseResponse
					{
						Id = t.Id,
						OrganizationId = t.Organization.Id,
						TarifId = t.Tarif.Id,
						DateCreated = t.DateCreated,
						StartDate = t.StartDate,
						EndDate = t.EndDate,
					}).ToArrayAsync();

				if (!licenses.Any())
					return Ok(new Result.Success<string> { Data = "В базе нет данных" });

				return Ok(new Result.Success<IEnumerable<LicenseAPI.LicenseResponse>> { Data = licenses });
			}
			catch { return BadRequest(new Result.Fail() { Data = { "Ошибка при выпролнении запроса" } }); }
		}

		[HttpPost("create")] // 6. POST Метод добавления лицензии для организации
		public async Task<ActionResult<License>> CreateLicense(LicenseAPI.LicenseRequest licenseData)
		{
			try
			{
				var currentLicenseDateStart = new DateTime();

				var errorIdResult = new Result.Fail();
				if (licenseData.OrganizationId <= 0)
					errorIdResult.Data.Add("Указана не корректный Id организации");
				if (licenseData.TarifId <= 0)
					errorIdResult.Data.Add("Указан не корректный Id тарифа");
				if (DateTime.TryParseExact(licenseData.DateStart, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime daeteTime))
					currentLicenseDateStart = daeteTime;
				else
					errorIdResult.Data.Add("Введите дату создания лицензии в формате dd.mm.yyyy" );
				if (errorIdResult.Data.Any())
					return BadRequest(errorIdResult);

				Organization neededOrganization = await _context.Organizations.FindAsync(licenseData.OrganizationId);
				Tarif neededTarif = await _context.Tarifs.FindAsync(licenseData.TarifId);

				var errorResult = new Result.Fail();
				if (neededOrganization == null)
					errorResult.Data.Add("В базе нет данных об организацияи с указанным Id");
				if (neededTarif == null)
					errorResult.Data.Add("В базе нет данных о тарифе с указанным Id");
				if (errorResult.Data.Any())
					return BadRequest(errorResult);

				License currentLicense = new License()
				{
					Organization = neededOrganization,
					Tarif = neededTarif,
					DateCreated = DateTime.Now,
					StartDate = currentLicenseDateStart,
					EndDate = currentLicenseDateStart + TimeSpan.FromDays(neededTarif.DaysCount),
				};
				_context.Licenses.Add(currentLicense);
				await _context.SaveChangesAsync();
				return CreatedAtAction(nameof(GetLicensesByOrg), new { id = currentLicense.Id }, new Result.Success<License> { Data = currentLicense });

			}
			catch { return BadRequest(new Result.Fail() { Data = { "Ошибка при выпролнении запроса" } }); }
		}

		[HttpDelete("delete")] // 7. POST Метод удаления лицензии для организации
		public async Task<IActionResult> DeleteLicense(int id) 
		{
			try
			{
				if (id <= 0)
					return BadRequest(new Result.Fail() { Data = { "Укажите корректный Id лицензии" } });

				var license = await _context.Licenses.FindAsync(id);

				if (license == null)
					return BadRequest(new Result.Fail() { Data = { "Указана не существующая лицензия или не корректный Id лицензии" } });

				_context.Licenses.Remove(license);
				await _context.SaveChangesAsync();

				return Ok(new Result.Success<string> { Data = "Данные удалены" });
			}
			catch { return BadRequest(new Result.Fail() { Data = { "Ошибка при выпролнении запроса" } }); }
		}
	}
}