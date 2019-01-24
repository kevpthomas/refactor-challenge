using System;
using System.Diagnostics;
using RefactorThis.Core.Interfaces;

namespace RefactorThis.Infrastructure.Log
{
    /// <summary>
    /// Provide Debug exception logging.
    /// </summary>
    public class DebugLogger : ILogger
    {
        public void Log(Exception ex)
        {
            Debug.Print(ex.Message);
        }
    }
}