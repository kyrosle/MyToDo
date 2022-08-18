using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Models;
using MyToDo.Share.Dtos;
using MyToDo.Share.Parameters;

namespace MyToDo.Api.Service
{
    public class MemoService : IMemoService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MemoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ApiResponse> AddAsync(MemoDto model)
        {
            try
            {
                var memo = mapper.Map<Memo>(model);
                await unitOfWork.GetRepository<Memo>().InsertAsync(memo);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return new ApiResponse(true, memo);
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
                var repository = unitOfWork.GetRepository<Memo>();
                var memo = await repository.GetFirstOrDefaultAsync(predicate: t => t.Id.Equals(id));
                repository.Delete(memo);
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
                var repository = unitOfWork.GetRepository<Memo>();

                var result = await repository.GetPagedListAsync(predicate: x =>
                    string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search),
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
                var repository = unitOfWork.GetRepository<Memo>();
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

        public async Task<ApiResponse> UpdateAsync(MemoDto model)
        {
            try
            {
                var repository = unitOfWork.GetRepository<Memo>();
                var momo = mapper.Map<Memo>(model);
                var result = await repository.GetFirstOrDefaultAsync(predicate: t => t.Id.Equals(momo.Id));

                result.Title = momo.Title;
                result.Content = momo.Content;
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
