﻿using ClinicBookingSystem_BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBookingSystem_DataAccessObject.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ClinicBookingSystem_DataAcessObject.DBContext
{
    public class ClinicBookingSystemContext : DbContext
    {
        public ClinicBookingSystemContext(DbContextOptions<ClinicBookingSystemContext> options): base(options)
        { }
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<BusinessService> BusinessServices => Set<BusinessService>();
        public DbSet<Specification> Specifications => Set<Specification>();
        public DbSet<Salary> Salaries => Set<Salary>();
        public DbSet<Clinic> Clinics => Set<Clinic>();
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<Claim> Claims => Set<Claim>();
        public DbSet<Slot> Slots => Set<Slot>();
        public DbSet<Application> Applications => Set<Application>();
        public DbSet<Result> Results => Set<Result>();
        public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
        public DbSet<Medicine> Medicines => Set<Medicine>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Billing> Billings => Set<Billing>();
        public DbSet<AppointmentBusinessService> AppointmentBusinessServices => Set<AppointmentBusinessService>();
        public DbSet<Meeting> Meetings => Set<Meeting>();
        public DbSet<Note> Notes => Set<Note>();
        public DbSet<Token> Tokens => Set<Token>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meeting>()
                .ToTable(tb => tb.HasTrigger("update_appointment_status_after_meeting_done"));
            modelBuilder.Entity<AppointmentBusinessService>()
                .ToTable(tb => tb.HasTrigger("update_appointment_status_after_appointmentservice_done"));
        }
    }
}