using LicenseServer.Database;
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
			var organizations = _context.Organizations
				.OrderBy(o => o.Id)
				.Skip((page - 1) * pageSize)
				.Take(pageSize);

			var totalCount = await _context.Organizations.CountAsync();

			return new PagedResult<Organization>
			{
				Items = await organizations.ToListAsync(),
				TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
				CurrentPage = page
			};
		}
	}
}
