using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicQueryEngine.Core.DataSet.Adapters
{
    public class PostgresSqlDialectAdapter : ISqlDialectAdapter
    {
        public string GenerateSelectQuery(
            SqlDataSet dataSet,
            List<WhereClause>? whereClauses = null,
            List<GroupByClause>? groupByClauses = null,
            List<HavingClause>? havingClauses = null,
            List<OrderByClause>? orderByClauses = null,
            LimitOffsetClause? limitOffset = null)
        {
            return BuildSqlQuery(dataSet, whereClauses, groupByClauses, havingClauses, orderByClauses, limitOffset);
        }

        public string GenerateView(string viewName, SqlDataSet dataSet)
        {
            var selectQuery = BuildSqlQuery(dataSet);
            return $"CREATE VIEW {viewName} AS\n{selectQuery};";
        }

        public string GenerateMaterializedView(string viewName, SqlDataSet dataSet)
        {
            var selectQuery = BuildSqlQuery(dataSet);
            return $"CREATE MATERIALIZED VIEW {viewName} AS\n{selectQuery};";
        }

        private string BuildSqlQuery(
            SqlDataSet dataSet,
            List<WhereClause>? whereClauses = null,
            List<GroupByClause>? groupByClauses = null,
            List<HavingClause>? havingClauses = null,
            List<OrderByClause>? orderByClauses = null,
            LimitOffsetClause? limitOffset = null)
        {
            var tables = new List<string> { dataSet.MainTable };
            tables.AddRange(dataSet.Relations.Select(r => r.JoinedTable).Distinct());
            tables.AddRange(dataSet.Joins.Select(j => j.JoinedTable).Distinct());

            var tableAliases = new Dictionary<string, string>();
            for (int i = 0; i < tables.Count; i++)
            {
                tableAliases[tables[i]] = $"{tables[i].Split(".")[^1]}{i + 1}";
            }

            var sb = new StringBuilder();
            sb.Append("SELECT ");
            if (dataSet.IsDistinct)
                sb.Append("DISTINCT ");

            var selectFields = new List<string>();
            selectFields.AddRange(dataSet.Fields.Select(f => $"{tableAliases[f.TableName]}.{f.Name}"));
            selectFields.AddRange(dataSet.AggregateFunctions.Select(agg =>
            {
                var alias = !string.IsNullOrEmpty(agg.Alias) ? $" AS {agg.Alias}" : "";
                return $"{agg.FunctionName}({tableAliases[agg.TableName]}.{agg.FieldName}){alias}";
            }));

            sb.AppendLine(string.Join(",\n    ", selectFields));
            sb.AppendLine($"FROM {dataSet.MainTable} {tableAliases[dataSet.MainTable]}");

            foreach (var relation in dataSet.Relations)
            {
                sb.AppendLine($"JOIN {relation.JoinedTable} {tableAliases[relation.JoinedTable]} ON");
                sb.AppendLine($"    {tableAliases[relation.SourceTable]}.{relation.SourceField} = {tableAliases[relation.JoinedTable]}.{relation.JoinedField}");
            }

            foreach (var join in dataSet.Joins)
            {
                var joinKeyword = join.JoinType switch
                {
                    JoinType.Left => "LEFT JOIN",
                    JoinType.Right => "RIGHT JOIN",
                    JoinType.Full => "FULL OUTER JOIN",
                    JoinType.Cross => "CROSS JOIN",
                    _ => "INNER JOIN"
                };

                sb.AppendLine($"{joinKeyword} {join.JoinedTable} {tableAliases[join.JoinedTable]} ON");
                sb.AppendLine($"    {tableAliases[join.SourceTable]}.{join.SourceField} = {tableAliases[join.JoinedTable]}.{join.JoinedField}");
            }

            var allWhereClauses = new List<string>();

            if (whereClauses?.Any() == true)
            {
                allWhereClauses.AddRange(whereClauses.Select(w => FormatWhereClause(w, tableAliases)));
            }

            if (dataSet.AdvancedWhereClauses.Any())
            {
                allWhereClauses.AddRange(dataSet.AdvancedWhereClauses.Select(w => FormatAdvancedWhereClause(w, tableAliases)));
            }

            if (allWhereClauses.Any())
            {
                sb.AppendLine("WHERE");
                sb.AppendLine(string.Join(" AND\n    ", allWhereClauses));
            }

            if (groupByClauses?.Any() == true)
            {
                sb.AppendLine("GROUP BY");
                var groupByParts = groupByClauses.Select(g => $"{tableAliases[g.TableName]}.{g.FieldName}");
                sb.AppendLine(string.Join(", ", groupByParts));
            }

            if (havingClauses?.Any() == true)
            {
                sb.AppendLine("HAVING");
                var havingParts = havingClauses.Select(h => FormatHavingClause(h));
                sb.AppendLine(string.Join(" AND\n    ", havingParts));
            }

            if (orderByClauses?.Any() == true)
            {
                sb.AppendLine("ORDER BY");
                var orderByParts = orderByClauses.Select(o =>
                    $"{tableAliases[o.TableName]}.{o.FieldName} {(o.Direction == SortDirection.Descending ? "DESC" : "ASC")}");
                sb.AppendLine(string.Join(", ", orderByParts));
            }

            if (limitOffset?.Limit.HasValue == true)
            {
                sb.AppendLine($"LIMIT {limitOffset.Limit}");
            }

            if (limitOffset?.Offset.HasValue == true)
            {
                sb.AppendLine($"OFFSET {limitOffset.Offset}");
            }

            return sb.ToString();
        }

        private string FormatWhereClause(WhereClause clause, Dictionary<string, string> tableAliases)
        {
            var value = clause.Value is string ? $"'{clause.Value}'" : clause.Value;
            return $"{tableAliases[clause.TableName]}.{clause.FieldName} {clause.Operator} {value}";
        }

        private string FormatAdvancedWhereClause(AdvancedWhereClause clause, Dictionary<string, string> tableAliases)
        {
            return clause.Operator switch
            {
                WhereOperator.Equal => $"{tableAliases[clause.TableName]}.{clause.FieldName} = {FormatValue(clause.Value)}",
                WhereOperator.NotEqual => $"{tableAliases[clause.TableName]}.{clause.FieldName} != {FormatValue(clause.Value)}",
                WhereOperator.GreaterThan => $"{tableAliases[clause.TableName]}.{clause.FieldName} > {FormatValue(clause.Value)}",
                WhereOperator.GreaterOrEqual => $"{tableAliases[clause.TableName]}.{clause.FieldName} >= {FormatValue(clause.Value)}",
                WhereOperator.LessThan => $"{tableAliases[clause.TableName]}.{clause.FieldName} < {FormatValue(clause.Value)}",
                WhereOperator.LessOrEqual => $"{tableAliases[clause.TableName]}.{clause.FieldName} <= {FormatValue(clause.Value)}",
                WhereOperator.Like => $"{tableAliases[clause.TableName]}.{clause.FieldName} LIKE '%{clause.Value}%'",
                WhereOperator.NotLike => $"{tableAliases[clause.TableName]}.{clause.FieldName} NOT LIKE '%{clause.Value}%'",
                WhereOperator.In => $"{tableAliases[clause.TableName]}.{clause.FieldName} IN ({FormatInValues(clause.Value)})",
                WhereOperator.NotIn => $"{tableAliases[clause.TableName]}.{clause.FieldName} NOT IN ({FormatInValues(clause.Value)})",
                WhereOperator.Between => $"{tableAliases[clause.TableName]}.{clause.FieldName} BETWEEN {FormatValue(clause.Value)} AND {FormatValue(clause.ValueEnd)}",
                WhereOperator.IsNull => $"{tableAliases[clause.TableName]}.{clause.FieldName} IS NULL",
                WhereOperator.IsNotNull => $"{tableAliases[clause.TableName]}.{clause.FieldName} IS NOT NULL",
                _ => throw new ArgumentException($"Operator {clause.Operator} not supported.")
            };
        }

        private string FormatValue(object? value)
        {
            if (value == null) return "NULL";
            if (value is string) return $"'{value}'";
            return value.ToString()!;
        }

        private string FormatInValues(object? values)
        {
            if (values is System.Collections.IEnumerable enumerable && !(values is string))
            {
                var parts = enumerable.Cast<object>().Select(v => v is string ? $"'{v}'" : v.ToString());
                return string.Join(", ", parts);
            }
            return FormatValue(values);
        }

        private string FormatHavingClause(HavingClause clause)
        {
            var value = clause.Value is string ? $"'{clause.Value}'" : clause.Value;
            return $"{clause.AggregateFunction}({clause.FieldName}) {clause.Operator} {value}";
        }
    }
}
