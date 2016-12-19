namespace LeagueSharp.Common
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    using SharpDX;

    /// <summary>
    ///     This class makes easier to handle packets.
    /// </summary>
    public class GamePacket
    {
        #region Fields

        /// <summary>
        ///     The channel
        /// </summary>
        public PacketChannel Channel = PacketChannel.C2S;

        /// <summary>
        ///     The flags
        /// </summary>
        public PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

        /// <summary>
        ///     The _header
        /// </summary>
        private readonly byte _header;

        /// <summary>
        ///     The binary reader.
        /// </summary>
        private readonly BinaryReader Br;

        /// <summary>
        ///     The binary writer
        /// </summary>
        private readonly BinaryWriter Bw;

        /// <summary>
        ///     The memory stream.
        /// </summary>
        private readonly MemoryStream Ms;

        /// <summary>
        ///     The raw packet
        /// </summary>
        private readonly byte[] rawPacket;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePacket" /> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public GamePacket(byte[] data)
        {
            this.Block = false;
            this.Ms = new MemoryStream(data);
            this.Br = new BinaryReader(this.Ms);
            this.Bw = new BinaryWriter(this.Ms);

            this.Br.BaseStream.Position = 0;
            this.Bw.BaseStream.Position = 0;
            this.rawPacket = data;
            this._header = data[0];
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePacket" /> class.
        /// </summary>
        /// <param name="args">The <see cref="GamePacketEventArgs" /> instance containing the event data.</param>
        public GamePacket(GamePacketEventArgs args)
        {
            this.Block = false;
            this.Ms = new MemoryStream(args.PacketData);
            this.Br = new BinaryReader(this.Ms);
            this.Bw = new BinaryWriter(this.Ms);

            this.Br.BaseStream.Position = 0;
            this.Bw.BaseStream.Position = 0;
            this.rawPacket = args.PacketData;
            this._header = args.PacketData[0];
            this.Channel = args.Channel;
            this.Flags = args.ProtocolFlag;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePacket" /> class.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="flags">The flags.</param>
        public GamePacket(
            byte header,
            PacketChannel channel = PacketChannel.C2S,
            PacketProtocolFlags flags = PacketProtocolFlags.Reliable)
        {
            this.Block = false;
            this.Ms = new MemoryStream();
            this.Br = new BinaryReader(this.Ms);
            this.Bw = new BinaryWriter(this.Ms);

            this.Br.BaseStream.Position = 0;
            this.Bw.BaseStream.Position = 0;
            this.WriteByte(header);
            this._header = header;
            this.Channel = channel;
            this.Flags = flags;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="GamePacket" /> is block.
        /// </summary>
        /// <value>
        ///     <c>true</c> if block; otherwise, <c>false</c>.
        /// </value>
        public bool Block { get; set; }

        /// <summary>
        ///     Gets the header.
        /// </summary>
        /// <value>
        ///     The header.
        /// </value>
        public byte Header
        {
            get
            {
                return this.ReadByte(0);
            } //Better in case header changes, but also resets position.
        }

        /// <summary>
        ///     Gets or sets the position.
        /// </summary>
        /// <value>
        ///     The position.
        /// </value>
        public long Position
        {
            get
            {
                return this.Br.BaseStream.Position;
            }
            set
            {
                if (value >= 0L)
                {
                    this.Br.BaseStream.Position = value;
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Dumps the packet.
        /// </summary>
        /// <param name="additionalInfo">if set to <c>true</c> writes additional information.</param>
        /// <returns></returns>
        public string Dump(bool additionalInfo = false)
        {
            var s = string.Concat(this.Ms.ToArray().Select(b => b.ToString("X2") + " "));
            if (additionalInfo)
            {
                s = "Channel: " + this.Channel + " Flags: " + this.Flags + " Data: " + s;
            }
            return s;
        }

        /// <summary>
        ///     Gets the raw packet.
        /// </summary>
        /// <returns></returns>
        public byte[] GetRawPacket()
        {
            return this.Ms.ToArray();
        }

        /// <summary>
        ///     Receives the packet.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public void Process(PacketChannel channel = PacketChannel.S2C)
        {
            return; //Blocked for now 4.21
            if (!this.Block)
            {
                Game.ProcessPacket(this.Ms.ToArray(), channel);
            }
        }

        /// <summary>
        ///     Reads a byte from the packet and increases the position by 1.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public byte ReadByte(long position = -1)
        {
            this.Position = position;
            return this.Br.ReadBytes(1)[0];
        }

        /// <summary>
        ///     Reads and returns a float.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public float ReadFloat(long position = -1)
        {
            this.Position = position;
            return BitConverter.ToSingle(this.Br.ReadBytes(4), 0);
        }

        /// <summary>
        ///     Reads and returns an integer.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public int ReadInteger(long position = -1)
        {
            this.Position = position;
            return BitConverter.ToInt32(this.Br.ReadBytes(4), 0);
        }

        /// <summary>
        ///     Reads and returns a double byte.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public short ReadShort(long position = -1)
        {
            this.Position = position;
            return BitConverter.ToInt16(this.Br.ReadBytes(2), 0);
        }

        /// <summary>
        ///     Reads and returns a string.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public string ReadString(long position = -1)
        {
            this.Position = position;
            var sb = new StringBuilder();

            for (var i = this.Position; i < this.Size(); i++)
            {
                var num = this.ReadByte();

                if (num == 0)
                {
                    return sb.ToString();
                }
                sb.Append(Convert.ToChar(num));
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Saves the packet dump to a file
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void SaveToFile(string filePath)
        {
            var w = File.AppendText(filePath);

            w.WriteLine(this.Dump(true));
            w.Close();
        }

        /// <summary>
        ///     Searches for the byte.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public int[] SearchByte(byte num)
        {
            //return rawPacket.IndexOf(new byte[num]).ToArray();
            return this.rawPacket.IndexOf(BitConverter.GetBytes(num)).ToArray();
        }

        /// <summary>
        ///     Searches for the float.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public int[] SearchFloat(float num)
        {
            return this.rawPacket.IndexOf(BitConverter.GetBytes(num)).ToArray();
        }

        /// <summary>
        ///     Searches for the game tile.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public int[][] SearchGameTile(Vector2 position)
        {
            var tile = NavMesh.WorldToGrid(position.X, position.Y);
            var cell = NavMesh.GetCell((short)tile.X, (short)tile.Y);

            var x = this.SearchShort(cell.GridX);
            var y = this.SearchShort(cell.GridY);

            return new[] { x, y };
        }

        /// <summary>
        ///     Searches for the game tile.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public int[][] SearchGameTile(Vector3 position)
        {
            return this.SearchGameTile(position.To2D());
        }

        /// <summary>
        ///     Searches for the game tile.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public int[][] SearchGameTile(GameObject obj)
        {
            return this.SearchGameTile(obj.Position.To2D());
        }

        /// <summary>
        ///     Searches for the hexadecimal string.
        /// </summary>
        /// <param name="hex">The hexadecimal string.</param>
        /// <returns></returns>
        public int[] SearchHexString(string hex)
        {
            hex = hex.Replace(" ", string.Empty);

            if ((hex.Length % 2) != 0)
            {
                hex = "0" + hex;
            }

            return
                this.rawPacket.IndexOf(
                    Enumerable.Range(0, hex.Length)
                        .Where(x => x % 2 == 0)
                        .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                        .ToArray()).ToArray();
        }

        /// <summary>
        ///     Searches for the integer.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public int[] SearchInteger(int num)
        {
            return this.rawPacket.IndexOf(BitConverter.GetBytes(num)).ToArray();
        }

        /// <summary>
        ///     Searches for the object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public int[] SearchObject(GameObject obj)
        {
            if (obj == null || !obj.IsValid || obj.NetworkId == 0)
            {
                return null;
            }

            return this.SearchInteger(obj.NetworkId);
        }

        /// <summary>
        ///     Searches  forthe object.
        /// </summary>
        /// <param name="networkId">The network identifier.</param>
        /// <returns></returns>
        public int[] SearchObject(int networkId)
        {
            return networkId == 0 ? null : this.SearchInteger(networkId);
        }

        /// <summary>
        ///     Searches for the position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
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
        ///     Searches for the position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public int[][] SearchPosition(Vector3 position)
        {
            return this.SearchPosition(position.To2D());
        }

        /// <summary>
        ///     Searches for the position.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public int[][] SearchPosition(GameObject unit)
        {
            return this.SearchPosition(unit.Position.To2D());
        }

        /// <summary>
        ///     Searches for the position.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
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
        ///     Searches for the short.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public int[] SearchShort(short num)
        {
            return this.rawPacket.IndexOf(BitConverter.GetBytes(num)).ToArray();
        }

        /// <summary>
        ///     Searches for the string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public int[] SearchString(string str)
        {
            return this.rawPacket.IndexOf(Utils.GetBytes(str)).ToArray();
        }

        /// <summary>
        ///     Sends the packet
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="flags">The flags.</param>
        public void Send(
            PacketChannel channel = PacketChannel.C2S,
            PacketProtocolFlags flags = PacketProtocolFlags.Reliable)
        {
            return; //Blocked for now 4.21
            if (!this.Block)
            {
                Game.SendPacket(
                    this.Ms.ToArray(),
                    this.Channel == PacketChannel.C2S ? channel : this.Channel,
                    this.Flags == PacketProtocolFlags.Reliable ? flags : this.Flags);
            }
        }

        /// <summary>
        ///     Returns the packet size.
        /// </summary>
        /// <returns></returns>
        public long Size()
        {
            return this.Br.BaseStream.Length;
        }

        /// <summary>
        ///     Writes a byte.
        /// </summary>
        /// <param name="b">The byte.</param>
        /// <param name="repeat">Specifies how many times to write the packet.</param>
        public void WriteByte(byte b, int repeat = 1)
        {
            for (var i = 0; i < repeat; i++)
            {
                this.Bw.Write(b);
            }
        }

        /// <summary>
        ///     Writes a float.
        /// </summary>
        /// <param name="f">The float.</param>
        public void WriteFloat(float f)
        {
            this.Bw.Write(f);
        }

        /// <summary>
        ///     Writes the hex string as bytes to the packet.
        /// </summary>
        /// <param name="hex">The hexadecimal string.</param>
        public void WriteHexString(string hex)
        {
            hex = hex.Replace(" ", string.Empty);

            if ((hex.Length % 2) != 0)
            {
                hex = "0" + hex;
            }

            this.Bw.Write(
                Enumerable.Range(0, hex.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                    .ToArray());
        }

        /// <summary>
        ///     Writes an integer.
        /// </summary>
        /// <param name="i">The integer.</param>
        public void WriteInteger(int i)
        {
            this.Bw.Write(i);
        }

        /// <summary>
        ///     Writes a short.
        /// </summary>
        /// <param name="s">The short.</param>
        public void WriteShort(short s)
        {
            this.Bw.Write(s);
        }

        /// <summary>
        ///     Writes the string.
        /// </summary>
        /// <param name="str">The string.</param>
        public void WriteString(string str)
        {
            this.Bw.Write(Encoding.UTF8.GetBytes(str));
        }

        #endregion
    }
}