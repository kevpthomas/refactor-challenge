using System;

namespace RefactorThis.Core.Interfaces
{
    /// <summary>
    /// Describes minimal functionality for exception logging.
    /// </summary>
    /// <remarks>
    /// Normally I would either use a framework interface (e.g., from NLog)
    /// or I would have a custom interface adapting an underlying framework.
    /// </remarks>
    public interface ILogger
    {
        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="ex">Exception to log.</param>
        void Log(Exception ex);
    }
}