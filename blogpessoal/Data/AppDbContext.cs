using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Data
{
    public class AppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

    }
}
