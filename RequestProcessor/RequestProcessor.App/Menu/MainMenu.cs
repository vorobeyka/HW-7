using System;
using System.Linq;
using System.Threading.Tasks;
using RequestProcessor.App.Logging;
using RequestProcessor.App.Services;
using RequestProcessor.App.Exceptions;

namespace RequestProcessor.App.Menu
{
    /// <summary>
    /// Main menu.
    /// </summary>
    internal class MainMenu : IMainMenu
    {
        private readonly IRequestPerformer _performer;
        private readonly IOptionsSource _optionsSource;
        private readonly ILogger _logger;
        /// <summary>
        /// Constructor with DI.
        /// </summary>
        /// <param name="options">Options source</param>
        /// <param name="performer">Request performer.</param>
        /// <param name="logger">Logger implementation.</param>
        public MainMenu(
            IRequestPerformer performer, 
            IOptionsSource options, 
            ILogger logger)
        {
            _performer = performer;
            _optionsSource = options;
            _logger = logger;
        }

        public async Task<int> StartAsync()
        {
            Console.WriteLine("HTTP request processor\nby Andrey Basystyi\nReading from json...");

            var options = (await _optionsSource.GetOptionsAsync());
            var validOptions = options.Where(opt => opt.Item1.IsValid).Count();
            var notValidOptions = options.Where(opt => !opt.Item1.IsValid).Count();

            #region Print valid/not valid options
            Console.WriteLine($"Was read {options.Count()} elements\nWhere:");
            _logger.Log($"Read {options.Count()} elements");
            Console.WriteLine($"{validOptions} are valid");
            _logger.Log($"Valid options: {validOptions}");
            if (notValidOptions != 0)
            {
                Console.WriteLine($"{notValidOptions} are invalid");
                _logger.Log($"Invalid options: {notValidOptions}");
            }
            #endregion
            if (options.Count() == 0)
            {
                Console.WriteLine("Empty options list");
                _logger.Log("Empty options");
            }

            try
            {
                var tasks = options.Select(opt => _performer.PerformRequestAsync(opt.Item1, opt.Item2)).ToArray();
                Console.WriteLine($"Start {tasks.Length} http-requests");
                var result = await Task.WhenAll(tasks);
                PrintResult(result);
                return result.Any(r => r == false) ? -1 : 0;
            }
            catch (PerformException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                _logger.Log(ex, "Handled PerformException");
                return -1;
            }
        }

        private static void PrintResult(bool[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine($"Request №{i + 1}: {(array[i] ? "success" : "failed")}");
            }
            Console.WriteLine("All tasks completed");
        }
    }
}
