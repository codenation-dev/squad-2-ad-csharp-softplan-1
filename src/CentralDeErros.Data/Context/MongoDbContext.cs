using MongoDB.Driver;
using CentralDeErros.Data.Audit;
using System;
using System.Security.Authentication;
using System.Diagnostics.CodeAnalysis;

namespace CentralDeErros.Data.Context
{
    [ExcludeFromCodeCoverage]
    public class MongoDbContext
    {
        public static string ConnectionString { get; set; }
        public static bool ConnectionMongoActive { get; set; }
        public const string DATABASE_NAME = "squad_2_csharp";

        private IMongoDatabase _database { get; }

        public MongoDbContext()
        {
            ConnectionMongoActive = false;
            if (ConnectionString == default)
                return;

            try
            {
                var settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));
                settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };

                var mongoClient = new MongoClient(settings);
                _database = mongoClient.GetDatabase(DATABASE_NAME);
                ConnectionMongoActive = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível se conectar com o servidor.", ex);
            }
        }

        public IMongoCollection<AuditModel> Audit
        {
            get
            {
                return _database.GetCollection<AuditModel>("auditoria");
            }
        }
    }
}
