using CentralDeErros.Application.ViewModels;
using CentralDeErros.Domain.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace CentralDeErros.Application.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class FilterLogExtensions
    {
        public static Func<Log, bool> LogFilter(this Func<Log, bool> source, LogFilterViewModel filter)
        {
            if (filter == null)
                return p => true;

            Func<Log, bool> predicateId = p => true;
            Func<Log, bool> predicateTitle = p => true;
            Func<Log, bool> predicateEvent = p => true;
            Func<Log, bool> predicateLevel = p => true;
            Func<Log, bool> predicateEnvironment = p => true;
            Func<Log, bool> predicateEnabled = p => true;
            Func<Log, bool> predicateCreatedAt = p => true;
            Func<Log, bool> predicateIp = p => true;
            Func<Log, bool> predicateToken = p => true;

            if (filter.Id != null)
                predicateId = p => p.Id == filter.Id;
            if (!string.IsNullOrEmpty(filter.Title))
                predicateTitle = p => p.Title == filter.Title;
            if (filter.Event.HasValue)
                predicateEvent = p => p.Event == filter.Event;
            if (!string.IsNullOrEmpty(filter.Level))
                predicateLevel = p => p.Level == filter.Level;
            if (!string.IsNullOrEmpty(filter.Environment))
                predicateEnvironment = p => p.Environment == filter.Environment;
            if (filter.Enabled.HasValue)
                predicateEnabled = p => p.Enabled == filter.Enabled;
            if (filter.CreatedAt.HasValue)
                predicateCreatedAt = p => p.CreatedAt == filter.CreatedAt;
            if (!string.IsNullOrEmpty(filter.Ip))
                predicateIp = p => p.Ip == filter.Ip;
            if (filter.Token.HasValue)
                predicateToken = p => p.Token == filter.Token;

            return p => predicateId(p) && predicateTitle(p) &&
                        predicateLevel(p) && predicateEvent(p) &&
                        predicateEnvironment(p) && predicateEnabled(p) &&
                        predicateCreatedAt(p) && predicateIp(p) &&
                        predicateToken(p);
        }
    }
}
