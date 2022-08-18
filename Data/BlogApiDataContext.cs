using System.Reflection;
using BlogApi.Data.Mappings;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Data
{
    public class BlogApiDataContext : DbContext
    {
        public BlogApiDataContext(DbContextOptions<BlogApiDataContext> options)
            : base(options) 
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* modelBuilder.ApplyConfiguration<CategoryMap>(new CategoryMap());
             modelBuilder.ApplyConfiguration<PostMap>(new PostMap());
             modelBuilder.ApplyConfiguration<RoleMap>(new RoleMap());
             modelBuilder.ApplyConfiguration<TagMap>(new TagMap());
             modelBuilder.ApplyConfiguration<UserMap>(new UserMap()); */
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}