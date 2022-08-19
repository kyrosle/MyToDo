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
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService toDoService;

        public ToDoController(IToDoService toDoService)
        {
            this.toDoService = toDoService;
        }

        [HttpGet]
        public async Task<ApiResponse> Get(int id) => await toDoService.GetSingleAsync(id);
        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] TodoParameter parameter) => await toDoService.GetAllAsync(parameter);
        [HttpGet]
        public async Task<ApiResponse> Summary() => await toDoService.SummaryAsync();
        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] ToDoDto todo) => await toDoService.AddAsync(todo);
        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] ToDoDto todo) => await toDoService.UpdateAsync(todo);
        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) => await toDoService.DeleteAsync(id);
    }
}
