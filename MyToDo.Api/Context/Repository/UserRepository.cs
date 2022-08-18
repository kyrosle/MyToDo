using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using MyToDo.Api.Models;

namespace MyToDo.Api.Context.Repository
{
    public class UserRepository : Repository<User>, IRepository<User>
    {
        public UserRepository(MyToDoDbContext dbContext) : base(dbContext)
        {
        }
    }
}
