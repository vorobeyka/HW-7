using RequestProcessor.App.Models;
using RequestProcessor.App.Models.Impl;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
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

            var httpMethod = MapMethod(requestOptions.Method);
            using var message = new HttpRequestMessage(httpMethod, new Uri(requestOptions.Address));
            if (httpMethod != HttpMethod.Get && requestOptions.ContentType != null)
            {
                message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(requestOptions.ContentType));
                message.Content = new StringContent(requestOptions.Body ?? "");
            }
            using var response = await _client.SendAsync(message);
            return new Response(response.IsSuccessStatusCode,
                                (int)response.StatusCode,
                                await response.Content.ReadAsStringAsync());
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
