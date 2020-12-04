using Microsoft.EntityFrameworkCore;
using web.model;

namespace web.Datacontext
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<MessageModel> Messages { get; set; }
    }
}