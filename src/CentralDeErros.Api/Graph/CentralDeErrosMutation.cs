using CentralDeErros.Api.Graph.Types.User;
using CentralDeErros.Api.Graph.Types.Log;
using CentralDeErros.Application.Interface;
using CentralDeErros.Application.ViewModels;
using CentralDeErros.CrossCutting.Utils;
using GraphQL.Types;
using GraphQL;
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace CentralDeErros.Api.Graph
{

    [ExcludeFromCodeCoverage]
    public class CentralDeErrosMutation : ObjectGraphType
    {
        public CentralDeErrosMutation(IUserAppService userAppService, ILogAppService logAppService)
        {
            Field<UserType>(
                "userInput",
                description: "Mutation add/edit user",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }),
                resolve: context =>
                {
                    var user = context.GetArgument<UserViewModel>("user");

                    if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Password))
                    {
                        context.Errors.Add(new ExecutionError("Campos obrigatórios não informados"));
                        return null;
                    }

                    user.Password = user.Password.ToHashMD5();

                    UserViewModel userLocal;
                    if (user.Id != null)
                        userLocal = userAppService.Find(p => p.Id == user.Id).FirstOrDefault();
                    else userLocal = userAppService.Find(p => p.Email == user.Email).FirstOrDefault();

                    if (userLocal != null)
                    {
                        user.Id = userLocal.Id;
                        userAppService.Update(user);
                        return user;
                    }

                    return userAppService.Add(user);
                });

            Field<LogType>(
                "logInput",
                description: "Mutation add/edit log",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<LogInputType>> { Name = "log" }),
                resolve: context =>
                {
                    var log = context.GetArgument<LogViewModel>("log");

                    LogViewModel logLocal;
                    if (log.Id != null)
                        logLocal = logAppService.Find(p => p.Id == log.Id).FirstOrDefault();
                    else
                        logLocal = logAppService.Find(p => p.Title == log.Title).FirstOrDefault();

                    if (logLocal != null)
                    {
                        log.Id = logLocal.Id;
                        logAppService.Update(log);
                        return log;
                    }

                    return logAppService.Add(log);
                });


            Field<StringGraphType>(
                "deleteUser",
                description: "Mutation delete user",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: context =>
                {
                    var id = context.GetArgument<Guid>("id");
                    if (userAppService.GetById(id) == null)
                    {
                        context.Errors.Add(new ExecutionError("Usuário não localizado"));
                        return null;
                    }

                    userAppService.Remove(id);
                    return "Usuário excluído com sucesso.";
                });

            Field<StringGraphType>(
                "deleteLog",
                description: "Mutation delete log",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
                resolve: context =>
                {
                    var id = context.GetArgument<Guid>("id");
                    if (logAppService.GetById(id) == null)
                    {
                        context.Errors.Add(new ExecutionError("Log não localizado"));
                        return null;
                    }

                    logAppService.Remove(id);
                    return "Log excluído com sucesso.";
                });

        }
    }
}
