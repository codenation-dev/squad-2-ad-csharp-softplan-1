using CentralDeErros.Application.ViewModels;
using GraphQL.Types;
using System.Diagnostics.CodeAnalysis;

namespace CentralDeErros.Api.Graph.Types.Log
{
    [ExcludeFromCodeCoverage]
    public class LogInputType : InputObjectGraphType<LogViewModel>
    {
        public LogInputType()
        {
            Name = "logInput";
            Description = "Input of log";

            Field(x => x.Id, true, typeof(IdGraphType)).Description("Id log");
            Field(x => x.Title).Description("Title of log");
            Field(x => x.Detail, true).Description("Detail of log");
            Field(x => x.Level).Description("Level log");
            Field(x => x.Event).Description("Event of log");
            Field(x => x.CreatedAt).Description("CreatedAt of log");
            Field(x => x.Environment).Description("Environment of log");
            Field(x => x.Ip, true).Description("Ip of log");
            Field(x => x.Enabled).Description("Enabled of log");
            Field(x => x.Token, type: typeof(StringGraphType)).Description("Token of user log");
        }
    }
}
