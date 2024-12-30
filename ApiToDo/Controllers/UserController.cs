using Microsoft.AspNetCore.Mvc;
using System.Net;
using ToDo.Application.Contracts.Services;
using ToDo.Application.DTOs;

namespace ToDo.Api.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService _serviceUser;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService serviceUser, ITokenService tokenService, ILogger<UserController> logger)
        {
            _serviceUser = serviceUser;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> Create(UserRequest user)
        {
            try
            {
                var userNew = await _serviceUser.Create(user);
                return CustomResponse((int)HttpStatusCode.Created, true, userNew);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), user);
                return CustomResponse((int)HttpStatusCode.BadRequest,false, ex);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(UserLoginRequest userLoginRequest)
        {
            try
            {
                var userRequest = new UserRequest { IsLogin = true, Email = userLoginRequest.Email, Password = userLoginRequest.Password };
                var token = await _tokenService.GenerateJwtToken(userRequest);
                return CustomResponse((int)HttpStatusCode.OK, true, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), userLoginRequest);
                return CustomResponse((int)HttpStatusCode.BadRequest, false, ex.Message.ToString());
            }
        }

    }
}