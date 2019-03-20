using System.IO;
using BitcoinMaths;

namespace Bitcoin
{
    public class TxOutput
    {
        public ulong Amount { get; }
        public Script ScriptPubKey { get; }

        private TxOutput(ulong amount, Script scriptPubKey)
        {
            Amount = amount;
            ScriptPubKey = scriptPubKey;
        }

        public void Serialise(BinaryWriter writer)
        {
            writer.Write(Amount);
            ScriptPubKey.Serialise(writer);
        }

        public static TxOutput Parse(BinaryReader reader)
        {
            var amount = reader.ReadUInt64();
            var scriptPubKey = Script.Parse(reader);

            return new TxOutput(amount, scriptPubKey);
        }
    }
}
