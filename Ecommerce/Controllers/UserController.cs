using Ecommerce.DTOs;
using Ecommerce.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService<UserDto, UserInsertDto, UserUpdateDto> _userService;
        private IUserLoginService<UserLoginDto> _userLoginService;

        public UserController(IUserService<UserDto, UserInsertDto, UserUpdateDto> userService,
            IUserLoginService<UserLoginDto> userLoginService)
        {
            _userService = userService;
            _userLoginService = userLoginService;
        }
        
        [HttpGet("login")]
        public ActionResult<string> Login(UserLoginDto userLoginDto)
        {
            var token = _userLoginService.Authenticate(userLoginDto);
            
            return token != null ? Ok(token) : NotFound();
        }
    }
}
