namespace DynamicQueryEngine.Core.DataSet.Complexity
{
    public class QueryComplexityCalculator
    {
        private const int BASE_COMPLEXITY = 10;
        private const int FIELD_COMPLEXITY = 5;
        private const int JOIN_COMPLEXITY = 15;
        private const int WHERE_COMPLEXITY = 8;
        private const int GROUP_BY_COMPLEXITY = 10;
        private const int HAVING_COMPLEXITY = 12;
        private const int ORDER_BY_COMPLEXITY = 3;
        private const int LIMIT_OFFSET_COMPLEXITY = 2;
        private const int AGGREGATE_FUNCTION_COMPLEXITY = 10;
        private const int DISTINCT_COMPLEXITY = 8;
        private const int ADVANCED_WHERE_COMPLEXITY = 12; // BETWEEN, LIKE, IN, etc

        public static QueryComplexityScore CalculateComplexity(SqlDataSet dataSet)
        {
            var score = new QueryComplexityScore();

            // Base complexity
            score.AddComplexity("Base", BASE_COMPLEXITY);

            // Fields
            score.AddComplexity("Fields", dataSet.Fields.Count * FIELD_COMPLEXITY);

            // JOINs
            score.AddComplexity("JOINs", dataSet.Joins.Count * JOIN_COMPLEXITY);

            // WHERE clauses
            score.AddComplexity("WHERE clauses", dataSet.WhereClauses.Count * WHERE_COMPLEXITY);

            // Advanced WHERE clauses
            score.AddComplexity("Advanced WHERE", dataSet.AdvancedWhereClauses.Count * ADVANCED_WHERE_COMPLEXITY);

            // GROUP BY
            score.AddComplexity("GROUP BY", dataSet.GroupByClauses.Count * GROUP_BY_COMPLEXITY);

            // HAVING
            score.AddComplexity("HAVING clauses", dataSet.HavingClauses.Count * HAVING_COMPLEXITY);

            // ORDER BY
            score.AddComplexity("ORDER BY", dataSet.OrderByClauses.Count * ORDER_BY_COMPLEXITY);

            // LIMIT/OFFSET
            if (dataSet.LimitOffset != null)
            {
                score.AddComplexity("LIMIT/OFFSET", LIMIT_OFFSET_COMPLEXITY);
            }

            // Aggregate Functions
            score.AddComplexity("Aggregate Functions", dataSet.AggregateFunctions.Count * AGGREGATE_FUNCTION_COMPLEXITY);

            // DISTINCT
            if (dataSet.IsDistinct)
            {
                score.AddComplexity("DISTINCT", DISTINCT_COMPLEXITY);
            }

            // SubQueries (multiplicam a complexidade)
            if (dataSet.SubQueries.Count > 0)
            {
                score.AddComplexity("SubQueries", dataSet.SubQueries.Count * 30);
            }

            return score;
        }
    }

    public class QueryComplexityScore
    {
        private Dictionary<string, int> _complexityBreakdown = new();
        public int TotalScore { get; private set; }

        public void AddComplexity(string component, int points)
        {
            if (points > 0)
            {
                _complexityBreakdown[component] = points;
                TotalScore += points;
            }
        }

        public ComplexityLevel GetLevel()
        {
            return TotalScore switch
            {
                <= 30 => ComplexityLevel.VerySimple,
                <= 60 => ComplexityLevel.Simple,
                <= 100 => ComplexityLevel.Moderate,
                <= 150 => ComplexityLevel.Complex,
                <= 200 => ComplexityLevel.VeryComplex,
                _ => ComplexityLevel.Extreme
            };
        }

        public Dictionary<string, int> GetBreakdown() => new(_complexityBreakdown);

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Query Complexity Score: {TotalScore}");
            sb.AppendLine($"Level: {GetLevel()}");
            sb.AppendLine("\nBreakdown:");
            
            foreach (var kvp in _complexityBreakdown.OrderByDescending(x => x.Value))
            {
                sb.AppendLine($"  - {kvp.Key}: {kvp.Value} points");
            }

            return sb.ToString();
        }
    }

    public enum ComplexityLevel
    {
        VerySimple,  // 0-30
        Simple,      // 31-60
        Moderate,    // 61-100
        Complex,     // 101-150
        VeryComplex, // 151-200
        Extreme      // 200+
    }
}
