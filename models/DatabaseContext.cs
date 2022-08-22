using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MDTPDQSync.models
{
    public partial class DatabaseContext : DbContext
    {
        private IConfiguration config;
        public DatabaseContext(IConfiguration config)
        {
            this.config = config;
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public virtual DbSet<Package> Packages { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbPath = config.GetValue<string>("dbPath");
                optionsBuilder.UseSqlite($"DataSource={dbPath};Mode=ReadOnly");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Package>(entity =>
            {
                entity.HasIndex(e => e.FolderId, "IX_Packages_FolderId");

                entity.HasIndex(e => e.PackageDefinitionId, "IX_Packages_PackageDefinitionId");

                entity.Property(e => e.PackageId).HasColumnType("integer");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
