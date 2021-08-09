using Base5.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base5.Persistence.Configurations
{
    public class KPIConfiguration : IEntityTypeConfiguration<KPI>
    {
        public void Configure(EntityTypeBuilder<KPI> builder)
        {
            builder.ToTable("TblKPI");

            builder.HasKey(a => a.KPIDNum);

            builder.Property(a => a.KPIDDescription)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(a => a.DepNo)
                .IsRequired();
        }
    }
}