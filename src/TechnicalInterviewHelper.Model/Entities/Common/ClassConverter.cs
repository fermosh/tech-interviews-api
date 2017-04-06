namespace TechnicalInterviewHelper.Model.Entities.Common
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Returns an interface
    /// </summary>
    /// <typeparam name="TInterface">The interface</typeparam>
    /// <typeparam name="TImplementation">The concrete class</typeparam>
    public class ConcreteListTypeConverter<TInterface, TImplementation> : JsonConverter where TImplementation : TInterface
    {
        /// <summary>
        /// Validates if object instance implements implements interface TInterface
        /// </summary>
        /// <param name="objectType">The objectType</param>
        /// <returns>True if the object implements interface TInterface</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TInterface);
        }

        /// <summary>
        /// Deserializes the JSON structure containded by the specific JsonReader into an instance of the specified type
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <param name="objectType">The objectType</param>
        /// <param name="existingValue">The existingValue</param>
        /// <param name="serializer">The serializer</param>
        /// <returns>The interface of type TInterface</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<TImplementation>(reader);
        }

        /// <summary>
        /// Serializes the specified object and writes the JSON structure to a STREAM using the specified JsonWriter
        /// </summary>
        /// <param name="writer">The writer</param>
        /// <param name="value">The valur</param>
        /// <param name="serializer">Ther serializer</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
