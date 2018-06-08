using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace App.WebApi.Models
{
    public class AppDbContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public AppDbContext() : base("name=AppDbContext")
        {
        }

        public System.Data.Entity.DbSet<App.WebApi.Models.Student> Students { get; set; }
    }

    #region Initial data

    // Change the base class as follows if you want to drop and create the database during development:
    //public class DbInitializer : DropCreateDatabaseAlways<AppDbContext>
    //public class DbInitializer : CreateDatabaseIfNotExists<AppDbContext>
    public class DbInitializer : DropCreateDatabaseIfModelChanges<AppDbContext>
    {

        protected override void Seed(AppDbContext context)
        {
            // Create Default Student.
            var studentList = new List<Student>
                {
                    new Student { Id = Convert.ToInt32(1), Name = "Rasel"},
                    new Student { Id = Convert.ToInt32(2), Name = "Sohel"},
                    new Student { Id = Convert.ToInt32(3), Name = "Azim"},
                    new Student { Id = Convert.ToInt32(4), Name = "Nahid"}

                };
            studentList.ForEach(item => context.Students.Add(item));
            context.SaveChanges();


        }
    }

    #endregion
}
