using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Models;
using MyToDo.Api.Service;
using MyToDo.Share.Dtos;
using MyToDo.Share.Parameters;

namespace MyToDo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MemoController : ControllerBase
    {
        private readonly IMemoService memoService;

        public MemoController(IMemoService memoService)
        {
            this.memoService = memoService;
        }

        [HttpGet]
        public async Task<ApiResponse> Get(int id) => await memoService.GetSingleAsync(id);
        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] TodoParameter parameter) => await memoService.GetAllAsync(parameter);
        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] MemoDto memo) => await memoService.AddAsync(memo);
        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] MemoDto memo) => await memoService.UpdateAsync(memo);
        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) => await memoService.DeleteAsync(id);
    }
}
