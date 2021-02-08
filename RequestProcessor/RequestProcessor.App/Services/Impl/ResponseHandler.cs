using RequestProcessor.App.Models;
using System.IO;
using System.Threading.Tasks;

namespace RequestProcessor.App.Services
{
    internal class ResponseHandler : IResponseHandler
    {
        public async Task HandleResponseAsync(IResponse response, IRequestOptions requestOptions, IResponseOptions responseOptions)
        {
            await File.WriteAllTextAsync(responseOptions.Path,
                $"Name: ${requestOptions.Name}\n$Code: ${response.Code}\n{response.Content}");
        }
    }
}
