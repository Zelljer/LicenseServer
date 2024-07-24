using LicenseServer.Database;
using LicenseServer.Database.Entity;
using LicenseServer.Database.Models;
using LicenseServer.Domain.Utils;
using LicenseServer.Models;
using Microsoft.EntityFrameworkCore;

namespace LicenseServer.Domain.Methods
{
	public class TarifService(ApplicationContext context)
	{
		private ApplicationContext _context = context;

		public async Task<IHTTPResult> CreateTarif(TarifAPI.TarifRequest tarif)
		{
			try
			{
				var errorResult = new Fail();

				if (!Enum.IsDefined(typeof(ProgramType), tarif.Program))
					errorResult.Data.Add("Указана не существующая прогрмма");

				if (tarif.Price < 0)
					errorResult.Data.Add("Указана не корректная цена");

				errorResult.Data
					.AddRange(Validator
					.IsValidData(tarif.DaysCount, "Укажите количество дней действия лицензии"));

				if (errorResult.Data.Any())
					return errorResult;

				TarifEntity currentTarif = new TarifEntity()
				{
					Name = tarif.Name,
					Program = tarif.Program,
					Price = tarif.Price,
					DaysCount = tarif.DaysCount
				};
				_context.Tarifs.Add(currentTarif);
				await _context.SaveChangesAsync();

				return new Success<TarifAPI.TarifRequest> { Data = tarif };
			}
			catch
			{
				return new Fail { Data = { "Произошла ошибка" } };
			}
		}

		public async Task<IHTTPResult> GetTarifs()
		{
			try
			{
				var tarif = await _context.Tarifs.Select(t => new TarifAPI.TarifResponse
				{
					Id = t.Id,
					Name = t.Name,
					Program = t.Program.ToString(),
					Price = t.Price,
					DaysCount = t.DaysCount,

				}).ToListAsync();

				return new Success<List<TarifAPI.TarifResponse>> { Data = tarif }; 
			}
			catch 
			{
				return new Fail { Data = { "Произошла ошибка" } };
			}
		}

		public async Task<IHTTPResult> GetTariffById(int id)
		{
			try
			{
				if (id <= 0)
					return new Success<TarifAPI.TarifResponse> { };

				var tarif = await _context.Tarifs.FindAsync(id);

				if (tarif == null)
					return new Success<TarifAPI.TarifResponse> { };

				var currentTarif = new TarifAPI.TarifResponse()
				{
					Id = tarif.Id,
					Name = tarif.Name,
					Program = tarif.Program.ToString(),
					Price = tarif.Price,
					DaysCount = tarif.DaysCount
				};

				return new Success<TarifAPI.TarifResponse> { Data = currentTarif };
			}
			catch
			{
				return new Fail { Data = { "Произошла ошибка" } };
			}
		}
	}
}
