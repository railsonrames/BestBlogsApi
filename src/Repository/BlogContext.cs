using Microsoft.EntityFrameworkCore;
using Model;

namespace Repository
{
    // this is used for our verification tests, don't rename or change the access modifier
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Post> Posts { get; set; }
    }
}