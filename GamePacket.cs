#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 GamePacket.cs is part of LeagueSharp.Common.
 
 LeagueSharp.Common is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 LeagueSharp.Common is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with LeagueSharp.Common. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

#region

using System;
using System.IO;
using System.Linq;
using System.Text;
using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    ///     This class makes easier to handle packets.
    /// </summary>
    public class GamePacket
    {
        /// <summary>
        /// The _header
        /// </summary>
        private readonly byte _header;

        /// <summary>
        /// The binary reader.
        /// </summary>
        private readonly BinaryReader Br;

        /// <summary>
        /// The binary writer
        /// </summary>
        private readonly BinaryWriter Bw;

        /// <summary>
        /// The memory stream.
        /// </summary>
        private readonly MemoryStream Ms;

        /// <summary>
        /// The raw packet
        /// </summary>
        private readonly byte[] rawPacket;

        /// <summary>
        /// The channel
        /// </summary>
        public PacketChannel Channel = PacketChannel.C2S;

        /// <summary>
        /// The flags
        /// </summary>
        public PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePacket"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public GamePacket(byte[] data)
        {
            Block = false;
            Ms = new MemoryStream(data);
            Br = new BinaryReader(Ms);
            Bw = new BinaryWriter(Ms);

            Br.BaseStream.Position = 0;
            Bw.BaseStream.Position = 0;
            rawPacket = data;
            _header = data[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePacket"/> class.
        /// </summary>
        /// <param name="args">The <see cref="GamePacketEventArgs"/> instance containing the event data.</param>
        public GamePacket(GamePacketEventArgs args)
        {
            Block = false;
            Ms = new MemoryStream(args.PacketData);
            Br = new BinaryReader(Ms);
            Bw = new BinaryWriter(Ms);

            Br.BaseStream.Position = 0;
            Bw.BaseStream.Position = 0;
            rawPacket = args.PacketData;
            _header = args.PacketData[0];
            Channel = args.Channel;
            Flags = args.ProtocolFlag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePacket"/> class.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="flags">The flags.</param>
        public GamePacket(byte header,
            PacketChannel channel = PacketChannel.C2S,
            PacketProtocolFlags flags = PacketProtocolFlags.Reliable)
        {
            Block = false;
            Ms = new MemoryStream();
            Br = new BinaryReader(Ms);
            Bw = new BinaryWriter(Ms);

            Br.BaseStream.Position = 0;
            Bw.BaseStream.Position = 0;
            WriteByte(header);
            _header = header;
            Channel = channel;
            Flags = flags;
        }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public byte Header
        {
            get { return ReadByte(0); } //Better in case header changes, but also resets position.
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public long Position
        {
            get { return Br.BaseStream.Position; }
            set
            {
                if (value >= 0L)
                {
                    Br.BaseStream.Position = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GamePacket"/> is block.
        /// </summary>
        /// <value>
        ///   <c>true</c> if block; otherwise, <c>false</c>.
        /// </value>
        public bool Block { get; set; }

        /// <summary>
        /// Returns the packet size.
        /// </summary>
        /// <returns></returns>
        public long Size()
        {
            return Br.BaseStream.Length;
        }

        /// <summary>
        /// Reads a byte from the packet and increases the position by 1.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public byte ReadByte(long position = -1)
        {
            Position = position;
            return Br.ReadBytes(1)[0];
        }

        /// <summary>
        /// Reads and returns a double byte.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public short ReadShort(long position = -1)
        {
            Position = position;
            return BitConverter.ToInt16(Br.ReadBytes(2), 0);
        }

        /// <summary>
        /// Reads and returns a float.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public float ReadFloat(long position = -1)
        {
            Position = position;
            return BitConverter.ToSingle(Br.ReadBytes(4), 0);
        }

        /// <summary>
        /// Reads and returns an integer.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public int ReadInteger(long position = -1)
        {
            Position = position;
            return BitConverter.ToInt32(Br.ReadBytes(4), 0);
        }

        /// <summary>
        /// Reads and returns a string.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public string ReadString(long position = -1)
        {
            Position = position;
            var sb = new StringBuilder();

            for (var i = Position; i < Size(); i++)
            {
                var num = ReadByte();

                if (num == 0)
                {
                    return sb.ToString();
                }
                sb.Append(Convert.ToChar(num));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Writes a byte.
        /// </summary>
        /// <param name="b">The byte.</param>
        /// <param name="repeat">Specifies how many times to write the packet.</param>
        public void WriteByte(byte b, int repeat = 1)
        {
            for (var i = 0; i < repeat; i++)
            {
                Bw.Write(b);
            }
        }

        /// <summary>
        /// Writes a short.
        /// </summary>
        /// <param name="s">The short.</param>
        public void WriteShort(short s)
        {
            Bw.Write(s);
        }

        /// <summary>
        /// Writes a float.
        /// </summary>
        /// <param name="f">The float.</param>
        public void WriteFloat(float f)
        {
            Bw.Write(f);
        }

        /// <summary>
        /// Writes an integer.
        /// </summary>
        /// <param name="i">The integer.</param>
        public void WriteInteger(int i)
        {
            Bw.Write(i);
        }

        /// <summary>
        /// Writes the hex string as bytes to the packet.
        /// </summary>
        /// <param name="hex">The hexadecimal string.</param>
        public void WriteHexString(string hex)
        {
            hex = hex.Replace(" ", string.Empty);

            if ((hex.Length % 2) != 0)
            {
                hex = "0" + hex;
            }

            Bw.Write(
                Enumerable.Range(0, hex.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                    .ToArray());
        }

        /// <summary>
        /// Writes the string.
        /// </summary>
        /// <param name="str">The string.</param>
        public void WriteString(string str)
        {
            Bw.Write(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// Searches for the byte.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public int[] SearchByte(byte num)
        {
            //return rawPacket.IndexOf(new byte[num]).ToArray();
            return rawPacket.IndexOf(BitConverter.GetBytes(num)).ToArray();
        }

        /// <summary>
        /// Searches for the short.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public int[] SearchShort(short num)
        {
            return rawPacket.IndexOf(BitConverter.GetBytes(num)).ToArray();
        }

        /// <summary>
        /// Searches for the float.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public int[] SearchFloat(float num)
        {
            return rawPacket.IndexOf(BitConverter.GetBytes(num)).ToArray();
        }

        /// <summary>
        /// Searches for the integer.
        /// </summary>
        /// <param name="num">The number.</param>
        /// <returns></returns>
        public int[] SearchInteger(int num)
        {
            return rawPacket.IndexOf(BitConverter.GetBytes(num)).ToArray();
        }

        /// <summary>
        /// Searches for the string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public int[] SearchString(string str)
        {
            return rawPacket.IndexOf(Utils.GetBytes(str)).ToArray();
        }

        /// <summary>
        /// Searches for the hexadecimal string.
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
                rawPacket.IndexOf(
                    Enumerable.Range(0, hex.Length)
                        .Where(x => x % 2 == 0)
                        .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                        .ToArray()).ToArray();
        }

        /// <summary>
        /// Searches for the object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public int[] SearchObject(GameObject obj)
        {
            if (obj == null || !obj.IsValid || obj.NetworkId == 0)
            {
                return null;
            }

            return SearchInteger(obj.NetworkId);
        }

        /// <summary>
        /// Searches  forthe object.
        /// </summary>
        /// <param name="networkId">The network identifier.</param>
        /// <returns></returns>
        public int[] SearchObject(int networkId)
        {
            return networkId == 0 ? null : SearchInteger(networkId);
        }

        /// <summary>
        /// Searches for the position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public int[][] SearchPosition(Vector2 position)
        {
            var x = SearchFloat(position.X);
            var y = SearchFloat(position.Y);

            if (x == null || y == null)
            {
                return null;
            }

            return new[] { x, y };
        }

        /// <summary>
        /// Searches for the position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public int[][] SearchPosition(Vector3 position)
        {
            return SearchPosition(position.To2D());
        }

        /// <summary>
        /// Searches for the position.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public int[][] SearchPosition(GameObject unit)
        {
            return SearchPosition(unit.Position.To2D());
        }

        /// <summary>
        /// Searches for the position.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public int[][] SearchPosition(Obj_AI_Base unit)
        {
            var pos = SearchPosition(unit.Position.To2D());
            var pos2 = SearchPosition(unit.ServerPosition.To2D());

            if (pos == null)
            {
                return pos2;
            }

            return pos2 == null ? pos : null;
        }

        /// <summary>
        /// Searches for the game tile.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public int[][] SearchGameTile(Vector2 position)
        {
            var tile = NavMesh.WorldToGrid(position.X, position.Y);
            var cell = NavMesh.GetCell((short) tile.X, (short) tile.Y);

            var x = SearchShort(cell.GridX);
            var y = SearchShort(cell.GridY);

            return new[] { x, y };
        }

        /// <summary>
        /// Searches for the game tile.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public int[][] SearchGameTile(Vector3 position)
        {
            return SearchGameTile(position.To2D());
        }

        /// <summary>
        /// Searches for the game tile.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public int[][] SearchGameTile(GameObject obj)
        {
            return SearchGameTile(obj.Position.To2D());
        }

        /// <summary>
        /// Gets the raw packet.
        /// </summary>
        /// <returns></returns>
        public byte[] GetRawPacket()
        {
            return Ms.ToArray();
        }

        /// <summary>
        /// Sends the packet
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="flags">The flags.</param>
        public void Send(PacketChannel channel = PacketChannel.C2S,
            PacketProtocolFlags flags = PacketProtocolFlags.Reliable)
        {
            return; //Blocked for now 4.21
            if (!Block)
            {
                Game.SendPacket(
                    Ms.ToArray(), Channel == PacketChannel.C2S ? channel : Channel,
                    Flags == PacketProtocolFlags.Reliable ? flags : Flags);
            }
        }

        /// <summary>
        /// Receives the packet.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public void Process(PacketChannel channel = PacketChannel.S2C)
        {
            return; //Blocked for now 4.21
            if (!Block)
            {
                Game.ProcessPacket(Ms.ToArray(), channel);
            }
        }

        /// <summary>
        /// Dumps the packet.
        /// </summary>
        /// <param name="additionalInfo">if set to <c>true</c> writes additional information.</param>
        /// <returns></returns>
        public string Dump(bool additionalInfo = false)
        {
            var s = string.Concat(Ms.ToArray().Select(b => b.ToString("X2") + " "));
            if (additionalInfo)
            {
                s = "Channel: " + Channel + " Flags: " + Flags + " Data: " + s;
            }
            return s;
        }

        /// <summary>
        /// Saves the packet dump to a file
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void SaveToFile(string filePath)
        {
            var w = File.AppendText(filePath);

            w.WriteLine(Dump(true));
            w.Close();
        }
    }
}