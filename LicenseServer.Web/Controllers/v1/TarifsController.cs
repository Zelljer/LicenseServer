using LicenseServer.Database.Dependencies;
using LicenseServer.Domain.Methods;
using LicenseServer.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LicenseServer.Controllers.v1
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class TarifsController(ILogger<TarifsController> logger) : ControllerBase
	{
		private readonly TarifService _tarifService = new TarifService();
		private readonly ILogger<TarifsController> _logger = logger;

		[Authorize(Roles = $"{nameof(RoleType.Admin)},{nameof(RoleType.Admin)}")]
		[HttpPost("create")] // 0. POST Метод создания тарифа
		public async Task<ActionResult> CreateTarif(TarifAPI.TarifRequest tarif)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(new { Status = "Fail", Data = "Введите корректные данные" });

				var createdTarif = await _tarifService.CreateTarif(tarif);
				return CreatedAtAction(nameof(GetAllTarifs), createdTarif);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new {Status = "Fail", Data = "Произошла ошибка при выполнении запроса" });
			}
		}

		[HttpGet("tarifs")] // 1. GET Метод получения списка информации о тарифах, которые продает ГК ТриАр (вовзращать список)
		public async Task<ActionResult> GetAllTarifs()
		{
			try
			{
				var tarifs = await _tarifService.GetAllTarifs();
				return Ok(tarifs);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new { Status = "Fail", Data = "Произошла ошибка при выполнении запроса" });
			}
		}

		[HttpGet("tarifsId")] // 2. GET Метод получения списка информации 1 тарифе, которую продает ГК ТриАр (возвращать 1 запись по id )
		public async Task<ActionResult> GetTariffById(int tarifId)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(new { Status = "Fail", Data = "Введите корректные данные" });

				var tarifs = await _tarifService.GetTariffById(tarifId);
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
