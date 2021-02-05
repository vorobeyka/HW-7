using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RequestProcessor.App.Logging
{
    internal class Logger : ILogger
    {
        public void Log(string message)
        {
            Debug.WriteLine(message);
        }

        public void Log(Exception exception, string message)
        {
            Debug.WriteLine($"Exception: {exception.Message}\n{message}");
        }
    }
}
