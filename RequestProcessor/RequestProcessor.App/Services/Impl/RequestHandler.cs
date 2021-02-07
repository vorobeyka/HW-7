using RequestProcessor.App.Models;
using RequestProcessor.App.Models.Impl;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RequestProcessor.App.Services.Impl
{
    internal class RequestHandler : IRequestHandler
    {
        private readonly HttpClient _client;

        public RequestHandler(HttpClient client)
        {
            _client = client;
        }

        public async Task<IResponse> HandleRequestAsync(IRequestOptions requestOptions)
        {
            if (requestOptions == null) throw new ArgumentNullException(nameof(requestOptions));
            if (!requestOptions.IsValid) throw new ArgumentOutOfRangeException(nameof(requestOptions));

            _client.Timeout = new TimeSpan(0, 0, 10);
            var httpMethod = MapMethod(requestOptions.Method);
            using var message = new HttpRequestMessage(httpMethod, new Uri(requestOptions.Address));
            using var response = await _client.SendAsync(message, HttpCompletionOption.ResponseContentRead);
            return new Response(response.IsSuccessStatusCode, 123, response.Content.ToString());
        }

        private static HttpMethod MapMethod(RequestMethod method)
        {
            switch (method)
            {
                case RequestMethod.Get:
                    return HttpMethod.Get;
                case RequestMethod.Post:
                    return HttpMethod.Post;
                case RequestMethod.Put:
                    return HttpMethod.Put;
                case RequestMethod.Patch:
                    return HttpMethod.Patch;
                case RequestMethod.Delete:
                    return HttpMethod.Delete;
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, "Invalid request method");
            }
        }
    }
}
