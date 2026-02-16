using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;

namespace DynamicQueryEngine.Core.DataSet.Adapters
{
    public interface ISqlDialectAdapter
    {
        string GenerateSelectQuery(
            SqlDataSet dataSet,
            List<WhereClause>? whereClauses = null,
            List<GroupByClause>? groupByClauses = null,
            List<HavingClause>? havingClauses = null,
            List<OrderByClause>? orderByClauses = null,
            LimitOffsetClause? limitOffset = null);

        string GenerateView(string viewName, SqlDataSet dataSet);
        string GenerateMaterializedView(string viewName, SqlDataSet dataSet);
    }
}
