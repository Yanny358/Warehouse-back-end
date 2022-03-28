using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WarehouseAPI.Entities;

namespace WarehouseAPI;

public class ApplicationDbContext : IdentityDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        base.OnModelCreating(modelBuilder);
    }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Storage> Storages { get; set; }
    public DbSet<Item> Items { get; set; }
}