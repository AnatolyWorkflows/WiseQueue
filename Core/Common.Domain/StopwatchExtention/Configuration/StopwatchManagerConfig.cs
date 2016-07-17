using System;
using System.Configuration;

namespace Common.Domain.StopwatchExtention.Configuration
{
    /// <summary>
    /// Configuration for the Stopwatch manager.
    /// </summary>
    public class StopwatchManagerConfig
    {
        #region Fields...

        /// <summary>
        /// Flag shows will manager show statistic or not.
        /// </summary>
        public bool UseBackgroundWorker { get; private set; }

        /// <summary>
        /// Sleep time that will be used in the StopwatchManager's background worker.
        /// </summary>
        public TimeSpan SleepTime { get; private set; }

        #endregion

        #region Constructors...

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="ConfigurationErrorsException">Using GetSection(StopwatchConfig): Error description.</exception>
        /// <remarks>For creating instances use factory methods.</remarks>
        private StopwatchManagerConfig(bool skipConfig = false)
        {
            if (skipConfig)
                return;

            try
            {
                // Get the current configuration file.
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                StopwatchManagerConfigurationSection customSection = config.GetSection(@"StopwatchConfig") as StopwatchManagerConfigurationSection;
                UseBackgroundWorker = customSection.UseBackgroundWorker;
                SleepTime = TimeSpan.FromSeconds(customSection.SleepTime);

            }
            catch (ConfigurationErrorsException err)
            {
                string errMsg = string.Format("Using GetSection(StopwatchConfig): {0}", err);
                throw new ConfigurationErrorsException(errMsg, err);
            }
        }

        #endregion

        #region Factory methods...

        /// <summary>
        /// Create configuration for running Stopwatch manager with background worker.
        /// </summary>
        /// <param name="sleepTime"> Sleep time that will be used in the StopwatchManager's background worker.</param>
        /// <returns>The <see cref="StopwatchManagerConfig"/> instance.</returns>
        /// <exception cref="ConfigurationErrorsException">Using GetSection(StopwatchConfig): Error description.</exception>
        public static StopwatchManagerConfig CreateBackgroundWorkerConfig(TimeSpan sleepTime)
        {
            StopwatchManagerConfig result = new StopwatchManagerConfig(true)
            {
                UseBackgroundWorker = true,
                SleepTime = sleepTime
            };

            return result;
        }

        /// <summary>
        /// Create configuration for running Stopwatch manager without background worker.
        /// </summary>
        /// <returns>The <see cref="StopwatchManagerConfig"/> instance.</returns>
        /// <exception cref="ConfigurationErrorsException">Using GetSection(StopwatchConfig): Error description.</exception>
        public static StopwatchManagerConfig CreateConfigWithoutBackgroundWorker()
        {
            StopwatchManagerConfig result = new StopwatchManagerConfig(true)
            {
                UseBackgroundWorker = false,
                SleepTime = TimeSpan.Zero
            };

            return result;
        }

        /// <summary>
        /// Create configuration using settings from the configuration file.
        /// </summary>
        /// <returns>The <see cref="StopwatchManagerConfig"/> instance.</returns>
        /// <exception cref="ConfigurationErrorsException">Using GetSection(StopwatchConfig): Error description.</exception>
        public static StopwatchManagerConfig CreateConfigFromConfigFile()
        {
            return new StopwatchManagerConfig();
        }

        #endregion
    }
}
