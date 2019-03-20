using System.IO;

namespace BitcoinMaths
{
    public static class BinaryReaderExtension
    {
        public static ulong ReadVarInt(this BinaryReader reader)
        {
            var leadingByte = reader.ReadByte();
            if (leadingByte == 253) //fd
            {
                return reader.ReadUInt16();
            }
            if (leadingByte == 254) //fe
            {
                return reader.ReadUInt32();
            }
            if (leadingByte == 255) //ff
            {
                return reader.ReadUInt64();
            }
            return leadingByte;
        }
    }
}
