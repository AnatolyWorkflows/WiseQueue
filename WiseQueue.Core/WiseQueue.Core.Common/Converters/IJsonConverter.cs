using System;

namespace WiseQueue.Core.Common.Converters
{
    /// <summary>
    /// Interface shows that <c>object</c> can convert items into the JSON format and create items from JSON.
    /// </summary>
    public interface IJsonConverter
    {
        /// <summary>
        /// <c>Convert</c> <c>object</c> into the JSON.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <returns>JSON.</returns>
        string ConvertToJson(object data);

        /// <summary>
        /// <c>Convert</c> JSON data into the <c>object</c>.
        /// </summary>
        /// <typeparam name="TObject"><see cref="Type"/> of object.</typeparam>
        /// <param name="jsonData">JSON.</param>
        /// <returns>The <typeparamref name="TObject"/> instance.</returns>
        TObject ConvertFromJson<TObject>(string jsonData);

        /// <summary>
        /// <c>Convert</c> JSON data into the <c>object</c>.
        /// </summary>
        /// <param name="jsonData">JSON.</param>
        /// <param name="type"><see cref="Type"/> of object</param>
        /// <returns>The object.</returns>
        object ConvertFromJson(string jsonData, Type type);
    }
}
