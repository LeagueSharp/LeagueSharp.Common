using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Network.Serialization;

namespace LeagueSharp.Network.Packets
{
    [PacketAttribute(0x55, typeof(Byte))]
    public class PKT_SwapItemReq : Packet
    {
        [SerializeAttribute(3, 3, new uint[] { 0x403741B0, 0x3A068D76, 1, 2, 0, 0x7F946157, unchecked((uint)-1), 0x634FDF6D })]
        public byte From { get; set; }
        [SerializeAttribute(0, 3, new uint[] { 0x6501D62E, 2, 1, 0x87CFCD92, 0xFE0A65A2, 0, unchecked((uint)-1), 0x21BD274B })]
        public byte To { get; set; }
    }
}
