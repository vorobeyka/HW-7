using RequestProcessor.App.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace RequestProcessor.App.Services
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
            var options = await JsonSerializer.DeserializeAsync<IEnumerable<RequestOptions>>(fs, new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() },
                WriteIndented = true,
            });
            var result = new List<(IRequestOptions, IResponseOptions)>();

            foreach (var i in options)
            {
                result.Add((i, i));
            }

            return result;
        }
    }
}
