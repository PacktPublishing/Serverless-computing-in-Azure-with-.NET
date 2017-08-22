using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TextScoreDashboard.Models
{
    public partial class TextScoresDBContext : DbContext
    {
        public TextScoresDBContext(DbContextOptions<TextScoresDBContext> options)
        : base(options)
        { }

        public virtual DbSet<DocumentTextScore> DocumentTextScore { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentTextScore>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.DocumentName)
                    .IsRequired()
                    .HasColumnType("varchar(255)");
            });
        }
    }
}