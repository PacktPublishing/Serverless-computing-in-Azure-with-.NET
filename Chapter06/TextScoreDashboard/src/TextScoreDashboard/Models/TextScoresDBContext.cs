using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TextScoreDashboard.Models
{
    public partial class TextScoresDBContext : DbContext
    {
        public virtual DbSet<AverageDocumentScore> AverageDocumentScore { get; set; }
        public virtual DbSet<DocumentTextScore> DocumentTextScore { get; set; }
        public virtual DbSet<TweetTextScore> TweetTextScore { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=tcp:textsentimentsqlsrv.database.windows.net,1433;Initial Catalog=TextScoresDB;Persist Security Info=False;User ID=sqladmin;Password=p@rol4iktvoi;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AverageDocumentScore>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasDefaultValueSql("getutcdate()");
            });

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

            modelBuilder.Entity<TweetTextScore>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getutcdate()");

                entity.Property(e => e.TweetText)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnType("varchar(255)");
            });
        }
    }
}