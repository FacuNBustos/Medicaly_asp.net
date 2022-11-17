using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Medicaly.Models;

namespace Medicaly.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Medicaly.Models.Gender> Gender { get; set; }
        public DbSet<Medicaly.Models.Specialty> Specialty { get; set; }
        public DbSet<Medicaly.Models.Patient> Patient { get; set; }
        public DbSet<Medicaly.Models.Staff> Staff { get; set; }
        public DbSet<Medicaly.Models.Turn> Turn { get; set; }
        public DbSet<Medicaly.Models.Medical_Record> Medical_Record { get; set; }
    }
}
