using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RequestProcessor.App.Models;
using System.Net.Http;

namespace RequestProcessor.App.Mappings
{
    internal class HttpMethodsMappings
    {
        public static async Task<RequestMethod> GetRequestMethod(string method)
        {
            switch (method.ToLower())
            {
                case "get":
                    return RequestMethod.Get;
                case "post":
                    return RequestMethod.Post;
                case "put":
                    return RequestMethod.Put;
                case "patch":
                    return RequestMethod.Patch;
                case "delete":
                    return RequestMethod.Delete;
                default:
                    return RequestMethod.Undefined;
            }
        }

        public static async Task<HttpMethod> GetHttpMethod(RequestMethod method)
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