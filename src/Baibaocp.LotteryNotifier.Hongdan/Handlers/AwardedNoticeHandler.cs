using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.Notifiers;
using Fighting.Extensions.Serialization.Abstractions;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Hongdan.Handlers
{
    public class AwardedNoticeHandler : INoticeHandler<AwardedNotifier>
    {
        private readonly HttpClient _client;

        private readonly ISerializer _serializer;

        private readonly LotteryNoticeOptions _options;

        public AwardedNoticeHandler(ISerializer serializer, LotteryNoticeOptions options)
        {
            _options = options;
            _serializer = serializer;
            _client = new HttpClient();
        }

        public async Task<bool> HandleAsync(AwardedNotifier notifier)
        {
            NoticeConfiguration configuration = _options.Configures.SingleOrDefault(predicate => predicate.LvpVenderId == notifier.LvpVenderId);
            HttpResponseMessage responseMessage = (await _client.PostAsync(configuration.Url, new StringContent(""))).EnsureSuccessStatusCode();
            string content = await responseMessage.Content.ReadAsStringAsync();
            return true;
        }
    }
}
