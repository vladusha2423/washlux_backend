using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashLux.Models
{
    public class RowContext : IdentityDbContext<User>
    {
        public DbSet<Row> Rows { get; set; }
        public DbSet<ServiceName> Services { get; set; }
        public RowContext(DbContextOptions<RowContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
