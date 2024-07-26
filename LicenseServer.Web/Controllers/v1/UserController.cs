﻿using LicenseServer.Domain.Methods;
using LicenseServer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LicenseServer.Web.Controllers.v1
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly UserService _userService;
		private readonly ILogger<UserController> _logger;
		public UserController(ILogger<UserController> logger)
		{
			_userService = new UserService();
			_logger = logger;
		}

		[HttpPost("authorization")] // POST Метод авторизация 
		public async Task<ActionResult> Register(UserAPI.UserAuthentificationRequest user)
		{
			try
			{
				var authorizatedUser = await _userService.Login(user);
				return Ok(authorizatedUser);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new { Status = "Fail", Data = "Произошла ошибка при выполнении запроса" });
			}
		}

		[HttpPost("registration")] // POST Метод регистрации
		public async Task<ActionResult> Register(UserAPI.UserRegistrationRequest user)
		{
			try
			{
				var registeredUser = await _userService.Register(user);
				return Ok(registeredUser);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new { Status = "Fail", Data = "Произошла ошибка при выполнении запроса" });
			}
		}
	}
}
