using System;
using System.IO;
using System.Numerics;
using BitcoinMaths;

namespace Bitcoin
{
    public class TxInput
    {
        private Lazy<TxOutput> Output { get; }

        public BigInteger PreviousTxId { get; }
        public uint PreviousTxIndex { get; }
        public Script ScriptSig { get; }
        public uint Sequence { get; }

        private TxInput(BigInteger previousTxId, uint previousTxIndex, Script scriptSig, uint sequence, ITxRepo txRepo)
        {
            PreviousTxId = previousTxId;
            PreviousTxIndex = previousTxIndex;
            ScriptSig = scriptSig;
            Sequence = sequence;

            Output = new Lazy<TxOutput>(() =>
            {
                var tx = txRepo.FetchAsync(PreviousTxId.ToByteArray(ByteArrayFormat.BigEndianUnsigned, 32).EncodeAsHex()).GetAwaiter().GetResult();
                return tx.Outputs[PreviousTxIndex];
            });
        }

        public ulong Amount
        {
            get { return Output.Value.Amount; }
        }

        public Script ScriptPubKey
        {
            get { return Output.Value.ScriptPubKey; }
        }

        public void Serialise(BinaryWriter writer)
        {
            writer.Write(PreviousTxId.ToByteArray(ByteArrayFormat.LittleEndianUnsigned, 32));
            writer.Write(PreviousTxIndex);
            ScriptSig.Serialise(writer);
            writer.Write(Sequence);
        }

        public static TxInput Parse(BinaryReader reader, ITxRepo txRepo)
        {
            var previousTxId = reader.ReadBytes(32).ToBigInteger(ByteArrayFormat.LittleEndianUnsigned);
            var previousTxIndex = reader.ReadUInt32();
            var scriptSig = Script.Parse(reader);
            var sequence = reader.ReadUInt32();

            return new TxInput(previousTxId, previousTxIndex, scriptSig, sequence, txRepo);
        }
    }
}
