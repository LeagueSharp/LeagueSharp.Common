using System;
using System.Collections.Generic;
using System.IO;
using LeagueSharp.Network.Serialization;
using LeagueSharp.Network.Util;

namespace LeagueSharp.Network.Packets
{
    [PacketAttribute(0xEc, typeof(Int16))]
    public class PKT_NPC_UpgradeSpellReq : Packet
    {
        [SerializeAttribute(7, 3, new uint[] { 1, 0xE9558054, 0x8D8AFC5F, 2, 0, unchecked((uint)-1), 0, 0xFD7AF57D })]
        public int CheatModuleInfo1 { get; set; }

        [SerializeAttribute(0, 3, new uint[] { 0x6501D62E, 2, 1, 0x87CFCD92, 0xFE0A65A2, 0, unchecked((uint)-1), 0x21BD274B })]
        public byte SpellSlot { get; set; }
        
        [SerializeAttribute(10, 3, new uint[] { 0x27AD44AD, 0, unchecked((uint)-1), 0, 0x2C414950, 0x2D7C723B, 2, 1 })]
        public int CheatModuleHash { get; set; }

        [SerializeAttribute(4, 3, new uint[] { 0x7F946157, 0x4C6EE815, 1, 0, 2, unchecked((uint)-1), 0, 0x212E9196 })]
        public int CheatModuleInfo2 { get; set; }

        [SerializeAttribute(3, 1)]
        public bool Evolve { get; set; }
    }
}