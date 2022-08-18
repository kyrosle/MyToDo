using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using MyToDo.Share;
using MyToDo.Share.Dtos;
using MyToDo.Share.Parameters;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface IToDoService: IBaseService<ToDoDto>
    {
        Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(TodoParameter parameter);
    }
}
