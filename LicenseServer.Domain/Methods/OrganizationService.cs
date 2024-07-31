using LicenseServer.Database;
using LicenseServer.Database.Entity;
using LicenseServer.Domain.Models;
using LicenseServer.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using ValidationsCollection;

namespace LicenseServer.Domain.Methods
{
	public class OrganizationService
	{

		public async Task<IHTTPResult> GetOrganizationsByPages(int page, int pageSize)
		{
			try
			{
				using var context = ApplicationContext.New;
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

				var organizations = await context.Organizations
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (!organizations.Any())
					return new Success<PagedResult<OrganizationsLiceses>> { Data = new PagedResult<OrganizationsLiceses>() };

				var licenses = await context.Licenses
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
					TotalPages = (int)Math.Ceiling(context.Organizations.Count() / (double)pageSize),
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
				using var context = ApplicationContext.New;
				var errorResult = new Fail();

				if (!Validations.IsValidInn(organization.Inn))
					errorResult.Data.Add("Не корректный ИНН");

				if (organization.Inn.Length != 12 && !Validations.IsValidKpp(organization.Kpp))
					errorResult.Data.Add("Не корректный КПП");

				errorResult.Data
					.AddRange(Validator
					.IsValidEmail(organization.Email));

				errorResult.Data
					.AddRange(Validator
					.IsValidPhone(organization.Phone));

				if (errorResult.Data.Any())
					return errorResult;

				OrganizationEntity currentOrganization = new()
				{
					Inn = organization.Inn,
					Kpp = organization.Inn.Length == 12 ? "" : organization.Kpp,
					Email = organization.Email,
					Phone = organization.Phone
				};
				context.Organizations.Add(currentOrganization);
				await context.SaveChangesAsync();

				return new Success<OrganizationEntity> { Data = currentOrganization };
			}
			catch 
			{
				return new Fail { Data = { "Произошла ошибка" } };
			}
		} 
	}
}