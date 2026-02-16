namespace DynamicQueryEngine.Core.DataSet.SqlQueryComponents
{
    public class LimitOffsetClause
    {
        public int? Limit { get; set; }
        public int? Offset { get; set; }

        public LimitOffsetClause(int? limit = null, int? offset = null)
        {
            if (limit.HasValue && limit < 0)
                throw new ArgumentException("Limit must be a positive number.", nameof(limit));

            if (offset.HasValue && offset < 0)
                throw new ArgumentException("Offset must be a positive number.", nameof(offset));

            Limit = limit;
            Offset = offset;
        }
    }
}
