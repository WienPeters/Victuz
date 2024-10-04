using WPCasusVictuz.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WPCasusVictuz.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> contextOptions) : base(contextOptions)
        {
        }
        public DbSet<Member> Members { get; set; }
        public DbSet<BoardMember> BoardMembers { get; set; }
        public DbSet<Aktivity> Activities { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Vote> Votes { get; set; }

       
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    string connection = @"Data Source=.;Initial Catalog=Victuz;Integrated Security=true;TrustServerCertificate=True;";
        //    optionsBuilder.UseSqlServer(connection);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define a value converter for List<string> (comma-separated string)
            var optionsConverter = new ValueConverter<List<string>, string>(
                v => string.Join(",", v),  // Convert List<string> to a comma-separated string
                v => v.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()  // Convert back to List<string>
            );

            // Define a value comparer for List<string> to compare the contents of the list
            var optionsComparer = new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2),  // Compare if two lists have the same elements
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),  // Get a combined hash code for the list
                c => c.ToList()  // Clone the list when copying
            );

            modelBuilder.Entity<Poll>()
                .Property(p => p.Options)
                .HasConversion(optionsConverter)  // Use the value converter for List<string>
                .Metadata.SetValueComparer(optionsComparer);  // Set the value comparer for List<string>


            // 2. Member to Registration (One-to-Many relationship)
            modelBuilder.Entity<Member>()

                .HasMany(m => m.Registrations)  // A member can have many registrations
                .WithOne(r => r.Member)  // A registration belongs to one member
                .HasForeignKey(r => r.MemberId)  // Foreign key in Registration
                
                .OnDelete(DeleteBehavior.Cascade);  // If a Member is deleted, all their Registrations are deleted
                

            // 3. Activity to Registration (One-to-Many relationship)
            modelBuilder.Entity<Aktivity>()
                .HasMany(a => a.Registrations)  // An activity can have many registrations
                .WithOne(r => r.Aktivity)  // A registration belongs to one activity
                .HasForeignKey(r => r.AktivityId)  // Foreign key in Registration
                .OnDelete(DeleteBehavior.Cascade);  // If an Activity is deleted, its Registrations are deleted

            // 4. BoardMember to Poll (One-to-Many relationship)
            modelBuilder.Entity<BoardMember>()
                .HasMany(bm => bm.CreatedPolls)  // A board member can create many polls
                .WithOne(p => p.CreatedBy)  // A poll belongs to one board member
                .HasForeignKey(p => p.CreatedByBoardMemberId)  // Foreign key in Poll
                .OnDelete(DeleteBehavior.NoAction);  // If a BoardMember is deleted, keep the Polls but set CreatedBy null (optional)

            modelBuilder.Entity<BoardMember>()
                .HasMany(bm => bm.CreatedAktivitys)
                .WithOne(a => a.MadeBy)
                .HasForeignKey(a => a.CreatedbyBM)
                .OnDelete(DeleteBehavior.NoAction);

            // 5. Poll to Vote (One-to-Many relationship)
            modelBuilder.Entity<Poll>()
                .HasMany(p => p.Votes)  // A poll can have many votes
                .WithOne(v => v.Poll)  // A vote belongs to one poll
                .HasForeignKey(v => v.PollId)  // Foreign key in Vote
                .OnDelete(DeleteBehavior.Cascade);  // If a Poll is deleted, its Votes are deleted

            // 6. Member to Vote (One-to-Many relationship)
            modelBuilder.Entity<Member>()
                .HasMany(m => m.Votes)  // A member can vote in many polls
                .WithOne(v => v.Member)  // A vote belongs to one member
                .HasForeignKey(v => v.MemberId)  // Foreign key in Vote
                .OnDelete(DeleteBehavior.Cascade);  // If a Member is deleted, their Votes are deleted
                                                    // Configure the relationship between Member and BoardMember
            modelBuilder.Entity<Member>()
                .HasOne(m => m.BoardMember)  // A Member can have one BoardMember (or none)
                .WithOne(b => b.Member)  // A BoardMember is linked to one Member
                .HasForeignKey<Member>(m => m.BoardMemberId);  // Foreign key in Member

            modelBuilder.Entity<BoardMember>()
                .HasOne(m => m.Member)
                .WithOne(b => b.BoardMember)
                .HasForeignKey<BoardMember>(m => m.MemberId);
        }
    }

}
