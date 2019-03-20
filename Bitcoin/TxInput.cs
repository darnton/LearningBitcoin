using System.IO;
using System.Numerics;
using BitcoinMaths;

namespace Bitcoin
{
    public class TxInput
    {
        public BigInteger PreviousTxId { get; }
        public uint PreviousTxIndex { get; }
        public Script ScriptSig { get; }
        public uint Sequence { get; }

        private TxInput(BigInteger previousTxId, uint previousTxIndex, Script scriptSig, uint sequence)
        {
            PreviousTxId = previousTxId;
            PreviousTxIndex = previousTxIndex;
            ScriptSig = scriptSig;
            Sequence = sequence;
        }

        public void Serialise(BinaryWriter writer)
        {
            writer.Write(PreviousTxId.ToByteArray(ByteArrayFormat.LittleEndianUnsigned, 32));
            writer.Write(PreviousTxIndex);
            ScriptSig.Serialise(writer);
            writer.Write(Sequence);
        }

        public static TxInput Parse(BinaryReader reader)
        {
            var previousTxId = reader.ReadBytes(32).ToBigInteger(ByteArrayFormat.LittleEndianUnsigned);
            var previousTxIndex = reader.ReadUInt32();
            var scriptSig = Script.Parse(reader);
            var sequence = reader.ReadUInt32();

            return new TxInput(previousTxId, previousTxIndex, scriptSig, sequence);
        }
    }
}
