﻿using DatingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
