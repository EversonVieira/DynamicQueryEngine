using DynamicQueryEngine.Core.Config;
using DynamicQueryEngine.Core.DataSet.Interfaces;
using DynamicQueryEngine.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicQueryEngine.Core.DataSet
{
    public class SqlDataSetBuilder : ISqlDataSetBuilder
    {
        private string? _mainTable;
        private List<SqlDataSetField> _fields = [];
        private List<SqlDataSetTableRelation> _relations = [];
        private ISqlDataSetCache? _cache;
        private TimeSpan? _timeSpan;
        private SqlDataSetBuilderConfig _config;

        public SqlDataSetBuilder(SqlDataSetBuilderConfig config)
        {
            _config = config;
        }

        public ISqlDataSetBuilder AddLocalCache()
        {
            _cache = new SqlDataSetMemoryCache();
            return this;
        }

        public ISqlDataSet Build()
        {

            if (string.IsNullOrWhiteSpace(_mainTable))
                throw new DynamicQueryEngineException("Failed to Build SqlDataSet: Missing table name");

            if (_fields.Count == 0)
            {
                throw new DynamicQueryEngineException("Failed to Build SqlDataSet: No fields added");
            }

            if (_config.ValidateScriptWhenBuilding)
            {
                // TO DO: Add some validators to check if the dataset is valid.
                // For example, check if the fields and relations are consistent, or if the main table exists in the fields or relations.
            }

            return new SqlDataSet(_fields, _mainTable!, _relations, _cache, _timeSpan);
        }

        public ISqlDataSetBuilder WithCacheExpiration(TimeSpan timeSpan)
        {
            _timeSpan = timeSpan;
            return this;
        }

        public ISqlDataSetBuilder WithExternalCache(ISqlDataSetCache externalCache)
        {
            _cache = externalCache;
            return this;
        }

        public ISqlDataSetBuilder WithField(string tableName, string fieldName, SqlDataSetFieldType type)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new DynamicQueryEngineException("Table Name cannot be unknown.");

            if (string.IsNullOrWhiteSpace(fieldName))
                throw new DynamicQueryEngineException("Field Name cannot be unknown.");



            _fields.Add(new SqlDataSetField(tableName, fieldName, type));
            return this;
        }

        public ISqlDataSetBuilder WithMainTable(string mainTable)
        {
            if (string.IsNullOrWhiteSpace(mainTable))
                throw new DynamicQueryEngineArgumentNullException(nameof(mainTable));

            if (!string.IsNullOrWhiteSpace(_mainTable))
                throw new DynamicQueryEngineException("Main table is already set.");

            _mainTable = mainTable;
            return this;
        }

        public ISqlDataSetBuilder WithRelation(string sourceTable, string joinedTable, string sourceField, string joinedField, SqlDataSetTableRelationType type)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(sourceField))
            {
                sb.AppendLine("Source field is required.");
            }

            if (string.IsNullOrWhiteSpace(joinedField))
            {
                sb.AppendLine("Joined field is required.");
            }

            if (string.IsNullOrWhiteSpace(sourceTable))
            {
                sb.AppendLine("Source table is required.");
            }

            if (string.IsNullOrWhiteSpace(joinedTable))
            {
                sb.AppendLine("Joined table is required.");
            }

            if (sb.Length > 0)
            {
                throw new DynamicQueryEngineException(sb.ToString());
            }

            _relations.Add(new SqlDataSetTableRelation(sourceTable, joinedTable, sourceField, joinedField, type));
            return this;
        }
    }
}
