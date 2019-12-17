using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CentralDeErros.Test.UnitTest.Domain
{
    public abstract class ModelBaseTest
    {
        private readonly DbContext context;

        protected string Model { get; set; }

        protected string Table { get; set; }

        public ModelBaseTest(DbContext context)
        {
            this.context = context;
        }

        public IEntityType GetEntity()
        {
            return context.Model.FindEntityType(Model);
        }

        private IEntityType GetEntity(string tableName)
        {
            return context.Model.GetEntityTypes().
                FirstOrDefault(x => GetTableName(x) == tableName);
        }

        protected IEnumerable<string> GetPrimaryKeys(IEntityType entity)
        {
            var keys = entity.FindPrimaryKey();
            return keys?.Properties.
                Select(x => this.GetFieldName(x)).
                ToList();
        }

        protected string GetTableName(IEntityType entity)
        {
            var annotation = entity.FindAnnotation("Relational:TableName");
            return annotation?.Value?.ToString();
        }

        protected IProperty FindField(IEntityType entity, string fieldName)
        {
            var properties = entity.GetProperties();
            return properties.FirstOrDefault(x => this.GetFieldName(x) == fieldName);
        }

        protected IEnumerable<string> GetFieldsName(IEntityType entity)
        {
            var properties = entity.GetProperties();
            return properties.Select(x => this.GetFieldName(x));
        }

        protected string GetFieldName(IProperty property)
        {
            var annotation = property.FindAnnotation("Relational:ColumnName");
            return annotation?.Value?.ToString();
        }

        protected int GetFieldSize(IProperty property)
        {
            return property.GetMaxLength().Value;
        }

        public void AssertTable()
        {
            var entity = GetEntity();
            Assert.NotNull(entity);
            var actual = this.GetTableName(entity);
            Assert.Equal(Table, actual);
        }

        public void AssertPrimaryKeys(params string[] keys)
        {
            var entity = GetEntity();
            Assert.NotNull(entity);

            var actualKeys = GetPrimaryKeys(entity);
            Assert.NotNull(actualKeys);
            Assert.Contains(keys, x => actualKeys.Contains(x));
        }

        public void AssertField(string fieldName, bool isNullable, Type fieldType, int? fieldSize)
        {
            var entity = GetEntity();
            Assert.NotNull(entity);
            Assert.Contains(fieldName, GetFieldsName(entity));

            var property = FindField(entity, fieldName);
            var expected = new
            {
                type = fieldType,
                nullable = isNullable,
                size = fieldSize.HasValue ? fieldSize.Value : 0
            }.ToString();
            var actual = new
            {
                type = property.ClrType,
                nullable = property.IsColumnNullable(),
                size = fieldSize.HasValue ? GetFieldSize(property) : 0
            }.ToString();
            Assert.Equal(expected, actual);
        }

        public void AssertForeignKey(string fieldName, string expectedRelatedTable, bool required, params string[] expectedKeys)
        {
            var entity = GetEntity();
            Assert.NotNull(entity);

            var relatedEntity = GetEntity(expectedRelatedTable);
            Assert.NotNull(relatedEntity);

            var property = FindField(entity, fieldName);
            Assert.NotNull(property);

            var foreignKey = entity.FindForeignKeys(property).
                FirstOrDefault(x => x.PrincipalEntityType == relatedEntity);
            Assert.NotNull(foreignKey);

            Assert.Equal(required, foreignKey.IsRequired);

            var actualKeys = foreignKey.PrincipalKey.Properties.Select(x => GetFieldName(x));
            Assert.Contains(expectedKeys, x => actualKeys.Contains(x));
        }

        public void AssertChildrenNavigation(string navigationTable)
        {
            var entity = GetEntity();
            Assert.NotNull(entity);

            var relatedEntity = GetEntity(navigationTable);
            Assert.NotNull(relatedEntity);

            var navigation = entity.GetNavigations().
                FirstOrDefault(x => x.ForeignKey.DeclaringEntityType == relatedEntity);

            Assert.NotNull(navigation);
            Assert.True(typeof(IEnumerable).IsAssignableFrom(navigation.ClrType));
        }

    }
}