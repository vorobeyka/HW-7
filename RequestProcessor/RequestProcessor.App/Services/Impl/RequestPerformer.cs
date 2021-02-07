using System;
using System.Threading.Tasks;
using RequestProcessor.App.Logging;
using RequestProcessor.App.Models;
using RequestProcessor.App.Models.Impl;
using RequestProcessor.App.Exceptions;

namespace RequestProcessor.App.Services
{
    /// <summary>
    /// Request performer.
    /// </summary>
    internal class RequestPerformer : IRequestPerformer
    {
        private readonly IRequestHandler _requestHandler;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger _logger;
        /// <summary>
        /// Constructor with DI.
        /// </summary>
        /// <param name="requestHandler">Request handler implementation.</param>
        /// <param name="responseHandler">Response handler implementation.</param>
        /// <param name="logger">Logger implementation.</param>
        public RequestPerformer(
            IRequestHandler requestHandler, 
            IResponseHandler responseHandler,
            ILogger logger)
        {
            _requestHandler = requestHandler;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<bool> PerformRequestAsync(
            IRequestOptions requestOptions, 
            IResponseOptions responseOptions)
        {
            IResponse response = null;
            try
            {
                response = await _requestHandler.HandleRequestAsync(requestOptions);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Task CANCELED!FFFFFFFFFFFFFFFFFFFFFFFFFFF");
                response = new Response(false, 0, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Task CANCELED!FFFFFFFFFFFFFFFFFFFFFFFFFFF");
                throw new PerformException(ex.Message, ex);
            }
            finally
            {
                await _responseHandler.HandleResponseAsync(response, requestOptions, responseOptions);
            }
            return true;
        }
    }
}
