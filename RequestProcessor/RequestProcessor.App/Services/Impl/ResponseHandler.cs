using RequestProcessor.App.Models;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RequestProcessor.App.Services
{
    internal class ResponseHandler : IResponseHandler
    {
        public async Task HandleResponseAsync(IResponse response, IRequestOptions requestOptions, IResponseOptions responseOptions)
        {
            if (!response.Handled) return;
            await File.WriteAllTextAsync(responseOptions.Path, response.Content);
        }
    }
}
