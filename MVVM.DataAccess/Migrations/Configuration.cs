namespace MVVM.DataAccess.Migrations
{
    using MVVM.Model;
    using System;
    using System.Collections.Generic;
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
            context.SaveChanges();
            context.PhoneNumbers.AddOrUpdate(pn => pn.Number, new FriendPhoneNumber { Number = "+12 12345678", FriendId = context.Friends.First().Id });

            context.Meetings.AddOrUpdate(pn => pn.Title,
                new Meeting
                {

                    Title = "WatchingMovie",
                    DateFrom = new DateTime(2018, 5, 26),
                    DateTo  =  new DateTime(2018, 7, 26),
                    Friends = new List<Friend>
                    {
                        context.Friends.Single(f=> f.FirstName == "Mohamed" && f.LastName =="Zghal"),
                        context.Friends.Single(f=>f.FirstName =="Med"&& f.LastName == "Zg" )
                    }
                });


        }
    }
}
