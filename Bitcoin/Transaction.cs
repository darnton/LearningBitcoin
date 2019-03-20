using System.IO;
using BitcoinMaths;

namespace Bitcoin
{
    public class Transaction
    {
        public uint Version { get; }
        public TxInput[] Inputs { get; }
        public TxOutput[] Outputs { get; }
        public uint Locktime { get; }

        private Transaction(uint version, TxInput[] inputs, TxOutput[] outputs, uint locktime)
        {
            Version = version;
            Inputs = inputs;
            Outputs = outputs;
            Locktime = locktime;
        }

        public string Id
        {
            get
            {
                var stream = new MemoryStream();
                Serialise(new BinaryWriter(stream));
                return stream.ToArray().DoubleSha256().EncodeAsHex();
            }
        }

        public void Serialise(BinaryWriter writer)
        {
            writer.Write(Version);
            writer.WriteVarInt((ulong)Inputs.Length);
            for (int i = 0; i < Inputs.Length; i++)
            {
                Inputs[i].Serialise(writer);
            }
            writer.WriteVarInt((ulong)Outputs.Length);
            for (int j = 0; j < Outputs.Length; j++)
            {
                Outputs[j].Serialise(writer);
            }
            writer.Write(Locktime);
        }

        public static Transaction Parse(BinaryReader reader)
        {
            var version = reader.ReadUInt32();
            var inputCount = reader.ReadVarInt();
            var inputs = new TxInput[inputCount];
            for (ulong i = 0; i < inputCount; i++)
            {
                inputs[i] = TxInput.Parse(reader);
            }
            var outputCount = reader.ReadVarInt();
            var outputs = new TxOutput[outputCount];
            for (ulong j = 0; j < outputCount; j++)
            {
                outputs[j] = TxOutput.Parse(reader);
            }
            var locktime = reader.ReadUInt32();

            return new Transaction(version, inputs, outputs, locktime);
        }
    }
}
