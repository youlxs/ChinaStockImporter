using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Yootech.ChinaStockImporter.BulkExtensions;
using Yootech.ChinaStockImporter.BulkExtensions.Mappings;
using Yootech.ChinaStockImporter.Context;

namespace Yootech.ChinaStockImporter.Repositories
{
    public class RepositoryBase<T> : IDisposable where T : class
    {
        protected IChinaStockContext _context;
        private DbSet<T> _entities;

        public RepositoryBase(IChinaStockContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public object ToDBNull(object value)
        {
            if (null != value)
                return value;
            return DBNull.Value;
        }

        public virtual IQueryable<T> GetAll()
        {
            return _entities;
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }

        public virtual T Single(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Single(predicate);
        }

        public virtual T First(Expression<Func<T, bool>> predicate)
        {
            return GetAll().First(predicate);
        }

        public virtual bool Any(Expression<Func<T, bool>> predicate)
        {
            return _entities.Any(predicate);
        }

        public virtual void Add(T obj)
        {
            _context.Entry(obj).State = EntityState.Added;
        }

        public virtual void BulkInsert(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
        }

        public virtual void Delete(T obj)
        {
            _context.Entry(obj).State = EntityState.Deleted;
        }

        public virtual bool Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        protected virtual void BulkInsert(DataTable dataTable)
        {
            var sqlConnection = ((ChinaStockContext) _context).Database.Connection as SqlConnection;

            if(sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();

            var transaction = sqlConnection.BeginTransaction();

            try
            {
                using (var sqlBulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, transaction))
                {
                    sqlBulkCopy.BatchSize = dataTable.Rows.Count;

                    sqlBulkCopy.DestinationTableName = dataTable.TableName;

                    sqlBulkCopy.WriteToServer(dataTable);
                }

                transaction.Commit();
            }
            catch
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                throw;
            }

        }
    }
}
