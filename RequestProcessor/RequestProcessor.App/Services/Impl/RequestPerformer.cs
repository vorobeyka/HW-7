using System;
using System.Threading.Tasks;
using RequestProcessor.App.Logging;
using RequestProcessor.App.Models;
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
            try
            {
                var response = await _requestHandler.HandleRequestAsync(requestOptions);
                await _responseHandler.HandleResponseAsync(response, requestOptions, responseOptions);
            }
            catch (TaskCanceledException ex)
            {

            }
            catch (System.Net.Http.HttpRequestException ex)
            {

            }
            catch (PerformException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "unexpected exception on request perform");
            }
            //TODO
            return true;
        }
    }
}
