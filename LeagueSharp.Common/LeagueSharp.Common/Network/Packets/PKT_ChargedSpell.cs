using System;
using System.Collections.Generic;
using System.IO;
using LeagueSharp.Network.Serialization;
using SharpDX;

namespace LeagueSharp.Network.Packets
{
    [PacketAttribute(0x103, typeof(Byte))]
    public class PKT_ChargedSpell : Packet
    {
        [SerializeAttribute(2, 3, new uint[] { 0x41659787, 0, 1, 0xF68409C9, 0xCB51772A, 0xCAA8873F, 2, unchecked ((uint)-1)})]
        public byte SpellSlot { get; set; }
        
        [SerializeAttribute(5, 1, 0xF14F0ADF)]
        public Vector3 TargetPosition { get; set; }

        [SerializeAttribute(0, 1)]
        public bool Unknown1 { get; set; }

        [SerializeAttribute(1, 1)]
        public bool Unknown2 { get; set; }
    }
}
