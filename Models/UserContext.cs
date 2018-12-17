
using Microsoft.EntityFrameworkCore;

namespace DojoActivity.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<User> UsersTable {get;set;}

        public DbSet<Activity> ActivityTable {get;set;}
        public DbSet<Participant> ParticipantTable {get;set;}
    }
}