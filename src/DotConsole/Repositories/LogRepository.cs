﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;

namespace DotConsole.Repositories
{
    internal class LogRepository : ILogger
    {
        private readonly AggregateCatalog catalog;
        private readonly CompositionContainer container;
        private string _logName = "Repository";

        /// <summary>
        /// Instantiates a new instance of the LogRepository Class.
        /// </summary>
        internal LogRepository()
        {
            Logs = new List<ILogger>();
            catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetCallingAssembly()));
            container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        /// <summary>
        /// The log collection used in the application.
        /// </summary>
        [ImportMany("Logs", typeof (ILogger))]
        internal IList<ILogger> Logs { get; set; }

        #region ILogger Members

        /// <summary>
        /// The name of the log.
        /// </summary>
        public string LogName
        {
            get { return _logName; }
            set { _logName = value; }
        }

        public void Write(string message)
        {
            Logs.ForEach(x => x.Write(message));
        }

        public void Write(string format, params object[] args)
        {
            Logs.ForEach(x => x.Write(format, args));
        }

        /// <summary>
        /// Writes a message to all logs in the collection.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void WriteLine(string message)
        {
            Logs.ForEach(x => x.WriteLine(message));
        }

        public void WriteLine(string format, params object[] args)
        {
            Logs.ForEach(x => x.WriteLine(format, args));
        }

        /// <summary>
        /// Writes a message designated as a warning to all logs in the collection.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void WriteWarning(string message)
        {
            Logs.ForEach(x => x.WriteWarning(message));
        }

        public void WriteWarning(string format, params object[] args)
        {
            Logs.ForEach(x => x.WriteWarning(format, args));
        }

        /// <summary>
        /// Writes a message designated as a error to all logs in the collection.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void WriteError(string message)
        {
            Logs.ForEach(x => x.WriteError(message));
        }

        public void WriteError(string format, params object[] args)
        {
            Logs.ForEach(x => x.WriteError(format, args));
        }

        /// <summary>
        /// Disposes all logs in the collection should they need such.
        /// </summary>
        public void Dispose()
        {
            Logs.ForEach(x => x.Dispose());
        }

        #endregion

        /// <summary>
        /// Retrieves a specific log by name.
        /// </summary>
        /// <param name="logName">The log name.</param>
        /// <returns>An instance of the log.</returns>
        public ILogger GetLog(string logName)
        {
            return Logs
                .Where(x => x.LogName == logName)
                .FirstOrDefault();
        }
    }
}