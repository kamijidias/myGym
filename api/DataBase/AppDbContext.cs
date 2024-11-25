using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.DataBase
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        public required DbSet<User> Users { get; set;}
    }
}