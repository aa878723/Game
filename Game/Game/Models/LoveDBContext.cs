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

        public virtual DbSet<LoveGame> LoveGames { get; set; } = null!;

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
            modelBuilder.Entity<LoveGame>(entity =>
            {
                entity.ToTable("LoveGame");

                entity.HasIndex(e => e.Accound, "IX_LoveGame")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "IX_LoveGame_1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Accound)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("accound");

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
