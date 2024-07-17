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
		public LicensesController(ApplicationContext context)
		{ 
			_context = context;
			
			/* Organization or1 = new Organization()
			{
				Inn = "946865737856883",
				Kpp = "54356566",
				Email = "example1@mail.ru",
				Phone = "+7457647853"
			};
			Organization or2 = new Organization()
			{
				Inn = "897656989787568",
				Kpp = "53785658",
				Email = "example2@mail.ru",
				Phone = "+74675756853"
			};
			Organization or3 = new Organization()
			{
				Inn = "4678672657537676",
				Kpp = "05789768",
				Email = "example3@mail.ru",
				Phone = "+7486587853"
			};
			Organization or4 = new Organization()
			{
				Inn = "3245642767653",
				Kpp = "876848678",
				Email = "example4@mail.ru",
				Phone = "+747854553"
			};
			Organization or5 = new Organization()
			{
				Inn = "845673567538658",
				Kpp = "946784678",
				Email = "example5@mail.ru",
				Phone = "+7754847853"
			};

			_context.Organizations.AddRange(or1, or2, or3, or4, or5);

			Tarif ta1 = new Tarif()
			{
				Name = "3 дня",
				Program = ProgramType.ProgramA,
				Price = 6000
			};
			Tarif ta2 = new Tarif()
			{
				Name = "неделя",
				Program = ProgramType.ProgramB,
				Price = 11000
			};
			Tarif ta3 = new Tarif()
			{
				Name = "месяц",
				Program = ProgramType.ProgramB,
				Price = 34000
			};
			Tarif ta4 = new Tarif()
			{
				Name = "полгода",
				Program = ProgramType.ProgramA,
				Price = 54000
			};
			Tarif ta5 = new Tarif()
			{
				Name = "год",
				Program = ProgramType.ProgramC,
				Price = 79000
			};

			_context.Tarifs.AddRange(ta1, ta2, ta3, ta4, ta5);

			License li1 = new License()
			{
				Organization = _context.Organizations.First(),
				Tarif = _context.Tarifs.First(),
				DateCreated = DateTime.Now,
				StartDate = DateTime.Now,
				EndDate = DateTime.Now + TimeSpan.FromDays(5),
			};
			License li2 = new License()
			{
				Organization = _context.Organizations.ToList()[2],
				Tarif = _context.Tarifs.ToList()[2],
				DateCreated = DateTime.Now,
				StartDate = DateTime.Now,
				EndDate = DateTime.Now + TimeSpan.FromDays(5),
			};
			License li3 = new License()
			{
				Organization = _context.Organizations.ToList()[3],
				Tarif = _context.Tarifs.ToList()[3],
				DateCreated = DateTime.Now,
				StartDate = DateTime.Now,
				EndDate = DateTime.Now + TimeSpan.FromDays(5),
			};
			License li4 = new License()
			{
				Organization = _context.Organizations.ToList()[4],
				Tarif = _context.Tarifs.ToList()[4],
				DateCreated = DateTime.Now,
				StartDate = DateTime.Now,
				EndDate = DateTime.Now + TimeSpan.FromDays(5),
			};
			License li5 = new License()
			{
				Organization = _context.Organizations.First(),
				Tarif = _context.Tarifs.Last(),
				DateCreated = DateTime.Now,
				StartDate = DateTime.Now,
				EndDate = DateTime.Now + TimeSpan.FromDays(5),
			};

			_context.Licenses.AddRange(li1,li2,li3,li4,li5);

			_context.SaveChanges();*/
		}

		[HttpGet("licensesOrg")] // 3. GET Метод получения активных лицензий по id организации 
		public async Task<ActionResult<IEnumerable<License>>> GetLicensesByOrg(int orgId) 
		{
			var licenses = _context.Licenses
				.Where(l => l.Organization.Id == orgId && l.EndDate < DateTime.Now);

			return await licenses.ToListAsync();
		}

		[HttpGet("licensesOrgProg")] // 4. GET Метод получения активных лицензий по id организации по конкретной программе
		public async Task<ActionResult<IEnumerable<License>>> GetLicensesByOrgWithProg(int orgId, ProgramType? programId = null)
		{
			var licenses = _context.Licenses
				.Where(l => l.Organization.Id == orgId && l.Tarif.Program == programId && l.EndDate < DateTime.Now);

			return await licenses.ToListAsync();
		}

		[HttpPost("create")] // 6. POST Метод добавления лицензии для организации
		public async Task<ActionResult<License>> CreateLicense(LicenseAPI license) 
		{
			Organization currentOrganization = new Organization()
			{
				Inn = license.Organization.Inn,
				Kpp = license.Organization.Kpp,
				Email = license.Organization.Email,
				Phone = license.Organization.Phone,
			};
			Tarif currentTarif = new Tarif()
			{
				Name = license.Tarif.Name,
				Program = license.Tarif.Program,
				Price = license.Tarif.Price,
			};
			License currentLicense = new License()
			{
				Organization = currentOrganization,
				Tarif = currentTarif,
				DateCreated = license.DateCreated,
				StartDate = license.StartDate,
				EndDate = license.EndDate,
			};
			_context.Licenses.Add(currentLicense);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetLicensesByOrg), new { id = currentLicense.Id }, currentLicense);
		}

		[HttpDelete("delete/{id}")] // 7. POST Метод удаления лицензии для организации
		public async Task<IActionResult> DeleteLicense(int id) 
		{
			var license = await _context.Licenses.FindAsync(id);

			if (license == null)
				return NotFound();

			_context.Licenses.Remove(license);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}