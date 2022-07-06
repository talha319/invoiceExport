using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace InvoiceExportPDF.Data
{
    public partial class InvoiceExportDBContext : DbContext
    {
        public InvoiceExportDBContext()
        {
        }

        public InvoiceExportDBContext(DbContextOptions<InvoiceExportDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=InvoiceExportDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Gsttex)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("GSTTex");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.OtherTex).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Qty).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Uom)
                    .HasMaxLength(50)
                    .HasColumnName("UOM");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.Role).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
