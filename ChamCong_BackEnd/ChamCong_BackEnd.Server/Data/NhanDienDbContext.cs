using ChamCong_BackEnd.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChamCong_BackEnd.Server.Data
{
    public class NhanDienDbContext : IdentityDbContext<ApplicationUser>
    {
        public NhanDienDbContext(DbContextOptions options) : base(options) { }
       
        public DbSet<Employee> Employees { get; set; }
        public DbSet<FaceModel> Faces { get; set; }
        public DbSet<Timekeeping> Timekeepings { get; set; }
        public DbSet<Time> times { get; set; }
        public DbSet<Unknown> Unknowns { get; set; }
      
    }
}
