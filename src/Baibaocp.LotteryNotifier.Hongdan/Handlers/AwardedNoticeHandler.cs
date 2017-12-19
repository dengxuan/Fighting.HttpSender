﻿using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.Notifiers;
using Fighting.Extensions.Serialization.Abstractions;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Hongdan.Handlers
{
    public class AwardedNoticeHandler : INoticeHandler<Awarded>
    {
        private readonly HttpClient _client;

        private readonly ISerializer _serializer;

        private readonly NoticeConfiguration _options;

        public AwardedNoticeHandler(ISerializer serializer, NoticeConfiguration options)
        {
            _options = options;
            _serializer = serializer;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_options.Url)
            };
        }

        public async Task<bool> HandleAsync(Awarded awarded)
        {
            HttpResponseMessage responseMessage = (await _client.PostAsync("/ordernotify/returnaward", new ByteArrayContent(_serializer.Serialize(awarded)))).EnsureSuccessStatusCode();
            byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();
            Result result = _serializer.Deserialize<Result>(bytes);
            return result.Code == 0;
        }
    }
}
