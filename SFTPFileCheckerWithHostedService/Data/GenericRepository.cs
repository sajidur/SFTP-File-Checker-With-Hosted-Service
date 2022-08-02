using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SFTPFileCheckerWithHostedService.Data
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        void Delete(long entity);
        void Update(T entity);
        Task<T> GetById(long id);
        Task<long> GetLastId(string table);
        long GetLastId();
        T Insert(T obj);
        void Save();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
    }
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private AppsDbContext _context = null;
        private DbSet<T> table = null;
        public GenericRepository(AppsDbContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await table.ToListAsync();
        }
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {

            IQueryable<T> query = table.Where(predicate);
            return query;
        }

        public async Task<T> GetById(long id)
        {
            return await table.FindAsync(id);
        }

        public T Insert(T obj)
        {
            table.Add(obj);
            return obj;
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public long GetLastId()
        {
            return 1;
        }
        public async Task<long> GetLastId(string table)
        {
            var connection = _context.Database.GetDbConnection();
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT MAX(\"Id\") FROM public.\"" + table + "\";";
                var result = command.ExecuteScalar();
                connection.Close();
                long s = 0;
                return (result == DBNull.Value) ? 1 : (long)result;
                //long.TryParse(result.Equals(DBNull.Value), out s);
                return s;
            }

        }
    }
}
