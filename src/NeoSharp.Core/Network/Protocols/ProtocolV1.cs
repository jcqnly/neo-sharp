﻿using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Extensions;
using NeoSharp.Core.Messaging;

namespace NeoSharp.Core.Network.Protocols
{
    public class ProtocolV1 : ProtocolBase
    {
        #region Properties

        /// <summary>
        /// Protocol version
        /// </summary>
        public override uint Version => 1;

        #endregion

        #region Variables

        private readonly IBinaryConverter _serializer;
        private readonly uint _magic;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Configuration</param>
        /// <param name="serializer">Serializer</param>
        public ProtocolV1(NetworkConfig config, IBinaryConverter serializer)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            _magic = config.Magic;
        }

        public override async Task SendMessageAsync(Stream stream, Message message,
            CancellationToken cancellationToken)
        {
            using (var memory = new MemoryStream())
            using (var writer = new BinaryWriter(memory, Encoding.UTF8))
            {
                writer.Write(_magic);
                writer.Write(Encoding.UTF8.GetBytes(message.Command.ToString().PadRight(12, '\0')));

                var payloadBuffer = message is ICarryPayload messageWithPayload
                    ? _serializer.Serialize(messageWithPayload.Payload)
                    : new byte[0];

                writer.Write((uint)payloadBuffer.Length);
                writer.Write(CalculateChecksum(payloadBuffer));
                writer.Write(payloadBuffer);
                writer.Flush();

                var buffer = memory.ToArray();
                await stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
            }
        }

        public override async Task<Message> ReceiveMessageAsync(Stream stream, CancellationToken cancellationToken)
        {
            var buffer = await FillBufferAsync(stream, 24, cancellationToken);

            using (var memory = new MemoryStream(buffer, false))
            using (var reader = new BinaryReader(memory, Encoding.UTF8))
            {
                if (reader.ReadUInt32() != _magic)
                    throw new FormatException();

                var command = Enum.Parse<MessageCommand>(Encoding.UTF8.GetString(reader.ReadBytes(12)).TrimEnd('\0'));

                if (!Cache.TryGetValue(command, out var type))
                    throw (new ArgumentException("command"));

                var message = (Message)Activator.CreateInstance(type);
                message.Command = command;

                var payloadLength = reader.ReadUInt32();
                if (payloadLength > Message.PayloadMaxSize)
                    throw new FormatException();

                var checksum = reader.ReadUInt32();

                var payloadBuffer = payloadLength > 0
                    ? await FillBufferAsync(stream, (int)payloadLength, cancellationToken)
                    : new byte[0];

                if (CalculateChecksum(payloadBuffer) != checksum)
                    throw new FormatException();

                if (message is ICarryPayload messageWithPayload)
                {
                    if (payloadLength == 0)
                        throw new FormatException();

                    // TODO: Prevent create the dummy object

                    messageWithPayload.Payload = _serializer.Deserialize(payloadBuffer,messageWithPayload.Payload.GetType());
                }

                return message;
            }
        }

        /// <summary>
        /// Calculate message checksum
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Checksum</returns>
        private static uint CalculateChecksum(byte[] value)
        {
            return value.Sha256(0, value.Length).Sha256(0, 32).ToUInt32(0);
        }
    }
}