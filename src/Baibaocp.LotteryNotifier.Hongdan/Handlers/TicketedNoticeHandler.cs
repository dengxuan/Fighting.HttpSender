using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.Notifiers;
using System.Net.Http;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Hongdan.Handlers
{
    public class TicketedNoticeHandler : INoticeHandler<TicketedNotifier>
    {
        private readonly HttpClient _client;

        private readonly NoticeConfiguration _configuration;

        public TicketedNoticeHandler(NoticeConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient();
        }

        public async Task<bool> HandleAsync(TicketedNotifier notifier)
        {
            HttpResponseMessage responseMessage = (await _client.PostAsync(_configuration.Url, new StringContent(""))).EnsureSuccessStatusCode();
            string content = await responseMessage.Content.ReadAsStringAsync();
            return true;
        }
    }
}
