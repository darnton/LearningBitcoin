using System.IO;
using BitcoinMaths;

namespace Bitcoin
{
    public class Script
    {
        private byte[] RawScript { get; }

        private Script(byte[] rawScript)
        {
            RawScript = rawScript;
        }

        public int Length
        {
            get { return RawScript.Length; }
        }

        public void Serialise(BinaryWriter writer)
        {
        }

        public static Script Parse(BinaryReader reader)
        {
            var scriptLength = (int)reader.ReadVarInt();
            
            return new Script(reader.ReadBytes(scriptLength));
        }
    }
}
