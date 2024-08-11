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
                    return HttpResults.StringResult.Fails(errorResult);

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

                return HttpResults.StringResult.Success("Тариф создан успешно"); 
			}
			catch
			{
                return HttpResults.StringResult.Fail("Ошибка");
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

                return HttpResults.TarifsResult.Success(tarif);
            }
			catch 
			{
                return HttpResults.TarifsResult.Fail("Ошибка");
            }
		}

		public async Task<HTTPResult<TarifAPI.TarifResponse>> GetTariffById(int tarifId)
		 {
			try
			{

                if (!Validator.isValidId(tarifId))
                    return HttpResults.TarifResult.Fail("Не существует тарифа с таким Id");

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

                return HttpResults.TarifResult.Success(currentTarif);
            }
			catch
			{
                return HttpResults.TarifResult.Fail("Ошибка");
            }
		}
	}
}
