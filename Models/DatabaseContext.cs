using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Models
{
    public class DatabaseContext : IdentityDbContext<User, IdentityRole<long>, long>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.SetCommandTimeout(180);
        }
    }
}