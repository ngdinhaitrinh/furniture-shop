using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebBanNoiThat.Models
{
    public partial class BanNoiThatContext : DbContext
    {
        public BanNoiThatContext()
        {
        }

        public BanNoiThatContext(DbContextOptions<BanNoiThatContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual DbSet<DanhMucSp> DanhMucSps { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }


  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
         
            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasKey(e => e.MaSp)
                    .HasName("PK__SanPham__2725081C535AFC31");

                entity.ToTable("SanPham");

                entity.Property(e => e.MaSp).HasColumnName("MaSP");

                entity.Property(e => e.AnhSp)
                    .IsRequired()
                    .HasColumnName("AnhSP");

                //entity.Property(e => e.VatLieu)
                //   .IsRequired()
                //   .HasMaxLength(1000)
                //   .HasColumnName("VatLieu");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.GiaSp).HasColumnName("GiaSP");

                entity.Property(e => e.MaDm).HasColumnName("MaDM");

                entity.Property(e => e.MotaSp)
                    .IsRequired()
                    .HasColumnName("MotaSP");

                entity.Property(e => e.NgaySua).HasColumnType("date");

                entity.Property(e => e.TenSp)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("TenSP");



                entity.HasOne(d => d.MaDmNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.MaDm)
                    .HasConstraintName("FK__SanPham__MotaSP__286302EC");
            });
        }

      
    }
}
