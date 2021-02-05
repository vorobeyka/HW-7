using RequestProcessor.App.Models;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace RequestProcessor.App.Services
{
    internal class OptionsSource : IOptionsSource
    {
        private readonly string _filePath;

        public OptionsSource(string filePath)
        {
            _filePath = filePath;
        }

        private RequestMethod GetMethod(string method)
        {
            switch (method)
            {
                case "GET": return RequestMethod.Get;
                case "DELETE": return RequestMethod.Delete;
                case "PATCH": return RequestMethod.Patch;
                case "POST": return RequestMethod.Post;
                case "PUT": return RequestMethod.Put;
                default: return RequestMethod.Undefined;
            }
        }

        public async Task<IEnumerable<(IRequestOptions, IResponseOptions)>> GetOptionsAsync()
        {
            var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            var options = await JsonSerializer.DeserializeAsync<IEnumerable<RequestOptions>>(fs);
            var result = new List<(IRequestOptions, IResponseOptions)>();

            foreach (var i in options)
            {
                i.Method = GetMethod(i.RequestMethodAsString);
                result.Add((i, i));
            }

            return result;
        }
    }
}
