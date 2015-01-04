using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Network.Serialization;

namespace LeagueSharp.Network.Packets
{
    [PacketAttribute(0xC6, typeof(Byte))]
    public class PKT_BuyItemReq : Packet
    {
        [SerializeAttribute(0, 3, new uint[] { 0x6501D62E, 2, 1, 0x87CFCD92, 0xFE0A65A2, 0, unchecked((uint)-1), 0x21BD274B })]
        public int ItemId { get; set; }
    }
}
