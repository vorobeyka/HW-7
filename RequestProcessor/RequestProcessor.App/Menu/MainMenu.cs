using System;
using System.Linq;
using System.Threading.Tasks;
using RequestProcessor.App.Logging;
using RequestProcessor.App.Services;

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
            var options = await _optionsSource.GetOptionsAsync();
            var tasks = options.Select(opt => _performer.PerformRequestAsync(opt.Item1, opt.Item2)).ToArray();
            if (true)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}
