using RequestProcessor.App.Models;
using System.IO;
using System.Threading.Tasks;

namespace RequestProcessor.App.Services
{
    internal class ResponseHandler : IResponseHandler
    {
        public async Task HandleResponseAsync(IResponse response, IRequestOptions requestOptions, IResponseOptions responseOptions)
        {
            var header = $"Name: {requestOptions.Name}\nIsValid: {requestOptions.IsValid}\n" +
                $"Handled: {response.Handled}\nCode: {response.Code}";
            await File.WriteAllTextAsync(responseOptions.Path, $"{header}\n{response.Content}");
        }
    }
}
