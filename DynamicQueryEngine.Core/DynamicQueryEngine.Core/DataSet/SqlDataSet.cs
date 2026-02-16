using DynamicQueryEngine.Core.DataSet.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace DynamicQueryEngine.Core.DataSet
{
    public class SqlDataSet : ISqlDataSet
    {
        public List<SqlDataSetField> Fields { get; init; }
        public string MainTable { get; init; }
        public List<SqlDataSetTableRelation> Relations { get; init; }
        public string Id { get; private set; }
        public ISqlDataSetCache? Cache { get; init; }
        public TimeSpan? TimeSpan { get; init; }

        internal SqlDataSet(List<SqlDataSetField> fields,
                            string mainTable,
                            List<SqlDataSetTableRelation> relations,
                            ISqlDataSetCache? cache,
                            TimeSpan? timeSpan)
        {
            Fields = fields;
            MainTable = mainTable;
            Relations = relations;
            Cache = cache;
            TimeSpan = timeSpan;
            Id = GenerateId();
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

            foreach (var relation in Relations.OrderBy(r => r.SourceTable, StringComparer.OrdinalIgnoreCase).ThenBy(r => r.JoinedTable, StringComparer.OrdinalIgnoreCase))
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

            var result = BuildSqlQuery();

            Cache?.Set(Id, result, TimeSpan);

            return result;
        }

        private string BuildSqlQuery()
        {

            List<string> tables = [MainTable];
            tables.AddRange(Relations.Select(r => r.JoinedTable));

            Dictionary<string, string> tableAliases = [];

            for (int i = 0; i < tables.Count; i++)
            {
                string[] tmpAliases = tables[i].Split(".");

                string alias = tmpAliases.Length > 1 ? string.Join(string.Empty, tmpAliases.Skip(1)) : tables[i];
                tableAliases[tables[i]] = $"{alias}{i}";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine(string.Join(", ", Fields.Select(f => $"{tableAliases[f.TableName]}.{f.Name}")));
            sb.AppendLine($" FROM {MainTable}");
            foreach (var relation in Relations)
            {
                sb.AppendLine(
                    $"""
                    JOIN {tableAliases[relation.JoinedTable]} ON 
                    {tableAliases[relation.SourceTable]}.{relation.SourceField} = {tableAliases[relation.JoinedTable]}.{relation.JoinedField}
                    """);
            }

            return sb.ToString();
        }
    }
}
