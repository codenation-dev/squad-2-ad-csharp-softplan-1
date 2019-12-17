using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CentralDeErros.Domain.Models;
using CentralDeErros.CrossCutting.Constants;

namespace CentralDeErros.Data.Context.Mappings
{
    public class LogMapping : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Log");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Title).HasColumnName("title").IsRequired().HasColumnType("varchar").HasMaxLength(255);
            builder.Property(p => p.Detail).HasColumnName("detail").IsRequired().HasColumnType("varchar").HasMaxLength(4000);
            builder.Property(p => p.Event).HasColumnName("event").IsRequired().HasColumnType("int");
            builder.Property(p => p.Level).HasColumnName("level").IsRequired().HasColumnType("char").HasMaxLength(1).HasDefaultValue(Constants.LEVEL_WARNING);
            builder.Property(p => p.Environment).HasColumnName("environment").IsRequired().HasColumnType("char").HasMaxLength(1).HasDefaultValue(Constants.ENVIRONMENT_DEVELOPMENT);
            builder.Property(p => p.Ip).HasColumnName("ip").HasColumnType("varchar").HasMaxLength(20);

            builder.HasOne(p => p.User)
                .WithMany(c => c.Logs)
                .HasForeignKey(p => p.Token)
                .IsRequired();
        }
    }
}
