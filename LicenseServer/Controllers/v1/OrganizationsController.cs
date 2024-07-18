using LicenseServer.Database;
using LicenseServer.Models.API;
using LicenseServer.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LicenseServer.Controllers.v1
{
    [ApiController]
	[Route("api/[controller]")]
	public class OrganizationsController : ControllerBase
	{
		private readonly ApplicationContext _context;
		public OrganizationsController(ApplicationContext context) => _context = context;

		[HttpGet("organizations")] // 5. GET Метод получения списка всех организаций из БД со списком всех лиценизий(постраничный вывод)
		public async Task<ActionResult<PagedResult<Organization>>> GetOrganizations(int page = 1, int pageSize = 10)
		{
			try
			{
				if (page == 0 || pageSize == 0)
					return BadRequest(new Result.Fail());

				var organizations = _context.Organizations
					.Skip((page - 1) * pageSize)
					.Take(pageSize);

				var totalCount = await _context.Organizations.CountAsync();

				var currentPage = new PagedResult<Organization>
				{
					Items = await organizations.ToListAsync(),
					TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
					CurrentPage = page
				};
				return Ok(new Result.Success<PagedResult<Organization>> { Status = "Success", Data = currentPage });
			}
			catch { return BadRequest(new Result.Fail()); }
		}

		[HttpPost("create")] // 8. POST Метод добавления организации
		public async Task<ActionResult<Tarif>> CreateOrganization(OrganizationAPI organization)
		{
			try
			{
				Organization currentOrganization = new Organization()
				{
					Inn = organization.Inn,
					Kpp = organization.Inn.Length == 12 ? "" : organization.Kpp,
					Email = organization.Email,
					Phone = organization.Phone
				};
				_context.Organizations.Add(currentOrganization);
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(GetOrganizations), new { id = currentOrganization.Id }, new Result.Success<Organization> { Status = "Success", Data = currentOrganization });
			}
			catch { return BadRequest(new Result.Fail()); }
		}
	}
}
