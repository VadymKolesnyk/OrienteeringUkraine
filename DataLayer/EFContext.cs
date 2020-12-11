using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Tables;

namespace DataLayer
{
    public class EFContext : DbContext
    {
        public DbSet<Application> Applications { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventGroup> EventGroups { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public EFContext(DbContextOptions<EFContext> options) : base(options)
        {

        }
    }
}
