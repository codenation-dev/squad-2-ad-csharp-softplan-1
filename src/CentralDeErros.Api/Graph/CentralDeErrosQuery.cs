using CentralDeErros.Api.Graph.Types.Log;
using CentralDeErros.Api.Graph.Types.User;
using CentralDeErros.Application.Interface;
using CentralDeErros.Application.ViewModels;
using GraphQL.Types;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CentralDeErros.Api.Graph
{
    [ExcludeFromCodeCoverage]
    public class CentralDeErrosQuery : ObjectGraphType
    {
        public CentralDeErrosQuery(IUserAppService userAppService, ILogAppService logAppService)
        {
            Field<ListGraphType<UserType>>("users",
                description:"Query of user",
                   arguments: new QueryArguments(new List<QueryArgument>
                   {
                       new QueryArgument<UserFilterType>
                    {
                        Name = "filter"
                    }
                   }),

           resolve: context =>
                   {
                       var filter = context.GetArgument<UserFilterViewModel?>("filter");
                       return userAppService.GetUsersFilter(filter);
                   }
               );

            Field<ListGraphType<LogType>>("logs",
                description: "Query of log",
                   arguments: new QueryArguments(new List<QueryArgument>
                   {
                       new QueryArgument<LogFilterType>
                    {
                        Name = "filter"
                    }
                   }),

           resolve: context =>
           {
               var filter = context.GetArgument<LogFilterViewModel?>("filter");
               return logAppService.GetLogsFilter(filter);
           }
               );
        }
    }
}
