﻿

using Solution.Data.Configurations;
using Solution.Domain.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Solution.Data
{

    public class PidevContext : DbContext
    {
        public PidevContext() : base("kindergarten") {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<KinderGarten> KinderGartens { get; set; }
        public DbSet<Event> Events { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DirecteurConfiguration());
            
            //participation configuration
            modelBuilder.Entity<Event>()
            .HasMany(b => b.Parents)
             .WithMany(c => c.Events)
             .Map(cs =>
                 {
              cs.MapLeftKey("EventId");
              cs.MapRightKey("ParentId");
              cs.ToTable("Participation");
                 });
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

        }
    }
}

