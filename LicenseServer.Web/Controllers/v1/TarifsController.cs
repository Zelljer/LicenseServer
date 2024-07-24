using LicenseServer.Database.Models;
using LicenseServer.Domain.Methods;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LicenseServer.Controllers.v1
{
	[ApiController]
	[Route("api/[controller]")]
	public class TarifsController(TarifService tarifService) : ControllerBase
	{
		private readonly TarifService _tarifService = tarifService;

		[HttpPost("create")] // 0. POST Метод создания тарифа
		public async Task<ActionResult> CreateTarif(TarifAPI.TarifRequest tarif)
		{
			try
			{
				var createdTarif = await _tarifService.CreateTarif(tarif);
				return CreatedAtAction(nameof(GetTarifs), createdTarif);
			}
			catch //(Exception ex)
			{
				return null;
				//_logger.LogError(ex.Message);
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
			catch //(Exception ex)
			{
				return null;
				//_logger.LogError(ex.Message);
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
			catch //(Exception ex)
			{
				return null;
				//_logger.LogError(ex.Message);
			}
		}
	}
}
