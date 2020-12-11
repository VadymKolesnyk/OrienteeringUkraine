using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Tables;

namespace DataLayer
{
    public class EFContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }

        public EFContext(DbContextOptions<EFContext> options) : base(options)
        {

        }
    }
}
