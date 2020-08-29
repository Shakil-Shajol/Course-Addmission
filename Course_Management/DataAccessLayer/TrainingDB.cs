using Course_Management.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Course_Management.DataAccessLayer
{
    public class TrainingDB:DbContext
    {
        public TrainingDB():base("TrainingDB")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trainer>()
                .HasMany(x => x.Skills)
                .WithMany(x => x.Trainers)
                .Map(m =>
                {
                    m.ToTable("TrainerSkills");
                    m.MapLeftKey("TrainerId");
                    m.MapRightKey("SkillId");
                });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}