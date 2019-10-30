using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Models
{
    public class DbContext : IdentityDbContext<User, IdentityRole<long>, long>
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
            Database.SetCommandTimeout(180);
        }
    }
}