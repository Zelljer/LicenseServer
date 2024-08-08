using LicenseServer.Database;
using LicenseServer.Database.Dependencies;
using LicenseServer.Database.Entity;
using LicenseServer.Domain.Models;
using LicenseServer.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Globalization;

namespace LicenseServer.Domain.Methods
{
	public class LicensService
	{
		public async Task<HTTPResult<List<LicenseAPI.LicenseResponse>>> GetLicensesByOrgId(int orgId)
		{
			try
			{
				using var context = ApplicationContext.New;
				if (orgId <= 0)
                    return new HTTPResult<List<LicenseAPI.LicenseResponse>> { Errors = new() { "Не корректный Id организации" }, IsSuccsess = false };

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

                return new HTTPResult<List<LicenseAPI.LicenseResponse>> { IsSuccsess = true, Data = licenses };
			}
			catch
			{
                return new HTTPResult<List<LicenseAPI.LicenseResponse>> { Errors = new() { "Ошибка" }, IsSuccsess = false };
            }
		}

		public async Task<HTTPResult<List<LicenseAPI.LicenseResponse>>> GetLicensesByOrgIdWithProgId(int orgId, ProgramType programId) 
		{
			try
			{
				using var context = ApplicationContext.New;
				var errorResult = new List<string>();

				errorResult.AddRange(Validator.IsValidData(orgId, "Не корректный Id организации"));

				if (context.Organizations.Find(orgId) == null)
					errorResult.Add("Нет организации с таким Id");

				if (!Enum.IsDefined(typeof(ProgramType), programId))
					errorResult.Add("Указана не существующая программа");

				if (errorResult.Any())
					return new HTTPResult<List<LicenseAPI.LicenseResponse>> { Errors = errorResult, IsSuccsess = false };

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

                return new HTTPResult<List<LicenseAPI.LicenseResponse>> { IsSuccsess = true, Data = licenses }; 
			}
			catch
			{
                return new HTTPResult<List<LicenseAPI.LicenseResponse>> { Errors = new() { "Ошибка" }, IsSuccsess = false };
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

				var currentLicenseDateStart = new DateTime();
				if (DateTime.TryParseExact(licenseData.DateStart, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime daeteTime))
					currentLicenseDateStart = daeteTime;
				else
					errorResult.Add("Введите дату создания лицензии в формате dd.mm.yyyy");

				if (errorResult.Any())
                    return new HTTPResult<string> { Errors = errorResult, IsSuccsess = false };

                var currentLicense = new LicenseEntity
				{
					Organization = neededOrganization,
					Tarif = neededTarif,
					DateCreated = DateTime.Now,
					StartDate = currentLicenseDateStart,
					EndDate = currentLicenseDateStart + TimeSpan.FromDays(neededTarif.DaysCount),
				};
				context.Licenses.Add(currentLicense);
				await context.SaveChangesAsync();

                return new HTTPResult<string> { IsSuccsess = true, Data = "Лицензия создана успешно" };
            }
			catch
			{
                return new HTTPResult<string> { Errors = new() { "Ошибка" }, IsSuccsess = false };
            }
		}

		public async Task<HTTPResult<string>> DeleteLicenseById(int licenseId)
		{
			try
			{
				using var context = ApplicationContext.New;
				if (licenseId <= 0)
					return new HTTPResult<string> { Errors = new() { "Не корректный Id лицензии" }, IsSuccsess = false };

                var license = await context.Licenses.FindAsync(licenseId);

				if (license == null)
                    return new HTTPResult<string> { Errors = new() { "Указана не существующая лицензия" }, IsSuccsess = false };

				context.Licenses.Remove(license);
				await context.SaveChangesAsync();

                return new HTTPResult<string> { IsSuccsess = true, Data = "Данные удалены" };
			}
			catch 
			{
                return new HTTPResult<string> { Errors = new() { "Ошибка" }, IsSuccsess = false };
            }
        
		}
	}
}