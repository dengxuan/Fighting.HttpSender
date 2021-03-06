﻿using Baibaocp.LotteryVender.Core.Entities;
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

        public virtual DbSet<LotteryVenderOrderEntity> BbcpOrders { get; set; }

        public virtual DbSet<LotteryVenderEntity> BbcpVenders { get; set; }

        public virtual DbSet<LotteryVenderAccountDetailEntity> BbcpVenderAccountDetails { get; set; }
    }
}
