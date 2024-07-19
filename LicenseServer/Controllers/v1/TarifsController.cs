using LicenseServer.Database;
using LicenseServer.Models.API;
using LicenseServer.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LicenseServer.Controllers.v1
{
    [ApiController]
	[Route("api/[controller]")]
	public class TarifsController : ControllerBase
	{
		private readonly ApplicationContext _context;
		public TarifsController(ApplicationContext context) => _context = context;

		[HttpPost("create")] // 0. POST Метод создания тарифа
		public async Task<ActionResult<Tarif>> CreateTarif(TarifAPI.TarifRequest tarif)
		{
			try
			{
				var errorResult = new Result.Fail();
				if (!Enum.IsDefined(typeof(ProgramType),tarif.Program))
					errorResult.Data.Add("Указана не существующая программа");
				if (tarif.DaysCount <= 0)
					errorResult.Data.Add("Количество дней не может быть меньше или равняться 0");
				if (errorResult.Data.Any())
					return BadRequest(errorResult);

				Tarif currentTarif = new Tarif()
				{
					Name = tarif.Name,
					Program = tarif.Program,
					Price = tarif.Price,
					DaysCount = tarif.DaysCount
				};
				_context.Tarifs.Add(currentTarif);
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(GetTarifs), new { id = currentTarif.Id }, new Result.Success<Tarif> { Data = currentTarif });
			}
			catch { return BadRequest(new Result.Fail() { Data = { "Ошибка при выпролнении запроса" } }); }
		}

		[HttpGet("tarifs")] // 1. GET Метод получения списка информации о тарифах, которые продает ГК ТриАр (вовзращать список)
		public async Task<ActionResult<IEnumerable<TarifAPI.TarifResponse>>> GetTarifs()
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
					return Ok(new Result.Success<string> { Data = "В базе нет данных" });

				return Ok(new Result.Success<IEnumerable<TarifAPI.TarifResponse>> { Data = tariff });
			}
			catch { return BadRequest(new Result.Fail() { Data = { "Ошибка при выпролнении запроса" } }); }
		}

		[HttpGet("tarifsId")] // 2. GET Метод получения списка информации 1 тарифе, которую продает ГК ТриАр (возвращать 1 запись по id )
		public async Task<ActionResult<TarifAPI.TarifResponse>> GetTariffById(int id)
		{
			try
			{
				if (id <= 0)
					return BadRequest(new Result.Fail() { Data = { "Укажите корректный Id тарифа" } });

				var tarif = await _context.Tarifs.FindAsync(id);
				if (tarif == null)
					return Ok(new Result.Success<string> { Data = "В базе нет данных о тарифе с указанным Id" });

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
			catch { return BadRequest(new Result.Fail() { Data = { "Ошибка при выпролнении запроса" } }); }
		}
	}
}
