﻿using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.Internal;
using Baibaocp.LotteryNotifier.Notifiers;
using Fighting.Extensions.Serialization.Abstractions;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Hongdan.Handlers
{
    public class TicketedNoticeHandler : INoticeHandler<Ticketed>
    {
        private readonly HttpClient _client;

        private readonly ISerializer _serializer;

        private readonly NoticeConfiguration _options;

        public TicketedNoticeHandler(ISerializer serializer, NoticeConfiguration options)
        {
            _options = options;
            _serializer = serializer;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_options.Url)
            };
        }

        public async Task<bool> HandleAsync(Ticketed ticketed)
        {
            HttpResponseMessage responseMessage = (await _client.PostAsync("/ordernotify/index", new ByteArrayContent(_serializer.Serialize(ticketed)))).EnsureSuccessStatusCode();
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
