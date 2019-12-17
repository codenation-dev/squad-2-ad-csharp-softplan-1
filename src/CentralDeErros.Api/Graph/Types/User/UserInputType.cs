using CentralDeErros.Api.Graph.Types.Log;
using CentralDeErros.Application.ViewModels;
using GraphQL.Types;
using System.Diagnostics.CodeAnalysis;

namespace CentralDeErros.Api.Graph.Types.User
{
    [ExcludeFromCodeCoverage]
    public class UserInputType : InputObjectGraphType<UserViewModel>
    {
        public UserInputType()
        {
            Name = "userInput";
            Description = "Input of user";

            Field(x => x.Id, true, typeof(IdGraphType)).Description("Id user");
            Field(x => x.Email).Description("Email of user");
            Field(x => x.Name).Description("Name of user");
            Field(x => x.Password).Description("Password of user");
            Field(x => x.Active, true).Description("Active user");
            Field(x => x.Role, true).Description("Role of user");
            Field(x => x.Logs, true, typeof(ListGraphType<LogInputType>)).Description("Logs");
        }
    }
}
