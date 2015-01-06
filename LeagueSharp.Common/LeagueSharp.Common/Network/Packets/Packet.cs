using System;
using System.IO;
using System.Linq;
using System.Reflection;
using LeagueSharp.Network.Cryptography;
using LeagueSharp.Network.Serialization;
using LeagueSharp.Network.Util;
using SharpDX;

namespace LeagueSharp.Network.Packets
{
    public abstract class Packet
    {
        private static Random random = new Random();
        public Int32 NetworkId { get; set; }

        public bool Decode(GamePacketEventArgs args)
        {
            return Decode(args.PacketData);
        }

        public bool Decode(byte[] data)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(data));

            reader.ReadInt16(); // skip packet id
            this.NetworkId = reader.ReadInt32();

            ushort bitmask =
                (ushort)
                    ((this.GetType().GetCustomAttribute(typeof (PacketAttribute)) as PacketAttribute).BitmaskType ==
                     typeof (Byte)
                        ? reader.ReadByte()
                        : reader.ReadUInt16());

            foreach (var p in this.GetType().GetProperties())
            {
                SerializeAttribute serializeAttr =
                    p.GetCustomAttribute(typeof (SerializeAttribute)) as SerializeAttribute;

                if (serializeAttr == null)
                    continue;

                object buffer = null;

                if (p.PropertyType == typeof (Int32)
                    || p.PropertyType == typeof (Int16)
                    || p.PropertyType == typeof (Byte))
                {
                    if (serializeAttr.Dict.Length == 8)
                    {
                        var entry =
                            (int) serializeAttr.Dict[bitmask.GetBits(serializeAttr.BitIndex, serializeAttr.Bits)];

                        if (entry < -1 || entry > 7)
                        {
                            Serializer.Decode(out buffer, p.PropertyType, reader, Operations.GetOperations((uint)entry), serializeAttr.ReverseByteOrder);
                        }
                        else
                        {
                            buffer = entry;
                        }
                    }
                    else if (serializeAttr.Dict.Length == 1)
                    {
                        if (bitmask.GetBits(serializeAttr.BitIndex, serializeAttr.Bits) == 0)
                        {
                            Serializer.Decode(out buffer, p.PropertyType, reader,
                                Operations.GetOperations((uint)serializeAttr.Dict[0]), serializeAttr.ReverseByteOrder);
                        }
                        else
                        {
                            buffer = 0;
                        }
                    }
                }
                else if (p.PropertyType == typeof (Vector3))
                {
                    if (bitmask.GetBits(serializeAttr.BitIndex, serializeAttr.Bits) == 0)
                    {
                        Serializer.Decode(out buffer, p.PropertyType, reader,
                            Operations.GetOperations(serializeAttr.Dict[0]), serializeAttr.ReverseByteOrder);
                    }
                }
                else if (p.PropertyType == typeof (Vector2))
                {
                    if (bitmask.GetBits(serializeAttr.BitIndex, serializeAttr.Bits) == 0)
                    {
                        Serializer.Decode(out buffer, p.PropertyType, reader,
                            Operations.GetOperations(serializeAttr.Dict[0]), serializeAttr.ReverseByteOrder);
                    }
                }
                else if (p.PropertyType == typeof (Boolean))
                {
                    buffer = bitmask.GetBits(serializeAttr.BitIndex, serializeAttr.Bits) == 1 ? true : false;
                }

                p.SetValue(this, Convert.ChangeType(buffer, p.PropertyType));
            }

            return true;
        }

        public byte[] Encode()
        {
            PacketAttribute packetAttr = this.GetType().GetCustomAttribute(typeof (PacketAttribute)) as PacketAttribute;

            var ms = new MemoryStream();
            var writer = new BinaryWriter(ms);

            ushort bitmask = 0;

            foreach (var p in this.GetType().GetProperties())
            {
                SerializeAttribute serializeAttr =
                    p.GetCustomAttribute(typeof (SerializeAttribute)) as SerializeAttribute;

                if (serializeAttr == null)
                    continue;

                if (p.PropertyType == typeof (Int32)
                    || p.PropertyType == typeof (Int16)
                    || p.PropertyType == typeof (Byte))
                {
                    var _data = (Int32)(dynamic)p.GetValue(this);

                    if (serializeAttr.Dict.Length == 8)
                    {
                        switch (_data)
                        {
                            case -1:
                            case 0:
                            case 1:
                            case 2:
                                bitmask = bitmask.SetRange(serializeAttr.BitIndex, serializeAttr.Bits, (ushort)serializeAttr.Dict.ToList().IndexOf((uint)_data));
                                break;
                            default:
                                var cryptOperationHashes =
                                    serializeAttr.Dict.Where(x => x > 7 && x != unchecked((uint) -1)).ToList();
                                var cryptOperation = cryptOperationHashes[random.Next()%cryptOperationHashes.Count];
                                bitmask = bitmask.SetRange(serializeAttr.BitIndex, serializeAttr.Bits,
                                    (ushort) serializeAttr.Dict.ToList().IndexOf(cryptOperation));
                                Serializer.Encode(p.GetValue(this), p.PropertyType, writer,
                                    Operations.GetOperations(cryptOperation), serializeAttr.ReverseByteOrder);
                                break;
                        }
                    }
                    else if (serializeAttr.Dict.Length == 1)
                    {
                        if (_data == 0)
                        {
                            bitmask = bitmask.SetRange(serializeAttr.BitIndex, serializeAttr.Bits, 1);
                        }
                        else
                        {
                            bitmask = bitmask.SetRange(serializeAttr.BitIndex, serializeAttr.Bits, 0);
                            Serializer.Encode(p.GetValue(this), p.PropertyType, writer,
                                Operations.GetOperations(serializeAttr.Dict[0]), serializeAttr.ReverseByteOrder);
                        }
                    }
                }
                else if (p.PropertyType == typeof (Vector3))
                {
                    var _data = (Vector3) p.GetValue(this);

                    if (_data.X != 0.0f
                        || _data.Y != 0.0f
                        || _data.Z != 0.0f)
                    {
                        bitmask = bitmask.SetRange(serializeAttr.BitIndex, serializeAttr.Bits, 0);
                        Serializer.Encode(_data, p.PropertyType, writer, Operations.GetOperations(serializeAttr.Dict[0]), serializeAttr.ReverseByteOrder);
                    }
                    else
                    {
                        bitmask = bitmask.SetRange(serializeAttr.BitIndex, serializeAttr.Bits, 1);
                    }
                }
                else if (p.PropertyType == typeof (Vector2))
                {
                    var _data = (Vector2) p.GetValue(this);

                    if (_data.X != 0.0f
                        || _data.Y != 0.0f)
                    {
                        bitmask = bitmask.SetRange(serializeAttr.BitIndex, serializeAttr.Bits, 0);
                        Serializer.Encode(_data, p.PropertyType, writer, Operations.GetOperations(serializeAttr.Dict[0]), serializeAttr.ReverseByteOrder);
                    }
                    else
                    {
                        bitmask = bitmask.SetRange(serializeAttr.BitIndex, serializeAttr.Bits, 1);
                    }
                }
                else if (p.PropertyType == typeof (Boolean))
                {
                    bitmask = bitmask.SetRange(serializeAttr.BitIndex, serializeAttr.Bits,
                        (ushort) ((bool) p.GetValue(this) == true ? 1 : 0));
                }
            }

            var packet = new byte[ms.Length + 8];
            BitConverter.GetBytes(packetAttr.Id).CopyTo(packet, 0);
            BitConverter.GetBytes(NetworkId).CopyTo(packet, 2);
            BitConverter.GetBytes(packetAttr.BitmaskType == typeof(Byte) ? (byte)bitmask : bitmask).CopyTo(packet, 6);
            Array.Copy(ms.GetBuffer(), 0, packet, packetAttr.BitmaskType == typeof(Byte) ? 7 : 8, ms.Length);

            return packet;
        }

        public static short GetPacketId(Type packetType)
        {
            PacketAttribute packetAttr = packetType.GetCustomAttribute(typeof (PacketAttribute)) as PacketAttribute;
            return packetAttr == null ? (short) 0 : packetAttr.Id;
        }

        public static short GetPacketId<T>()
        {
            PacketAttribute packetAttr = typeof (T).GetCustomAttribute(typeof (PacketAttribute)) as PacketAttribute;
            return packetAttr == null ? (short) 0 : packetAttr.Id;
        }

        public static Packet CreatePacket(short packetId)
        {
            foreach (var t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.BaseType == typeof (Packet)
                    && t != typeof (Packet))
                {
                    var packAttr = t.GetCustomAttribute(typeof (PacketAttribute)) as PacketAttribute;

                    if (packAttr != null
                        && packAttr.Id == packetId)
                    {
                        return Activator.CreateInstance(t) as Packet;
                    }
                }
            }

            return null;
        }
    }
}