using Baibaocp.LotterySales.Core.Entities;
using Fighting.Storaging.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Baibaocp.Storaging.EntityFrameworkCore
{
    public class BaibaocpStorage : Storage
    {
        private readonly StorageOptions _storageOptions;
        public BaibaocpStorage(DbContextOptions options, IOptions<StorageOptions> storageOptions) : base(options)
        {
            _storageOptions = storageOptions.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_storageOptions.DefaultNameOrConnectionString);
        }

        public virtual DbSet<LotterySalesOrderEntity> BbcpOrders { get; set; }
    }
}
