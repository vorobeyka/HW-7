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
            var fileStream = new FileStream(responseOptions.Path, FileMode.OpenOrCreate, FileAccess.Write);
            if (!response.Handled) return;
        }
    }
}
