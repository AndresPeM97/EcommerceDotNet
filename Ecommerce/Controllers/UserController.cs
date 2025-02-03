using System.Security.Claims;
using Ecommerce.DTOs;
using Ecommerce.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserLoginService<UserLoginDto> _userLoginService;

        public UserController(IUserLoginService<UserLoginDto> userLoginService)
        {
            _userLoginService = userLoginService;
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<TokenDto>> UserLogin(UserLoginDto userLoginDto)
        {
            var token = await _userLoginService.Authenticate(userLoginDto);
            
            return token != null ? Ok(token) : NotFound();
        }

        [HttpPost("register/user")]
        public async Task<ActionResult<UserDto>> UserRegister(UserInsertDto userInsertDto)
        {
            var userDto = await _userLoginService.UserRegister(userInsertDto);
            
            return userDto != null ? Ok(userDto) : BadRequest(_userLoginService.Errors);
        }
        [HttpPost("register/customer")]
        public async Task<ActionResult<UserDto>> CustomerRegister(UserInsertDto userInsertDto)
        {
            var userDto = await _userLoginService.CustomerRegister(userInsertDto);
            
            return userDto != null ? Ok(userDto) : BadRequest(_userLoginService.Errors);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var userDto = await _userLoginService.UserGetInfo(userEmail);
            return userDto != null ? Ok(userDto) : BadRequest(_userLoginService.Errors);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<UserDto>> UserUpdate(UserUpdateDto userUpdateDto)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var userDto = await _userLoginService.UserUpdateInfo(userUpdateDto, userEmail);
            
            return userDto != null ? Ok(userDto) : BadRequest(_userLoginService.Errors);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<ActionResult<UserDto>> UserDelete()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var userDto = await _userLoginService.UserDelete(userEmail);
            
            return userDto != null ? Ok(userDto) : BadRequest(_userLoginService.Errors);
        }
    }
}
