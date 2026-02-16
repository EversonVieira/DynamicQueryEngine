using DynamicQueryEngine.Core.DataSet.Adapters;
using DynamicQueryEngine.Core.DataSet.Complexity;
using DynamicQueryEngine.Core.DataSet.Interfaces;
using DynamicQueryEngine.Core.DataSet.Serialization;
using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace DynamicQueryEngine.Core.DataSet
{
    public class SqlDataSet : ISqlDataSet
    {
        public List<SqlDataSetField> Fields { get; init; }
        public string MainTable { get; init; }
        public string Id { get; private set; }
        public ISqlDataSetCache? Cache { get; init; }
        public TimeSpan? TimeSpan { get; init; }
        public ISqlDialectAdapter DialectAdapter { get; init; }
        public List<WhereClause> WhereClauses { get; init; } = [];
        public List<AdvancedWhereClause> AdvancedWhereClauses { get; init; } = [];
        public List<SubQueryWhereClause> SubQueryWhereClauses { get; init; } = [];
        public List<GroupByClause> GroupByClauses { get; init; } = [];
        public List<HavingClause> HavingClauses { get; init; } = [];
        public List<OrderByClause> OrderByClauses { get; init; } = [];
        public LimitOffsetClause? LimitOffset { get; init; }
        public List<JoinClause> Joins { get; init; } = [];
        public List<SubQuery> SubQueries { get; init; } = [];
        public bool IsDistinct { get; init; }
        public List<AggregateFunction> AggregateFunctions { get; init; } = [];
        public QueryComplexityScore ComplexityScore { get; private set; }

        internal SqlDataSet(List<SqlDataSetField> fields,
                            string mainTable,

                            ISqlDataSetCache? cache,
                            TimeSpan? timeSpan,
                            ISqlDialectAdapter dialectAdapter,
                            List<WhereClause>? whereClauses = null,
                            List<AdvancedWhereClause>? advancedWhereClauses = null,
                            List<SubQueryWhereClause>? subQueryWhereClauses = null,
                            List<GroupByClause>? groupByClauses = null,
                            List<HavingClause>? havingClauses = null,
                            List<OrderByClause>? orderByClauses = null,
                            LimitOffsetClause? limitOffset = null,
                            List<JoinClause>? joins = null,
                            List<SubQuery>? subQueries = null,
                            bool isDistinct = false,
                            List<AggregateFunction>? aggreageFunctions = null)
        {
            Fields = fields;
            MainTable = mainTable;
            Cache = cache;
            TimeSpan = timeSpan;
            DialectAdapter = dialectAdapter;
            WhereClauses = whereClauses ?? [];
            AdvancedWhereClauses = advancedWhereClauses ?? [];
            SubQueryWhereClauses = subQueryWhereClauses ?? [];
            GroupByClauses = groupByClauses ?? [];
            HavingClauses = havingClauses ?? [];
            OrderByClauses = orderByClauses ?? [];
            LimitOffset = limitOffset;
            Joins = joins ?? [];
            SubQueries = subQueries ?? [];
            IsDistinct = isDistinct;
            AggregateFunctions = aggreageFunctions ?? [];
            Id = GenerateId();
            ComplexityScore = QueryComplexityCalculator.CalculateComplexity(this);
        }

        private string GenerateId()
        {
            var sb = new StringBuilder();

            sb.Append(MainTable.ToLowerInvariant());

            foreach (var field in Fields.OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase))
            {
                sb.Append(field.Name.ToLowerInvariant());
                sb.Append(field.TableName.ToLowerInvariant());
            }

            foreach (var relation in Joins.OrderBy(r => r.SourceTable, StringComparer.OrdinalIgnoreCase).ThenBy(r => r.JoinedTable, StringComparer.OrdinalIgnoreCase))
            {
                sb.Append(relation.SourceTable.ToLowerInvariant());
                sb.Append(relation.SourceField.ToLowerInvariant());
                sb.Append(relation.JoinedTable.ToLowerInvariant());
                sb.Append(relation.JoinedField.ToLowerInvariant());
            }

            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
                return Convert.ToHexString(hash);
            }
        }

        public string GetSqlQuery()
        {
            if (Cache is not null)
            {
                var cached = Cache.Get(Id);

                if (!string.IsNullOrWhiteSpace(cached))
                {
                    return cached;
                }
            }

            var result = DialectAdapter.GenerateSelectQuery(this, WhereClauses, GroupByClauses, HavingClauses, OrderByClauses, LimitOffset);

            Cache?.Set(Id, result, TimeSpan);

            return result;
        }

        public string GetComplexityReport()
        {
            return ComplexityScore.ToString();
        }
        public string ExportToJson()
        {
            return SqlDataSetJsonExporter.ExportToJson(this);
        }

        public string ExportToJson(int? limit, int? offset)
        {
            return SqlDataSetJsonExporter.ExportToJson(this, limit, offset);
        }

        public string GenerateViewSql(string viewName)
        {
            return DialectAdapter.GenerateView(viewName, this);
        }

        public string GenerateMaterializedViewSql(string viewName)
        {
            return DialectAdapter.GenerateMaterializedView(viewName, this);
        }
    }
}
