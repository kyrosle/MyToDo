using MyToDo.Share.Dtos;
namespace MyToDo.Api.Service
{
    public interface ILoginServer
    {
        Task<ApiResponse> LoginAsync(string Account, string PassWord);
        Task<ApiResponse> Register(UserDto user);
    }
}
