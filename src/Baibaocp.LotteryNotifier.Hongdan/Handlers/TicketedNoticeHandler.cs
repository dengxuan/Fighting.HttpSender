using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.Notifiers;
using Fighting.Extensions.Serialization.Abstractions;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Hongdan.Handlers
{
    public class TicketedNoticeHandler : INoticeHandler<TicketedNotifier>
    {
        private readonly HttpClient _client;

        private readonly ISerializer _serializer;

        private readonly LotteryNoticeOptions _options;

        public TicketedNoticeHandler(ISerializer serializer, LotteryNoticeOptions options)
        {
            _options = options;
            _serializer = serializer;
            _client = new HttpClient();
        }

        public async Task<bool> HandleAsync(TicketedNotifier notifier)
        {
            NoticeConfiguration configuration = _options.Configures.SingleOrDefault(predicate => predicate.LvpVenderId == notifier.LvpVenderId);
            HttpResponseMessage responseMessage = (await _client.PostAsync(configuration.Url, new ByteArrayContent(_serializer.Serialize(notifier)))).EnsureSuccessStatusCode();
            byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();
            Result result = _serializer.Deserialize<Result>(bytes);
            return result.Code == 0;
        }
    }

   public class Result
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string Data { get; set; }
    }
}
