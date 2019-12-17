using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CentralDeErros.Domain.Models;
using CentralDeErros.CrossCutting.Constants;
using CentralDeErros.CrossCutting.Utils;
using System.IO;
using System.Linq;

namespace CentralDeErros.Data.Context
{
    public class Contexto : DbContext
    {
        public Contexto()
        {

        }
        public Contexto(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder build)
        {
            foreach (var property in build.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(string)))
            {
                if (property.GetMaxLength() == null) property.SetMaxLength(255);
            }

            build.ApplyConfigurationsFromAssembly(typeof(Contexto).Assembly);

            build.Entity<User>().HasData(new User { Name = "Administrador", Email = "admin@mail.com", Password = "123456".ToHashMD5(), Role = Constants.PERFIL_ADMIN });
            build.Entity<User>().HasData(new User { Name = "Usuário Teste", Email = "usuario@mail.com", Password = "123456".ToHashMD5(), Role = Constants.PERFIL_USUARIO });
            build.Entity<User>().HasData(new User { Name = "Usuário Teste (Inativo)", Email = "usuario_inativo@mail.com", Password = "123456".ToHashMD5(), Role = Constants.PERFIL_USUARIO, Active = false});
        }
    }
}
