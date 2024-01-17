using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Persistance.Context;

public sealed class AppDbContext : IdentityDbContext<User, IdentityRole, string> //sealed keyi başka classa inherit olmasını engeller
{
    public AppDbContext(DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder) { 
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly); //Configurationları DbCOntexte bağlar.(Kaç tane configuration yazarsan yaz)
        modelBuilder.Ignore<IdentityUserLogin<string>>();
        modelBuilder.Ignore<IdentityUserRole<string>>();
        modelBuilder.Ignore<IdentityUserClaim<string>>();
        modelBuilder.Ignore<IdentityUserToken<string>>();
        modelBuilder.Ignore<IdentityRoleClaim<string>>();
        modelBuilder.Ignore<IdentityRole<string>>();//Identity kütüphanesinden gelen bazı satırların gelmesini engelledik.
    
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();
        foreach (var entry in entries)
        {
            if(entry.State == EntityState.Added)
            {
                entry.Property(p=> p.CreatedDate).CurrentValue = DateTime.Now;
            } // ekleme işleminde createddate kısmını otomatik dolduracak

            if (entry.State == EntityState.Modified)
            {
                entry.Property(p => p.UpdatedDate).CurrentValue = DateTime.Now;
            } // UpdatedDate kısmını otomatik dolduracak
        }
        return base.SaveChangesAsync(cancellationToken);
    }



    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer();
    } veritabanı bağlantısı için bu da bir seçenek ancak yukarıdaki best practicedir.*/


}
