using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Helpers
{
    public static class BinarySerializeHelper
    {
        public static void Serialize<T>(T dictionary, Stream stream)
        {
            using (stream)
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, dictionary);
            }
        }

        public static T Deserialize<T>(Stream stream)
        {
            using (stream)
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
