#region LICENSE
/*
 Copyright 2014 - 2014 LeagueSharp
 Orbwalking.cs is part of LeagueSharp.Common.
 
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

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    /// This class makes easier to handle packets.
    /// </summary>
    public class GamePacket
    {
        private readonly BinaryReader Br;
        private readonly BinaryWriter Bw;
        private readonly MemoryStream Ms;
        private readonly byte[] rawPacket;

        public GamePacket(byte[] data)
        {
            Ms = new MemoryStream(data);
            Br = new BinaryReader(Ms);
            Bw = new BinaryWriter(Ms);
            Br.BaseStream.Position = 0;
            Bw.BaseStream.Position = 0;
            rawPacket = data;
        }

        public GamePacket(byte header)
        {
            Ms = new MemoryStream();
            Br = new BinaryReader(Ms);
            Bw = new BinaryWriter(Ms);
            Br.BaseStream.Position = 0;
            Bw.BaseStream.Position = 0;
            WriteByte(header);
        }

        public long Position
        {
            get { return Br.BaseStream.Position; }
            set { Br.BaseStream.Position = value; }
        }

        /// <summary>
        /// Returns the packet size.
        /// </summary>
        public long Size()
        {
            return Br.BaseStream.Length;
        }

        /// <summary>
        /// Reads a byte from the packet and increases the position by 1.
        /// </summary>
        public byte ReadByte()
        {
            return Br.ReadBytes(1)[0];
        }

        /// <summary>
        /// Reads and returns a double byte.
        /// </summary>
        public short ReadShort()
        {
            return BitConverter.ToInt16(Br.ReadBytes(2), 0);
        }

        /// <summary>
        /// Reads and returns a float.
        /// </summary>
        public float ReadFloat()
        {
            return BitConverter.ToSingle(Br.ReadBytes(4), 0);
        }

        /// <summary>
        /// Reads and returns an integer.
        /// </summary>
        public int ReadInteger()
        {
            return BitConverter.ToInt32(Br.ReadBytes(4), 0);
        }


        /// <summary>
        /// Writes a byte.
        /// </summary>
        public void WriteByte(byte b, int repeat = 1)
        {
            for (int i = 0; i < repeat; i++)
            {
                Bw.Write(b);
            }
        }

        /// <summary>
        /// Writes a short.
        /// </summary>
        public void WriteShort(short s)
        {
            Bw.Write(s);
        }

        /// <summary>
        /// Writes a float.
        /// </summary>
        public void WriteFloat(float f)
        {
            Bw.Write(f);
        }

        /// <summary>
        /// Writes an integer.
        /// </summary>
        public void WriteInteger(int i)
        {
            Bw.Write(i);
        }

        /// <summary>
        /// Writes the hex string as bytes to the packet.
        /// </summary>
        public void WriteHexString(string hex)
        {
            Bw.Write(
                Enumerable.Range(0, hex.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                    .ToArray());
        }

        /// <summary>
        /// Writes the string.
        /// </summary>
        public void WriteString(string str)
        {
            Bw.Write(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// Sends the packet
        /// </summary>
        public void Send(PacketChannel channel = PacketChannel.C2S,
            PacketProtocolFlags flags = PacketProtocolFlags.Reliable)
        {
            Game.SendPacket(Ms.ToArray(), channel, flags);
        }

        /// <summary>
        /// Receives the packet.
        /// </summary>
        public void Process(PacketChannel channel = PacketChannel.S2C)
        {
            Game.ProcessPacket(Ms.ToArray(), channel);
        }

        /// <summary>
        /// Dumps the packet.
        /// </summary>
        public string Dump()
        {
            var result = new StringBuilder(rawPacket.Length * 3);
            foreach (var b in rawPacket)
            {
                result.AppendFormat("{0:X2} ", b);
            }
            return result.ToString();
        }

        /// <summary>
        /// Saves the packet dump to a file
        /// </summary>
        public void SaveToFile(string filePath = "E:\\PacketLog.txt")
        {
            var w = File.AppendText(filePath);
            w.WriteLine(Dump());
            w.Close();
        }
    }
}