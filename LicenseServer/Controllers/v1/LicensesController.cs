using LicenseServer.Database;
using LicenseServer.Models.API;
using LicenseServer.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel;
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
					return BadRequest(new Result.Fail());

				var licenses = await _context.Licenses.Include(l => l.Organization).Include(l => l.Tarif)
					.Where(l => l.Organization.Id == orgId && l.EndDate > DateTime.Now).ToListAsync();

				return Ok(new Result.Success<IEnumerable<License>> { Status = "Success", Data = licenses }); 
			}
			catch { return BadRequest(new Result.Fail()); }
		}

		[HttpGet("licensesOrgProg")] // 4. GET Метод получения активных лицензий по id организации по конкретной программе
		public async Task<ActionResult<IEnumerable<License>>> GetLicensesByOrgWithProg(int orgId, ProgramType programId )
		{
			try
			{
				if (orgId == 0 || programId==0)
					return BadRequest(new Result.Fail());

				var licenses = await _context.Licenses.Include(l => l.Organization).Include(l => l.Tarif)
					.Where(l => l.Organization.Id == orgId && l.Tarif.Program == programId && l.EndDate > DateTime.Now).ToListAsync();

				return Ok(new Result.Success<IEnumerable<License>> { Status = "Success", Data = licenses });
			}
			catch { return BadRequest(new Result.Fail()); }
		}

		[HttpPost("create")] // 6. POST Метод добавления лицензии для организации
		public async Task<ActionResult<License>> CreateLicense(LicenseAPI licenseData)
		{
			try
			{
				Organization neededOrganization = await _context.Organizations.FindAsync(licenseData.OrganizationId);
				Tarif neededTarif = await _context.Tarifs.FindAsync(licenseData.TarifId);

				if (neededTarif == null || neededOrganization == null)
					return NotFound(new Result.Fail());

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

				return CreatedAtAction(nameof(GetLicensesByOrg), new { id = currentLicense.Id }, new Result.Success<License> { Status = "Success", Data = currentLicense });

			}
			catch { return BadRequest(new Result.Fail()); }
		}

		[HttpDelete("delete/{id}")] // 7. POST Метод удаления лицензии для организации
		public async Task<IActionResult> DeleteLicense(int id) 
		{
			var license = await _context.Licenses.FindAsync(id);

			if (license == null)
				return NotFound(new Result.Fail());

			_context.Licenses.Remove(license);
			await _context.SaveChangesAsync();

			return Ok(new Result.Success<Array> { Status = "Success", Data = null });
		}
	}
}