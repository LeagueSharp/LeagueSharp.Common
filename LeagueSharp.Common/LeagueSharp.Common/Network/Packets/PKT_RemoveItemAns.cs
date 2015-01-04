using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Network.Serialization;

namespace LeagueSharp.Network.Packets
{
    class PKT_RemoveItemAns : Packet, ISerialized
    {
        public static short PacketId { get { return 0xD3; } }
        private SerializedData<Byte> _slot = new SerializedData<byte>(0, 3, new List<uint>()
        {
            0x6501D62E,
            2,
            1,
            0x87CFCD92,
            0xFE0A65A2,
            0,
            unchecked ((uint)-1),
            0x21BD274B
        });

        private SerializedData<Byte> _stacks = new SerializedData<byte>(3, 3, new List<uint>()
        {
            0x403741B0,
            0x3A068D76,
            1,
            2,
            0,
            0x7F946157,
            unchecked ((uint)-1),
            0x634FDF6D
        });

        private SerializedData<Boolean> _unknown = new SerializedData<bool>(6, 1);

        public byte Slot
        {
            get { return _slot.Data; }
            set { _slot.Data = value; }
        }

        public byte Stacks
        {
            get { return _stacks.Data; }
            set { _stacks.Data = value; }
        }

        public bool Unknown
        {
            get { return _unknown.Data; }
            set { _unknown.Data = value; }
        }

        public bool Decode(byte[] data)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(data));

            this.NetworkId = reader.ReadInt32();

            UInt16 bitmask = (UInt16)reader.ReadByte();

            _slot.Decode(bitmask, reader);
            _stacks.Decode(bitmask, reader);
            _unknown.Decode(bitmask, reader);

            return true;
        }

        public byte[] Encode()
        {
            var ms = new MemoryStream();
            var writer = new BinaryWriter(ms);

            ushort bitmask = 0;

            _slot.Encode(ref bitmask, writer);
            _stacks.Encode(ref bitmask, writer);
            _unknown.Encode(ref bitmask, writer);

            var packet = new byte[ms.Length + 8];
            BitConverter.GetBytes(PacketId).CopyTo(packet, 0);
            BitConverter.GetBytes(NetworkId).CopyTo(packet, 2);
            BitConverter.GetBytes((byte)bitmask).CopyTo(packet, 6);
            Array.Copy(ms.GetBuffer(), 0, packet, 7, ms.Length);

            return packet;
        }
    }
}
