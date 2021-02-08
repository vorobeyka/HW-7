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
            var returnValue = true;
            IResponse response;
            try
            {
                _logger.Log($"Start request {requestOptions.Name}");
                response = await _requestHandler.HandleRequestAsync(requestOptions);
            }
            catch (ArgumentNullException ex)
            {
                _logger.Log(ex, $"Argument {ex.ParamName} is Null");
                return false;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.Log(ex, "Invlaid options element");
                return false;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Create empty response");
                response = new Response(false, 0, null);
                returnValue = false;
            }

            try
            {
                _logger.Log($"Start response handle\nHandled: {response.Handled}\nCode: {response.Code}");
                await _responseHandler.HandleResponseAsync(response, requestOptions, responseOptions);
                _logger.Log($"Response was handled");
            }
            catch (Exception ex)
            {
                _logger.Log($"Response was not handled. Throw PerformException");
                throw new PerformException(ex.Message, ex.InnerException);
            }
            return returnValue;
        }
    }
}
