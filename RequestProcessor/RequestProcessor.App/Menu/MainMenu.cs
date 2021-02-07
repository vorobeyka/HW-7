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
            Console.WriteLine("HTTP request processor\nby Andrey Basystyi");
            try
            {
                Console.WriteLine("Reading from json...");
                var options = (await _optionsSource.GetOptionsAsync());
                Console.WriteLine($"Was read {options.Count()} elements\nWhere:");

                var validOptions = options.Where(opt => opt.Item1.IsValid).Count();
                var notValidOptions = options.Where(opt => !opt.Item1.IsValid).Count();
                Console.WriteLine($"{validOptions} are valid");
                if (notValidOptions != 0)
                {
                    Console.WriteLine($"{notValidOptions} are invalid");
                }
                if (validOptions == 0)
                {
                    Console.WriteLine("All elements are invalid");
                    return 0;
                }

                var tasks = options.Where(opt => opt.Item1.IsValid && opt.Item2.IsValid)
                    .Select(opt => _performer.PerformRequestAsync(opt.Item1, opt.Item2)).ToArray();
                Console.WriteLine($"Start {tasks.Length} http-requests");
                var result = Task.WhenAll(tasks);
                if (result.IsFaulted)
                {
                    throw result.Exception.InnerException;
                }
                Console.WriteLine("All tasks completed");
            }
            catch (PerformException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
            return 0;
        }
    }
}
