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
        public DbSet<Picture> Pictures { get; set; } 
        public DbSet<Member> Members { get; set; }
        public DbSet<BoardMember> BoardMembers { get; set; }
        public DbSet<Aktivity> Activities { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Vote> Votes { get; set; }


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
                .HasConversion(optionsConverter)
                .Metadata.SetValueComparer(optionsComparer);

            // Member to Registration (One-to-Many)
            modelBuilder.Entity<Member>()
                .HasMany(m => m.Registrations)
                .WithOne(r => r.Member)
                .HasForeignKey(r => r.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            // Aktivity to Registration (One-to-Many)
            modelBuilder.Entity<Aktivity>()
                .HasMany(a => a.Registrations)
                .WithOne(r => r.Aktivity)
                .HasForeignKey(r => r.AktivityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ensure Member can't register twice for the same activity
            modelBuilder.Entity<Registration>()
                .HasIndex(r => new { r.MemberId, r.AktivityId })
                .IsUnique();

            // Poll to Vote (One-to-Many)
            modelBuilder.Entity<Poll>()
                .HasMany(p => p.Votes)
                .WithOne(v => v.Poll)
                .HasForeignKey(v => v.PollId)
                .OnDelete(DeleteBehavior.Cascade);

            // Member to Vote (One-to-Many)
            modelBuilder.Entity<Member>()
                .HasMany(m => m.Votes)
                .WithOne(v => v.Member)
                .HasForeignKey(v => v.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            // Member to BoardMember (One-to-One)
            modelBuilder.Entity<Member>()
                .HasOne(m => m.BoardMember)
                .WithOne(b => b.Member)
                .HasForeignKey<BoardMember>(b => b.MemberId);

            // BoardMember to Polls (One-to-Many)
            modelBuilder.Entity<BoardMember>()
                .HasMany(bm => bm.CreatedPolls)
                .WithOne(p => p.CreatedBy)
                .HasForeignKey(p => p.CreatedByBoardMemberId)
                .OnDelete(DeleteBehavior.NoAction);

            // BoardMember to Aktivity (One-to-Many)
            modelBuilder.Entity<BoardMember>()
                .HasMany(bm => bm.CreatedAktivitys)
                .WithOne(a => a.MadeBy)
                .HasForeignKey(a => a.CreatedbyBM)
                .OnDelete(DeleteBehavior.NoAction);


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


            modelBuilder.Entity<Registration>()
            .HasIndex(r => new { r.MemberId, r.AktivityId })
            .IsUnique(); // Zorg ervoor dat een lid zich niet meerdere keren kan registreren voor dezelfde activiteit

            modelBuilder.Entity<Aktivity>()
                .HasIndex(a => a.Name)
                .HasDatabaseName("IX_Aktivity_Name");

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

            // Definieer de relaties
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Aktivity)
                .WithMany(a => a.Registrations)
                .HasForeignKey(r => r.AktivityId)
                .OnDelete(DeleteBehavior.Cascade); // Als een activiteit wordt verwijderd, worden alle registraties ook verwijderd

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Member)
                .WithMany(m => m.Registrations)
                .HasForeignKey(r => r.MemberId)
                .OnDelete(DeleteBehavior.Cascade); // Als een lid wordt verwijderd, worden hun registraties ook verwijderd

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
