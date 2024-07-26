using LicenseServer.Database;
using LicenseServer.Database.Dependencies;
using LicenseServer.Database.Entity;
using LicenseServer.Domain.Models;
using LicenseServer.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LicenseServer.Domain.Methods
{
	public class LicensService
	{
		public async Task<IHTTPResult> GetLicensesByOrg(int orgId)
		{
			try
			{
				using (var context = ApplicationContext.New)
				{
					if (orgId <= 0)
						return new Fail { Data = { "Не корректный Id организации" } };

					var licenses = await context.Licenses
						.Where(l => l.Organization.Id == orgId)
						.Select(t => new LicenseAPI.LicenseResponse
						{
							Id = t.Id,
							OrganizationId = t.Organization.Id,
							TarifId = t.Tarif.Id,
							DateCreated = t.DateCreated,
							StartDate = t.StartDate,
							EndDate = t.EndDate,
						}).ToListAsync();

					return new Success<List<LicenseAPI.LicenseResponse>> { Data = licenses };
				}
			}
			catch
			{
				return new Fail { Data = { "Произошла ошибка" } };
			}
		}

		public async Task<IHTTPResult> GetLicensesByOrgWithProg(int orgId, ProgramType programId) 
		{
			try
			{
				using (var context = ApplicationContext.New)
				{
					var errorResult = new Fail();

					errorResult.Data.AddRange(Validator.IsValidData(orgId, "Не корректный Id организации"));

					if (context.Organizations
						.Find(orgId) == null)
						errorResult.Data.Add("Нет организации с таким Id");

					if (!Enum.IsDefined(typeof(ProgramType), programId))
						errorResult.Data.Add("Указана не существующая программа");

					if (errorResult.Data.Any())
						return errorResult;

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

					return new Success<List<LicenseAPI.LicenseResponse>> { Data = licenses };
				}
			}
			catch
			{
				return new Fail{ Data = { "Произошла ошибка" } };
			}
		}
		
		public async Task<IHTTPResult> CreateLicense(LicenseAPI.LicenseRequest licenseData)
		{
			try
			{
				using (var context = ApplicationContext.New)
				{
					var errorIdResult = new Fail();

					errorIdResult.Data
						.AddRange(Validator
						.IsValidData(licenseData.OrganizationId, "Не корректный Id организации"));

					if (!context.Organizations
						.Where(o => o.Id == licenseData.OrganizationId)
						.Any())
						errorIdResult.Data.Add("Нет организации с таким Id");

					errorIdResult.Data
						.AddRange(Validator
						.IsValidData(licenseData.TarifId, "Не корректный Id тарифа"));

					if (!context.Tarifs
						.Where(t => t.Id == licenseData.TarifId)
						.Any())
						errorIdResult.Data.Add("Нет тарифа с таким Id");

					var currentLicenseDateStart = new DateTime();
					if (DateTime.TryParseExact(licenseData.DateStart, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime daeteTime))
						currentLicenseDateStart = daeteTime;
					else
						errorIdResult.Data.Add("Введите дату создания лицензии в формате dd.mm.yyyy");

					var neededOrganization = context.Organizations.Find(licenseData.OrganizationId);
					var neededTarif = context.Tarifs.Find(licenseData.TarifId);

					if (neededOrganization == null || neededTarif == null)
						errorIdResult.Data.Add("В базе нет данных о организациях или тарифах");

					if (errorIdResult.Data.Any())
						return errorIdResult;

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

					return new Success<LicenseAPI.LicenseRequest> { Data = licenseData };
				}
			}
			catch
			{
				return new Fail { Data = { "Произошла ошибка" } };
			}
		}

		public async Task<IHTTPResult> DeleteLicense(int id)
		{
			try
			{
				using (var context = ApplicationContext.New)
				{
					if (id <= 0)
						return new Fail { Data = { "Не корректный Id лицензии" } };

					var license = await context.Licenses.FindAsync(id);

					if (license == null)
						return new Fail { Data = { "Указана не существующая лицензия" } };

					context.Licenses.Remove(license);
					await context.SaveChangesAsync();

					return new Success<string> { Data = "Данные удалены" };
				}
			}
			catch 
			{
				return new Fail { Data = { "Произошла ошибка" } };
			}
		}
	}
}