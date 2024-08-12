using LicenseServer.Database;
using LicenseServer.Database.Dependencies;
using LicenseServer.Database.Entity;
using LicenseServer.Domain.Models;
using LicenseServer.Domain.Utils;
using System.ComponentModel;

namespace LicenseServer.Domain.Methods
{
    public class LicensService
	{
		public async Task<HTTPResult<List<LicenseAPI.LicenseResponse>>> GetLicensesByOrgId(int orgId)
		{
			try
			{
                var errorResult = Validator.OrgIdValidation(orgId);

                if (errorResult.Any())
                    return HttpResults.LicensesResult.Fails(errorResult);

				var licenses = await DataGetter.APILicensesByOrganizationId(orgId);

                return HttpResults.LicensesResult.Success(licenses);
			}
			catch
			{
                return HttpResults.LicensesResult.Fail("Ошибка");
            }
		}

		public async Task<HTTPResult<List<LicenseAPI.LicenseResponse>>> GetLicensesByOrgIdWithProgId(int orgId, ProgramType programId) 
		{
			try
			{
				var errorResult = Validator.OrgIdAndProgIdValidation(orgId, programId);

				if (errorResult.Any())
					return HttpResults.LicensesResult.Fails(errorResult);

				var licenses = await DataGetter.APILicensesByOrganizationIdWithProgramId(orgId, programId);

                return HttpResults.LicensesResult.Success(licenses); 
			}
			catch
			{
                return HttpResults.LicensesResult.Fail("Ошибка");
            }
		}

		public async Task<HTTPResult<string>> CreateLicense(LicenseAPI.LicenseRequest licenseData)
		{
			try
			{
				var errorResult = Validator.LicenseValidation(licenseData);

                if (errorResult.Any())
                    return HttpResults.StringResult.Fails(errorResult);

                using var context = ApplicationContext.New;
                var currentLicense = await DataGetter.APILicenseToLicenseEntity(licenseData, context); 
                await DataManager.AddEntityAsync(currentLicense, context);

                return HttpResults.StringResult.Success("Лицензия создана успешно");
            }
			catch 
			{
                return HttpResults.StringResult.Fail("Ошибка");
            }
		}

		public async Task<HTTPResult<string>> DeleteLicenseById(int licenseId)
		{
			try
			{
				var idErrors = Validator.IsValidData(licenseId, "Не корректный Id лицензии");

                if (idErrors.Any())
                    return HttpResults.StringResult.Fails(idErrors);

                var license = await DataGetter.LicenseEntityById(licenseId);

                if (!Validator.isValidObject(license))
                    return HttpResults.StringResult.Fail("Указана не существующая лицензия");

				await DataManager.RemoveEntityAsync(license);

                return HttpResults.StringResult.Success("Данные удалены");
			}
			catch 
			{
                return HttpResults.StringResult.Fail("Ошибка");
            }
        
		}
	}
}