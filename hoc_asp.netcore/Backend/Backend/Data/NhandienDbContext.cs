using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class NhandienDbContext :DbContext
    {
        public NhandienDbContext(DbContextOptions<NhandienDbContext> opt):base(opt){}
        #region DbSet
        public DbSet<Employee> employees { get; set; }
        public DbSet<FaceModels> faceModels { get; set; }
        public DbSet<TimeKeeping> timeKeepings { get; set; }
        public DbSet<TimeSheet> timeSheets { get; set; }
        public DbSet<Unknown> unknowns { get; set; }
        #endregion
    }
}
