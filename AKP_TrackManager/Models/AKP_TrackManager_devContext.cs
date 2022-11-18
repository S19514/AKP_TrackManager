using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AKP_TrackManager.Models
{
    public partial class AKP_TrackManager_devContext : DbContext
    {
        public AKP_TrackManager_devContext(DbContextOptions<AKP_TrackManager_devContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accident> Accidents { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarAccidentByMember> CarAccidentByMembers { get; set; }
        public virtual DbSet<CarMember> CarMembers { get; set; }
        public virtual DbSet<ClubMembership> ClubMemberships { get; set; }
        public virtual DbSet<Lap> Laps { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MemberCarOnLap> MemberCarOnLaps { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<TrackConfiguration> TrackConfigurations { get; set; }
        public virtual DbSet<TrainingAttandance> TrainingAttandances { get; set; }
        public virtual DbSet<training> training { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");

            modelBuilder.Entity<Accident>(entity =>
            {
                entity.ToTable("Accident");

                entity.Property(e => e.AccidentDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("Car");

                entity.Property(e => e.EngingeCapacity).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Make)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RegPlate)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<CarAccidentByMember>(entity =>
            {
                entity.HasKey(e => e.CarAccidentMemberId)
                    .HasName("CarAccidentByMember_pk");

                entity.ToTable("CarAccidentByMember");

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

                entity.Property(e => e.MemberMemberId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("Member_MemberId");

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

                entity.Property(e => e.AbsoluteTime).HasColumnType("time(3)");

                entity.Property(e => e.MeasuredTime).HasColumnType("time(3)");

                entity.Property(e => e.PenaltyTime).HasColumnType("time(0)");

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

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FriendlyName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Town)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("Member");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsBlocked).HasColumnName("isBlocked");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RoleRoleId).HasColumnName("Role_RoleId");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.RoleRole)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.RoleRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Member_Role");
            });

            modelBuilder.Entity<MemberCarOnLap>(entity =>
            {
                entity.HasKey(e => e.MemberLapId)
                    .HasName("MemberCarOnLap_pk");

                entity.ToTable("MemberCarOnLap");

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

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TrackConfiguration>(entity =>
            {
                entity.HasKey(e => e.TrackId)
                    .HasName("TrackConfiguration_pk");

                entity.ToTable("TrackConfiguration");

                entity.Property(e => e.Length).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PresetImageLink)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PresetName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TrainingAttandance>(entity =>
            {
                entity.ToTable("TrainingAttandance");

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
