using LicenseServer.Database;
using LicenseServer.Database.Dependencies;
using LicenseServer.Database.Entity;
using LicenseServer.Domain.Models;
using LicenseServer.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace LicenseServer.Domain.Methods
{
    public class LicensService
	{
		public async Task<HTTPResult<List<LicenseAPI.LicenseResponse>>> GetLicensesByOrgId(int orgId)
		{
			try
			{
                if (!Validator.isValidId(orgId))
                    return HttpResults.LicensesResult.Fail("Не корректный Id организации");

                using var context = ApplicationContext.New;
				var licenses = await context.Licenses
					.Where(l => l.Organization.Id == orgId && l.EndDate > DateTime.Now)
					.Select(t => new LicenseAPI.LicenseResponse
					{
						Id = t.Id,
						OrganizationId = t.Organization.Id,
						TarifId = t.Tarif.Id,
						DateCreated = t.DateCreated,
						StartDate = t.StartDate,
						EndDate = t.EndDate,
					}).ToListAsync();

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
				using var context = ApplicationContext.New;
				var errorResult = new List<string>();

				errorResult.AddRange(Validator.IsValidData(orgId, "Не корректный Id организации"));

				if (context.Organizations.FirstOrDefault(x => x.Id == orgId) == null)
					errorResult.Add("Нет организации с таким Id");

				if (!Enum.IsDefined(typeof(ProgramType), programId))
					errorResult.Add("Указана не существующая программа");

				if (errorResult.Any())
					return HttpResults.LicensesResult.Fails(errorResult);

				var licenses = await context.Licenses
					.Include(l => l.Organization)
					.Include(l => l.Tarif)
					.Where(l => l.Organization.Id == orgId && l.Tarif.Program == programId && l.EndDate > DateTime.Now)
					.Select(t => new LicenseAPI.LicenseResponse
					{
						Id = t.Id,
						OrganizationId = t.Organization.Id,
						TarifId = t.Tarif.Id,
						DateCreated = t.DateCreated,
						StartDate = t.StartDate,
						EndDate = t.EndDate,
					}).ToListAsync();

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
				using var context = ApplicationContext.New;
				var errorResult = new List<string>();

				errorResult
					.AddRange(Validator
					.IsValidData(licenseData.OrganizationId, "Не корректный Id организации"));

				errorResult
					.AddRange(Validator
					.IsValidData(licenseData.TarifId, "Не корректный Id тарифа"));

				var neededOrganization = context.Organizations.Find(licenseData.OrganizationId);
				var neededTarif = context.Tarifs.Find(licenseData.TarifId);

				if (neededOrganization == null)
					errorResult.Add("Нет организации с таким Id");

				if (neededTarif == null)
					errorResult.Add("Нет тарифа с таким Id");

                errorResult
                    .AddRange(Validator
                    .IsValidDate(licenseData.DateStart));

                if (errorResult.Any())
                    return HttpResults.StringResult.Fails(errorResult);

                var currentLicense = new LicenseEntity
				{
					Organization = neededOrganization,
					Tarif = neededTarif,
					DateCreated = DateTime.Now,
					StartDate = DateTime.Parse(licenseData.DateStart),
					EndDate = DateTime.Parse(licenseData.DateStart).AddDays(neededTarif.DaysCount),
				};
				context.Licenses.Add(currentLicense);
				await context.SaveChangesAsync();

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
                if (!Validator.isValidId(licenseId))
                    return HttpResults.StringResult.Fail("Не корректный Id лицензии");

                using var context = ApplicationContext.New;

                var license = await context.Licenses.FindAsync(licenseId);

				if (license == null)
                    return HttpResults.StringResult.Fail("Указана не существующая лицензия");

				context.Licenses.Remove(license);
				await context.SaveChangesAsync();

                return HttpResults.StringResult.Success("Данные удалены");
			}
			catch 
			{
                return HttpResults.StringResult.Fail("Ошибка");
            }
        
		}
	}
}