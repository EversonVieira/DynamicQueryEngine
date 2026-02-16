using DynamicQueryEngine.Core.Config;
using DynamicQueryEngine.Core.DataSet.Adapters;
using DynamicQueryEngine.Core.DataSet.Complexity;
using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;

namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    public interface ISqlDataSet
    {
        public List<SqlDataSetField> Fields { get; init; }
        public string MainTable { get; init; }
        public List<SqlDataSetTableRelation> Relations { get; init; }
        public string Id { get; }
        public ISqlDataSetCache? Cache { get; init; }
        public TimeSpan? TimeSpan { get; init;  }
        public ISqlDialectAdapter DialectAdapter { get; init; }
        public List<WhereClause> WhereClauses { get; init; }
        public List<AdvancedWhereClause> AdvancedWhereClauses { get; init; }
        public List<GroupByClause> GroupByClauses { get; init; }
        public List<HavingClause> HavingClauses { get; init; }
        public List<OrderByClause> OrderByClauses { get; init; }
        public LimitOffsetClause? LimitOffset { get; init; }
        public List<JoinClause> Joins { get; init; }
        public List<SubQuery> SubQueries { get; init; }
        public bool IsDistinct { get; init; }
        public List<AggregateFunction> AggregateFunctions { get; init; }
        public QueryComplexityScore ComplexityScore { get; }
        public string GetSqlQuery();
        public string GenerateViewSql(string viewName);
        public string GenerateMaterializedViewSql(string viewName);
        public string ExportToJson();
        public string ExportToJson(int? limit, int? offset);
        public string GetComplexityReport();
    }
}