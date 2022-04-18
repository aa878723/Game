using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Meals.Models
{
    public partial class FoodDBContext : DbContext
    {
        public FoodDBContext()
        {
        }

        public FoodDBContext(DbContextOptions<FoodDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EasyFood> EasyFoods { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-GH3BRQGP\\SQLEXPRESS;Initial Catalog=FoodDB;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EasyFood>(entity =>
            {
                entity.ToTable("EasyFood");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MealStyle)
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.Name).HasMaxLength(10);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
