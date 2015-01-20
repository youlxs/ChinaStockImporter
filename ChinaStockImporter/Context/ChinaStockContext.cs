using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using Yootech.ChinaStockImporter.Entity;
using Yootech.ChinaStockImporter.Mapping;

namespace Yootech.ChinaStockImporter.Context
{
    public class ChinaStockContext : DbContext, IChinaStockContext, IDisposable
    {        
       static ChinaStockContext()
        {
            Database.SetInitializer<ChinaStockContext>(null);
        }

       public ChinaStockContext()
           : base("Name=ChinaStockContext")
        {
        }
 
       public IEnumerable<T> SqlQuery<T>(string query, params object[] parameters)
       {
           return base.Database.SqlQuery<T>(query, parameters);
       }

       public new DbSet<T> Set<T>() where T : class
       {
           return base.Set<T>();
       }

       public new DbSet Set(Type type)
       {
           return base.Set(type);
       }

       public new DbEntityEntry Entry(object obj)
       {
           return base.Entry(obj);
       }

       public new bool SaveChanges()
       {
           return base.SaveChanges() > 0;
       }

       public void AutoDetectChanges(bool autoDetectChanges)
       {
           Configuration.AutoDetectChangesEnabled = autoDetectChanges;
           Configuration.ValidateOnSaveEnabled = autoDetectChanges;
       }

       protected override void OnModelCreating(DbModelBuilder modelBuilder)
       {
           modelBuilder.Configurations.Add(new ChinaStockInfoMap());           
       }

        public DbSet<StockInfo> StockInfos { get; set; }

        void IDisposable.Dispose()
        {
            base.Dispose();
        }

    }
}
