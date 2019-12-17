﻿using System;
using System.Collections.Generic;

namespace CentralDeErros.Domain.Interfaces.Base
{
    /// <summary>
    /// Interface base do repositório
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IRepositoryBase<TModel> where TModel : class
    {
        /// <summary>
        /// Método para incluir um novo registro
        /// </summary>
        /// <param name="obj">Objeto a ser incluído</param>
        TModel Add(TModel obj);

        /// <summary>
        /// Método para alterar um registro
        /// </summary>
        /// <param name="obj">Objeto a ser alterado</param>
        void Update(TModel obj);

        /// <summary>
        /// Método para excluir um registro
        /// </summary>
        /// <param name="id">Identificador do registro</param>
        void Remove(Guid id);

        /// <summary>
        /// Método para buscar um registro especifico
        /// </summary>
        /// <param name="id">Identificador do registro</param>
        /// <returns></returns>
        TModel GetById(Guid id);

        /// <summary>
        /// Método para retornar todos os registros
        /// </summary>
        /// <returns>IList<TModel></returns>
        IList<TModel> GetAll();

        /// <summary>
        /// Método para efetuar a pesquisa de registro
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>IList<TModel> </returns>
        IList<TModel> Find(Func<TModel, bool> predicate);
    }
}
