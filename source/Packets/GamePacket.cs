// <copyright file="GamePacket.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    using SharpDX;

    /// <summary>
    ///     Game packet utils, decoding and encoding of game packets.
    /// </summary>
    public class GamePacket
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePacket" /> class.
        /// </summary>
        /// <param name="data">
        ///     The data.
        /// </param>
        public GamePacket(byte[] data)
        {
            this.Block = false;

            this.MemoryStream = new MemoryStream(data);
            this.BinaryReader = new BinaryReader(this.MemoryStream);
            this.BinaryWriter = new BinaryWriter(this.MemoryStream);

            this.BinaryReader.BaseStream.Position = 0;
            this.BinaryWriter.BaseStream.Position = 0;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePacket" /> class.
        /// </summary>
        /// <param name="gamePacketEventArgs">
        ///     The game packet event args.
        /// </param>
        public GamePacket(GamePacketEventArgs gamePacketEventArgs)
        {
            this.Block = false;

            this.MemoryStream = new MemoryStream(gamePacketEventArgs.PacketData);
            this.BinaryReader = new BinaryReader(this.MemoryStream);
            this.BinaryWriter = new BinaryWriter(this.MemoryStream);

            this.BinaryReader.BaseStream.Position = 0;
            this.BinaryWriter.BaseStream.Position = 0;

            this.Channel = gamePacketEventArgs.Channel;
            this.Flags = gamePacketEventArgs.ProtocolFlag;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePacket" /> class.
        /// </summary>
        /// <param name="header">
        ///     The header.
        /// </param>
        /// <param name="channel">
        ///     The channel.
        /// </param>
        /// <param name="flags">
        ///     The flags.
        /// </param>
        public GamePacket(
            byte header,
            PacketChannel channel = PacketChannel.C2S,
            PacketProtocolFlags flags = PacketProtocolFlags.Reliable)
        {
            this.Block = false;

            this.MemoryStream = new MemoryStream();
            this.BinaryReader = new BinaryReader(this.MemoryStream);
            this.BinaryWriter = new BinaryWriter(this.MemoryStream);

            this.BinaryReader.BaseStream.Position = 0;
            this.BinaryWriter.BaseStream.Position = 0;

            this.WriteByte(header);
            this.Channel = channel;
            this.Flags = flags;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether to block the packet.
        /// </summary>
        public bool Block { get; set; }

        /// <summary>
        ///     Gets or sets the channel.
        /// </summary>
        public PacketChannel Channel { get; set; } = PacketChannel.C2S;

        /// <summary>
        ///     Gets or sets the flags.
        /// </summary>
        public PacketProtocolFlags Flags { get; set; } = PacketProtocolFlags.Reliable;

        /// <summary>
        ///     Gets the header.
        /// </summary>
        public byte Header => this.ReadByte(0);

        /// <summary>
        ///     Gets or sets the position.
        /// </summary>
        public long Position
        {
            get
            {
                return this.BinaryReader.BaseStream.Position;
            }

            set
            {
                if (value >= 0L)
                {
                    this.BinaryReader.BaseStream.Position = value;
                }
            }
        }

        #endregion

        #region Properties

        private BinaryReader BinaryReader { get; }

        private BinaryWriter BinaryWriter { get; }

        private MemoryStream MemoryStream { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Dumps the packet info.
        /// </summary>
        /// <param name="additionalInfo">
        ///     A value indicating whether to dump with additional info.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public string Dump(bool additionalInfo = false)
        {
            var s = string.Concat(this.MemoryStream.ToArray().Select(b => b.ToString("X2") + " "));
            if (additionalInfo)
            {
                s = $"Channel: {this.Channel}, Flags: {this.Flags}, Data: {s}";
            }

            return s;
        }

        /// <summary>
        ///     Gets the raw packet.
        /// </summary>
        /// <returns>
        ///     The <see cref="byte" /> array.
        /// </returns>
        public byte[] GetRawPacket() => this.MemoryStream.ToArray();

        /// <summary>
        ///     Processes the packet.
        /// </summary>
        /// <param name="channel">
        ///     The channel.
        /// </param>
        [Obsolete("Sole purpose for compability, will not actually process the packet.")]
        public void Process(PacketChannel channel = PacketChannel.C2S)
        {
        }

        /// <summary>
        ///     Reads a byte.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="byte" />.
        /// </returns>
        public byte ReadByte(long position = -1)
        {
            this.Position = position;
            return this.BinaryReader.ReadByte();
        }

        /// <summary>
        ///     Reads a float.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="float" />.
        /// </returns>
        public float ReadFloat(long position = -1)
        {
            this.Position = position;
            return BitConverter.ToSingle(this.BinaryReader.ReadBytes(4), 0);
        }

        /// <summary>
        ///     Reads an integer.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        public int ReadInteger(long position = -1)
        {
            this.Position = position;
            return BitConverter.ToInt32(this.BinaryReader.ReadBytes(4), 0);
        }

        /// <summary>
        ///     Reads a short.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="short" />.
        /// </returns>
        public short ReadShort(long position = -1)
        {
            this.Position = position;
            return BitConverter.ToInt16(this.BinaryReader.ReadBytes(2), 0);
        }

        /// <summary>
        ///     Reads a string.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public string ReadString(long position = -1)
        {
            this.Position = position;
            var stringBuilder = new StringBuilder();

            for (var i = this.Position; i < this.Size(); ++i)
            {
                var num = this.ReadByte();
                if (num == 0)
                {
                    return stringBuilder.ToString();
                }

                stringBuilder.Append(Convert.ToChar(num));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Saves the packet information to a file.
        /// </summary>
        /// <param name="filePath">
        ///     The file path.
        /// </param>
        public void SaveToFile(string filePath)
        {
            using (var file = File.AppendText(filePath))
            {
                file.WriteLine(this.Dump(true));
            }
        }

        /// <summary>
        ///     Searches for a byte.
        /// </summary>
        /// <param name="num">
        ///     The number.
        /// </param>
        /// <returns>
        ///     The <see cref="int" /> array.
        /// </returns>
        public int[] SearchByte(byte num) => this.MemoryStream.ToArray().IndexOf(BitConverter.GetBytes(num)).ToArray();

        /// <summary>
        ///     Searches for a float.
        /// </summary>
        /// <param name="num">
        ///     The number.
        /// </param>
        /// <returns>
        ///     The <see cref="int" /> array.
        /// </returns>
        public int[] SearchFloat(float num) => this.MemoryStream.ToArray().IndexOf(BitConverter.GetBytes(num)).ToArray();

        /// <summary>
        ///     Searches for a game tile.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="int" /> 2d array.
        /// </returns>
        public int[][] SearchGameTile(Vector2 position)
        {
            var tile = NavMesh.WorldToGrid(position.X, position.Y);
            var cell = NavMesh.GetCell((short)tile.X, (short)tile.Y);

            var x = this.SearchShort(cell.GridX);
            var y = this.SearchShort(cell.GridY);

            return new[] { x, y };
        }

        /// <summary>
        ///     Searches for a game tile.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="int" /> 2d array.
        /// </returns>
        public int[][] SearchGameTile(Vector3 position) => this.SearchGameTile(position.To2D());

        /// <summary>
        ///     Searches for a game tile.
        /// </summary>
        /// <param name="obj">
        ///     The game object.
        /// </param>
        /// <returns>
        ///     The <see cref="int" /> 2d array.
        /// </returns>
        public int[][] SearchGameTile(GameObject obj) => this.SearchGameTile(obj.Position.To2D());

        /// <summary>
        ///     Searches for a hex string.
        /// </summary>
        /// <param name="hex">
        ///     The hex.
        /// </param>
        /// <returns>
        ///     The <see cref="int" /> array.
        /// </returns>
        public int[] SearchHexString(string hex)
        {
            hex = hex.Replace(" ", string.Empty);

            if ((hex.Length % 2) != 0)
            {
                hex = "0" + hex;
            }

            return
                this.MemoryStream.ToArray()
                    .IndexOf(
                        Enumerable.Range(0, hex.Length)
                            .Where(x => x % 2 == 0)
                            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                            .ToArray())
                    .ToArray();
        }

        /// <summary>
        ///     Searches for an integer.
        /// </summary>
        /// <param name="num">
        ///     The number.
        /// </param>
        /// <returns>
        ///     The <see cref="int" /> array.
        /// </returns>
        public int[] SearchInteger(int num) => this.MemoryStream.ToArray().IndexOf(BitConverter.GetBytes(num)).ToArray();

        /// <summary>
        ///     Searches for a game object.
        /// </summary>
        /// <param name="obj">
        ///     The object.
        /// </param>
        /// <returns>
        ///     The <see cref="int" /> array.
        /// </returns>
        public int[] SearchObject(GameObject obj)
        {
            if (obj == null || !obj.IsValid || obj.NetworkId == 0)
            {
                return null;
            }

            return this.SearchInteger(obj.NetworkId);
        }

        /// <summary>
        ///     Searches for a game object.
        /// </summary>
        /// <param name="networkId">
        ///     The network id.
        /// </param>
        /// <returns>
        ///     The <see cref="int" /> array.
        /// </returns>
        public int[] SearchObject(int networkId) => networkId == 0 ? null : this.SearchInteger(networkId);

        /// <summary>
        ///     Searches for a position.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="int" /> 2d array.
        /// </returns>
        public int[][] SearchPosition(Vector2 position)
        {
            var x = this.SearchFloat(position.X);
            var y = this.SearchFloat(position.Y);

            if (x == null || y == null)
            {
                return null;
            }

            return new[] { x, y };
        }

        /// <summary>
        ///     Searches for a position.
        /// </summary>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <returns>
        ///     The <see cref="int" /> 2d array.
        /// </returns>
        public int[][] SearchPosition(Vector3 position) => this.SearchPosition(position.To2D());

        /// <summary>
        ///     Searches for a position.
        /// </summary>
        /// <param name="obj">
        ///     The object.
        /// </param>
        /// <returns>
        ///     The <see cref="int" /> 2d array.
        /// </returns>
        public int[][] SearchPosition(GameObject obj) => this.SearchPosition(obj.Position.To2D());

        /// <summary>
        ///     Searches for a position.
        /// </summary>
        /// <param name="unit">
        ///     The unit.
        /// </param>
        /// <returns>
        ///     The <see cref="int" /> 2d array.
        /// </returns>
        public int[][] SearchPosition(Obj_AI_Base unit)
        {
            var pos = this.SearchPosition(unit.Position.To2D());
            var pos2 = this.SearchPosition(unit.ServerPosition.To2D());

            if (pos == null)
            {
                return pos2;
            }

            return pos2 == null ? pos : null;
        }

        /// <summary>
        ///     Searches for a short.
        /// </summary>
        /// <param name="num">
        ///     The number.
        /// </param>
        /// <returns>
        ///     The <see cref="int" /> array.
        /// </returns>
        public int[] SearchShort(short num) => this.MemoryStream.ToArray().IndexOf(BitConverter.GetBytes(num)).ToArray();

        /// <summary>
        ///     Searches for a string.
        /// </summary>
        /// <param name="str">
        ///     The string.
        /// </param>
        /// <returns>
        ///     The <see cref="int" /> array.
        /// </returns>
        public int[] SearchString(string str) => this.MemoryStream.ToArray().IndexOf(Utils.GetBytes(str)).ToArray();

        /// <summary>
        ///     Sends the packet.
        /// </summary>
        /// <param name="channel">
        ///     The channel.
        /// </param>
        /// <param name="flags">
        ///     The flags.
        /// </param>
        [Obsolete("Sole purpose for compability, will not actually send the packet.")]
        public void Send(
            PacketChannel channel = PacketChannel.S2C,
            PacketProtocolFlags flags = PacketProtocolFlags.Reliable)
        {
        }

        /// <summary>
        ///     Gets the size of the packet data.
        /// </summary>
        /// <returns>
        ///     The <see cref="long" />.
        /// </returns>
        public long Size() => this.BinaryReader.BaseStream.Length;

        /// <inheritdoc />
        public override string ToString()
        {
            return this.Dump();
        }

        /// <summary>
        ///     Writes a byte.
        /// </summary>
        /// <param name="b">
        ///     The byte.
        /// </param>
        /// <param name="repeat">
        ///     The repeat value (amount).
        /// </param>
        public void WriteByte(byte b, int repeat = 1)
        {
            for (var i = 0; i < repeat; i++)
            {
                this.BinaryWriter.Write(b);
            }
        }

        /// <summary>
        ///     Writes a float.
        /// </summary>
        /// <param name="f">
        ///     The float.
        /// </param>
        public void WriteFloat(float f) => this.BinaryWriter.Write(f);

        /// <summary>
        ///     Writes a hex string.
        /// </summary>
        /// <param name="hex">
        ///     The hex string.
        /// </param>
        public void WriteHexString(string hex)
        {
            hex = hex.Replace(" ", string.Empty);

            if ((hex.Length % 2) != 0)
            {
                hex = "0" + hex;
            }

            this.BinaryWriter.Write(
                Enumerable.Range(0, hex.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                    .ToArray());
        }

        /// <summary>
        ///     Writes an integer.
        /// </summary>
        /// <param name="i">
        ///     The integer.
        /// </param>
        public void WriteInteger(int i) => this.BinaryWriter.Write(i);

        /// <summary>
        ///     Writes a short.
        /// </summary>
        /// <param name="s">
        ///     The short.
        /// </param>
        public void WriteShort(short s) => this.BinaryWriter.Write(s);

        /// <summary>
        ///     Writes a string.
        /// </summary>
        /// <param name="str">
        ///     The string.
        /// </param>
        public void WriteString(string str) => this.BinaryWriter.Write(Encoding.UTF8.GetBytes(str));

        #endregion
    }
}