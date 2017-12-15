using Baibaocp.LotteryVender.Core.Entities;
using Fighting.Storaging.Dapper.Repositories;
using Fighting.Storaging.Repositories.Abstractions;
using System.Data;

namespace Baibaocp.Storaging.Dapper.Repositories
{
    public class LotteryVenderRepository : DapperRepository<LotteryVenderEntity, string>, IRepository<LotteryVenderEntity, string>
    {
        public LotteryVenderRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}
