using System;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions options): base(options)
        {



        }

        public DbSet<AppUser> Users{ get; set; }
        
    }   
}