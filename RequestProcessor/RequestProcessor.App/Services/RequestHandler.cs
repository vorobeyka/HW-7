using RequestProcessor.App.Models;
using RequestProcessor.App.Mappings;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RequestProcessor.App.Services
{
    internal class RequestHandler : IRequestHandler
    {
        private HttpClient _client;

        public RequestHandler(HttpClient client)
        {
            _client = client;
        }

        public async Task<IResponse> HandleRequestAsync(IRequestOptions requestOptions)
        {
            if (requestOptions == null) throw new ArgumentNullException(nameof(requestOptions));
            if (!requestOptions.IsValid) throw new ArgumentOutOfRangeException(nameof(requestOptions));

            _client.Timeout = new TimeSpan(0, 0, 10);
            var method = await HttpMethodsMappings.GetHttpMethod(requestOptions.Method);
            using var message = new HttpRequestMessage(method, new Uri(requestOptions.Address));
            using var response = await _client.SendAsync(message, HttpCompletionOption.ResponseContentRead);
            return new Response(response.IsSuccessStatusCode, 123, response.Content.ToString());
        }
    }
}
