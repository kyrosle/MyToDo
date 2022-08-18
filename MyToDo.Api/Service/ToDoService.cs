using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Models;
using MyToDo.Share.Dtos;
using MyToDo.Share.Parameters;
using System.Reflection.Metadata;

namespace MyToDo.Api.Service
{
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ToDoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ApiResponse> AddAsync(ToDoDto model)
        {
            try
            {
                var todo = mapper.Map<ToDo>(model);
                await unitOfWork.GetRepository<ToDo>().InsertAsync(todo);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, todo);
                else return new ApiResponse("Insert Failed");

            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: t => t.Id.Equals(id));
                repository.Delete(todo);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, "");
                else return new ApiResponse("DeleteAsync Failed");
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();

                var result = await repository.GetPagedListAsync(predicate:
                    x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search),
                    pageIndex: parameter.PageIndex,
                    pageSize: parameter.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.CreateDate));

                if (result.TotalCount > 0)
                    return new ApiResponse(true, result);
                else return new ApiResponse("GetPagedListAsync Failed");
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(TodoParameter parameter)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();

                //! 没加括号符号逻辑错误
                var result = await repository.GetPagedListAsync(predicate:
                x => (string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search))                && (parameter.Status == null ? true :  x.Status.Equals(parameter.Status))
                ,
                pageIndex: parameter.PageIndex,
                    pageSize: parameter.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.CreateDate));
                if (result.TotalCount > 0)
                    return new ApiResponse(true, result);
                else return new ApiResponse("GetPagedListAsync Failed");
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();
                var result = await repository.GetFirstOrDefaultAsync(predicate: t => t.Id.Equals(id));
                if (result is not null)
                    return new ApiResponse(true, result);
                else return new ApiResponse("GetSingleAsync Failed");
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(ToDoDto model)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();
                var todo = mapper.Map<ToDo>(model);
                var result = await repository.GetFirstOrDefaultAsync(predicate: t => t.Id.Equals(todo.Id));

                result.Title = todo.Title;
                result.Content = todo.Content;
                result.Status = todo.Status;
                result.UpdateDate = DateTime.Now;

                repository.Update(result);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, result);
                else return new ApiResponse("Update Failed");
            }
            catch (Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }
    }
}
