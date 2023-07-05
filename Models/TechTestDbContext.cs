using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TechTest.Models;

namespace TechTest.Models;

public partial class TechTestDbContext : DbContext
{
    public string Connection { get; }

    public TechTestDbContext(string value)
    {
        Connection = value;
    }
    public TechTestDbContext()
    {
    }

    public TechTestDbContext(DbContextOptions<TechTestDbContext> options)
        : base(options)
    {
    }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
			   .SetBasePath(Directory.GetCurrentDirectory())
			   .AddJsonFile("appsettings.json")
			   .Build();
			var connectionString = configuration.GetConnectionString("login");
			optionsBuilder.UseSqlServer(connectionString);
		}
	}


	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<TechTest.Models.UserModel>? PERSONA { get; set; }
    public DbSet<TechTest.Models.LoginModel>? USUARIO { get; set; }
}
