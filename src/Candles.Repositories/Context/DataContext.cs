using Candles.Domain.Entities;
using Candles.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Candles.Repositories.Context
{
    public class DataContext : DbContext
    {
        private const string Schema = "candles";

        private string _connectionString;

        public DataContext()
        {
        }

        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<CandleEntity> Candles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionString == null)
            {
                System.Console.Write("Enter connection string: ");
                _connectionString = System.Console.ReadLine();
            }

            optionsBuilder.UseNpgsql(_connectionString,
                o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            SetupCandles(modelBuilder);
        }

        private static void SetupCandles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CandleEntity>()
                .HasKey(entity => new {entity.AssetPairId, entity.Type, entity.Time});

            modelBuilder.Entity<CandleEntity>()
                .Property(o => o.Type)
                .HasConversion(new EnumToNumberConverter<CandleType, short>());
        }
    }
}
