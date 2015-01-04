using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Network.Serialization;

namespace LeagueSharp.Network.Packets
{
    class PKT_InteractReq : Packet, ISerialized
    {
        public static short PacketId { get { return  0x86; } }
        private SerializedData<Int32> _targetNetworkId = new SerializedData<int>(0, 1, new List<uint>() { 0x988EDF01 });

        public Int32 TargetNetworkId
        {
            get { return _targetNetworkId.Data; }
            set { _targetNetworkId.Data = value; }
        }

        public bool Decode(byte[] data)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(data));

            reader.BaseStream.Position += 2;
            this.NetworkId = reader.ReadInt32();

            UInt16 bitmask = (UInt16)reader.ReadByte();

            _targetNetworkId.Decode(bitmask, reader);

            return true;
        }

        public byte[] Encode()
        {
            var ms = new MemoryStream();
            var writer = new BinaryWriter(ms);

            ushort bitmask = 0;

            _targetNetworkId.Encode(ref bitmask, writer);

            var packet = new byte[ms.Length + 8];
            BitConverter.GetBytes(PacketId).CopyTo(packet, 0);
            BitConverter.GetBytes(NetworkId).CopyTo(packet, 2);
            BitConverter.GetBytes((byte)bitmask).CopyTo(packet, 6);
            Array.Copy(ms.GetBuffer(), 0, packet, 7, ms.Length);

            return packet;
        }
    }
}
