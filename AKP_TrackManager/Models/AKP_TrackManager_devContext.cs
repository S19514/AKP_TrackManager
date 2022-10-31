﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AKP_TrackManager.Models
{
    public partial class AKP_TrackManager_devContext : DbContext
    {
        public AKP_TrackManager_devContext()
        {
        }

        public AKP_TrackManager_devContext(DbContextOptions<AKP_TrackManager_devContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accident> Accidents { get; set; } = null!;
        public virtual DbSet<Car> Cars { get; set; } = null!;
        public virtual DbSet<CarAccidentByMember> CarAccidentByMembers { get; set; } = null!;
        public virtual DbSet<CarMember> CarMembers { get; set; } = null!;
        public virtual DbSet<ClubMembership> ClubMemberships { get; set; } = null!;
        public virtual DbSet<Lap> Laps { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<MemberCarOnLap> MemberCarOnLaps { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<TrackConfiguration> TrackConfigurations { get; set; } = null!;
        public virtual DbSet<TrainingAttandance> TrainingAttandances { get; set; } = null!;
        public virtual DbSet<training> training { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=PIOTRLOJKO;Initial Catalog=AKP_TrackManager_dev;Persist Security Info=True;User ID=sa");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accident>(entity =>
            {
                entity.ToTable("Accident");

                entity.Property(e => e.AccidentId).ValueGeneratedNever();

                entity.Property(e => e.AccidentDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("Car");

                entity.Property(e => e.CarId).ValueGeneratedNever();

                entity.Property(e => e.EngingeCapacity).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Make)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Model)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CarAccidentByMember>(entity =>
            {
                entity.HasKey(e => e.CarAccidentMemberId)
                    .HasName("CarAccidentByMember_pk");

                entity.ToTable("CarAccidentByMember");

                entity.Property(e => e.CarAccidentMemberId).ValueGeneratedNever();

                entity.Property(e => e.AccidentAccidentId).HasColumnName("Accident_AccidentId");

                entity.Property(e => e.CarCarId).HasColumnName("Car_CarId");

                entity.Property(e => e.MemberMemberId).HasColumnName("Member_MemberId");

                entity.HasOne(d => d.AccidentAccident)
                    .WithMany(p => p.CarAccidentByMembers)
                    .HasForeignKey(d => d.AccidentAccidentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CarAccidentByMember_Accident");

                entity.HasOne(d => d.CarCar)
                    .WithMany(p => p.CarAccidentByMembers)
                    .HasForeignKey(d => d.CarCarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CarAccidentByMember_Car");

                entity.HasOne(d => d.MemberMember)
                    .WithMany(p => p.CarAccidentByMembers)
                    .HasForeignKey(d => d.MemberMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CarAccidentByMember_Member");
            });

            modelBuilder.Entity<CarMember>(entity =>
            {
                entity.ToTable("CarMember");

                entity.Property(e => e.CarMemberId).ValueGeneratedNever();

                entity.Property(e => e.CarCarId).HasColumnName("Car_CarId");

                entity.Property(e => e.MemberMemberId).HasColumnName("Member_MemberId");

                entity.HasOne(d => d.CarCar)
                    .WithMany(p => p.CarMembers)
                    .HasForeignKey(d => d.CarCarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CarDriver_Car");

                entity.HasOne(d => d.MemberMember)
                    .WithMany(p => p.CarMembers)
                    .HasForeignKey(d => d.MemberMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CarDriver_Member");
            });

            modelBuilder.Entity<ClubMembership>(entity =>
            {
                entity.HasKey(e => e.MembershipId)
                    .HasName("ClubMembership_pk");

                entity.ToTable("ClubMembership");

                entity.Property(e => e.MembershipId).ValueGeneratedNever();

                entity.Property(e => e.FeeAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.MemberMemberId).HasColumnName("Member_MemberId");

                entity.HasOne(d => d.MemberMember)
                    .WithMany(p => p.ClubMemberships)
                    .HasForeignKey(d => d.MemberMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ClubMembership_Member");
            });

            modelBuilder.Entity<Lap>(entity =>
            {
                entity.ToTable("Lap");

                entity.Property(e => e.LapId).ValueGeneratedNever();

                entity.Property(e => e.TrainingTrainingId).HasColumnName("Training_TrainingId");

                entity.HasOne(d => d.TrainingTraining)
                    .WithMany(p => p.Laps)
                    .HasForeignKey(d => d.TrainingTrainingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Lap_Training");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.LocationId).ValueGeneratedNever();

                entity.Property(e => e.Country)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Town)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("Member");

                entity.Property(e => e.MemberId).ValueGeneratedNever();

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MemberCarOnLap>(entity =>
            {
                entity.HasKey(e => e.MemberLapId)
                    .HasName("MemberCarOnLap_pk");

                entity.ToTable("MemberCarOnLap");

                entity.Property(e => e.MemberLapId).ValueGeneratedNever();

                entity.Property(e => e.CarCarId).HasColumnName("Car_CarId");

                entity.Property(e => e.LapLapId).HasColumnName("Lap_LapId");

                entity.Property(e => e.MemberMemberId).HasColumnName("Member_MemberId");

                entity.HasOne(d => d.CarCar)
                    .WithMany(p => p.MemberCarOnLaps)
                    .HasForeignKey(d => d.CarCarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MemberOnLap_Car");

                entity.HasOne(d => d.LapLap)
                    .WithMany(p => p.MemberCarOnLaps)
                    .HasForeignKey(d => d.LapLapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MemberCarOnLap_Lap");

                entity.HasOne(d => d.MemberMember)
                    .WithMany(p => p.MemberCarOnLaps)
                    .HasForeignKey(d => d.MemberMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MemberOnLap_Member");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ClubMembershipMembershipId).HasColumnName("ClubMembership_MembershipId");

                entity.Property(e => e.MemberMemberId).HasColumnName("Member_MemberId");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.HasOne(d => d.ClubMembershipMembership)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.ClubMembershipMembershipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Payment_ClubMembership");

                entity.HasOne(d => d.MemberMember)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.MemberMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Payment_Member");
            });

            modelBuilder.Entity<TrackConfiguration>(entity =>
            {
                entity.HasKey(e => e.TrackId)
                    .HasName("TrackConfiguration_pk");

                entity.ToTable("TrackConfiguration");

                entity.Property(e => e.TrackId).ValueGeneratedNever();
            });

            modelBuilder.Entity<TrainingAttandance>(entity =>
            {
                entity.ToTable("TrainingAttandance");

                entity.Property(e => e.TrainingAttandanceId).ValueGeneratedNever();

                entity.Property(e => e.MemberMemberId).HasColumnName("Member_MemberId");

                entity.Property(e => e.TrainingTrainingId).HasColumnName("Training_TrainingId");

                entity.HasOne(d => d.MemberMember)
                    .WithMany(p => p.TrainingAttandances)
                    .HasForeignKey(d => d.MemberMemberId)
                    .HasConstraintName("TrainingAttandance_Member");

                entity.HasOne(d => d.TrainingTraining)
                    .WithMany(p => p.TrainingAttandances)
                    .HasForeignKey(d => d.TrainingTrainingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TrainingAttandance_Training");
            });

            modelBuilder.Entity<training>(entity =>
            {
                entity.ToTable("Training");

                entity.Property(e => e.TrainingId).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EndTime).HasColumnType("time(0)");

                entity.Property(e => e.LocationLocationId).HasColumnName("Location_LocationId");

                entity.Property(e => e.StartTime).HasColumnType("time(0)");

                entity.Property(e => e.TrackConfigurationTrackId).HasColumnName("TrackConfiguration_TrackId");

                entity.HasOne(d => d.LocationLocation)
                    .WithMany(p => p.training)
                    .HasForeignKey(d => d.LocationLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Training_Location");

                entity.HasOne(d => d.TrackConfigurationTrack)
                    .WithMany(p => p.training)
                    .HasForeignKey(d => d.TrackConfigurationTrackId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Training_TrackConfiguration");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
