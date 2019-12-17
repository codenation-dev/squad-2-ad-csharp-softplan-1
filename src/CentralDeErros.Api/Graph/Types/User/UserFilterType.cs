using CentralDeErros.Domain.Filters;
using GraphQL.Types;
using System.Diagnostics.CodeAnalysis;

namespace CentralDeErros.Api.Graph.Types.User
{
    [ExcludeFromCodeCoverage]
    public class UserFilterType : InputObjectGraphType<UserFilter>
    {
        public UserFilterType()
        {
            Name = "userFilter";
            Description = "Filter of user";

            Field(vf => vf.Id, true, typeof(IdGraphType)).Description("Id user");
            Field(vf => vf.Name, true, typeof(StringGraphType)).Description("Name of user");
            Field(vf => vf.Email, true, typeof(StringGraphType)).Description("Email of user");
            Field(vf => vf.Active, true, typeof(BooleanGraphType)).Description("Active user");
        }
    }
}
