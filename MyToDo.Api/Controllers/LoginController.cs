using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Service;
using MyToDo.Share.Dtos;

namespace MyToDo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginServer loginServer;

        public LoginController(ILoginServer loginServer)
        {
            this.loginServer = loginServer;
        }
        [HttpPost]
        public async Task<ApiResponse> Login([FromBody] UserDto param) => await loginServer.LoginAsync(param.Account, param.PassWord);

        [HttpPost]
        public async Task<ApiResponse> Register([FromBody] UserDto userDto) => await loginServer.Register(userDto);
    }
}
