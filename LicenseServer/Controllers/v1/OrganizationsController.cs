using LicenseServer.Database;
using LicenseServer.Models.API;
using LicenseServer.Models.Database;
using LicenseServer.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace LicenseServer.Controllers.v1
{
    [ApiController]
	[Route("api/[controller]")]
	public class OrganizationsController : ControllerBase
	{
		private readonly ApplicationContext _context;
		public OrganizationsController(ApplicationContext context) => _context = context;

		[HttpGet("organizations")] // 5. GET Метод получения списка всех организаций из БД со списком всех лицензий (постраничный вывод)
		public async Task<ActionResult<PagedResult<OrganizationsLiceses>>> GetOrganizations(int page = 1, int pageSize = 10)
		{
			try
			{
				var errorResult = new Result.Fail();
				errorResult.Data.AddRange(Validator.IsValidData(page, "Укажите нужный номер страницы. Отсчет страниц начинается с 1"));
				errorResult.Data.AddRange(Validator.IsValidData(pageSize, "Укажите, сколько элементов будет отображаться на странице (размер страницы)"));
				if (errorResult.Data.Any())
					return BadRequest(errorResult);

				var organizations = await _context.Organizations
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (!organizations.Any())
					return Ok(new Result.Success<string> { Data = "В базе нет данных или совершен переход на не существующую страницу" });

				var licenses = await _context.Licenses.Include(l => l.Organization).Include(l => l.Tarif).ToListAsync();

				var data = organizations.Select(organization => new OrganizationsLiceses
				{
					Organization = organization,
					Licenses = licenses.Where(l =>  l.Organization.Id == organization.Id) 
					.Select(l => new LicenseAPI.LicenseResponse
					{
						Id = l.Id,
						OrganizationId = l.Organization.Id, 
						TarifId = l.Tarif.Id, 
						DateCreated = l.DateCreated,
						StartDate = l.StartDate,
						EndDate = l.EndDate,
					}).ToList()}).ToList();

					var currentPage = new PagedResult<OrganizationsLiceses>
					{
						Items = data,
						TotalPages = (int)Math.Ceiling(organizations.Count / (double)pageSize),
						CurrentPage = page
					};

				return Ok(new Result.Success<PagedResult<OrganizationsLiceses>> { Data = currentPage });
			}
			catch
			{
				return BadRequest(new Result.Fail { Data = { "Ошибка при выполнении запроса." } });
			}
		}

		[HttpPost("create")] // 8. POST Метод добавления организации
		public async Task<ActionResult<Tarif>> CreateOrganization(OrganizationAPI.OrganizationRequest organization)
		{
			try
			{
				var errorResult = new Result.Fail();
				errorResult.Data.AddRange(Validator.IsValidInn(organization.Inn));
				if (organization.Inn.Length != 12)
					errorResult.Data.AddRange(Validator.IsValidKpp(organization.Kpp));
				errorResult.Data.AddRange(Validator.IsValidEmail(organization.Email));
				errorResult.Data.AddRange(Validator.IsValidPhone(organization.Phone));
				if (errorResult.Data.Any())
					return BadRequest(errorResult);

				Organization currentOrganization = new Organization()
				{
					Inn = organization.Inn,
					Kpp = organization.Inn.Length == 12 ? "" : organization.Kpp,
					Email = organization.Email,
					Phone = organization.Phone
				};
				_context.Organizations.Add(currentOrganization);
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(GetOrganizations), new { id = currentOrganization.Id }, new Result.Success<Organization> { Data = currentOrganization });
			}
			catch { return BadRequest(new Result.Fail() { Data = { "Ошибка при выпролнении запроса" } }); }
		}
	}
}
