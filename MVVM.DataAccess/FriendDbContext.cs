using MVVM.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MVVM.DataAccess
{
    public class FriendDbContext : DbContext
    {
        public DbSet<Friend> Friends { get; set; }

        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<FriendPhoneNumber> PhoneNumbers { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
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
