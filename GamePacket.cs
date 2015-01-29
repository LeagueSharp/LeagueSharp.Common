﻿#region LICENSE

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
        private readonly byte _header;
        private readonly BinaryReader Br;
        private readonly BinaryWriter Bw;
        private readonly MemoryStream Ms;
        private readonly byte[] rawPacket;
        private readonly PacketChannel Channel = PacketChannel.C2S;
        private readonly PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

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

        public byte Header
        {
            get { return ReadByte(0); } //Better in case header changes, but also resets position.
        }

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

        private bool Block { get; set; }

        /// <summary>
        ///     Returns the packet size.
        /// </summary>
        private long Size()
        {
            return Br.BaseStream.Length;
        }

        /// <summary>
        ///     Reads a byte from the packet and increases the position by 1.
        /// </summary>
        public byte ReadByte(long position = -1)
        {
            Position = position;
            return Br.ReadBytes(1)[0];
        }

        /// <summary>
        ///     Reads and returns a double byte.
        /// </summary>
        public short ReadShort(long position = -1)
        {
            Position = position;
            return BitConverter.ToInt16(Br.ReadBytes(2), 0);
        }

        /// <summary>
        ///     Reads and returns a float.
        /// </summary>
        public float ReadFloat(long position = -1)
        {
            Position = position;
            return BitConverter.ToSingle(Br.ReadBytes(4), 0);
        }

        /// <summary>
        ///     Reads and returns an integer.
        /// </summary>
        public int ReadInteger(long position = -1)
        {
            Position = position;
            return BitConverter.ToInt32(Br.ReadBytes(4), 0);
        }

        /// <summary>
        ///     Reads and returns a string.
        /// </summary>
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
        ///     Writes a byte.
        /// </summary>
        public void WriteByte(byte b, int repeat = 1)
        {
            for (var i = 0; i < repeat; i++)
            {
                Bw.Write(b);
            }
        }

        /// <summary>
        ///     Writes a short.
        /// </summary>
        public void WriteShort(short s)
        {
            Bw.Write(s);
        }

        /// <summary>
        ///     Writes a float.
        /// </summary>
        public void WriteFloat(float f)
        {
            Bw.Write(f);
        }

        /// <summary>
        ///     Writes an integer.
        /// </summary>
        public void WriteInteger(int i)
        {
            Bw.Write(i);
        }

        /// <summary>
        ///     Writes the hex string as bytes to the packet.
        /// </summary>
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
        ///     Writes the string.
        /// </summary>
        public void WriteString(string str)
        {
            Bw.Write(Encoding.UTF8.GetBytes(str));
        }

        public int[] SearchByte(byte num)
        {
            //return rawPacket.IndexOf(new byte[num]).ToArray();
            return rawPacket.IndexOf(BitConverter.GetBytes(num)).ToArray();
        }

        private int[] SearchShort(short num)
        {
            return rawPacket.IndexOf(BitConverter.GetBytes(num)).ToArray();
        }

        private int[] SearchFloat(float num)
        {
            return rawPacket.IndexOf(BitConverter.GetBytes(num)).ToArray();
        }

        private int[] SearchInteger(int num)
        {
            return rawPacket.IndexOf(BitConverter.GetBytes(num)).ToArray();
        }

        public int[] SearchString(string str)
        {
            return rawPacket.IndexOf(Utils.GetBytes(str)).ToArray();
        }

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

        public int[] SearchObject(GameObject obj)
        {
            if (obj == null || !obj.IsValid || obj.NetworkId == 0)
            {
                return null;
            }

            return SearchInteger(obj.NetworkId);
        }

        public int[] SearchObject(int networkId)
        {
            return networkId == 0 ? null : SearchInteger(networkId);
        }

        private int[][] SearchPosition(Vector2 position)
        {
            var x = SearchFloat(position.X);
            var y = SearchFloat(position.Y);

            if (x == null || y == null)
            {
                return null;
            }

            return new[] { x, y };
        }

        private int[][] SearchPosition(Vector3 position)
        {
            return SearchPosition(position.To2D());
        }

        private int[][] SearchPosition(GameObject unit)
        {
            return SearchPosition(unit.Position.To2D());
        }

        private int[][] SearchPosition(Obj_AI_Base unit)
        {
            var pos = SearchPosition(unit.Position.To2D());
            var pos2 = SearchPosition(unit.ServerPosition.To2D());

            if (pos == null)
            {
                return pos2;
            }

            return pos2 == null ? pos : null;
        }

        private int[][] SearchGameTile(Vector2 position)
        {
            var tile = NavMesh.WorldToGrid(position.X, position.Y);
            var cell = NavMesh.GetCell((short) tile.X, (short) tile.Y);

            var x = SearchShort(cell.GridX);
            var y = SearchShort(cell.GridY);

            return new[] { x, y };
        }

        private int[][] SearchGameTile(Vector3 position)
        {
            return SearchGameTile(position.To2D());
        }

        private int[][] SearchGameTile(GameObject obj)
        {
            return SearchGameTile(obj.Position.To2D());
        }

        public byte[] GetRawPacket()
        {
            return Ms.ToArray();
        }

        /// <summary>
        ///     Dumps the packet.
        /// </summary>
        private string Dump(bool additionalInfo = false)
        {
            var s = string.Concat(Ms.ToArray().Select(b => b.ToString("X2") + " "));
            if (additionalInfo)
            {
                s = "Channel: " + Channel + " Flags: " + Flags + " Data: " + s;
            }
            return s;
        }

        /// <summary>
        ///     Saves the packet dump to a file
        /// </summary>
        public void SaveToFile(string filePath)
        {
            var w = File.AppendText(filePath);

            w.WriteLine(Dump(true));
            w.Close();
        }
    }
}