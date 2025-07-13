using Microsoft.EntityFrameworkCore;
using RobotControllerApi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotControllerApi.Infrastructure.Repositories
{
   public class RoboDbContext : DbContext
    {
        public RoboDbContext(DbContextOptions<RoboDbContext> options) : base(options) { }
        public DbSet<Robot> Robots { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public RoboDbContext()
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Robot>().HasKey(r => r.Id);
            modelBuilder.Entity<Robot>().Property(r => r.Facing).HasConversion<string>();
            modelBuilder.Entity<Room>().HasKey(b => b.Id);            
        }
    }
}
