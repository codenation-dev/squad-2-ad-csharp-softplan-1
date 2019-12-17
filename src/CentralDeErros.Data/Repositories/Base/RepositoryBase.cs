using CentralDeErros.Data.Audit;
using CentralDeErros.Data.Context;
using CentralDeErros.Domain.Interfaces.Base;
using CentralDeErros.Domain.Models.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralDeErros.Data.Repositories.Base
{
    /// <summary>
    /// Classe de repositório base
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class RepositoryBase<TModel> : IRepositoryBase<TModel> where TModel : ModelBase
    {
        private readonly Contexto _context;
        protected readonly MongoDbContext _mongoDbContext;

        private enum Operation { inclusion, change, exclusion };

        public RepositoryBase(Contexto contexto)
        {
            _context = contexto;
            _mongoDbContext = new MongoDbContext();
        }

        /// <summary>
        /// Método para pesquisar
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>IList<TModel></returns>
        public IList<TModel> Find(Func<TModel, bool> predicate)
        {
            return _context.Set<TModel>().AsNoTracking().Where(predicate).ToList();
        }

        /// <summary>
        /// Método para listar todos
        /// </summary>
        /// <returns>IList<TModel></returns>
        public IList<TModel> GetAll()
        {
            return _context.Set<TModel>().AsNoTracking().ToList();
        }

        /// <summary>
        /// Método para buscar registro especifico
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>TModel</returns>
        public TModel GetById(Guid id)
        {
            return _context.Set<TModel>().AsNoTracking().FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Método para incluir
        /// </summary>
        /// <param name="obj">Registro a ser inlcuído</param>
        public TModel Add(TModel obj)
        {
            _context.Set<TModel>().Add(obj);
            _context.SaveChanges();

            IncludeAudit(obj.Id, Operation.inclusion);

            return obj;
        }

        /// <summary>
        /// Método para remover
        /// </summary>
        /// <param name="id">Identificador</param>
        public void Remove(Guid id)
        {
            _context.Set<TModel>().Remove(this.GetById(id));
            _context.SaveChanges();

            IncludeAudit(id, Operation.inclusion);
        }

        /// <summary>
        /// Método para atualizar
        /// </summary>
        /// <param name="obj">Registro a ser atualizado</param>
        public void Update(TModel obj)
        {
            _context.Update<TModel>(obj);
            _context.SaveChanges();

            IncludeAudit(obj.Id, Operation.inclusion);
        }

        /// <summary>
        /// Efetuando a auditoria da operação realizada
        /// </summary>
        /// <param name="entitieId">Registro manipulado (incluído/excluído/alterado)</param>
        /// <param name="op">Operação</param>
        /// <param name="userToken">Token do usuário</param>
        private void IncludeAudit(Guid entitieId, Operation op )
        {
            if (!MongoDbContext.ConnectionMongoActive)
                return;

            Task.Run(() => {
                var audit = new AuditModel
                {
                    UserToken = "8fd56826-43cb-4604-aecc-298d8c4e551f",
                    Date = DateTime.UtcNow,
                    Entitie = typeof(TModel).Name,
                    EntitieId = entitieId.ToString(),
                    Operation = op.ToString()
                };

                _mongoDbContext.Audit.InsertOne(audit);
            });
        }
    }
}