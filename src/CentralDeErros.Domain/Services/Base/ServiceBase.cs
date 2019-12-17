﻿using CentralDeErros.Domain.Interfaces.Base;
using CentralDeErros.Domain.Models.Base;
using System;
using System.Collections.Generic;

namespace CentralDeErros.Domain.Services.Base
{
    /// <summary>
    /// Classe ServiceBase
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ServiceBase<TModel> : IServiceBase<TModel> where TModel : ModelBase
    {
        protected IRepositoryBase<TModel> _repositoryBase;

        public ServiceBase(IRepositoryBase<TModel> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        /// <summary>
        /// Método para incluir
        /// </summary>
        /// <param name="obj">Registro a ser inlcuído</param>
        public TModel Add(TModel obj)
        {
            return _repositoryBase.Add(obj);
        }

        /// <summary>
        /// Método para pesquisar
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>IList<TModel></returns>
        public IList<TModel> Find(Func<TModel, bool> predicate)
        {
            return _repositoryBase.Find(predicate);
        }

        /// <summary>
        /// Método para listar todos
        /// </summary>
        /// <returns>IList<TModel></returns>
        public IList<TModel> GetAll()
        {
            return _repositoryBase.GetAll();
        }

        /// <summary>
        /// Método para buscar registro especifico
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>TModel</returns>
        public TModel GetById(Guid id)
        {
            return _repositoryBase.GetById(id);
        }

        /// <summary>
        /// Método para remover
        /// </summary>
        /// <param name="id">Identificador</param>
        public void Remove(Guid id)
        {
            _repositoryBase.Remove(id);
        }

        /// <summary>
        /// Método para atualizar
        /// </summary>
        /// <param name="obj">Registro a ser atualizado</param>
        public void Update(TModel obj)
        {
            _repositoryBase.Update(obj);
        }
    }
}