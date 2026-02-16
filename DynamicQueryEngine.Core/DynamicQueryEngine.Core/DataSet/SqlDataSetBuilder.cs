using DynamicQueryEngine.Core.Config;
using DynamicQueryEngine.Core.DataSet.Adapters;
using DynamicQueryEngine.Core.DataSet.Interfaces;
using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;
using DynamicQueryEngine.Core.DataSet.Validation;
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
        private ISqlDialectAdapter? _dialectAdapter;
        private List<WhereClause> _whereClauses = [];
        private List<AdvancedWhereClause> _advancedWhereClauses = [];
        private List<GroupByClause> _groupByClauses = [];
        private List<HavingClause> _havingClauses = [];
        private List<OrderByClause> _orderByClauses = [];
        private LimitOffsetClause? _limitOffset;
        private List<JoinClause> _joins = [];
        private List<SubQuery> _subQueries = [];
        private ISqlDataSetValidator? _validator;
        private bool _isDistinct;
        private List<AggregateFunction> _aggreageFunctions = [];

        public SqlDataSetBuilder(SqlDataSetBuilderConfig config)
        {
            _config = config;
        }

        public ISqlDataSetBuilder AddLocalCache()
        {
            _cache = new SqlDataSetMemoryCache();
            return this;
        }

        public ISqlDataSetBuilder WithDialect(ISqlDialectAdapter dialectAdapter)
        {
            if (dialectAdapter == null)
                throw new DynamicQueryEngineArgumentNullException(nameof(dialectAdapter));

            _dialectAdapter = dialectAdapter;
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

            if (_config.ValidateScriptWhenBuilding && _validator != null)
            {
                _validator.ValidateTable(_mainTable);
                foreach (var field in _fields)
                {
                    _validator.ValidateField(field.TableName, field.Name, field.Type);
                }
            }

            _dialectAdapter ??= new PostgresSqlDialectAdapter();

            return new SqlDataSet(_fields, _mainTable!, _relations, _cache, _timeSpan, _dialectAdapter, _whereClauses, _advancedWhereClauses, _groupByClauses, _havingClauses, _orderByClauses, _limitOffset, _joins, _subQueries, _isDistinct, _aggreageFunctions);
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

        public ISqlDataSetBuilder WithWhere(string tableName, string fieldName, string op, object value)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new DynamicQueryEngineException("Table name is required for WHERE clause.");

            if (string.IsNullOrWhiteSpace(fieldName))
                throw new DynamicQueryEngineException("Field name is required for WHERE clause.");

            if (string.IsNullOrWhiteSpace(op))
                throw new DynamicQueryEngineException("Operator is required for WHERE clause.");

            if (value == null)
                throw new DynamicQueryEngineException("Value is required for WHERE clause.");

            _whereClauses.Add(new WhereClause(tableName, fieldName, op, value));
            return this;
        }

        public ISqlDataSetBuilder WithGroupBy(string tableName, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new DynamicQueryEngineException("Table name is required for GROUP BY clause.");

            if (string.IsNullOrWhiteSpace(fieldName))
                throw new DynamicQueryEngineException("Field name is required for GROUP BY clause.");

            _groupByClauses.Add(new GroupByClause(tableName, fieldName));
            return this;
        }

        public ISqlDataSetBuilder WithHaving(string aggregateFunction, string fieldName, string op, object value)
        {
            if (string.IsNullOrWhiteSpace(aggregateFunction))
                throw new DynamicQueryEngineException("Aggregate function is required for HAVING clause.");

            if (string.IsNullOrWhiteSpace(fieldName))
                throw new DynamicQueryEngineException("Field name is required for HAVING clause.");

            if (string.IsNullOrWhiteSpace(op))
                throw new DynamicQueryEngineException("Operator is required for HAVING clause.");

            if (value == null)
                throw new DynamicQueryEngineException("Value is required for HAVING clause.");

            _havingClauses.Add(new HavingClause(aggregateFunction, fieldName, op, value));
            return this;
        }

        public ISqlDataSetBuilder WithOrderBy(string tableName, string fieldName, SortDirection direction = SortDirection.Ascending)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new DynamicQueryEngineException("Table name is required for ORDER BY clause.");

            if (string.IsNullOrWhiteSpace(fieldName))
                throw new DynamicQueryEngineException("Field name is required for ORDER BY clause.");

            _orderByClauses.Add(new OrderByClause(tableName, fieldName, direction));
            return this;
        }

        public ISqlDataSetBuilder WithLimitOffset(int? limit, int? offset = null)
        {
            _limitOffset = new LimitOffsetClause(limit, offset);
            return this;
        }

        public ISqlDataSetBuilder WithJoin(string sourceTable, string joinedTable, string sourceField, string joinedField, JoinType joinType = JoinType.Inner)
        {
            if (string.IsNullOrWhiteSpace(sourceTable))
                throw new DynamicQueryEngineException("Source table is required for JOIN clause.");

            if (string.IsNullOrWhiteSpace(joinedTable))
                throw new DynamicQueryEngineException("Joined table is required for JOIN clause.");

            if (string.IsNullOrWhiteSpace(sourceField))
                throw new DynamicQueryEngineException("Source field is required for JOIN clause.");

            if (string.IsNullOrWhiteSpace(joinedField))
                throw new DynamicQueryEngineException("Joined field is required for JOIN clause.");

            _joins.Add(new JoinClause(sourceTable, joinedTable, sourceField, joinedField, joinType));
            return this;
        }

        public ISqlDataSetBuilder WithSubQuery(string alias, SqlDataSet subQuery)
        {
            if (string.IsNullOrWhiteSpace(alias))
                throw new DynamicQueryEngineException("Alias is required for sub-query.");

            if (subQuery == null)
                throw new DynamicQueryEngineArgumentNullException(nameof(subQuery));

            _subQueries.Add(new SubQuery(alias, subQuery));
            return this;
        }

        public ISqlDataSetBuilder WithValidator(ISqlDataSetValidator validator)
        {
            _validator = validator ?? throw new DynamicQueryEngineArgumentNullException(nameof(validator));
            return this;
        }

        public string ExportToJson()
        {
            if (string.IsNullOrWhiteSpace(_mainTable))
                throw new DynamicQueryEngineException("Cannot export to JSON: DataSet not built yet.");

            var tempDataSet = Build();
            return tempDataSet.ExportToJson();
        }

        // ========== NOVOS MÉTODOS ==========

        public ISqlDataSetBuilder WithAdvancedWhere(string tableName, string fieldName, WhereOperator op, object? value = null, object? valueEnd = null)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new DynamicQueryEngineException("Table name is required for advanced WHERE clause.");

            if (string.IsNullOrWhiteSpace(fieldName))
                throw new DynamicQueryEngineException("Field name is required for advanced WHERE clause.");

            _advancedWhereClauses.Add(new AdvancedWhereClause(tableName, fieldName, op, value, valueEnd));
            return this;
        }

        public ISqlDataSetBuilder WithDistinct()
        {
            _isDistinct = true;
            return this;
        }

        public ISqlDataSetBuilder WithAggregateFunction(string functionName, string tableName, string fieldName, string? alias = null)
        {
            if (string.IsNullOrWhiteSpace(functionName))
                throw new DynamicQueryEngineException("Function name is required for aggregate function.");

            if (string.IsNullOrWhiteSpace(tableName))
                throw new DynamicQueryEngineException("Table name is required for aggregate function.");

            if (string.IsNullOrWhiteSpace(fieldName))
                throw new DynamicQueryEngineException("Field name is required for aggregate function.");

            _aggreageFunctions.Add(new AggregateFunction(functionName, tableName, fieldName, alias));
            return this;
        }

        public string GetComplexityReport()
        {
            var tempDataSet = Build();
            return tempDataSet.GetComplexityReport();
        }
    }
}

