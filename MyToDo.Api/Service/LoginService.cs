using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Models;
using MyToDo.Share.Dtos;

namespace MyToDo.Api.Service
{
    public class LoginService : ILoginServer
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LoginService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ApiResponse> LoginAsync(string Account, string PassWord)
        {
            try
            {
                var model = await unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(
                    predicate: x => x.Account.Equals(Account) && x.PassWord.Equals(PassWord));
                if (model is null)
                    return new ApiResponse("Account or PassWord Error");
                return new ApiResponse(true, model);
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }

        public async Task<ApiResponse> Register(UserDto user)
        {
            try
            {
                var model = mapper.Map<User>(user);
                var repository = unitOfWork.GetRepository<User>();
                var userModel = await repository.GetFirstOrDefaultAsync(
                    predicate: x => x.Account.Equals(model.Account) && x.PassWord.Equals(model.PassWord));
                if (userModel is not null)
                    return new ApiResponse("Account is Existed");

                await repository.InsertAsync(model);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, model);
                else return new ApiResponse("Insert Account Failed");
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }
    }
}
