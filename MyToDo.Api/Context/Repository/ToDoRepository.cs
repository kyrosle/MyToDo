using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using MyToDo.Api.Models;

namespace MyToDo.Api.Context.Repository
{
    public class ToDoRepository : Repository<ToDo>, IRepository<ToDo>
    {
        public ToDoRepository(MyToDoDbContext dbContext) : base(dbContext)
        {
        }
    }
}
