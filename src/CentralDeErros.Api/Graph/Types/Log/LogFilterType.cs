using CentralDeErros.Domain.Filters;
using GraphQL.Types;
using System.Diagnostics.CodeAnalysis;

namespace CentralDeErros.Api.Graph.Types.Log
{
    [ExcludeFromCodeCoverage]
    public class LogFilterType : InputObjectGraphType<LogFilter>
    {
        public LogFilterType()
        {
            Name = "logFilter";
            Description = "Filter of log";

            Field(vf => vf.Id, true, typeof(IdGraphType)).Description("Id log");
            Field(vf => vf.Title, true, typeof(StringGraphType)).Description("Title of log");
            Field(vf => vf.Event, true, typeof(IntGraphType)).Description("Event of log");
            Field(vf => vf.Level, true, typeof(StringGraphType)).Description("Level log");
            Field(vf => vf.Environment, true, typeof(StringGraphType)).Description("Environment of log");
            Field(vf => vf.Enabled, true, typeof(BooleanGraphType)).Description("Enabled of log");
            Field(vf => vf.CreatedAt, true, typeof(DateGraphType)).Description("CreatedAt of log");
            Field(vf => vf.Ip, true, typeof(StringGraphType)).Description("Ip of log");
            Field(vf => vf.Token, true, typeof(IdGraphType)).Description("Token of user log");
        }
    }
}
