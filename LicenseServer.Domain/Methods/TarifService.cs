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

		public async Task<HTTPResult<string>> CreateTarif(TarifAPI.TarifRequest tarif)
		{
			try
			{

				var errorResult = new List<string>();

				if (!Enum.IsDefined(typeof(ProgramType), tarif.Program))
					errorResult.Add("Не существующая прогрмма");

				if (tarif.Price < 0)
					errorResult.Add("Не корректная цена");

				errorResult
					.AddRange(Validator
					.IsValidData(tarif.DaysCount, "Укажите количество дней действия лицензии"));

				if (errorResult.Any())
                    return new HTTPResult<string> { Errors = errorResult, IsSuccsess = false };

                TarifEntity currentTarif = new TarifEntity()
				{
					Name = tarif.Name,
					Program = tarif.Program,
					Price = tarif.Price,
					DaysCount = tarif.DaysCount
				};

                using var context = ApplicationContext.New;
                context.Tarifs.Add(currentTarif);
				await context.SaveChangesAsync();

                return new HTTPResult<string> { IsSuccsess = true, Data = "Тариф создан успешно" }; 
			}
			catch
			{
                return new HTTPResult<string> { Errors = new() { "Ошибка" }, IsSuccsess = false };
            }
		}

		public async Task<HTTPResult<List<TarifAPI.TarifResponse>>> GetAllTarifs()
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

                return new HTTPResult<List<TarifAPI.TarifResponse>> { IsSuccsess = true, Data = tarif };
            }
			catch 
			{
                return new HTTPResult<List<TarifAPI.TarifResponse>> { Errors = new() { "Ошибка" }, IsSuccsess = false };
            }
		}

		public async Task<HTTPResult<TarifAPI.TarifResponse>> GetTariffById(int tarifId)
		 {
			try
			{

                if (tarifId <= 0)
                    return new HTTPResult<TarifAPI.TarifResponse> { Errors = new() { "Не существует тарифа с таким Id" }, IsSuccsess = false };

                using var context = ApplicationContext.New;
                var tarif = await context.Tarifs.FindAsync(tarifId);

                if (tarif == null)
                    return new HTTPResult<TarifAPI.TarifResponse> { IsSuccsess = true };

                var currentTarif = new TarifAPI.TarifResponse()
                {
                    Id = tarif.Id,
                    Name = tarif.Name,
                    Program = tarif.Program.ToString(),
                    Price = tarif.Price,
                    DaysCount = tarif.DaysCount
                };

                return new HTTPResult<TarifAPI.TarifResponse> { IsSuccsess = true, Data = currentTarif };
            }
			catch
			{
                return new HTTPResult<TarifAPI.TarifResponse> { Errors = new() { "Ошибка" }, IsSuccsess = false};
            }
		}
	}
}
