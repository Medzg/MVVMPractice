﻿using MVVM.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MVVM.DataAccess
{
    public class FriendDbContext : DbContext
    {
        public DbSet<Friend> Friends { get; set; }
        public FriendDbContext():base("FriendDb")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}