using System;
using System.Threading.Tasks;
using RequestProcessor.App.Menu;
using RequestProcessor.App.Services;
using RequestProcessor.App.Services.Impl;
using RequestProcessor.App.Logging;
using System.Net.Http;

namespace RequestProcessor.App
{
    /// <summary>
    /// Entry point.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Options file path.
        /// </summary>
        private static readonly string _optionsFilePath = "options.json";
        /// <summary>
        /// Entry point.
        /// </summary>
        /// <returns>Returns exit code.</returns>
        private static async Task<int> Main()
        {
            try
            {
                var options = new OptionsSource(_optionsFilePath);
                var logger = new Logger();
                var client = new HttpClient();
                var requestHandler = new RequestHandler(client);
                var responseHandler = new ResponseHandler();
                var requestPerformer = new RequestPerformer(requestHandler, responseHandler, logger);

                var mainMenu = new MainMenu(requestPerformer, options, logger);

                return await mainMenu.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Critical unhandled exception");
                Console.WriteLine(ex);
                return -1;
            }
        }
    }
}
