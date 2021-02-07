using RequestProcessor.App.Models;
using RequestProcessor.App.Models.Impl;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace RequestProcessor.App.Services.Impl
{
    internal class OptionsSource : IOptionsSource
    {
        private readonly string _filePath;

        public OptionsSource(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<IEnumerable<(IRequestOptions, IResponseOptions)>> GetOptionsAsync()
        {
            var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            var options = await JsonSerializer.DeserializeAsync<List<RequestOptions>>(fs);
            options.ForEach(opt => opt.IsValid = IsValidOptions(opt));

            return options.Select(x => ((IRequestOptions)x, (IResponseOptions)x));
        }

        private static bool IsValidOptions(RequestOptions options)
        {
            try
            {
                new Uri(options.Address);
            }
            catch (Exception)
            {
                return false;
            }
            return !(string.IsNullOrEmpty(options.Name)
                     || string.IsNullOrEmpty(options.Address)
                     || (string.IsNullOrEmpty(options.ContentType) && !string.IsNullOrEmpty(options.Body))
                     || (string.IsNullOrEmpty(options.Path)));
        }
    }
}
