using LicenseServer.Database;
using LicenseServer.Models;
using LicenseServer.Models.API;
using LicenseServer.Models.Database;
using LicenseServer.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LicenseServer.Controllers.v1
{
	[ApiController]
	[Route("api/[controller]")]
	public class TarifsController(ApplicationContext context, ILogger<TarifsController> logger) : ControllerBase
	{
		private readonly ApplicationContext _context = context;
		private readonly ILogger<TarifsController> _logger = logger;

		[HttpPost("create")] // 0. POST Метод создания тарифа
		public async Task<ActionResult> CreateTarif(TarifAPI.TarifRequest tarif)
		{
			try
			{
				var errorResult = new Result.Fail();

				if (!Enum.IsDefined(typeof(ProgramType), tarif.Program))
					errorResult.Data.Add("Указана не существующая прогрмма");

				if (tarif.Price < 0)
					errorResult.Data.Add("Указана не корректная цена");

				errorResult.Data
					.AddRange(Validator
					.IsValidData(tarif.DaysCount, "Укажите количество дней действия лицензии"));

				if (errorResult.Data.Any())
					return BadRequest(errorResult);

				TarifEntity currentTarif = new TarifEntity()
				{
					Name = tarif.Name,
					Program = tarif.Program,
					Price = tarif.Price,
					DaysCount = tarif.DaysCount
				};
				_context.Tarifs.Add(currentTarif);
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(GetTarifs), new { id = currentTarif.Id }, new Result.Success<TarifEntity> { Data = currentTarif });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new Result.Fail() { Data = { "Ошибка при выполнении запроса" } }); 
			}
		}

		[HttpGet("tarifs")] // 1. GET Метод получения списка информации о тарифах, которые продает ГК ТриАр (вовзращать список)
		public async Task<ActionResult> GetTarifs()
		{
			try
			{
				var tariff =await  _context.Tarifs.Select(t => new TarifAPI.TarifResponse
				{
					Id = t.Id,
					Name = t.Name,
					Program = t.Program.ToString(),
					Price = t.Price,
					DaysCount = t.DaysCount,
					
				}).ToListAsync();

				if (!tariff.Any())
						return Ok(new Result.Success<string> { });

				return Ok(new Result.Success<IEnumerable<TarifAPI.TarifResponse>> { Data = tariff });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new Result.Fail() { Data = { "Ошибка при выполнении запроса" } }); 
			}
		}

		[HttpGet("tarifsId")] // 2. GET Метод получения списка информации 1 тарифе, которую продает ГК ТриАр (возвращать 1 запись по id )
		public async Task<ActionResult> GetTariffById(int id)
		{
			try
			{
				if (id <= 0)
					return BadRequest(new Result.Fail() { Data = { "Укажите корректный Id тарифа" } });

				var tarif = await _context.Tarifs.FindAsync(id);
				if (tarif == null)
					return Ok(new Result.Success<string> { });

				TarifAPI.TarifResponse currentTarif = new TarifAPI.TarifResponse()
				{
					Id = tarif.Id,
					Name = tarif.Name,
					Program = tarif.Program.ToString(),
					Price = tarif.Price,
					DaysCount = tarif.DaysCount
				};

				return Ok(new Result.Success<TarifAPI.TarifResponse>() { Data = currentTarif }) ;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new Result.Fail() { Data = { "Ошибка при выполнении запроса" } });  
			}
		}
	}
}
