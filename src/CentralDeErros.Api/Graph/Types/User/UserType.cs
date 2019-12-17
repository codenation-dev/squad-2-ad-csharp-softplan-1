using GraphQL.Types;
using CentralDeErros.Application.ViewModels;
using CentralDeErros.Api.Graph.Types.Log;
using System.Diagnostics.CodeAnalysis;

namespace CentralDeErros.Api.Graph.Types.User
{
    [ExcludeFromCodeCoverage]
    public class UserType : ObjectGraphType<UserViewModel>
    {
        public UserType()
        {
            Name = "user";
            Description = "Entitie User";
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Id user");
            Field(x => x.Email).Description("Email of user");
            Field(x => x.Name).Description("Name of user");
            Field(x => x.Active).Description("Active user");
            Field(x => x.Role, true).Description("Role of user");
            Field(x => x.Logs, true, typeof(ListGraphType<LogType>)).Description("Logs");
        }
    }
}
