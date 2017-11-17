using Baibaocp.LotteryVender.Sending.Abstractions;
using Baibaocp.LotteryVender.Sending.Shanghai;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Baibaocp.LotteryVender.Sending.Order
{
    public class Order : IOrder
    {
        public string Id { get; set; }
        public int LotteryId { get; set; }
        public int LotteryPlayId { get; set; }
        public int? IssueNumber { get; set; }
        public string InvestCode { get; set; }
        public bool InvestType { get; set; }
        public int InvestCount { get; set; }
        public int InvestTimes { get; set; }
        public int InvestAmount { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage res = client.GetAsync("http://www.baidu.com").GetAwaiter().GetResult();
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<IOrdering<Order>, ShanghaiOrdering<Order>>();
            services.AddSingleton<ShanghaiSenderOptions>(new ShanghaiSenderOptions
            {
                CommandId = "101",
                SecretKey = "ourpartner",
                Url = "http://115.29.193.120/",
                VenderId = "800"
            });
            IServiceProvider sp = services.BuildServiceProvider();
            IOrdering<Order> ordering = sp.GetRequiredService<IOrdering<Order>>();
            ordering.SendAsync(new Order
            {
                Id = Guid.NewGuid().ToString("N"),
                LotteryId = 23529,
                LotteryPlayId = 2,
                InvestAmount = 200,
                InvestCode = "08 12 15 19 28-02 07",
                InvestCount = 1,
                InvestTimes = 1,
                InvestType = false,
                IssueNumber = 2017135
            }).GetAwaiter().GetResult();
        }
    }
}
