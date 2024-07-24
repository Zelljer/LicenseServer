using LicenseServer.Database;
using LicenseServer.Database.Entity;
using LicenseServer.Database.Models;
using LicenseServer.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using ValidationsCollection;

namespace LicenseServer.Domain.Methods
{
	public class OrganizationService(ApplicationContext context)
	{
		private ApplicationContext _context = context;

		public async Task<IHTTPResult> GetOrganizations(int page, int pageSize)
		{
			try
			{
				var errorResult = new Fail();

				errorResult.Data
					.AddRange(Validator
					.IsValidData(page, "Укажите нужный номер страницы. Отсчет страниц начинается с 1")
					);

				errorResult.Data
					.AddRange(Validator
					.IsValidData(pageSize, "Укажите, сколько элементов будет отображаться на странице (размер страницы)")
					);

				if (errorResult.Data.Any())
					return errorResult;

				var organizations = await _context.Organizations
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (!organizations.Any())
					return new Success<PagedResult<OrganizationsLiceses>> { Data = new PagedResult<OrganizationsLiceses>() };

				var licenses = await _context.Licenses
					.Include(l => l.Organization)
					.Include(l => l.Tarif)
					.ToListAsync();

				var data = organizations.Select(organization => new OrganizationsLiceses
				{
					Organization = organization,
					Licenses = licenses.Where(l => l.Organization.Id == organization.Id)
					.Select(l => new LicenseAPI.LicenseResponse
					{
						Id = l.Id,
						OrganizationId = l.Organization.Id,
						TarifId = l.Tarif.Id,
						DateCreated = l.DateCreated,
						StartDate = l.StartDate,
						EndDate = l.EndDate,
					}).ToList()
				});

				var currentPage = new PagedResult<OrganizationsLiceses>
				{
					Items = data,
					TotalPages = (int)Math.Ceiling(_context.Organizations.Count() / (double)pageSize),
					CurrentPage = page
				};

				return new Success<PagedResult<OrganizationsLiceses>> { Data = currentPage };
			}
			catch
			{
				return new Fail { Data = { "Произошла ошибка" } };
			}
		}

		public async Task<IHTTPResult> CreateOrganization(OrganizationAPI.OrganizationRequest organization)
		{
			try
			{
				var errorResult = new Fail();

				if (!Validations.IsValidInn(organization.Inn))
					errorResult.Data.Add("Введен не корректный ИНН");

				if (organization.Inn.Length != 12 && !Validations.IsValidKpp(organization.Kpp))
					errorResult.Data.Add("Введен не корректный КПП");

				errorResult.Data
					.AddRange(Validator
					.IsValidEmail(organization.Email));

				errorResult.Data
					.AddRange(Validator
					.IsValidPhone(organization.Phone));

				if (errorResult.Data.Any())
					return errorResult;

				OrganizationEntity currentOrganization = new OrganizationEntity()
				{
					Inn = organization.Inn,
					Kpp = organization.Inn.Length == 12 ? null : organization.Kpp,
					Email = organization.Email,
					Phone = organization.Phone
				};
				_context.Organizations.Add(currentOrganization);
				await _context.SaveChangesAsync();

				return new Success<OrganizationAPI.OrganizationRequest> { Data = organization };
			}
			catch
			{
				return new Fail { Data = { "Произошла ошибка" } };
			}
		} 
	}
}