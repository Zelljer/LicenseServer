using LicenseServer.Database;
using LicenseServer.Models;
using LicenseServer.Models.API;
using LicenseServer.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using LicenseEntity = LicenseServer.Models.Database.LicenseEntity;

namespace LicenseServer.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class LicensesController(ApplicationContext context, ILogger<LicensesController> logger) : ControllerBase
	{
		private readonly ApplicationContext _context = context;
		private readonly ILogger<LicensesController> _logger = logger;

		[HttpGet("licensesOrg")] // 3. GET Метод получения активных лицензий по id организации 
		public async Task<ActionResult> GetLicensesByOrg(int orgId) 
		{
			try
			{
				if (orgId <= 0)
					return BadRequest(new Result.Fail() { Data = { "Укажите корректный Id лицензии" } });

				var licenses = await _context.Licenses
					.Where(l => l.Organization.Id == orgId)
					.Select(t => new LicenseAPI.LicenseResponse
					{
						Id = t.Id,
						OrganizationId = t.Organization.Id,
						TarifId = t.Tarif.Id,
						DateCreated = t.DateCreated,
						StartDate = t.StartDate,
						EndDate = t.EndDate,
					}).ToArrayAsync();

				if (!licenses.Any())
					return Ok(new Result.Success<string> { } );

				return Ok(new Result.Success<IEnumerable<LicenseAPI.LicenseResponse>> { Data = licenses });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new Result.Fail() { Data = { "Ошибка при выполнении запроса" } }); 
			}
		}

		[HttpGet("licensesOrgProg")] // 4. GET Метод получения активных лицензий по id организации по конкретной программе
		public async Task<ActionResult> GetLicensesByOrgWithProg(int orgId, ProgramType programId )
		{
			try
			{
				var errorResult = new Result.Fail();

				errorResult.Data.AddRange(Validator.IsValidData(orgId, "Указан не корректный Id организации"));

				if (_context.Organizations
					.Find(orgId) == null)
					errorResult.Data.Add("Нет организации с таким Id");

				if (!Enum.IsDefined(typeof(ProgramType), programId))
					errorResult.Data.Add("Указана не существующая программа");

				if (errorResult.Data.Any())
					return BadRequest(errorResult);

				var licenses = await _context.Licenses
					.Include(l => l.Organization)
					.Include(l => l.Tarif)

					.Where(l => l.Organization.Id == orgId && l.Tarif.Program == programId && l.EndDate > DateTime.Now)
					.Select(t => new LicenseAPI.LicenseResponse
					{
						Id = t.Id,
						OrganizationId = t.Organization.Id,
						TarifId = t.Tarif.Id,
						DateCreated = t.DateCreated,
						StartDate = t.StartDate,
						EndDate = t.EndDate,
					}).ToArrayAsync();

				if (!licenses.Any())
					return Ok(new Result.Success<string> { });

				return Ok(new Result.Success<IEnumerable<LicenseAPI.LicenseResponse>> { Data = licenses });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new Result.Fail() { Data = { "Ошибка при выполнении запроса" } }); 
			}
		}

		[HttpPost("create")] // 6. POST Метод добавления лицензии для организации
		public async Task<ActionResult> CreateLicense(LicenseAPI.LicenseRequest licenseData)
		{
			try
			{
				var errorIdResult = new Result.Fail();

				errorIdResult.Data
					.AddRange(Validator
					.IsValidData(licenseData.OrganizationId, "Укажите Id организации"));

				if (!_context.Organizations
					.Where(o => o.Id == licenseData.OrganizationId)
					.Any())
					errorIdResult.Data.Add("Нет организации с таким Id");

				errorIdResult.Data
					.AddRange(Validator
					.IsValidData(licenseData.TarifId, "Указан не корректный Id тарифа"));

				if (!_context.Tarifs
					.Where(t => t.Id == licenseData.TarifId).Any())
					errorIdResult.Data.Add("Нет тарифа с таким Id");

				var currentLicenseDateStart = new DateTime();
				if (DateTime.TryParseExact(licenseData.DateStart, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime daeteTime))
					currentLicenseDateStart = daeteTime;
				else
					errorIdResult.Data.Add("Введите дату создания лицензии в формате dd.mm.yyyy" );

				var neededOrganization = _context.Organizations.Find(licenseData.OrganizationId);
				var neededTarif = _context.Tarifs.Find(licenseData.TarifId);

				if (neededOrganization == null || neededTarif == null)
					errorIdResult.Data.Add("В базе нет данных о организациях или тарифах");

				if (errorIdResult.Data.Any())
					return BadRequest(errorIdResult);

				var currentLicense = new LicenseEntity
				{
					Organization = neededOrganization,
					Tarif = neededTarif,
					DateCreated = DateTime.Now,
					StartDate = currentLicenseDateStart,
					EndDate = currentLicenseDateStart + TimeSpan.FromDays(neededTarif.DaysCount),
				};
				_context.Licenses.Add(currentLicense);
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(GetLicensesByOrg), new { id = currentLicense.Id }, new Result.Success<LicenseEntity> { Data = currentLicense });

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new Result.Fail() { Data = { "Ошибка при выпролнении запроса" } }); 
			}
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
					return BadRequest(new Result.Fail() { Data = { "Указана не существующая лицензия" } });

				_context.Licenses.Remove(license);
				await _context.SaveChangesAsync();

				return Ok(new Result.Success<string> { Data = "Данные удалены" });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new Result.Fail() { Data = { "Ошибка при выполнении запроса" } }); 
			}
		}
	}
}