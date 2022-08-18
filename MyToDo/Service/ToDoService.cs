using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using MyToDo.Share;
using MyToDo.Share.Dtos;
using MyToDo.Share.Parameters;
using RestSharp;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class ToDoService : BaseService<ToDoDto> , IToDoService
    {
        private readonly HttpRestClient client;

        public ToDoService(HttpRestClient client) : base(client, "ToDo")
        {
            this.client = client;
        }

        public async Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(TodoParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.GET;
            request.Route = $"api/ToDo/GetAll?PageIndex={parameter.PageIndex}" +
                $"&PageSize={parameter.PageSize}" +
                $"&search={parameter.Search}"+
                $"&status={parameter.Status}";
            return await client.ExecuteAsync<PagedList<ToDoDto>>(request);
        }
    }
}
