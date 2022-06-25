using FinalPraktika3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalPraktika3.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions opt) : base(opt) { }
        public DbSet<Comment> Comments { get; set; }
    }
}
