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
        [HttpGet]
        public async Task<ApiResponse> Login(string Account, string PassWord) => await loginServer.LoginAsync(Account, PassWord);

        [HttpPost]
        public async Task<ApiResponse> Register([FromBody] UserDto userDto) => await loginServer.Register(userDto);
    }
}
