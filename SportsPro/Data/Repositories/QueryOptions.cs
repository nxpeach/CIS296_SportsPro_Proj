using System.Linq.Expressions;

namespace SportsPro.Data.Repositories
{
    public class QueryOptions<T>
    {
        public Expression<Func<T, object>>? OrderBy { get; set; }
        public Expression<Func<T, object>>? ThenOrderBy { get; set; }

        public List<Expression<Func<T, bool>>> WhereClauses { get; } = new();
        public List<Expression<Func<T, object>>> Includes { get; } = new();

        public bool HasWhere => WhereClauses.Count > 0;
        public bool HasIncludes => Includes.Count > 0;
        public bool HasOrderBy => OrderBy != null;

        public void AddWhere(Expression<Func<T, bool>> whereClause) =>
            WhereClauses.Add(whereClause);

        public void AddInclude(Expression<Func<T, object>> include) =>
            Includes.Add(include);
    }
}
