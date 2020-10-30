using Microsoft.EntityFrameworkCore;

namespace SeleniumWeb.Models
{
    public class MyDbContext : DbContext
    {
        private readonly DbContextOptions<MyDbContext> _options;

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            _options = options;
        }
    }
}
