﻿using LicenseServer.Database.Dependencies;
using LicenseServer.Domain.Methods;
using LicenseServer.Domain.Models;
using LicenseServer.Domain.Utils;
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

		[Authorize(Roles = $"{nameof(RoleType.Admin)},{nameof(RoleType.Manager)}")]
		[HttpPost("create")] // 0. POST Метод создания тарифа
		public async Task<ActionResult> CreateTarif(TarifAPI.TarifRequest tarif)
		{
			try
			{
				if (!ModelState.IsValid)
                    return ResponseResults.ErrorOkResult("Введите корректные данные");

                var createdTarif = await _tarifService.CreateTarif(tarif);
				return Ok(createdTarif);
			}
			catch (Exception ex)
			{
                return ResponseResults.ErrorBadResult("Произошла ошибка при выполнении запроса", logger, ex);
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
                return ResponseResults.ErrorBadResult("Произошла ошибка при выполнении запроса", logger, ex);
            }
		}

		[HttpGet("tarifsId")] // 2. GET Метод получения списка информации 1 тарифе, которую продает ГК ТриАр (возвращать 1 запись по id )
		public async Task<ActionResult> GetTariffById(int tarifId)
		{
			try
			{
				if (!ModelState.IsValid)
                    return ResponseResults.ErrorOkResult("Введите корректные данные");

                var tarifs = await _tarifService.GetTariffById(tarifId);
				return Ok(tarifs);
			}
			catch (Exception ex)
			{
                return ResponseResults.ErrorBadResult("Произошла ошибка при выполнении запроса", logger, ex);
            }
		}
	}
}
