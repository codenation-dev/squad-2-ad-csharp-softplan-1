using CentralDeErros.Application.ViewModels;
using CentralDeErros.Domain.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace CentralDeErros.Application.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class FilterUserExtensions
    {
        public static Func<User, bool> UserFilter(this Func<User, bool> source, UserFilterViewModel filter)
        {
            if (filter == null)
                return p => true;

            Func<User, bool> predicateId = p => true;
            Func<User, bool> predicateName = p => true;
            Func<User, bool> predicateEmail = p => true;
            Func<User, bool> predicateActive = p => true;
            Func<User, bool> predicateRole = p => true;

            if (filter.Id != null)
                predicateId = p => p.Id == filter.Id;
            if (!string.IsNullOrEmpty(filter.Name))
                predicateName = p => p.Name == filter.Name;
            if (!string.IsNullOrEmpty(filter.Email))
                predicateEmail = p => p.Email == filter.Email;
            if (filter.Active.HasValue)
                predicateActive = p => p.Active == filter.Active;
            if (!string.IsNullOrEmpty(filter.Role))
                predicateRole = p => p.Role == filter.Role;

            return p => predicateId(p) && predicateName(p) &&
                        predicateEmail(p) && predicateActive(p) &&
                        predicateRole(p);
    }
    }
}
