using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CentralDeErros.Data.Audit
{
    public class AuditModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //[BsonElement("usuarioId")]
        public string UserToken { get; set; }

        public DateTime Date { get; set; }

        public string Entitie { get; set; }

        public string Operation { get; set; }

        public string EntitieId { get; set; }
    }
}
