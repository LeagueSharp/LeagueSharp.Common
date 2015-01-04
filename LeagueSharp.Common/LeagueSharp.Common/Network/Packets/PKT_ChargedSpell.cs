using System;
using System.Collections.Generic;
using System.IO;
using LeagueSharp.Network.Serialization;
using SharpDX;

namespace LeagueSharp.Network.Packets
{
    public class PKT_ChargedSpell : Packet, ISerialized
    {
        public static short PacketId { get { return 0x103; } }

        private SerializedData<Byte> _spellSlot = new SerializedData<byte>(2, 3, new List<uint>()
        {
            0x41659787,
            0,
            1,
            0xF68409C9,
            0xCB51772A,
            0xCAA8873F,
            2,
            unchecked ((uint)-1)
        });

        private SerializedData<Vector3> _targetPosition = new SerializedData<Vector3>(5, 1, new List<uint>() { 0xF14F0ADF });

        private SerializedData<Boolean> _unknown1 = new SerializedData<Boolean>(0, 1);
        private SerializedData<Boolean> _unknown2 = new SerializedData<Boolean>(1, 1);

        public Byte SpellSlot
        {
            get { return _spellSlot.Data; }
            set { _spellSlot.Data = value; }
        }

        public Vector3 TargetPosition
        {
            get { return _targetPosition.Data; }
            set { _targetPosition.Data = value; }
        }
        public bool Unknown1
        {
            get { return _unknown1.Data; }
            set { _unknown1.Data = value; }
        }
        public bool Unknown2
        {
            get { return _unknown2.Data; }
            set { _unknown2.Data = value; }
        }

        public bool Decode(byte[] data)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(data));

            reader.BaseStream.Position += 2;
            this.NetworkId = reader.ReadInt32();

            UInt16 bitmask = (UInt16) reader.ReadByte();

            _spellSlot.Decode(bitmask, reader);
            _targetPosition.Decode(bitmask, reader);
            _unknown1.Decode(bitmask, reader);
            _unknown2.Decode(bitmask, reader);

            return true;
        }

        public byte[] Encode()
        {
            var ms = new MemoryStream();
            var writer = new BinaryWriter(ms);

            ushort bitmask = 0;

            _spellSlot.Encode(ref bitmask, writer);
            _targetPosition.Encode(ref bitmask, writer);
            _unknown1.Encode(ref bitmask, writer);
            _unknown2.Encode(ref bitmask, writer);

            var packet = new byte[ms.Length + 8];
            BitConverter.GetBytes(PacketId).CopyTo(packet, 0);
            BitConverter.GetBytes(NetworkId).CopyTo(packet, 2);
            BitConverter.GetBytes((byte)bitmask).CopyTo(packet, 6);
            Array.Copy(ms.GetBuffer(), 0, packet, 7, ms.Length);

            return packet;
        }
    }
}
