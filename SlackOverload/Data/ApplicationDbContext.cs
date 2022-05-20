using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SlackOverload.Models;

namespace SlackOverload.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public virtual DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Reply> Replies { get; set; }
        public virtual DbSet<Vote> Votes { get; set; }


        public virtual DbSet<IdentityRole> Roles { get; set; }

    }
}
