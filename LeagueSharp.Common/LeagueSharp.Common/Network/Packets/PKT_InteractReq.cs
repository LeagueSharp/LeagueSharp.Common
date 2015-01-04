using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Network.Serialization;

namespace LeagueSharp.Network.Packets
{
    [PacketAttribute(0x86, typeof(Byte))]
    public class PKT_InteractReq : Packet
    {
        [SerializeAttribute(0, 1, 0x988EDF01)]
        public int TargetNetworkId { get; set; }
    }
}
