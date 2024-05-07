using hoc_asp.Models;
using static hoc_asp.Data.MyDbContext;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace hoc_asp.Data
{
    public class MyDbContext
    {
        public class MyDbContext : DbContext
        {
            public MyDbContext(DbContextOptions options) : base(options) { }


            #region DbSet
            public DbSet<HangHoa> HangHoas { get; set; }
            public DbSet<Loai> Loais { get; set; }
            public DbSet<DonHang> DonHangs { get; set; }
            public DbSet<DonHangChiTiet> DonHangChiTiets { get; set; }
            #endregion

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<DonHang>(e =>
                {
                    e.ToTable("DonHang");
                    e.HasKey(dh => dh.MaDh);
                    e.Property(dh => dh.NgatDat).HasDefaultValueSql("getutcdate()");
                    e.Property(dh => dh.NguoiNhan).IsRequired().HasMaxLength(100);
                });

                modelBuilder.Entity<DonHangChiTiet>(entity =>
                {
                    entity.ToTable("ChiTietDonHang");
                    entity.HasKey(e => new { e.MaDh, e.MaHh });

                    entity.HasOne(e => e.DonHang)
                        .WithMany(e => e.DonHangChiTiets)
                        .HasForeignKey(e => e.MaDh)
                        .HasConstraintName("FK_DonHangCT_DonHang");

                    entity.HasOne(e => e.HangHoa)
                        .WithMany(e => e.DonHangChiTiets)
                        .HasForeignKey(e => e.MaHh)
                        .HasConstraintName("FK_DonHangCT_HangHoa");
                });
            }
        }
    }
}
