﻿//-----------------------------------------------------------------------
// <copyright file="LogManager.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.Logging
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides access to log for applications. This class cannot be inherited.
    /// </summary>
    public static class LogManager
    {
        /// <summary>
        /// Field LoggerDictionary.
        /// </summary>
        private static readonly Dictionary<int, Logger> LoggerDictionary = new Dictionary<int, Logger>();

        /// <summary>
        /// Field SyncRoot.
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// Opens the log file for the current application.
        /// </summary>
        /// <param name="logFile">Log file for the current application; if null or string.Empty use the default log file.</param>
        /// <returns>Logger instance.</returns>
        public static Logger Open(string logFile = null)
        {
            LogConfig logConfig = new LogConfig();

            if (!string.IsNullOrEmpty(logFile))
            {
                logConfig.LogFile = logFile;
            }

            int key = logConfig.GetHashCode();

            if (LoggerDictionary.ContainsKey(key))
            {
                return LoggerDictionary[key];
            }

            lock (SyncRoot)
            {
                if (LoggerDictionary.ContainsKey(key))
                {
                    return LoggerDictionary[key];
                }
                else
                {
                    Logger result = new Logger(logConfig);

                    LoggerDictionary.Add(key, result);

                    return result;
                }
            }
        }

        /// <summary>
        /// Opens the log file for the current application.
        /// </summary>
        /// <param name="logFile">Log file for the current application; if null or string.Empty use the default log file.</param>
        /// <param name="loggerSetup">LoggerSetup info for the logger instance; if null use the default LoggerSetup info.</param>
        /// <returns>Logger instance.</returns>
        public static Logger Open(string logFile, LoggerSetup loggerSetup)
        {
            LogConfig logConfig = new LogConfig();

            if (!string.IsNullOrEmpty(logFile))
            {
                logConfig.LogFile = logFile;
            }

            if (loggerSetup != null)
            {
                logConfig.LoggerSetup = loggerSetup;
            }

            int key = logConfig.GetHashCode();

            if (LoggerDictionary.ContainsKey(key))
            {
                return LoggerDictionary[key];
            }

            lock (SyncRoot)
            {
                if (LoggerDictionary.ContainsKey(key))
                {
                    return LoggerDictionary[key];
                }
                else
                {
                    Logger result = new Logger(logConfig);

                    LoggerDictionary.Add(key, result);

                    return result;
                }
            }
        }

        /// <summary>
        /// Opens the log file for the current application.
        /// </summary>
        /// <param name="logFile">Log file for the current application; if null or string.Empty use the default log file.</param>
        /// <param name="configFile">Configuration file which contains LoggerSetup info; if null or string.Empty use the default configuration file.</param>
        /// <param name="throwOnError">true to throw any exception that occurs.-or- false to ignore any exception that occurs.</param>
        /// <returns>Logger instance.</returns>
        public static Logger Open(string logFile, string configFile, bool throwOnError = false)
        {
            LogConfig logConfig = new LogConfig();

            if (!string.IsNullOrEmpty(logFile))
            {
                logConfig.LogFile = logFile;
            }

            logConfig.LoggerSetup = LogConfigManager.GetLoggerSetup(configFile, throwOnError);

            int key = logConfig.GetHashCode();

            if (LoggerDictionary.ContainsKey(key))
            {
                return LoggerDictionary[key];
            }

            lock (SyncRoot)
            {
                if (LoggerDictionary.ContainsKey(key))
                {
                    return LoggerDictionary[key];
                }
                else
                {
                    Logger result = new Logger(logConfig, configFile);

                    LoggerDictionary.Add(key, result);

                    return result;
                }
            }
        }

        /// <summary>
        /// Opens the log configuration file for the current application.
        /// </summary>
        /// <param name="configFile">Configuration file which contains LogConfig info; if null or string.Empty use the default configuration file.</param>
        /// <param name="throwOnError">true to throw any exception that occurs.-or- false to ignore any exception that occurs.</param>
        /// <returns>Logger instance.</returns>
        public static Logger OpenConfig(string configFile = null, bool throwOnError = false)
        {
            LogConfig logConfig = LogConfigManager.GetLogConfig(configFile, throwOnError);

            int key = logConfig.GetHashCode();

            if (LoggerDictionary.ContainsKey(key))
            {
                return LoggerDictionary[key];
            }

            lock (SyncRoot)
            {
                if (LoggerDictionary.ContainsKey(key))
                {
                    return LoggerDictionary[key];
                }
                else
                {
                    Logger result = new Logger(logConfig, configFile);

                    LoggerDictionary.Add(key, result);

                    return result;
                }
            }
        }

        /// <summary>
        /// Opens the log configuration file for the current application.
        /// </summary>
        /// <param name="logConfig">LogConfig instance; if null, use the default configuration.</param>
        /// <returns>Logger instance.</returns>
        public static Logger OpenConfig(LogConfig logConfig)
        {
            LogConfig logConfigInfo = logConfig ?? new LogConfig();

            int key = logConfigInfo.GetHashCode();

            if (LoggerDictionary.ContainsKey(key))
            {
                return LoggerDictionary[key];
            }

            lock (SyncRoot)
            {
                if (LoggerDictionary.ContainsKey(key))
                {
                    return LoggerDictionary[key];
                }
                else
                {
                    Logger result = new Logger(logConfigInfo);

                    LoggerDictionary.Add(key, result);

                    return result;
                }
            }
        }
    }
}
