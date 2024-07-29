using LicenseServer.Domain.Methods;
using LicenseServer.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LicenseServer.Controllers.v1
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class TarifsController : ControllerBase
	{
		private readonly TarifService _tarifService;
		private readonly ILogger<TarifsController> _logger;
		public TarifsController(ILogger<TarifsController> logger)
		{
			_tarifService = new TarifService();
			_logger = logger;
		}

		[HttpPost("create")] // 0. POST Метод создания тарифа
		public async Task<ActionResult> CreateTarif(TarifAPI.TarifRequest tarif)
		{
			try
			{
				var createdTarif = await _tarifService.CreateTarif(tarif);
				return CreatedAtAction(nameof(GetTarifs), createdTarif);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new {Status = "Fail", Data = "Произошла ошибка при выполнении запроса" });
			}
		}

		[HttpGet("tarifs")] // 1. GET Метод получения списка информации о тарифах, которые продает ГК ТриАр (вовзращать список)
		public async Task<ActionResult> GetTarifs()
		{
			try
			{
				var tarifs = await _tarifService.GetTarifs();
				return Ok(tarifs);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new { Status = "Fail", Data = "Произошла ошибка при выполнении запроса" });
			}
		}

		[HttpGet("tarifsId")] // 2. GET Метод получения списка информации 1 тарифе, которую продает ГК ТриАр (возвращать 1 запись по id )
		public async Task<ActionResult> GetTariffById(int id)
		{
			try
			{
				var tarifs = await _tarifService.GetTariffById(id);
				return Ok(tarifs);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new { Status = "Fail", Data = "Произошла ошибка при выполнении запроса" });
			}
		}
	}
}
