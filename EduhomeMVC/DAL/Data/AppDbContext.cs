using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Parallax> Parallaxes { get; set; }
        public DbSet<ParallaxImage> ParallaxImages { get; set; }
    }
}
