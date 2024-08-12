using LicenseServer.Domain.Models;
using LicenseServer.Domain.Utils;

namespace LicenseServer.Domain.Methods
{
    public class OrganizationService
	{
        public async Task<HTTPResult<OrganizationAPI.OrganizationResponse>> GetOrganizationById(int organizationId)
        {
            try
            {
                var idErrors = Validator.IsValidData(organizationId, "Не существует организации с таким Id");
                if (idErrors.Any())
                    return HttpResults.OrganizationResult.Fails(idErrors);

                var currentOrganization = await DataGetter.APIOrganizationById(organizationId);

                if (!Validator.isValidObject(currentOrganization))
                    return HttpResults.OrganizationResult.Empty();
				else
					return HttpResults.OrganizationResult.Success(currentOrganization);
            }
            catch
            {
                return HttpResults.OrganizationResult.Fail("Ошибка");
            }
        }


        public async Task<HTTPResult<PagedResult<OrganizationsLiceses>>> GetOrganizationsByPages(int page, int pageSize)
		{
			try
			{
                var errorResult = Validator.isValidPage(page, pageSize);

				if (errorResult.Any())
					return HttpResults.OrganizationsPagedResult.Fails(errorResult);

                var currentPage = await DataGetter.PagedOrganizationsLiceses(page, pageSize);

                return HttpResults.OrganizationsPagedResult.Success(currentPage);
			}
			catch
			{
                return HttpResults.OrganizationsPagedResult.Fail("Ошибка");
            }
		}

		public async Task<HTTPResult<string>> CreateOrganization(OrganizationAPI.OrganizationRequest organization)
		{
			try
			{
                var errorResult = Validator.OrganizationValidation(organization);

				if (errorResult.Any())
                    return HttpResults.StringResult.Fails(errorResult);

                var currentOrganization = DataGetter.OrganizationAPIToOrganizationEntity(organization);

                await DataManager.AddEntityAsync(currentOrganization);

                return HttpResults.StringResult.Success("Организация создана успешно");
            }
			catch 
			{
                return HttpResults.StringResult.Fail("Ошибка");
            }
		} 
	}
}