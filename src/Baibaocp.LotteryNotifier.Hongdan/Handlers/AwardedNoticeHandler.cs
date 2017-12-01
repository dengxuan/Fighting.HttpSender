using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.Notifiers;
using System.Net.Http;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Hongdan.Handlers
{
    public class AwardedNoticeHandler : INoticeHandler<AwardedNotifier>
    {
        private readonly HttpClient _client;

        private readonly NoticeConfiguration _configuration;

        public AwardedNoticeHandler(NoticeConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient();
        }

        public async Task<bool> HandleAsync(AwardedNotifier notifier)
        {
            HttpResponseMessage responseMessage = (await _client.PostAsync(_configuration.Url, new StringContent(""))).EnsureSuccessStatusCode();
            string content = await responseMessage.Content.ReadAsStringAsync();
            return true;
        }
    }
}
