using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BitcoinMaths;

namespace Bitcoin
{
    public class Script
    {
        public List<byte[]> Commands { get; }

        private Script(List<byte[]> commands)
        {
            Commands = commands;
        }

        public static Script operator +(Script a, Script b)
        {
            return new Script(a.Commands.Concat(b.Commands).ToList());
        }

        public void Serialise(BinaryWriter writer)
        {
            var rawBytes = new List<byte>();
            foreach (var command in Commands)
            {
                if (command.Length > 1)
                {
                    if (command.Length < 75)
                    {
                        rawBytes.Add((byte)command.Length);
                    }
                    else if (command.Length < 256)
                    {
                        rawBytes.Add((byte)OpCode.OP_PUSHDATA1);
                        rawBytes.Add((byte)command.Length);
                    }
                    else if (command.Length <= 520)
                    {
                        rawBytes.Add((byte)OpCode.OP_PUSHDATA2);
                        rawBytes.AddRange(BitConverter.GetBytes((short)command.Length));
                    }
                }
                rawBytes.AddRange(command);
            }

            writer.WriteVarInt((ulong)rawBytes.Count);
            writer.Write(rawBytes.ToArray());
        }

        public override string ToString()
        {
            var scriptBuilder = new StringBuilder();
            foreach (var command in Commands)
            {
                string commandText;
                if (command.Length == 1)
                {
                    var opCodeName = Enum.GetName(typeof(OpCode), command[0]);
                    commandText = string.IsNullOrEmpty(opCodeName) ? $"{command[0]:x2}" : opCodeName;
                }
                else
                {
                    commandText = command.EncodeAsHex();
                }
                scriptBuilder.Append($"{commandText} ");
            }

            return scriptBuilder.ToString().TrimEnd();
        }

        public static Script Parse(BinaryReader reader)
        {
            var scriptLength = (int)reader.ReadVarInt();
            var initialPosition = reader.BaseStream.Position;
            if (initialPosition + scriptLength > reader.BaseStream.Length)
            {
                throw new ParsingException($"Script parsing failed. Script length ({scriptLength} bytes) too long.");
            }

            var currentPosition = initialPosition;
            var commands = new List<byte[]>();
            while (currentPosition < initialPosition + scriptLength)
            {
                var currentValue = reader.ReadByte();
                if (currentValue >= 1 && currentValue <= 75)
                {
                    commands.Add(reader.ReadBytes(currentValue));
                }
                else if (currentValue == (byte)OpCode.OP_PUSHDATA1)
                {
                    var dataLength = reader.ReadByte();
                    commands.Add(reader.ReadBytes(dataLength));
                }
                else if (currentValue == (byte)OpCode.OP_PUSHDATA2)
                {
                    var dataLength = reader.ReadInt16();
                    commands.Add(reader.ReadBytes(dataLength));
                }
                else
                {
                    commands.Add(new byte[] { currentValue });
                }

                currentPosition = reader.BaseStream.Position;
            }
            var bytesConsumed = currentPosition - initialPosition;
            if (bytesConsumed != scriptLength)
            {
                throw new ParsingException($"Script parsing failed. {bytesConsumed} bytes consumed. Script length was {scriptLength}.");
            }

            return new Script(commands);
        }
    }
}
