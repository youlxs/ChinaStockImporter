using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Yootech.ChinaStockImporter.Entity;

namespace Yootech.ChinaStockImporter.Context
{
    public interface IChinaStockContext
    {                
        IEnumerable<T> SqlQuery<T>(string query, params object[] parameters);

        DbSet<T> Set<T>() where T : class;
        DbSet Set(Type type);
        DbEntityEntry Entry(object obj);
        bool SaveChanges();

        void AutoDetectChanges(bool autoDetectChanges);

        DbSet<StockInfo> StockInfos { get; set; }
        
        void Dispose();
    }
}
