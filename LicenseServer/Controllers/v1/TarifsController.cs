using LicenseServer.Database;
using LicenseServer.Models.API;
using LicenseServer.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LicenseServer.Controllers.v1
{
    [ApiController]
	[Route("api/[controller]")]
	public class TarifsController : ControllerBase
	{
		private readonly ApplicationContext _context;
		public TarifsController(ApplicationContext context) => _context = context;

		[HttpPost("create")] // 0. POST Метод создания тарифа
		public async Task<ActionResult<Tarif>> CreateTarif(TarifAPI tarif)
		{
			try
			{
				Tarif currentTarif = new Tarif()
				{
					Name = tarif.Name,
					Program = tarif.Program,
					Price = tarif.Price,
					DaysCount = tarif.DaysCount
				};
				_context.Tarifs.Add(currentTarif);
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(GetTarifs), new { id = currentTarif.Id }, new Result.Success<Tarif> { Status = "Success", Data = currentTarif });
			}
			catch { return BadRequest(new Result.Fail()); }
		}

		[HttpGet("tarifs")] // 1. GET Метод получения списка информации о тарифах, которые продает ГК ТриАр (вовзращать список)
		public async Task<ActionResult<IEnumerable<Tarif>>> GetTarifs()
		{
			try
			{
				var tariff = await _context.Tarifs.ToListAsync();
				return Ok(new Result.Success<IEnumerable<Tarif>> { Status = "Success", Data = tariff });
			}
			catch { return BadRequest(new Result.Fail()); }
		}

		[HttpGet("tarifs/{id}")] // 2. GET Метод получения списка информации 1 тарифе, которую продает ГК ТриАр (возвращать 1 запись по id )
		public async Task<ActionResult<Tarif>> GetTariffById(int id)
		{
			try
			{
				if (id == 0)
					return BadRequest(new Result.Fail());

				var tariff = await _context.Tarifs.FindAsync(id);
				return Ok(new Result.Success<Tarif>() { Status = "Success", Data = tariff }) ;
			}
			catch { return BadRequest(new Result.Fail()); }
		}
	}
}
