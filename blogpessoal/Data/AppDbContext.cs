using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

    }
}

