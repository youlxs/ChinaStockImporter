using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Yootech.ChinaStockImporter.Configuration;
using Yootech.ChinaStockImporter.Entity;

namespace Yootech.ChinaStockImporter.Mapping
{
    class ChinaStockInfoMap: EntityTypeConfiguration<StockInfo>
    {
        public ChinaStockInfoMap()
        {
            // Primary Key
            this.HasKey(t => new {t.Symbol, t.Date});

            // Properties
            this.Property(t => t.Symbol)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.Symbol)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable(ConfigurationData.ChinaStockTableName);
            this.Property(t => t.Symbol).HasColumnName("Symbol");
            this.Property(t => t.StockName).HasColumnName("StockName");
            this.Property(t => t.Open).HasColumnName("Open");
            this.Property(t => t.High).HasColumnName("High");
            this.Property(t => t.Low).HasColumnName("Low");
            this.Property(t => t.Close).HasColumnName("Close");
            this.Property(t => t.AdjClose).HasColumnName("Adjclose");
            this.Property(t => t.Volume).HasColumnName("Volume");
            this.Property(t => t.Date).HasColumnName("Date");
        }
    }   
}
