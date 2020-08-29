namespace Course_Management.Migrations.Security
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Course_Management.IdentityModels.AuthenticationDb>
    {
        public Configuration()
        {

            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            MigrationsDirectory = @"Migrations\Security";
        }

        protected override void Seed(Course_Management.IdentityModels.AuthenticationDb context)
        {
            context.Roles.Add(new IdentityModels.Role { RoleName = "User" });
            context.Roles.Add(new IdentityModels.Role { RoleName = "Admin" });
            context.Roles.Add(new IdentityModels.Role { RoleName = "Co-Ordinator" });
            context.SaveChanges();
            context.Users.Add(new IdentityModels.User { Username = "Shakil", FirstName = "Shakil", LastName = "Ahmmed", Email = "admin@mail.com", IsActive = true, Password = "Sa123*",ActivationCode= Guid.NewGuid() });
            context.Users.Add(new IdentityModels.User { Username = "Ahmmed", FirstName = "Shakil", LastName = "Ahmmed", Email = "co-ordinator@outlook.com", IsActive = true, Password = "Sa123*",ActivationCode= Guid.NewGuid() });
            context.SaveChanges();
            var us = context.Users.Include("Roles").FirstOrDefault(x => x.Username == "Shakil");
            var ro = context.Roles.FirstOrDefault(x => x.RoleName == "Admin");
            context.Users.Include("Roles").FirstOrDefault(x => x.UserId == us.UserId).Roles.Add(ro);
            var us2 = context.Users.Include("Roles").FirstOrDefault(x => x.Username == "Ahmmed");
            var ro2 = context.Roles.FirstOrDefault(x => x.RoleName == "Co-Ordinator");
            context.Users.Include("Roles").FirstOrDefault(x => x.UserId == us2.UserId).Roles.Add(ro2);
            context.SaveChanges();
        }
    }
}
