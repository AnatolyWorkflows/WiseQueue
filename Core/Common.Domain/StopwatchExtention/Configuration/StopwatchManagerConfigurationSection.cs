using System;
using System.Configuration;

namespace Common.Domain.StopwatchExtention.Configuration
{
    public sealed class StopwatchManagerConfigurationSection : ConfigurationSection
    {
        // The collection (property bag) that contains 
        // the section properties.
        private static ConfigurationPropertyCollection properties;

        // Internal flag to disable 
        // property setting.
        private static bool readOnly;

        private static readonly ConfigurationProperty useBackgroundWorkerDefaultProperty =
            new ConfigurationProperty("UseBackgroundWorker",
            typeof(Boolean), "true",
            ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty sleepTimeDefaultProperty =
            new ConfigurationProperty("SleepTime",
            typeof(Int32), "15",
            ConfigurationPropertyOptions.None);

        // CustomSection constructor.
        public StopwatchManagerConfigurationSection()
        {
            // Property initialization
            properties = new ConfigurationPropertyCollection
            {
                useBackgroundWorkerDefaultProperty,
                sleepTimeDefaultProperty
            };

        }


        // This is a key customization. 
        // It returns the initialized property bag.
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return properties;
            }
        }


        private new bool IsReadOnly
        {
            get
            {
                return readOnly;
            }
        }

        // Use this to disable property setting.
        private void ThrowIfReadOnly(string propertyName)
        {
            if (IsReadOnly)
                throw new ConfigurationErrorsException(
                    "The property " + propertyName + " is read only.");
        }


        // Customizes the use of CustomSection
        // by setting _ReadOnly to false.
        // Remember you must use it along with ThrowIfReadOnly.
        protected override object GetRuntimeObject()
        {
            // To enable property setting just assign true to
            // the following flag.
            readOnly = true;
            return base.GetRuntimeObject();
        }


        public bool UseBackgroundWorker
        {
            get
            {
                return (bool)this["UseBackgroundWorker"];
            }
            set
            {
                // With this you disable the setting.
                // Remember that the _ReadOnly flag must
                // be set to true in the GetRuntimeObject.
                ThrowIfReadOnly("UseBackgroundWorker");
                this["UseBackgroundWorker"] = value;
            }
        }

        public Int32 SleepTime
        {
            get
            {
                return (Int32)this["SleepTime"];
            }
            set
            {
                // With this you disable the setting.
                // Remember that the _ReadOnly flag must
                // be set to true in the GetRuntimeObject.
                ThrowIfReadOnly("UseBackgroundWorker");
                this["SleepTime"] = value;
            }
        }
    }
}