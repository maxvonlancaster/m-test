using m_test.DAL.Entities;
using m_test.DAL.EventEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace m_test.DAL;

public class LabDbContext : DbContext
{
    public LabDbContext() { }
    public LabDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<StudentCourse> StudentCourses { get; set; }

    public DbSet<EventNew> EventNews { get; set; }
    public DbSet<EventStructure> EventStructures { get; set; }
    public DbSet<SubEvent> SubEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentCourse>()
          .HasKey(sc => new { sc.StudentId, sc.CourseId }); 

        modelBuilder.Entity<StudentCourse>()
          .HasOne(sc => sc.Student)
          .WithMany(s => s.StudentCourses)
          .HasForeignKey(sc => sc.StudentId);

        modelBuilder.Entity<StudentCourse>()
          .HasOne(sc => sc.Course)
          .WithMany(c => c.StudentCourses)
          .HasForeignKey(sc => sc.CourseId);


        modelBuilder.Entity<SubEvent>()
          .HasOne(sc => sc.EventNew)
          .WithMany(c => c.SubEvents)
          .HasForeignKey(sc => sc.EventNewId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SchoolDB;Trusted_Connection=True;");
    }
}