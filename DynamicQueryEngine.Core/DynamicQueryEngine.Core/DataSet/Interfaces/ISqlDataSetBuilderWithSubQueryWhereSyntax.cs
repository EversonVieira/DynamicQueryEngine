using DynamicQueryEngine.Core.DataSet.SqlQueryComponents;

namespace DynamicQueryEngine.Core.DataSet.Interfaces
{
    /// <summary>
    /// Interface para suporte a subqueries em cláusulas WHERE
    /// </summary>
    public interface ISqlDataSetBuilderWithSubQueryWhereSyntax
    {
        /// <summary>
        /// Adiciona uma cláusula WHERE com subquery
        /// </summary>
        /// <param name="tableName">Nome da tabela (null para EXISTS/NOT EXISTS)</param>
        /// <param name="fieldName">Nome do campo (null para EXISTS/NOT EXISTS)</param>
        /// <param name="op">Operador da subquery</param>
        /// <param name="subQuery">DataSet contendo a subquery</param>
        /// <returns>O builder para encadeamento</returns>
        ISqlDataSetBuilder WithSubQueryWhere(string? tableName, string? fieldName, SubQueryOperator op, SqlDataSet subQuery);
    }
}
