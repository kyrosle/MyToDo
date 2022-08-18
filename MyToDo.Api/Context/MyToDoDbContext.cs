using Microsoft.EntityFrameworkCore;
using MyToDo.Api.Models;

namespace MyToDo.Api.Context
{
    public class MyToDoDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<Memo> Memos { get; set; }
        public MyToDoDbContext(DbContextOptions<MyToDoDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
