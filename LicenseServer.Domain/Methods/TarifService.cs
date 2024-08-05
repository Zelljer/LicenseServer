using LicenseServer.Database;
using LicenseServer.Database.Dependencies;
using LicenseServer.Database.Entity;
using LicenseServer.Domain.Models;
using LicenseServer.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace LicenseServer.Domain.Methods
{
	public class TarifService
	{

		public async Task<TestResult<string>> CreateTarif(TarifAPI.TarifRequest tarif)
		{
			try
			{
				using var context = ApplicationContext.New;
				var errorResult = new List<string>();

				if (!Enum.IsDefined(typeof(ProgramType), tarif.Program))
					errorResult.Add("Не существующая прогрмма");

				if (tarif.Price < 0)
					errorResult.Add("Не корректная цена");

				errorResult
					.AddRange(Validator
					.IsValidData(tarif.DaysCount, "Укажите количество дней действия лицензии"));

				if (errorResult.Any())
                    return new TestResult<string> { Errors = errorResult, IsSuccsess = false };

                TarifEntity currentTarif = new TarifEntity()
				{
					Name = tarif.Name,
					Program = tarif.Program,
					Price = tarif.Price,
					DaysCount = tarif.DaysCount
				};

				context.Tarifs.Add(currentTarif);
				await context.SaveChangesAsync();

                return new TestResult<string> { IsSuccsess = true, Data = "Тариф создан успешно" }; 
			}
			catch
			{
                return new TestResult<string> { Errors = new() { "Ошибка" }, IsSuccsess = false };
            }
		}

		public async Task<TestResult<List<TarifAPI.TarifResponse>>> GetAllTarifs()
		{
			try
			{
				using var context = ApplicationContext.New;
				var tarif = await context.Tarifs.Select(t => new TarifAPI.TarifResponse
				{
					Id = t.Id,
					Name = t.Name,
					Program = t.Program.ToString(),
					Price = t.Price,
					DaysCount = t.DaysCount,

				}).ToListAsync();

                return new TestResult<List<TarifAPI.TarifResponse>> { IsSuccsess = true, Data = tarif };
            }
			catch 
			{
                return new TestResult<List<TarifAPI.TarifResponse>> { Errors = new() { "Ошибка" }, IsSuccsess = false };
            }
		}

		public async Task<TestResult<TarifAPI.TarifResponse>> GetTariffById(int tarifId)
		 {
			try
			{
				using (var context = ApplicationContext.New)
				{
					if (tarifId <= 0)
						return new TestResult<TarifAPI.TarifResponse> { Errors = new() { "Не существует тарифа с таким Id" }, IsSuccsess=false };

					var tarif = await context.Tarifs.FindAsync(tarifId);

					if (tarif == null)
                        return new TestResult<TarifAPI.TarifResponse> { IsSuccsess = true, Data = new TarifAPI.TarifResponse()};

                    var currentTarif = new TarifAPI.TarifResponse()
					{
						Id = tarif.Id,
						Name = tarif.Name,
						Program = tarif.Program.ToString(),
						Price = tarif.Price,
						DaysCount = tarif.DaysCount
					};

					return new TestResult<TarifAPI.TarifResponse> { IsSuccsess = true, Data = currentTarif };
				}
			}
			catch
			{
                return new TestResult<TarifAPI.TarifResponse> { Errors = new() { "Ошибка" }, IsSuccsess = false};
            }
		}
	}
}
