using System;

namespace LeagueSharp.Network
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketAttribute : Attribute
    {
        public Int16 Id { get; private set; }
        public Type BitmaskType { get; private set; }

        public PacketAttribute(Int16 id, Type bitmaskType)
        {
            this.Id = id;
            this.BitmaskType = bitmaskType;
        }
    }
}
