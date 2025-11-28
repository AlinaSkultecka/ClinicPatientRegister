using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;


namespace ClinicPatientRegister_v2.Models;

public partial class ClinicDbContext : DbContext
{
    public ClinicDbContext()
    {
    }

    public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Nurse> Nurses { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)   // Add your configuration file name "Server=(localdb)\\MSSQLLocalDB;Database=YOUR_DB_NAME;Trusted_Connection=True"
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("MyDb"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCC218448989");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments).HasConstraintName("FK__Appointme__Docto__30F848ED");

            entity.HasOne(d => d.Nurse).WithMany(p => p.Appointments).HasConstraintName("FK__Appointme__Nurse__31EC6D26");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Patie__300424B4");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctors__2DC00EBF1432A814");

            entity.Property(e => e.DateOfBirth).HasComputedColumnSql("(datefromparts(substring([PersonalNumber12],(1),(4)),substring([PersonalNumber12],(5),(2)),substring([PersonalNumber12],(7),(2))))", true);
            entity.Property(e => e.PersonalNumber12).IsFixedLength();
        });

        modelBuilder.Entity<Nurse>(entity =>
        {
            entity.HasKey(e => e.NurseId).HasName("PK__Nurses__43847849FA975134");

            entity.Property(e => e.DateOfBirth).HasComputedColumnSql("(datefromparts(substring([PersonalNumber12],(1),(4)),substring([PersonalNumber12],(5),(2)),substring([PersonalNumber12],(7),(2))))", true);
            entity.Property(e => e.PersonalNumber12).IsFixedLength();
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patients__970EC3666165E58E");

            entity.Property(e => e.DateOfBirth).HasComputedColumnSql("(datefromparts(substring([PersonalNumber12],(1),(4)),substring([PersonalNumber12],(5),(2)),substring([PersonalNumber12],(7),(2))))", true);
            entity.Property(e => e.PersonalNumber12).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
