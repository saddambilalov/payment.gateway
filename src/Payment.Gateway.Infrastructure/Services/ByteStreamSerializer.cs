namespace Payment.Gateway.Infrastructure.Services
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using Interfaces;

    public class ByteStreamSerializer : IByteStreamSerializer
    {
        public byte[] Serialize<TInput>(TInput objectToSerialize) where TInput : new()
        {
            ValidateIfIsSerializable<TInput>();

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {

                formatter.Serialize(stream, objectToSerialize);
                return stream.ToArray();
            }
        }

        private static void ValidateIfIsSerializable<TInput>() where TInput : new()
        {
            var type = typeof(TInput);

            if (!type.IsSerializable
                                 && !(typeof(ISerializable).IsAssignableFrom(type)))
                throw new InvalidOperationException("A serializable Type is required");
        }

        public TOutput Deserialize<TOutput>(byte[] arrayToDeserialize) where TOutput : new()
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(arrayToDeserialize))
            {
                return (TOutput)formatter.Deserialize(stream);
            }
        }
    }
}