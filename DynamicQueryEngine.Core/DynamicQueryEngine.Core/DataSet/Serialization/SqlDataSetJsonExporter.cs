using System.Text.Json;
using System.Text.Json.Serialization;

namespace DynamicQueryEngine.Core.DataSet.Serialization
{
    public class SqlDataSetJsonExporter
    {
        public static string ExportToJson(SqlDataSet dataSet)
        {
            var queryDefinition = new SqlDataSetQueryDefinition
            {
                MainTable = dataSet.MainTable,
                Fields = dataSet.Fields.Select(f => new FieldDefinition 
                { 
                    TableName = f.TableName, 
                    Name = f.Name, 
                    Type = f.Type.ToString() 
                }).ToList(),
                Relations = dataSet.Joins.Select(r => new RelationDefinition
                {
                    SourceTable = r.SourceTable,
                    JoinedTable = r.JoinedTable,
                    SourceField = r.SourceField,
                    JoinedField = r.JoinedField,
                    Type = r.JoinType.ToString()
                }).ToList(),
                WhereClauses = dataSet.WhereClauses.Select(w => new WhereDefinition
                {
                    TableName = w.TableName,
                    FieldName = w.FieldName,
                    Operator = w.Operator,
                    Value = w.Value.ToString()
                }).ToList(),
                GroupByClauses = dataSet.GroupByClauses.Select(g => new GroupByDefinition
                {
                    TableName = g.TableName,
                    FieldName = g.FieldName
                }).ToList(),
                HavingClauses = dataSet.HavingClauses.Select(h => new HavingDefinition
                {
                    AggregateFunction = h.AggregateFunction,
                    FieldName = h.FieldName,
                    Operator = h.Operator,
                    Value = h.Value.ToString()
                }).ToList(),
                OrderByClauses = dataSet.OrderByClauses.Select(o => new OrderByDefinition
                {
                    TableName = o.TableName,
                    FieldName = o.FieldName,
                    Direction = o.Direction.ToString()
                }).ToList(),
                DataSetId = dataSet.Id
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(queryDefinition, options);
        }

        public static string ExportToJson(SqlDataSet dataSet, int? limit, int? offset)
        {
            var json = ExportToJson(dataSet);
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;
            
            if (limit.HasValue)
                jsonObject["limit"] = limit;
            if (offset.HasValue)
                jsonObject["offset"] = offset;

            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(jsonObject, options);
        }
    }

    public class SqlDataSetQueryDefinition
    {
        [JsonPropertyName("mainTable")]
        public string MainTable { get; set; }

        [JsonPropertyName("fields")]
        public List<FieldDefinition> Fields { get; set; } = [];

        [JsonPropertyName("relations")]
        public List<RelationDefinition> Relations { get; set; } = [];

        [JsonPropertyName("whereClauses")]
        public List<WhereDefinition> WhereClauses { get; set; } = [];

        [JsonPropertyName("groupByClauses")]
        public List<GroupByDefinition> GroupByClauses { get; set; } = [];

        [JsonPropertyName("havingClauses")]
        public List<HavingDefinition> HavingClauses { get; set; } = [];

        [JsonPropertyName("orderByClauses")]
        public List<OrderByDefinition> OrderByClauses { get; set; } = [];

        [JsonPropertyName("dataSetId")]
        public string DataSetId { get; set; }
    }

    public class FieldDefinition
    {
        [JsonPropertyName("tableName")]
        public string TableName { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class RelationDefinition
    {
        [JsonPropertyName("sourceTable")]
        public string SourceTable { get; set; }

        [JsonPropertyName("joinedTable")]
        public string JoinedTable { get; set; }

        [JsonPropertyName("sourceField")]
        public string SourceField { get; set; }

        [JsonPropertyName("joinedField")]
        public string JoinedField { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class WhereDefinition
    {
        [JsonPropertyName("tableName")]
        public string TableName { get; set; }

        [JsonPropertyName("fieldName")]
        public string FieldName { get; set; }

        [JsonPropertyName("operator")]
        public string Operator { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class GroupByDefinition
    {
        [JsonPropertyName("tableName")]
        public string TableName { get; set; }

        [JsonPropertyName("fieldName")]
        public string FieldName { get; set; }
    }

    public class HavingDefinition
    {
        [JsonPropertyName("aggregateFunction")]
        public string AggregateFunction { get; set; }

        [JsonPropertyName("fieldName")]
        public string FieldName { get; set; }

        [JsonPropertyName("operator")]
        public string Operator { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class OrderByDefinition
    {
        [JsonPropertyName("tableName")]
        public string TableName { get; set; }

        [JsonPropertyName("fieldName")]
        public string FieldName { get; set; }

        [JsonPropertyName("direction")]
        public string Direction { get; set; }
    }
}
