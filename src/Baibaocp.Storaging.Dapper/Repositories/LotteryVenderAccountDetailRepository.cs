using Baibaocp.LotteryVender.Core.Entities;
using Fighting.Storaging.Dapper.Repositories;
using Fighting.Storaging.Repositories.Abstractions;
using System.Data;

namespace Baibaocp.Storaging.Dapper.Repositories
{
    public class LotteryVenderAccountDetailRepository : DapperRepository<LotteryVenderAccountDetailEntity, string>, IRepository<LotteryVenderAccountDetailEntity, string>
    {
        public LotteryVenderAccountDetailRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}
