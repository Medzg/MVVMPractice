namespace MVVM.DataAccess.Migrations
{
    using MVVM.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVVM.DataAccess.FriendDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MVVM.DataAccess.FriendDbContext context)
        {
            context.Friends.AddOrUpdate(f => f.FirstName,
                new Friend { FirstName = "Mohamed", LastName = "Zghal" },
     new Friend { FirstName = "Med", LastName = "Zg" },
             new Friend { FirstName = "Mohd", LastName = "Zal" },
            new Friend { FirstName = "Mod", LastName = "hal" },
            new Friend { FirstName = "amed", LastName = "al" });

            context.ProgrammingLanguages.AddOrUpdate(pl => pl.Name, 
                new ProgrammingLanguage { Name = "C#" },
                new ProgrammingLanguage { Name = "JS" },
                new ProgrammingLanguage { Name = "Java" }
                );
        }
    }
}
