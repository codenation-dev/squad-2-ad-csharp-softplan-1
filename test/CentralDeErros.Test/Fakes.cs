using Newtonsoft.Json;
using CentralDeErros.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using CentralDeErros.Data.Context;
using System.Linq;

namespace CentralDeErros.Test
{
    /// Fake Data
    public class Fakes
    {
        public DbContextOptions<Contexto> FakeOptions { get; }

        private Dictionary<Type, string> DataFileNames { get; } =
            new Dictionary<Type, string>();
        private string FileName<T>() { return DataFileNames[typeof(T)]; }

        public Fakes(string testName)
        {
            FakeOptions = new DbContextOptionsBuilder<Contexto>()
            .UseInMemoryDatabase(databaseName: $"CentralDeErros_{testName}")
            .Options;

            DataFileNames.Add(typeof(User), "TestData\\users.json");
            DataFileNames.Add(typeof(Log), "TestData\\logs.json");
        }

        public void FillWithAll()
        {
            FillWith<User>();
            FillWith<Log>();
        }

        public void FillWith<T>() where T : class
        {
            using var context = new Contexto(FakeOptions);
            if (context.Set<T>().Count() == 0)
            {
                foreach (T item in GetFakeData<T>())
                    context.Set<T>().Add(item);
                context.SaveChanges();
            }
        }

        public List<T> GetFakeData<T>()
        {
            string content = File.ReadAllText(FileName<T>());
            return JsonConvert.DeserializeObject<List<T>>(content);
        }        
    }
}
