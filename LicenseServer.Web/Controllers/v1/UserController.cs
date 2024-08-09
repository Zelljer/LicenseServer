﻿using LicenseServer.Domain.Methods;
using LicenseServer.Domain.Models;
using LicenseServer.Domain.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LicenseServer.Web.Controllers.v1
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly UserService _userService;
		private readonly ILogger<UserController> _logger;
		public UserController(ILogger<UserController> logger, UserService userService)
		{
			_userService = userService;
			_logger = logger;
		}

		[HttpPost("authorization")] // POST Метод авторизация 
		public async Task<ActionResult> UserLogin(UserAPI.UserAuthentificationRequest user)
		{
			try
			{
                if (!ModelState.IsValid)
                    return ResponseResults.ErrorOkResult("Введите корректные данные");

                var authorizatedUser = await _userService.UserLogin(user);
				return Ok(authorizatedUser); 
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
                return ResponseResults.ErrorBadResult("Произошла ошибка при выполнении запроса");
            }
		}

		[HttpPost("registration")] // POST Метод регистрации
		public async Task<ActionResult> UserRegistration(UserAPI.UserRegistrationRequest user)
		{
			try
			{
                if (!ModelState.IsValid)
                    return ResponseResults.ErrorOkResult("Введите корректные данные");

                var registeredUser = await _userService.UserRegistration(user);
				return Ok(registeredUser);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
                return ResponseResults.ErrorBadResult("Произошла ошибка при выполнении запроса");
            }
		}

		[HttpGet("check-token")] // POST Метод проверкитокена
		public IActionResult CheckToken()
		{
			var token = Request.Cookies["Authorization"];

			if (string.IsNullOrEmpty(token))
                return ResponseResults.ErrorOkResult("Токен не найден в куки");

			return ResponseResults.SuccessResult(token);
		}
	}
}
