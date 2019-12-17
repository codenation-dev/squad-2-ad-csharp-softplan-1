using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CentralDeErros.Domain.Models;

namespace CentralDeErros.Data.Context.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("token");
            builder.Property(p => p.Name).HasColumnName("name").IsRequired().HasColumnType("varchar").HasMaxLength(255);
            builder.Property(p => p.Email).HasColumnName("email").IsRequired().HasColumnType("varchar").HasMaxLength(255);
            builder.Property(p => p.Password).HasColumnName("password").IsRequired().HasColumnType("varchar").HasMaxLength(50);
            builder.Property(p => p.Role).HasColumnName("role").HasColumnType("varchar").HasMaxLength(50).HasDefaultValue("USUARIO");
        }
    }
}
