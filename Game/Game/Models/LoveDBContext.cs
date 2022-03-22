using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Game.Models
{
    public partial class LoveDBContext : DbContext
    {
        public LoveDBContext()
        {
        }

        public LoveDBContext(DbContextOptions<LoveDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Friend> Friends { get; set; } = null!;
        public virtual DbSet<LoveGame> LoveGames { get; set; } = null!;
        public virtual DbSet<Money> Money { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Pair> Pairs { get; set; } = null!;
        public virtual DbSet<PersonalTalking> PersonalTalkings { get; set; } = null!;
        public virtual DbSet<Talking> Talkings { get; set; } = null!;
        public virtual DbSet<TalkingRoom> TalkingRooms { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-GH3BRQGP\\SQLEXPRESS;Initial Catalog=LoveDB;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friend>(entity =>
            {
                entity.ToTable("Friend");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Account)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Friend1)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("Friend");
            });

            modelBuilder.Entity<LoveGame>(entity =>
            {
                entity.ToTable("LoveGame");

                entity.HasIndex(e => e.Account, "IX_LoveGame")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "IX_LoveGame_1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Account)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Career).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.Hobby).HasMaxLength(50);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.Property(e => e.SexualOrientation).HasMaxLength(50);
            });

            modelBuilder.Entity<Money>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Account)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Card)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Qe)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("QE");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Account)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.OrderNumber).HasMaxLength(50);

                entity.Property(e => e.OrderTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Pair>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Account)
                    .HasMaxLength(12)
                    .IsFixedLength();

                entity.Property(e => e.Lover)
                    .HasMaxLength(12)
                    .HasColumnName("lover")
                    .IsFixedLength();

                entity.Property(e => e.Want).HasColumnName("want");
            });

            modelBuilder.Entity<PersonalTalking>(entity =>
            {
                entity.ToTable("PersonalTalking");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Account)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Friend)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Message).HasMaxLength(50);

                entity.Property(e => e.Picture)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SendTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Talking>(entity =>
            {
                entity.ToTable("Talking");

                entity.Property(e => e.Account)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Message).HasMaxLength(50);

                entity.Property(e => e.MessageTime).HasColumnType("datetime");

                entity.Property(e => e.Room).HasMaxLength(10);
            });

            modelBuilder.Entity<TalkingRoom>(entity =>
            {
                entity.ToTable("TalkingRoom");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RoomName).HasMaxLength(10);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
