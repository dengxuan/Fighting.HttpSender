using Baibaocp.LotteryVender.Core.Entities;
using Fighting.Storaging.Dapper.Repositories;
using Fighting.Storaging.Repositories.Abstractions;
using System.Data;

namespace Baibaocp.Storaging.Dapper.Repositories
{
    public class LotteryVenderOrderRepository : DapperRepository<LotteryVenderOrderEntity, string>, IRepository<LotteryVenderOrderEntity, string>
    {
        public LotteryVenderOrderRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}
