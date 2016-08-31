using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebsiteForAds.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public System.Data.Entity.DbSet<WebsiteForAds.Models.Image> Images { get; set; }
    }
    public class DbConnectionContext : DbContext
    {
        public DbConnectionContext() : base("name=dbContext")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges
            <DbConnectionContext>());
        }
        public DbSet<Image> ImageGallery { get; set; }
    }
}