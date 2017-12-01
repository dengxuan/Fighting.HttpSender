﻿using Baibaocp.LotteryDispatcher.Abstractions;
using Fighting.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatcher.Shanghai
{
    public abstract class ShanghaiExecuteHandler<TExecuter, TResult> : IExecuteHandler<TExecuter, TResult> where TExecuter : IExecuter where TResult : IResult
    {
        private readonly string _command;

        private readonly ILogger<ShanghaiExecuteHandler<TExecuter, TResult>> _logger;

        private readonly HttpClient _httpClient;

        private readonly ShanghaiDispatcherOptions _options;

        public ShanghaiExecuteHandler(ShanghaiDispatcherOptions options, ILoggerFactory loggerFactory, string command)
        {
            _options = options;
            _command = command;
            _logger = loggerFactory.CreateLogger<ShanghaiExecuteHandler<TExecuter, TResult>>();
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.Deflate
            };
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(options.Url)
            };
        }

        private string Signature(string command, string ldpVenderId, string value, out DateTime timestamp)
        {
            timestamp = DateTime.Now;
            string text = string.Format("{0}{1}{2:yyyyMMddHHmm}{3}{4}", ldpVenderId, command, timestamp, value, _options.SecretKey);
            return text.ToMd5();
        }

        protected abstract string MakeRequest(TExecuter executer);

        protected abstract TResult MakeResult(XDocument document);

        public virtual async Task<TResult> HandleAsync(TExecuter executer)
        {
            string value = MakeRequest(executer);
            string sign = Signature(_command, executer.LdpVenderId, value, out DateTime timestamp);
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wAgent", executer.LdpVenderId),
                new KeyValuePair<string, string>("wAction",_command),
                new KeyValuePair<string, string>("wMsgID", timestamp.ToString("yyyyMMddHHmm")),
                new KeyValuePair<string, string>("wSign",sign.ToLower()),
                new KeyValuePair<string, string>("wParam",value),
            });
            try
            {
                _logger.LogDebug("Request message: {0}", content.ToString());
                HttpResponseMessage responseMessage = (await _httpClient.PostAsync("lotsale/lot", content)).EnsureSuccessStatusCode();
                byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();
                string msg = Encoding.GetEncoding("GB2312").GetString(bytes);
                _logger.LogDebug("Response message: {0}", msg);
                XDocument document = XDocument.Parse(msg);
                return MakeResult(document);
            }
            catch (Exception ex)
            {
                _logger.LogError("Request Exception:", ex);
                throw ex;
            }
        }
    }
}
