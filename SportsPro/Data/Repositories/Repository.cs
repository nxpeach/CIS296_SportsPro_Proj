using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected SportsProContext context;
        private DbSet<T> dbSet;

        public Repository(SportsProContext ctx)
        {
            context = ctx;
            dbSet = context.Set<T>();
        }

        public IQueryable<T> List(QueryOptions<T> options)
        {
            IQueryable<T> query = dbSet;

            // Apply WhereClauses
            foreach (var clause in options.WhereClauses)
            {
                query = query.Where(clause);
            }

            // Apply Includes
            foreach (var include in options.Includes)
            {
                query = query.Include(include);
            }

            // Apply Ordering (if any)
            if (options.OrderBy != null)
            {
                query = query.OrderBy(options.OrderBy);
            }

            return query;
        }


        public T? Get(int id) => dbSet.Find(id);
        public void Insert(T entity) => dbSet.Add(entity);
        public void Update(T entity) => dbSet.Update(entity);
        public void Delete(T entity) => dbSet.Remove(entity);
        public void Save() => context.SaveChanges();
    }
}
