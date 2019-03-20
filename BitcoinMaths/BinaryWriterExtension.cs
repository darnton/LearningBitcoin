using System.IO;

namespace BitcoinMaths
{
    public static class BinaryWriterExtension
    {
        public static void WriteVarInt(this BinaryWriter writer, ulong value)
        {
            if (value < 253)
            {
                writer.Write((byte)value);
                return;
            }
            if (value < 65536) //2^16
            {
                writer.Write((byte)253);
                writer.Write((short)value);
                return;
            }
            if (value < 4294967296) //2^32
            {
                writer.Write((byte)254);
                writer.Write((int)value);
                return;
            }
            writer.Write((byte)255);
            writer.Write(value);
        }
    }
}
