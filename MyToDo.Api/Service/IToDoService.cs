using MyToDo.Api.Models;
using MyToDo.Share.Dtos;
using MyToDo.Share.Parameters;

namespace MyToDo.Api.Service
{
    public interface IToDoService: IBaseService<ToDoDto>
    {
        Task<ApiResponse> GetAllAsync(TodoParameter parameter);
        Task<ApiResponse> SummaryAsync();
    }
}
