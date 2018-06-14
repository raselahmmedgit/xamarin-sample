namespace Levelnis.Learning.CallingWebApiFromMvc.Api.Identity
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ApiFromMvcConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<ApplicationUser>().ToTable("SysUser");
        //    modelBuilder.Entity<IdentityRole>().ToTable("SysRole");
        //    modelBuilder.Entity<IdentityUserRole>().ToTable("SysUserRole");
        //    modelBuilder.Entity<IdentityUserLogin>().ToTable("SysUserLogin");
        //    modelBuilder.Entity<IdentityUserClaim>().ToTable("SysUserClaim");
        //    Database.SetInitializer<ApplicationDbContext>(null);
        //}
    }
}