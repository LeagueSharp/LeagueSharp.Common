using System;
using LeagueSharp.Network.Serialization;
using SharpDX;

namespace LeagueSharp.Network.Packets
{
    [PacketAttribute(0xDE, typeof(Int16))]
    public class PKT_NPC_CastSpellReq : Packet
    {
        [SerializeAttribute(6, 1, 0x842A6B66, true)]
        public Vector2 From { get; set; }

        [SerializeAttribute(5, 1, 0xF14F0ADF)]
        public Vector2 To { get; set; }
        
        [SerializeAttribute(7, 1, 0x1BF4047)]
        public int TargetNetworkId { get; set; }

        [SerializeAttribute(2, 3, new uint[]{ 0x41659787, 0, 1, 0xF68409C9, 0xCB51772A, 0xCAA8873F, 2, unchecked ((uint)-1) })]
        public byte SpellSlot { get; set; }

        [SerializeAttribute(0, 1)]
        public bool Unknown1 { get; set; }

        [SerializeAttribute(1, 1)]
        public bool Unknown2 { get; set; }
    }
}
