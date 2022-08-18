using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using MyToDo.Api.Models;

namespace MyToDo.Api.Context.Repository
{
    public class MemoRepository : Repository<Memo>, IRepository<Memo>
    {
        public MemoRepository(MyToDoDbContext dbContext) : base(dbContext)
        {
        }
    }
}
