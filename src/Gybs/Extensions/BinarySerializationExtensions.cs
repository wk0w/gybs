using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace Gybs.Extensions
{
    /// <summary>
    /// Extensions for binary serialization.
    /// </summary>
    public static class BinarySerializationExtensions
    {
        /// <summary>
        /// Serializes the object to the binary data using memory stream.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="MemoryStream"/> and <see cref="BinaryFormatter"/> classes.
        /// </remarks>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The binary data.</returns>
        public static byte[] SerializeToBinary(this object obj)
        {
            using (var ms = new MemoryStream())
            {
                CreateFormatter().Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deserializes the byte array to an object using memory stream and casts it to the output type.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="MemoryStream"/> and <see cref="BinaryFormatter"/> classes.
        /// </remarks>
        /// <typeparam name="TOutputType">The type to cast.</typeparam>
        /// <param name="data">The binary data.</param>
        /// <returns>The deserialized object.</returns>
        public static TOutputType DeserializeFromBinary<TOutputType>(this byte[] data)
        {
            return (TOutputType)DeserializeFromBinary(data);
        }

        /// <summary>
        /// Deserializes the byte array to an object.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="MemoryStream"/> and <see cref="BinaryFormatter"/> classes.
        /// </remarks>
        /// <param name="data">The binary data.</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromBinary(this byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                return CreateFormatter().Deserialize(ms);
            }
        }

        private static BinaryFormatter CreateFormatter()
        {
            return new BinaryFormatter
            {
                AssemblyFormat = FormatterAssemblyStyle.Simple
            };
        }
    }
}
