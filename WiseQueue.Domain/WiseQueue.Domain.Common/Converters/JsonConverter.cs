using System;
using Common.Core.BaseClasses;
using Common.Core.Logging;
using Newtonsoft.Json;
using WiseQueue.Core.Common.Converters;
using WiseQueue.Core.Common.Models;

namespace WiseQueue.Domain.Common.Converters
{
    /// <summary>
    /// Converting object into JSON and JSON into the object.
    /// </summary>
    public sealed class JsonConverter : BaseLoggerObject, IJsonConverter
    {
        #region Fields...
        /// <summary>
        /// The <see cref="JsonSerializerSettings"/> instance.
        /// </summary>
        private readonly JsonSerializerSettings jsonSerializerSettings;
        #endregion

        #region Constructors...
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        public JsonConverter(ICommonLoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            jsonSerializerSettings = new JsonSerializerSettings();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="jsonSerializerSettings">The <see cref="JsonSerializerSettings"/> instance.</param>
        /// <param name="loggerFactory">The <see cref="ICommonLoggerFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="jsonSerializerSettings"/> is <see langword="null" />.</exception>
        public JsonConverter(JsonSerializerSettings jsonSerializerSettings, ICommonLoggerFactory loggerFactory): this(loggerFactory)
        {
            if (jsonSerializerSettings == null)
                throw new ArgumentNullException("jsonSerializerSettings");

            this.jsonSerializerSettings = jsonSerializerSettings;
        }
        #endregion

        #region Implementation of IJsonConverter

        /// <summary>
        /// <c>Convert</c> <c>object</c> into the JSON.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <returns>JSON.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null" />.</exception>
        public string ConvertToJson(object data)
        {
            logger.WriteTrace("Converting {0} into JSON...", data);

            if (data == null)
                throw new ArgumentNullException("data");

            string result = JsonConvert.SerializeObject(data, jsonSerializerSettings);

            logger.WriteTrace("The {0} has been converted into JSON: {1}", data, result);
            return result;
        }

        /// <summary>
        /// <c>Convert</c> JSON data into the <c>object</c>.
        /// </summary>
        /// <typeparam name="TObject"><see cref="Type"/> of object.</typeparam>
        /// <param name="jsonData">JSON.</param>
        /// <returns>The <typeparamref name="TObject"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="jsonData"/> is <see langword="null" />.</exception>
        public TObject ConvertFromJson<TObject>(string jsonData)
        {
            logger.WriteTrace("Converting JSON ({0}) into object (Type: {1})...", jsonData, typeof(TObject));

            if (string.IsNullOrWhiteSpace(jsonData)) 
                throw new ArgumentNullException("jsonData");

            TObject result = JsonConvert.DeserializeObject<TObject>(jsonData, jsonSerializerSettings);

            logger.WriteTrace("The JSON ({0}) has been converted into {1}", jsonData, result);
            return result;
        }

        /// <summary>
        /// <c>Convert</c> JSON data into the <c>object</c>.
        /// </summary>
        /// <param name="jsonData">JSON.</param>
        /// <param name="type"><see cref="Type"/> of object</param>
        /// <returns>The object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="jsonData"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null" />.</exception>
        public object ConvertFromJson(string jsonData, Type type)
        {
            logger.WriteTrace("Converting JSON ({0}) into object (Type: {1})...", jsonData, type);

            if (string.IsNullOrWhiteSpace(jsonData))
                throw new ArgumentNullException("jsonData");
            if (type == null)
                throw new ArgumentNullException("type");

            var result = JsonConvert.DeserializeObject(jsonData, type, jsonSerializerSettings);

            logger.WriteTrace("The JSON ({0}) has been converted into {1}", jsonData, result);
            return result;
        }

        #endregion
    }

}
