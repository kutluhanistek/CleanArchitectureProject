using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Persistance.Configuration;
// entity ismi ve sonuna Configuration
public sealed class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Cars");//veri tabanındaki tablonun adını oluşturuyoruz.
        builder.HasKey(p => p.Id);// Id nin primary key olduğunu belirtiyoruz
        builder.HasIndex(p => p.Name);//indexleme yapar
    }
}