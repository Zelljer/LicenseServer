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
        public async Task<HTTPResult<OrganizationAPI.OrganizationResponse>> GetOrganizationById(int organizationId)
        {
            try
            {
                using var context = ApplicationContext.New;

                if (organizationId <= 0)
                    return new HTTPResult<OrganizationAPI.OrganizationResponse> { Errors = new() { "Не существует организации с таким Id" }, IsSuccsess = false };

                var organization = await context.Organizations.FindAsync(organizationId);

                if (organization == null)
                    return new HTTPResult<OrganizationAPI.OrganizationResponse> { IsSuccsess = true };

                var currentOrganization = new OrganizationAPI.OrganizationResponse()
                {
                    Id = organization.Id,
                    Name = organization.Name,
                    Inn = organization.Inn,
                    Kpp = organization.Kpp,
                    Email = organization.Email,
                    Phone = organization.Phone,
                };

                return new HTTPResult<OrganizationAPI.OrganizationResponse> { IsSuccsess = true, Data = currentOrganization };
            }
            catch
            {
                return new HTTPResult<OrganizationAPI.OrganizationResponse> { Errors = new() { "Ошибка" }, IsSuccsess = false };
            }
        }


        public async Task<HTTPResult<PagedResult<OrganizationsLiceses>>> GetOrganizationsByPages(int page, int pageSize)
		{
			try
			{
				using var context = ApplicationContext.New;
                var errorResult = new List<string>();

                errorResult
					.AddRange(Validator
					.IsValidData(page, "Укажите нужный номер страницы. Отсчет страниц начинается с 1")
					);

				errorResult
					.AddRange(Validator
					.IsValidData(pageSize, "Укажите, сколько элементов будет отображаться на странице (размер страницы)")
					);

				if (errorResult.Any())
					 return new HTTPResult<PagedResult<OrganizationsLiceses>> { Errors = errorResult, IsSuccsess = false };

                var organizations = await context.Organizations
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (!organizations.Any())
                    return new HTTPResult<PagedResult<OrganizationsLiceses>> { IsSuccsess = true };

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

                return new HTTPResult<PagedResult<OrganizationsLiceses>> { IsSuccsess = true, Data = currentPage };
			}
			catch
			{
                return new HTTPResult<PagedResult<OrganizationsLiceses>> { Errors = new() { "Ошибка" }, IsSuccsess = false };
            }
		}

		public async Task<HTTPResult<string>> CreateOrganization(OrganizationAPI.OrganizationRequest organization)
		{
			try
			{
				using var context = ApplicationContext.New;
                var errorResult = new List<string>();

                errorResult
                    .AddRange(Validator
                    .IsValidData(organization.Name, "Не корректное название"));

                if (!Validations.IsValidInn(organization.Inn))
					errorResult.Add("Не корректный ИНН");

				if (organization.Inn.Length != 12 && !Validations.IsValidKpp(organization.Kpp))
					errorResult.Add("Не корректный КПП");

				errorResult
					.AddRange(Validator
					.IsValidEmail(organization.Email));

				errorResult
					.AddRange(Validator
					.IsValidPhone(organization.Phone));

				if (errorResult.Any())
                    return new HTTPResult<string> { Errors = errorResult, IsSuccsess = false };

                OrganizationEntity currentOrganization = new()
				{
                    Name = organization.Name,
                    Inn = organization.Inn,
					Kpp = organization.Inn.Length == 12 ? "" : organization.Kpp,
					Email = organization.Email,
					Phone = organization.Phone
				};
				context.Organizations.Add(currentOrganization);
				await context.SaveChangesAsync();

                return new HTTPResult<string> { Data = "Организация создана успешно", IsSuccsess = true };
            }
			catch 
			{
                return new HTTPResult<string> { Errors = new() { "Ошибка" }, IsSuccsess = false };
            }
		} 
	}
}