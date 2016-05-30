﻿using System;
using NLog;
using WiseQueue.Core.Common.Logging;

namespace WiseQueue.Domain.NLogger
{
    /// <summary>
    /// <c>Logger</c> that use <see cref="NLog"/> libraries.
    /// </summary>
    public sealed class WiseQueueNLogLogger : IWiseQueueLogger
    {
        #region Fields...
        /// <summary>
        /// The <see cref="Logger"/> instance.
        /// </summary>
        private readonly Logger logger;
        #endregion

        #region Constructors...
        /// <summary>
        /// Constructor.
        /// </summary>
        public WiseQueueNLogLogger()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The logger's <c>name</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        public WiseQueueNLogLogger(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            logger = LogManager.GetLogger(name);
        }
        #endregion

        #region Implementation of IWiseQueueLogger

        /// <summary>
        /// Write trace information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        public void WriteTrace(string msg)
        {
            logger.Trace(msg);
        }

        /// <summary>
        /// Write trace information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        public void WriteTrace(string msg, params object[] args)
        {
            logger.Trace(msg, args);
        }

        /// <summary>
        /// Write debug information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        public void WriteDebug(string msg)
        {
            logger.Debug(msg);
        }

        /// <summary>
        /// Write debug information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        public void WriteDebug(string msg, params object[] args)
        {
            logger.Debug(msg, args);
        }

        /// <summary>
        /// Write information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        public void WriteInfo(string msg)
        {
            logger.Info(msg);
        }

        /// <summary>
        /// Write information into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        public void WriteInfo(string msg, params object[] args)
        {
            logger.Info(msg, args);
        }

        /// <summary>
        /// Write error message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        public void WriteError(string msg)
        {
            logger.Error(msg);
        }

        /// <summary>
        /// Write error message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        public void WriteError(string msg, params object[] args)
        {
            logger.Error(msg, args);
        }

        /// <summary>
        /// Write error message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="exception"><see cref="Exception"/> that will be added into the message.</param>
        public void WriteError(Exception exception, string msg)
        {
            logger.Error(exception, msg);
        }

        /// <summary>
        /// Write error message into the log.
        /// </summary>
        /// <param name="msg">A message.</param>
        /// <param name="args">Arguments</param>
        /// <param name="exception"><see cref="Exception"/> that will be added into the message.</param>
        public void WriteError(Exception exception, string msg, params object[] args)
        {
            logger.Error(exception, msg, args);
        }

        #endregion
    }
}
