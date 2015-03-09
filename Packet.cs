#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Packet.cs is part of LeagueSharp.Common.
 
 LeagueSharp.Common is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 LeagueSharp.Common is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with LeagueSharp.Common. If not, see <http://www.gnu.org/licenses/>.
*/

/*
 Hi there BoL devs - looking for an update again? :^ ) 
*/

#endregion

#region

using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;

#endregion

namespace LeagueSharp.Common
{

    [Obsolete("Use Network.Packets", false)]
    public static class Packet
    {
        static Packet()
        {
            Console.WriteLine(@"LeagueSharp.Common.Packet will be removed soon, use LeagueSharp.Network.Packets instead");
        }

        public enum ActionStates
        {
            BeginRecall = 111207118,
            FinishRecall = 97690254,
        }

        public enum AttackTypePacket
        {
            Circular = 0,
            ConeSkillShot = 1, // Cone and Skillshot spells
            TargetedAA = 2, // Targeted spells and AAs
        }

        public enum DamageTypePacket
        {
            Magical = 4,
            CriticalAttack = 11,
            Physical = 12,
            True = 36,
        }

        public enum Emotes
        {
            Dance = 0x00,
            Joke = 0x03,
            Taunt = 0x01,
            Laugh = 0x02,
        }

        public enum FloatTextPacket
        {
            Invulnerable,
            Special,
            Heal,
            ManaHeal,
            ManaDmg,
            Dodge,
            Critical,
            Experience,
            Gold,
            Level,
            Disable,
            QuestRecv,
            QuestDone,
            Score,
            PhysDmg,
            MagicDmg,
            TrueDmg,
            EnemyPhysDmg,
            EnemyMagicDmg,
            EnemyTrueDmg,
            EnemyCritical,
            Countdown,
            Legacy,
            LegacyCritical,
            Debug
        }

        public enum MultiPacketType
        {
            /* Confirmed in IDA */
            Unknown100 = 0x00,
            Unknown101 = 0x01,
            Unknown102 = 0x02,

            Unknown115 = 0x15,
            Unknown116 = 0x16,
            Unknown124 = 0x24,
            Unknown11A = 0x1A,
            Unknown11E = 0x1E, // currently empty

            /* These others could be packets with a handler */
            Unknown104 = 0x04, //confirmed in game/ida, related to spellslots
            Unknown118 = 0x08, //sion ult
            Unknown120 = 0x20, // confirmed in game
            SpawnTurret = 0x23, // confirmed in ida

            /* New List: Confirmed in Game */
            //FE 19 00 00 40 07 01 00 01 00 00 00 02 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00
            InitSpell = 0x07, //also stack count for stackables, teemo shroom, akali, etc? 
            Unknown10C = 0x0C, //this packet is like 0x127

            Unknown122 = 0x22,
            Unknown125 = 0x25, // sion ult, other stuff
            //FE 05 00 00 40 25 01 03 EC 06 00 00 00 01 <== sion
//            FE 19 00 00 40 25 01 00 00 07 00 00 00 06 FB 16 00 40 56 04 00 40 B2 04 00 40 B2 04 00 40 56 05 00 40 FB 16 00 40
//FE 19 00 00 40 25 01 00 00 07 00 00 00 06 FB 16 00 40 56 04 00 40 B2 04 00 40 B2 04 00 40 56 05 00 40 FB 16 00 40
//FE 19 00 00 40 25 01 00 00 07 00 00 00 06 FB 16 00 40 56 04 00 40 B2 04 00 40 B2 04 00 40 56 05 00 40 FB 16 00 40
//FE 19 00 00 40 25 01 00 00 07 00 00 00 06 FB 16 00 40 56 04 00 40 B2 04 00 40 B2 04 00 40 56 05 00 40 FB 16 00 40
//FE 19 00 00 40 25 01 00 00 07 00 00 00 06 56 04 00 40 B2 04 00 40 B2 04 00 40 56 05 00 40 56 05 00 40 FB 16 00 40
            NPCDeath = 0x26, //confirmed in ida, struct from intwars/ida
            Unknown129 = 0x29, //related to spells (kalista ally unit), add?
            Unknown12A = 0x2A, //related to spells (kalist ally unit after 0x129), maybe delete?
            //FE 06 00 00 40 2A 01 3C 00 00 00
            Unknown12C = 0x2C,
            //FE 00 00 00 00 2C 01 81 00 00 00 00 FF FF FF FF 
//FE 00 00 00 00 2C 01 80 00 00 00 00 FF FF FF FF 
            Unknown12E = 0x2E, //confirmed in ida
            Unknown12F = 0x2F, //FE 05 00 00 40 2F 01 00


            AddBuff = 0x09, // buff added by towers in new SR
            UndoToken = 0x0B,
            ObjectCreation = 0x0D, // azir ult
            SurrenderState = 0x0E,
            OnAttack = 0x0F,
            DeathTimer = 0x17,
            ChangeItem = 0x1C, //like hpp=>biscuit
            ActionState = 0x21, // ?? triggers on recall
            UndoConfirm = 0x27,
            LockCamera = 0x2B, // Sion Ult

            Unknown = 0xFF, // Default, not real packet
        }

        public enum PingType
        {
            Normal = 0,
            Fallback = 5,
            EnemyMissing = 3,
            Danger = 2,
            OnMyWay = 4,
            AssistMe = 6,
        }

        public static class C2S
        {
            #region Ping - 4.21

            /// <summary>
            ///     Ping Packet. Sent by the client when pings are sent.
            /// </summary>
            public static class Ping
            {
                public static byte Header = 0x1D;
                public static PacketChannel Channel = PacketChannel.C2S;
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;
             
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteByte(1);
                    result.WriteInteger(0);
                    result.WriteInteger(packetStruct.TargetNetworkId);
                    result.WriteFloat(packetStruct.X);
                    result.WriteFloat(packetStruct.Y);
                    result.WriteByte((byte) packetStruct.Type);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 10 };
                    return new Struct(
                        packet.ReadFloat(), packet.ReadFloat(), packet.ReadInteger(), (PingType) packet.ReadByte());
                }

                public struct Struct
                {
                    public int TargetNetworkId;
                    public PingType Type;
                    public float X;
                    public float Y;

                    public Struct(float x = 0f, float y = 0f, int targetNetworkId = 0, PingType type = PingType.Normal)
                    {
                        X = x;
                        Y = y;
                        TargetNetworkId = targetNetworkId;
                        Type = type;
                    }
                }
            }

            #endregion

            #region LevelUpSpell - 4.21

            /// <summary>
            ///     Packet sent when leveling up a spell.
            /// </summary>
            public static class LevelUpSpell
            {
                public static byte Header = 0xEC;
                public static PacketChannel Channel = PacketChannel.C2S;
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteInteger(0);
                    result.WriteByte((byte) packetStruct.Slot);
                    var bit = packetStruct.Evolution ? (byte) 0x01 : (byte) 0x0;
                    result.WriteByte(bit);
                    result.WriteInteger(0);
                    result.WriteInteger(0);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 2 };
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger();
                    packet.Position += 4;
                    result.Slot = (SpellSlot) packet.ReadByte();
                    result.Evolution = packet.ReadByte() == 0x01 ? true : false;

                    return new Struct(packet.ReadInteger(), (SpellSlot) packet.ReadByte());
                }

                public struct Struct
                {
                    public bool Evolution;
                    public int NetworkId;
                    public SpellSlot Slot;

                    public Struct(int networkId = -1, SpellSlot slot = SpellSlot.Q, bool evolve = false)
                    {
                        NetworkId = (networkId == -1) ? ObjectManager.Player.NetworkId : networkId;
                        Slot = slot;
                        Evolution = evolve;
                    }
                }
            }

            #endregion

            #region Move

            /// <summary>
            ///     Packet sent when issuing GameObjectOrder's.
            /// </summary>
            public static class Move
            {
                public static byte Header = 0x13;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.SourceNetworkId);

                    //random data :S
                    result.WriteInteger(0);
                    result.WriteInteger(0);
                    result.WriteInteger(0);
                    result.WriteInteger(0);

                    result.WriteFloat(packetStruct.X);
                    result.WriteFloat(packetStruct.Y);
                    result.WriteByte(packetStruct.MoveType);
                    result.WriteInteger(packetStruct.TargetNetworkId);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 2;
                    result.SourceNetworkId = packet.ReadInteger();
                    result.UnitNetworkId = result.SourceNetworkId;

                    packet.ReadInteger();
                    packet.ReadInteger();
                    packet.ReadInteger();
                    packet.ReadInteger();

                    result.X = packet.ReadFloat();
                    result.Y = packet.ReadFloat();
                    result.MoveType = packet.ReadByte();
                    result.TargetNetworkId = packet.ReadInteger();
                    return result;
                }

                public struct Struct
                {
                    public byte MoveType;
                    public int SourceNetworkId;
                    public int TargetNetworkId;
                    public int UnitNetworkId;
                    public int WaypointCount;
                    public float X;
                    public float Y;

                    public Struct(float x = 0f,
                        float y = 0f,
                        byte moveType = 2,
                        int targetNetworkId = 0,
                        int unitNetworkId = -1,
                        int sourceNetworkId = -1)
                    {
                        SourceNetworkId = (sourceNetworkId == -1) ? ObjectManager.Player.NetworkId : sourceNetworkId;
                        MoveType = moveType;
                        X = x;
                        Y = y;
                        TargetNetworkId = targetNetworkId;
                        UnitNetworkId = (unitNetworkId == -1) ? ObjectManager.Player.NetworkId : unitNetworkId;
                        WaypointCount = 1;
                    }
                }
            }

            #endregion

            #region Cast - 4.21

            /// <summary>
            ///     Packet sent when casting spells.
            /// </summary>
            public static class Cast
            {
                public static byte Header = 0xDE;
                public static PacketChannel Channel = PacketChannel.C2S;
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.SourceNetworkId);
                    result.WriteFloat(packetStruct.FromX);
                    result.WriteFloat(packetStruct.FromY);
                    result.WriteFloat(packetStruct.ToX);
                    result.WriteFloat(packetStruct.ToY);
                    result.WriteInteger(packetStruct.TargetNetworkId);
                    result.WriteByte((byte)packetStruct.Slot);
                    result.WriteByte(0); //packetStruct.SpellFlag == 0xFF ? GetSpellByte(packetStruct.Slot) : packetStruct.SpellFlag
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 2;
                    result.SourceNetworkId = packet.ReadInteger();
                    result.FromX = packet.ReadFloat();
                    result.FromY = packet.ReadFloat();
                    result.ToX = packet.ReadFloat();
                    result.ToY = packet.ReadFloat();
                    result.Slot = (SpellSlot)packet.ReadByte();
                    result.SpellFlag = packet.ReadByte();
                    return result;
                }

                private static byte GetSpellByte(SpellSlot spell)
                {
                    switch (spell)
                    {
                        case SpellSlot.Q:
                            return 0xE8;
                        case SpellSlot.W:
                            return 0xE8;
                        case SpellSlot.E:
                            return 0xE8;
                        case SpellSlot.R:
                            return 0xE8;
                        case SpellSlot.Item1:
                            return 0;
                        case SpellSlot.Item2:
                            return 0;
                        case SpellSlot.Item3:
                            return 0;
                        case SpellSlot.Item4:
                            return 0;
                        case SpellSlot.Item5:
                            return 0;
                        case SpellSlot.Item6:
                            return 0;
                        case SpellSlot.Trinket:
                            return 0;
                        case SpellSlot.Recall:
                            return 0;
                        case SpellSlot.Unknown:
                            return 0;
                        default:
                            return 0;
                    }
                }

                public struct Struct
                {
                    public float FromX;
                    public float FromY;
                    public SpellSlot Slot;
                    public int SourceNetworkId;
                    public byte SpellFlag;
                    public int TargetNetworkId;
                    public float ToX;
                    public float ToY;

                    public Struct(int targetNetworkId = 0,
                        SpellSlot slot = SpellSlot.Q,
                        int sourceNetworkId = -1,
                        float fromX = 0f,
                        float fromY = 0f,
                        float toX = 0f,
                        float toY = 0f,
                        byte spellFlag = 0xFF)
                    {
                        SourceNetworkId = (sourceNetworkId == -1) ? ObjectManager.Player.NetworkId : sourceNetworkId;
                        Slot = slot;
                        FromX = fromX;
                        FromY = fromY;
                        ToX = toX;
                        ToY = toY;
                        TargetNetworkId = targetNetworkId;
                        SpellFlag = spellFlag;
                    }
                }
            }

            #endregion

            #region ChargedCast - 4.21

            /// <summary>
            ///     Packet sent when casting charged spells second cast.
            /// </summary>

            public static class ChargedCast
            {
                public static byte Header = 0x03;
                public static PacketChannel Channel = PacketChannel.C2S;
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;
             
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteByte(1);
                    result.WriteInteger(packetStruct.SourceNetworkId);
                    result.WriteByte((byte) packetStruct.Slot);
                    result.WriteFloat(packetStruct.ToX);
                    result.WriteFloat(packetStruct.ToY);
                    result.WriteFloat(packetStruct.ToZ);
                    result.WriteByte(2);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 2;
                    result.SourceNetworkId = packet.ReadInteger();
                    result.Slot = (SpellSlot) packet.ReadByte();
                    result.ToX = packet.ReadFloat();
                    result.ToY = packet.ReadFloat();
                    result.ToZ = packet.ReadFloat();
                    return result;
                }

                public struct Struct
                {
                    public SpellSlot Slot;
                    public int SourceNetworkId;
                    public float ToX;
                    public float ToY;
                    public float ToZ;

                    public Struct(SpellSlot slot,
                        float toX = 0f,
                        float toY = 0f,
                        float toZ = 0f,
                        int sourceNetworkId = -1)
                    {
                        SourceNetworkId = (sourceNetworkId == -1) ? ObjectManager.Player.NetworkId : sourceNetworkId;
                        Slot = slot;
                        ToX = toX;
                        ToY = toY;
                        ToZ = toZ;
                    }
                }
            }

            #endregion

            #region BuyItem - 4.21

            /// <summary>
            ///     Packet sent when buying items.
            /// </summary>
            public static class BuyItem
            {
                public static byte Header = 0xC6;
                public static PacketChannel Channel = PacketChannel.C2S;
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteInteger(packetStruct.ItemId);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 2;
                    result.NetworkId = packet.ReadInteger();
                    result.ItemId = packet.ReadInteger();
                    return result;
                }

                public struct Struct
                {
                    public int ItemId;
                    public int NetworkId;

                    public Struct(int itemId, int networkId = -1)
                    {
                        ItemId = itemId;
                        NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }
                }
            }

            #endregion

            #region SellItem - 4.21

            /// <summary>
            ///     Packet sent when selling items.
            /// </summary>
            public static class SellItem
            {
                public static byte Header = 0x72;
                public static PacketChannel Channel = PacketChannel.C2S;
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteByte(packetStruct.InventorySlot);
                    result.WriteByte(1);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 2 };
                    var networkId = packet.ReadInteger();
                    var slot = packet.ReadByte();

                    return new Struct(slot, networkId);
                }

                public struct Struct
                {
                    public byte InventorySlot;
                    public int NetworkId;
                    public SpellSlot SpellSlot;

                    public Struct(byte slot, int networkId = -1)
                    {
                        InventorySlot = slot;
                        SpellSlot = (SpellSlot) (InventorySlot + (byte) SpellSlot.Item1);
                        NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }


                    public Struct(SpellSlot slot, int networkId = -1)
                    {
                        SpellSlot = slot;
                        InventorySlot = (byte) ((byte) SpellSlot - (byte) SpellSlot.Item1);
                        NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }
                }
            }

            #endregion

            #region SwapItem - 4.21

            /// <summary>
            ///     Packet sent when swapping items.
            /// </summary>
            public static class SwapItem
            {
                public static byte Header = 0x55;
                public static PacketChannel Channel = PacketChannel.C2S;
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteByte(packetStruct.FromSlotByte);
                    result.WriteByte(packetStruct.ToSlotByte);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 2;
                    result.NetworkId = packet.ReadInteger();
                    result.FromSlotByte = packet.ReadByte();
                    result.ToSlotByte = packet.ReadByte();
                    return result;
                }

                public struct Struct
                {
                    public byte FromSlotByte;
                    public int NetworkId;
                    public byte ToSlotByte;

                    public Struct(byte fromSlotByte, byte toSlotByte, int networkId = -1)
                    {
                        FromSlotByte = fromSlotByte;
                        ToSlotByte = toSlotByte;
                        NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }
                }
            }

            #endregion

            #region Emote - 4.21

            /// <summary>
            ///     Packet sent when sending emotes.
            /// </summary>
            public static class Emote
            {
                public static byte Header = 0x14;
                public static PacketChannel Channel = PacketChannel.C2S;
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteByte(1);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteByte(packetStruct.EmoteId);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 2;
                    result.NetworkId = packet.ReadInteger();
                    result.EmoteId = packet.ReadByte();
                    return result;
                }

                public struct Struct
                {
                    public byte EmoteId;
                    public int NetworkId;

                    public Struct(byte emoteId, int networkId = -1)
                    {
                        EmoteId = emoteId;
                        NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }
                }
            }

            #endregion

            #region InteractObject - 4.21

            /// <summary>
            ///     Packet sent when interacting with Thresh Lantern and Dominion capturing.
            /// </summary>
            public static class InteractObject
            {
                public static byte Header = 0x86;
                public static PacketChannel Channel = PacketChannel.C2S;
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteInteger(packetStruct.SourceNetworkId);
                    result.WriteInteger(packetStruct.ObjectNetworkId);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 1;
                    result.SourceNetworkId = packet.ReadInteger();
                    result.ObjectNetworkId = packet.ReadInteger();
                    return result;
                }

                public struct Struct
                {
                    public int ObjectNetworkId;
                    public int SourceNetworkId;

                    public Struct(int networkId, int objectNetworkId)
                    {
                        SourceNetworkId = networkId;
                        ObjectNetworkId = objectNetworkId;
                    }
                }
            }

            #endregion

            #region SetTarget - 4.21

            /// <summary>
            ///     Packet sent when left clicking a target.
            /// </summary>
            public static class SetTarget
            {
                public static byte Header = 0x04;
                public static PacketChannel Channel = PacketChannel.C2S;
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                public static Struct Decoded(byte[] data)
                {
                    var result = new Struct { NetworkId = new GamePacket(data).ReadInteger(6) };
                    result.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.NetworkId);
                    return result;
                }

                public struct Struct
                {
                    public int NetworkId;
                    public Obj_AI_Base Unit;
                }
            }

            #endregion

            #region HeartBeat - 4.21

            /// <summary>
            ///     Packet sent frequently as heartbeat to servers.
            /// Related to 0x29 (Recv)
            /// </summary>
            public static class HeartBeat
            {
                public static byte Header = 0x4C;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct { RecvTime = packet.ReadFloat(1), AckTime = packet.ReadFloat(5) };
                    return result;
                }

                public struct Struct
                {
                    public float AckTime;
                    public float RecvTime;
                }
            }

            #endregion

            #region UpdateConfirm

            /// <summary>
            ///     Packet sent to acknowledge received update packet.
            /// </summary>
            public static class UpdateConfirm
            {
                public static byte Header = 0xA8;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct { SequenceId = packet.ReadInteger(5) };
                    return result;
                }

                public struct Struct
                {
                    public int SequenceId;
                }
            }

            #endregion

            #region Refund - 4.21

            /// <summary>
            ///     Sent by client on refund.
            /// </summary>
            public static class Refund
            {
                public static byte Header = 0x54;
                public static PacketChannel Channel = PacketChannel.C2S;
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.NetworkId);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 2;
                    result.NetworkId = packet.ReadInteger();
                    return result;
                }

                public struct Struct
                {
                    public int NetworkId;

                    public Struct(int networkId = -1)
                    {
                        NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }
                }
            }

            #endregion

            #region ScoreScreen - 4.21 (NO STRUCT)

            /// <summary>
            ///     Sent by client on opening score screen.
            /// </summary>
            public static class ScoreScreen
            {
                public static byte Header = 0x15;
            }

            #endregion

            #region Camera - 4.21 (NO STRUCT)

            /// <summary>
            ///     Sent by client on center/lock camera.
            /// </summary>
            public static class Camera
            {
                public static byte Header = 0x64;
            }

            #endregion

            #region Zoom - 4.21 (NO STRUCT)

            /// <summary>
            ///     Sent by client on zoom level change or move camera.
            /// </summary>
            public static class Zoom
            {
                public static byte Header = 0xDC;
            }

            #endregion

            #region LeaveGame - 4.21 (NO STRUCT)

            /// <summary>
            ///     Sent by client on exiting the game.
            /// </summary>
            public static class LeaveGame
            {
                //75 00 00 00 00 00 5A
                public static byte Header = 0x75;
            }

            #endregion

            #region Surrender - 4.21 (NO STRUCT)

            /// <summary>
            ///     Sent by client on surrendering.
            /// </summary>
            public static class Surrender
            {
                //A2 00 00 00 00 00 AB
                public static byte Header = 0xA2;
            }

            #endregion

            #region EndGame - 4.21 (NO STRUCT)

            /// <summary>
            ///     Sent by client when the game is over.
            /// </summary>
            public static class EndGame
            {
                //1E 00 00 00 00 00
                public static byte Header = 0x1E;
            }

            #endregion

            #region WaypointConfirm 4.21 (NO STRUCT)

            /// <summary>
            ///     Sent by client to confirm receiving waypoints.
            /// </summary>
            public static class WaypointConfirm
            {
                //36 00 00 00 00 00 83 7F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 
                public static byte Header = 0x36;
            }

            #endregion

            #region Pause 4.21 (NO STRUCT)

            /// <summary>
            ///     Sent by client on pause.
            /// </summary>
            public static class Pause
            {
                //00 01 00 00 00 00 00 00 00 00 00 00 00 00 90 
                public static byte Header = 0x00;
            }

            #endregion


            #region Resume 4.21 (NO STRUCT)

            /// <summary>
            ///     Sent by client on resume.
            /// </summary>
            public static class Resume
            {
                //19 00 00 00 00 00 00 00 00 00 90  
                public static byte Header = 0x19;
            }

            #endregion
        }

        public static class S2C
        {
            #region Ping - 4.21

            /// <summary>
            ///     RPing Packet. Received when ally team players send a SPing packet.
            /// </summary>
            public static class Ping
            {
                public static byte Header = 0x60;
                public static PacketChannel Channel = PacketChannel.C2S;
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteByte(0);
                    result.WriteInteger(0);
                    result.WriteInteger(packetStruct.TargetNetworkId);
                    result.WriteByte((byte)packetStruct.Type);
                    result.WriteInteger(packetStruct.SourceNetworkId);
                    result.WriteFloat(packetStruct.X);
                    result.WriteFloat(packetStruct.Y);
                    var bit = packetStruct.Silent ? (byte) 0x14 : (byte) 0x1B;
                    result.WriteByte(bit);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 6 };
                    var targetNetworkId = packet.ReadInteger();
                    var type = packet.ReadByte();
                    var sourceNetworkId = packet.ReadInteger();
                    var x = packet.ReadFloat();
                    var y = packet.ReadFloat();
                    var silent = (packet.ReadByte() & 1) != 1;
                    return new Struct(
                        x, y, targetNetworkId, sourceNetworkId,
                        (PingType)type, silent);
                }

                public struct Struct
                {
                    public bool Silent;
                    public int SourceNetworkId;
                    public int TargetNetworkId;
                    public PingType Type;
                    public float X;
                    public float Y;

                    public Struct(float x = 0f,
                        float y = 0f,
                        int targetNetworkId = 0,
                        int sourceNetworkId = 0,
                        PingType type = PingType.Normal,
                        bool silent = false)
                    {
                        X = x;
                        Y = y;
                        TargetNetworkId = targetNetworkId;
                        SourceNetworkId = sourceNetworkId;
                        Type = type;
                        Silent = silent;
                    }
                }
            }

            #endregion

            #region GainVision - 4.21

            /// <summary>
            ///     Gets received when a unit leaves FOW.
            /// </summary>
            public static class GainVision
            {
                public static byte Header = 0xFC;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    //Not fully encoded
                    var result = new GamePacket(Header);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.UnitNetworkId);
                    result.WriteShort(0);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    result.UnitNetworkId = packet.ReadInteger(2);
                    return result;
                }

                public struct Struct
                {
                    public int UnitNetworkId;

                    public Struct(int unitNetworkId = 0)
                    {
                        UnitNetworkId = unitNetworkId;
                    }
                }
            }

            #endregion

            #region LoseVision - 4.21

            /// <summary>
            ///     Gets received when a unit enters FOW.
            /// </summary>
            public static class LoseVision
            {
                public static byte Header = 0xCD;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.UnitNetworkId);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 2 };
                    return new Struct(packet.ReadInteger());
                }

                public struct Struct
                {
                    public int UnitNetworkId;

                    public Struct(int unitNetworkId = 0)
                    {
                        UnitNetworkId = unitNetworkId;
                    }
                }
            }

            #endregion

            #region EmptyJungleCamp - 4.21 partially
            /// <summary>
            ///     Gets received when gaining vision of an empty jungle camp.
            /// </summary>
            public static class EmptyJungleCamp
            {
                public static byte Header = 0x93;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteByte(0);
                    result.WriteInteger(0);
                    result.WriteInteger(packetStruct.CampId);
                    result.WriteInteger(packetStruct.UnitNetworkId);

                    //No idea where this is now or if it still exists :^)
                    result.WriteByte(packetStruct.EmptyType);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    result.CampId = packet.ReadInteger(6);
                    result.UnitNetworkId = packet.ReadInteger();
                    //No idea where this is now or if it still exists :^)
                    result.EmptyType = packet.ReadByte();
                    return result;
                }

                public struct Struct
                {
                    public int CampId;
                    public byte EmptyType;
                    public int UnitNetworkId;

                    public Struct(int unitNetworkId = 0, int campId = 0, byte emptyType = 0)
                    {
                        UnitNetworkId = unitNetworkId;
                        CampId = campId;
                        EmptyType = emptyType;
                    }
                }
            }

            #endregion

            #region CastAns

            /// <summary>
            ///     Received when a unit casts a spell.
            /// </summary>
            public static class CastAns
            {
                public static byte Header = 0xB5;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    //TODO when the packet is fully decoded.
                    return new GamePacket(Header);
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.SourceNetworkId = packet.ReadInteger(1);
                    result.SourceUnit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.SourceNetworkId);


                    result.SpellFlag = packet.ReadShort(10);
                    result.SpellHash = packet.ReadInteger();
                    result.SpellNetworkId = packet.ReadInteger();

                    packet.ReadByte(); //this always used to be 0, maybe flag now
                    packet.ReadFloat(); // always 1
                    packet.ReadFloat(); // always player nID
                    packet.ReadFloat(); // always player nID

                    result.MissileHash = packet.ReadInteger();
                    result.MissileNetworkId = packet.ReadInteger();

                    var p = packet.Position + 8;
                    result.ToPosition = new Vector2(packet.ReadFloat(), packet.ReadFloat(p));
                    packet.Position += 4;

                    var c = packet.ReadByte(65);
                    if (c > 0) //hopefully c is 1 always
                    {
                        result.TargetNetworkId = packet.ReadInteger();
                        result.TargetUnit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.TargetNetworkId);
                        packet.ReadByte(); // for 0
                    }
                    result.ChannelTime = packet.ReadFloat();
                    result.Delay = packet.ReadFloat();
                    result.Visible = packet.ReadFloat();
                    result.IsVisible = result.Visible > 0;
                    result.Cooldown = packet.ReadFloat();

                    packet.ReadInteger();
                    packet.ReadByte();

                    result.SpellSlot = (SpellSlot) packet.ReadByte();
                    result.SpellFlag2 = packet.ReadByte();
                    result.ManaCost = packet.ReadFloat();

                    p = packet.Position + 8;
                    result.FromPosition = new Vector2(packet.ReadFloat(), packet.ReadFloat(p));

                    return result;
                }

                public struct Struct
                {
                    public float ChannelTime;
                    public float Cooldown; // sometimes 0
                    public float Delay;
                    public Vector2 FromPosition;
                    public bool IsVisible;
                    public float ManaCost;
                    public int MissileHash;
                    public int MissileNetworkId;
                    public int SourceNetworkId;
                    public Obj_AI_Base SourceUnit;
                    public float Speed;
                    public short SpellFlag;
                    public byte SpellFlag2;
                    public int SpellHash;
                    public int SpellNetworkId;
                    public SpellSlot SpellSlot;
                    public int TargetNetworkId;
                    public Obj_AI_Base TargetUnit;
                    public Vector2 ToPosition;
                    public float Visible; // >0 visible
                }
            }

            #endregion

            #region Dash - 4.12 only header updated

            /// <summary>
            ///     Gets received when a unit dashes.
            /// </summary>
            public static class Dash
            {
                public static byte Header = 0xD7;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    //TODO when the packet is fully decoded.
                    return new GamePacket(Header);
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.UnitNetworkId = packet.ReadInteger(12);
                    result.Speed = 900;//packet.ReadFloat();

                    return result;
                }

                public struct Struct
                {
                    public float Speed;
                    public int UnitNetworkId;

                    public Struct(int unitNetworkId = 0, float speed = 0)
                    {
                        UnitNetworkId = unitNetworkId;
                        Speed = speed;
                    }
                }
            }

            #endregion

            #region GameEnd - 4.21

            /// <summary>
            ///     Gets received when the game ends.
            /// </summary>
            public static class GameEnd
            {
                public static byte Header = 0xED;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(0);
                    result.WriteByte(packetStruct.Winner);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.Winner = packet.ReadByte(5);
                    return result;
                }

                public struct Struct
                {
                    public byte Winner;

                    public Struct(byte winner = 1)
                    {
                        Winner = winner;
                    }
                }
            }

            #endregion

            #region TowerAggro

            /// <summary>
            ///     Gets received when a tower starts targeting a unit
            /// </summary>
            public static class TowerAggro
            {
                public static byte Header = 0x6A;
                public static readonly Dictionary<int, int> AggroList = new Dictionary<int, int>();

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.TurretNetworkId);
                    result.WriteInteger(packetStruct.TargetNetworkId);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.TurretNetworkId = packet.ReadInteger(1);
                    result.TargetNetworkId = packet.ReadInteger();

                    if (result.TurretNetworkId != 0)
                    {
                        AggroList[result.TurretNetworkId] = result.TargetNetworkId;
                    }

                    return result;
                }

                public struct Struct
                {
                    public int TargetNetworkId;
                    public int TurretNetworkId;

                    public Struct(int turretNetworkId, int targetNetworkId)
                    {
                        TurretNetworkId = turretNetworkId;
                        TargetNetworkId = targetNetworkId;
                    }
                }
            }

            #endregion

            #region UpdateModel

            /// <summary>
            ///     Gets received when the model changes.
            /// </summary>
            public static class UpdateModel
            {
                public static byte Header = 0x1A;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteInteger(packetStruct.NetworkId & ~0x40000000);
                    result.WriteByte((byte) (packetStruct.BOk ? 1 : 0));
                    result.WriteInteger(packetStruct.SkinId);
                    for (var i = 0; i < 32; i++)
                    {
                        if (i < packetStruct.ModelName.Length)
                        {
                            result.WriteByte((byte) packetStruct.ModelName[i]);
                        }
                        else
                        {
                            result.WriteByte(0x00);
                        }
                    }

                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(2);
                    result.Id = packet.ReadInteger();
                    result.BOk = packet.ReadByte() == 0x01;
                    result.SkinId = packet.ReadInteger();

                    return result;
                }

                public struct Struct
                {
                    public bool BOk;
                    public int Id;
                    public string ModelName;
                    public int NetworkId;
                    public int SkinId;

                    public Struct(int networkId, int skinId, string modelName, bool bOk = true, int id = -1)
                    {
                        NetworkId = networkId;
                        Id = id != -1 ? id : NetworkId & ~0x40000000;
                        BOk = bOk;
                        SkinId = skinId;
                        ModelName = modelName;
                    }
                }
            }

            #endregion

            #region Teleport - 4.21

            /// <summary>
            ///     Gets received when a unit starts, aborts or finishes a teleport (such as recall, teleport, twisted fate ulti, shen
            ///     ulti,...)
            /// </summary>
            public static class Teleport
            {
                public enum Status
                {
                    Start,
                    Abort,
                    Finish,
                    Unknown
                }

                public enum Type
                {
                    Recall,
                    Teleport,
                    TwistedFate,
                    Shen,
                    Unknown
                }

                internal interface ITeleport
                {
                    Type Type { get; }
                    int GetDuration(GameObjectTeleportEventArgs packetData);
                }

                internal class RecallTeleport : ITeleport
                {
                    public Type Type
                    {
                        get { return Type.Recall; }
                    }

                    public int GetDuration(GameObjectTeleportEventArgs args)
                    {
                        return Utility.GetRecallTime(args.RecallName);
                    }
                }

                internal class TeleportTeleport : ITeleport
                {
                    public Type Type
                    {
                        get { return Type.Teleport; }
                    }

                    public int GetDuration(GameObjectTeleportEventArgs args)
                    {
                        return 3500;
                    }
                }

                internal class TwistedFateTeleport : ITeleport
                {
                    public Type Type
                    {
                        get { return Type.TwistedFate; }
                    }

                    public int GetDuration(GameObjectTeleportEventArgs args)
                    {
                        return 1500;
                    }
                }

                internal class ShenTeleport : ITeleport
                {
                    public Type Type
                    {
                        get { return Type.Shen; }
                    }

                    public int GetDuration(GameObjectTeleportEventArgs args)
                    {
                        return 3000;
                    }
                }


                public static byte Header = 0x44;

                private const int ErrorGap = 100; //in ticks

                private static readonly IDictionary<string, ITeleport> TypeByString = new Dictionary<string, ITeleport>
                {
                    {"Recall", new RecallTeleport()},
                    {"Teleport", new TeleportTeleport()},
                    {"Gate", new TwistedFateTeleport()},
                    {"Shen", new ShenTeleport()},
                };

                private static readonly IDictionary<int, TeleportData> RecallDataByNetworkId =
                    new Dictionary<int, TeleportData>();

                public static GamePacket Encoded(Struct packetStruct)
                {
                    //TODO when the packet is fully decoded.
                    return new GamePacket(Header);
                }

                public static Struct Decoded(GameObject sender, GameObjectTeleportEventArgs args) //
                {
                    var result = new Struct
                    {
                        Status = Status.Unknown,
                        Type = Type.Unknown
                    };

                    if(sender == null || !sender.IsValid || !(sender is Obj_AI_Hero))
                    {
                        return result;
                    }

                    result.UnitNetworkId = sender.NetworkId;

                    var hero = sender as Obj_AI_Hero;

                    if (!RecallDataByNetworkId.ContainsKey(result.UnitNetworkId))
                    {
                        RecallDataByNetworkId[result.UnitNetworkId] = new TeleportData {Type = Type.Unknown};
                    }

                    if (!string.IsNullOrEmpty(args.RecallType))
                    {
                        if (TypeByString.ContainsKey(args.RecallType))
                        {
                            ITeleport teleportMethod = TypeByString[args.RecallType];

                            int duration = teleportMethod.GetDuration(args);
                            Type type = teleportMethod.Type;
                            int time = Utils.TickCount;

                            RecallDataByNetworkId[result.UnitNetworkId] = new TeleportData
                            {
                                Duration = duration,
                                Type = type,
                                Start = time
                            };

                            result.Status = Status.Start;
                            result.Duration = duration;
                            result.Type = type;
                            result.Start = time;
                        }
                    }
                    else
                    {
                        bool shorter = Utils.TickCount - RecallDataByNetworkId[result.UnitNetworkId].Start <
                                       RecallDataByNetworkId[result.UnitNetworkId].Duration - ErrorGap;
                        result.Status = shorter ? Status.Abort : Status.Finish;
                        result.Type = RecallDataByNetworkId[result.UnitNetworkId].Type;
                        result.Duration = 0;
                        result.Start = 0;
                    }
                    return result;
                }

                internal struct TeleportData
                {
                    public Type Type { get; set; }
                    public int Start { get; set; }
                    public int Duration { get; set; }
                }

                public struct Struct
                {
                    public int Duration;
                    public Status Status;
                    public Type Type;
                    public int Start;
                    public int UnitNetworkId;

                    public Struct(int unitNetworkId, Status status, Type type, int duration, int start = 0)
                    {
                        UnitNetworkId = unitNetworkId;
                        Status = status;
                        Type = type;
                        Duration = duration;
                        Start = start;
                    }
                }
            }

            #endregion

           #region PlayEmote - 4.21

            /// <summary>
            ///     Gets received when an unit uses an emote.
            /// </summary>
            public static class PlayEmote
            {
                public static byte Header = 0xAA;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteByte(packetStruct.EmoteId);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    result.NetworkId = packet.ReadInteger(2);
                    result.EmoteId = packet.ReadByte();
                    return result;
                }

                public struct Struct
                {
                    public byte EmoteId;
                    public int NetworkId;

                    public Struct(byte emoteId, int networkId = -1)
                    {
                        EmoteId = emoteId;
                        NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }
                }
            }

            #endregion

            #region Damage - 4.21 partially

            /// <summary>
            ///     Packet received when a unit deals damage.
            /// </summary>
            public class Damage
            {
                public static byte Header = 0x23;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    //23 00 05 00 00 40 05 00 00 40 06 00 00 40 04 F7 33 B4 3F 01 66
                    var packet = new GamePacket(Header);
                    packet.WriteByte(0);
                    packet.WriteInteger(packetStruct.TargetNetworkId);
                    packet.WriteByte((byte) packetStruct.Type);
                    packet.WriteShort(packetStruct.Unknown); // Unknown value
                    packet.WriteFloat(packetStruct.DamageAmount);
                    packet.WriteInteger(packetStruct.TargetNetworkIdCopy);
                    packet.WriteInteger(packetStruct.SourceNetworkId);


                    return packet;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    packet.Position = 2;
                    result.TargetNetworkId = packet.ReadInteger();
                    result.TargetNetworkIdCopy = packet.ReadInteger();
                    result.SourceNetworkId = packet.ReadInteger();
                    //wrong:
                    result.Type = (DamageTypePacket) packet.ReadByte();
                    result.Unknown = packet.ReadShort();
                    result.DamageAmount = packet.ReadFloat();
                    return result;
                }

                public struct Struct
                {
                    public float DamageAmount;
                    public int SourceNetworkId;
                    public int TargetNetworkId;
                    public int TargetNetworkIdCopy;
                    public DamageTypePacket Type;
                    public short Unknown;

                    public Struct(float damageAmount,
                        int sourceNetworkId,
                        int targetNetworkId,
                        int targetNetworkIdCopy,
                        DamageTypePacket type,
                        short unknown)
                    {
                        DamageAmount = damageAmount;
                        SourceNetworkId = sourceNetworkId;
                        TargetNetworkId = targetNetworkId;
                        TargetNetworkIdCopy = targetNetworkIdCopy;
                        Type = type;
                        Unknown = unknown;
                    }
                }
            }

            #endregion

            #region FloatText

            /// <summary>
            ///     Packet received print float text.
            /// </summary>
            public class FloatText
            {
                public static byte Header = 0x19;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var packet = new GamePacket(Header);

                    packet.WriteInteger(0);
                    packet.WriteInteger(packetStruct.NetworkId);
                    packet.WriteByte((byte) packetStruct.Type);
                    packet.WriteInteger(packetStruct.NetworkId);
                    packet.WriteString(packetStruct.Text);
                    packet.WriteByte(0);

                    return packet;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
                    result.Type = (FloatTextPacket) packet.ReadByte();
                    //result.Text = packet.ReadString();

                    return result;
                }


                public struct Struct
                {
                    public int NetworkId;
                    public string Text;
                    public FloatTextPacket Type;

                    public Struct(string text, FloatTextPacket type, int networkId = 0)
                    {
                        NetworkId = networkId == 0 ? ObjectManager.Player.NetworkId : networkId;
                        Text = text;
                        Type = type;
                    }
                }
            }

            #endregion

            #region DebugMessage

            /// <summary>
            ///     Packet received for debug message.
            /// </summary>
            public class DebugMessage
            {
                public static byte Header = 0xF7;

                public static GamePacket Encoded(String debugString)
                {
                    var packet = new GamePacket(Header);

                    packet.WriteByte(0, 8);
                    packet.WriteString(debugString);
                    packet.WriteByte(0);
                    return packet;
                }
            }

            #endregion

            #region HighlightUnit

            /// <summary>
            ///     Packet highlights unit.
            /// </summary>
            public class HighlightUnit
            {
                public static byte Header = 0x59;

                public static GamePacket Encoded(int networkId)
                {
                    var packet = new GamePacket(Header);

                    packet.WriteInteger(0);
                    packet.WriteInteger(networkId);

                    return packet;
                }
            }

            #endregion

            #region RemoveHighlightUnit

            /// <summary>
            ///     Packet remove highlights unit.
            /// </summary>
            public class RemoveHighlightUnit
            {
                public static byte Header = 0xB4;

                public static GamePacket Encoded(int networkId)
                {
                    var packet = new GamePacket(Header);

                    packet.WriteInteger(0);
                    packet.WriteInteger(networkId);

                    return packet;
                }
            }

            #endregion

            #region PlayerDisconnect - 4.21

            /// <summary>
            ///     Packet received on player disconnect.
            /// </summary>
            public class PlayerDisconnect
            {
                public static byte Header = 0xFE;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();


                    packet.Position = 6;
                    result.NetworkId = packet.ReadInteger();
                    result.Player = ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(result.NetworkId);

                    return result;
                }

                public struct Struct
                {
                    public int NetworkId;
                    public Obj_AI_Hero Player;
                }
            }

            #endregion

            #region PlayerReconnect

            /// <summary>
            ///     Packet received when a player presses the "Reconnect" Button.
            /// </summary>
            public class PlayerReconnect
            {
                public static byte Header = 0x0;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();


                    packet.Position = 4;
                    result.ClientId = packet.ReadInteger();
                    result.Player = ObjectManager.Get<Obj_AI_Hero>().ElementAt(result.ClientId);

                    return result;
                }

                public struct Struct
                {
                    public int ClientId;
                    public Obj_AI_Hero Player;
                }
            }

            #endregion

            #region PlayerReconnected

            /// <summary>
            ///     Packet received when a player reconnected.
            /// </summary>
            public class PlayerReconnected
            {
                public static byte Header = 0xF;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();


                    packet.Position = 5;
                    result.ClientId = packet.ReadInteger();
                    result.Player = ObjectManager.Get<Obj_AI_Hero>().ElementAt(result.ClientId);

                    return result;
                }

                public struct Struct
                {
                    public int ClientId;
                    public Obj_AI_Hero Player;
                }
            }

            #endregion

            #region GainBuff

            /// <summary>
            ///     Packet received on gaining buff.
            /// </summary>
            public class GainBuff
            {
                public static byte Header = 0xB7;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
                    result.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.NetworkId);

                    result.BuffSlot = packet.ReadByte();
                    result.Type = (BuffType) packet.ReadByte();
                    result.Stack = packet.ReadByte();
                    result.Visible = packet.ReadByte() > 0;
                    result.BuffId = packet.ReadInteger();

                    result.TargetNetworkId = packet.ReadInteger();
                    result.Target = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.TargetNetworkId);

                    packet.Position += 4;

                    result.Duration = packet.ReadFloat();

                    result.SourceNetworkId = packet.ReadInteger();
                    result.Source = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.SourceNetworkId);

                    return result;
                }

                public struct Struct
                {
                    public int BuffId;
                    public byte BuffSlot;
                    public float Duration;
                    public int NetworkId;
                    public Obj_AI_Base Source;
                    public int SourceNetworkId;
                    public int Stack;
                    public Obj_AI_Base Target;
                    public int TargetNetworkId;
                    public BuffType Type;
                    public Obj_AI_Base Unit;
                    public bool Visible;
                }
            }

            #endregion

            #region LoseBuff

            /// <summary>
            ///     Packet received on losing buff.
            /// </summary>
            public class LoseBuff
            {
                public static byte Header = 0x7B;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
                    result.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.NetworkId);

                    result.BuffSlot = packet.ReadByte();
                    result.BuffId = packet.ReadInteger();
                    result.Duration = packet.ReadFloat();

                    return result;
                }

                public struct Struct
                {
                    public int BuffId;
                    public byte BuffSlot;
                    public float Duration;
                    public int NetworkId;
                    public Obj_AI_Base Unit;
                }
            }

            #endregion

            #region SetCooldown

            /// <summary>
            ///     One packet that sets cooldown.
            /// </summary>
            public class SetCooldown
            {
                public static byte Header = 0x85;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
                    result.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.NetworkId);
                    result.Slot = (SpellSlot) packet.ReadByte();
                    packet.Position += 1;
                    result.TotalCooldown = packet.ReadFloat();
                    result.CurrentCooldown = packet.ReadFloat();

                    return result;
                }

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var packet = new GamePacket(Header);
                    packet.WriteInteger(packetStruct.NetworkId);
                    packet.WriteByte((byte) packetStruct.Slot);
                    packet.WriteByte(0xF8);
                    packet.WriteFloat(packetStruct.TotalCooldown);
                    packet.WriteFloat(packetStruct.CurrentCooldown);

                    return packet;
                }

                public struct Struct
                {
                    public float CurrentCooldown;
                    public int NetworkId;
                    public SpellSlot Slot;
                    public float TotalCooldown;
                    public Obj_AI_Base Unit;

                    public Struct(int networkId, SpellSlot slot, float totalCd, float currentCd)
                    {
                        NetworkId = networkId;
                        Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(NetworkId);
                        Slot = slot;
                        TotalCooldown = totalCd;
                        CurrentCooldown = currentCd;
                    }
                }
            }

            #endregion

            #region StartItemCooldown

            /// <summary>
            ///     One packet that starts cooldown (mostly for items).
            /// </summary>
            public class StartItemCooldown
            {
                public static byte Header = 0x9F;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
                    result.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.NetworkId);
                    result.InventorySlot = packet.ReadByte();
                    result.SpellSlot = (SpellSlot) (result.InventorySlot + (byte) SpellSlot.Item1);
                    return result;
                }


                public struct Struct
                {
                    public byte InventorySlot;
                    public int NetworkId;
                    public SpellSlot SpellSlot;
                    public Obj_AI_Base Unit;
                }
            }

            #endregion

            #region BuyItemAns

            /// <summary>
            ///     Packet received on buying item.
            /// </summary>
            public class BuyItemAns
            {
                public static byte Header = 0x6F;

                public static GamePacket Encoded(Struct pStruct)
                {
                    var packet = new GamePacket(Header);
                    packet.WriteInteger(pStruct.NetworkId);
                    packet.WriteShort((short) pStruct.Item.Id);
                    packet.WriteByte(0, 2);
                    packet.WriteByte(pStruct.InventorySlot);
                    packet.WriteByte((byte) pStruct.Stack);
                    packet.WriteByte((byte) pStruct.Charge);
                    packet.WriteByte(pStruct.ReplaceItem);

                    return packet;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
                    result.Item = new Items.Item(packet.ReadShort(), 0);
                    packet.Position += 2;
                    result.InventorySlot = packet.ReadByte();
                    result.SpellSlot = (SpellSlot) (result.InventorySlot + (byte) SpellSlot.Item1);
                    result.Stack = packet.ReadByte();
                    result.Charge = packet.ReadByte();
                    result.ReplaceItem = packet.ReadByte();

                    return result;
                }

                public struct Struct
                {
                    public int Charge;
                    public byte InventorySlot;
                    public Items.Item Item;
                    public int NetworkId;
                    public byte ReplaceItem;
                    public SpellSlot SpellSlot;
                    public int Stack;
                    public Obj_AI_Hero Unit;

                    public Struct(int id,
                        byte slot,
                        byte replace = 0x7B,
                        int stack = 1,
                        int charge = 0,
                        int networkId = -1)
                    {
                        Item = new Items.Item(id, 0);
                        InventorySlot = slot;
                        SpellSlot = (SpellSlot) (InventorySlot + (byte) SpellSlot.Item1);
                        ReplaceItem = replace;
                        Stack = stack;
                        Charge = charge;
                        NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                        Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(NetworkId);
                    }
                }
            }

            #endregion

            #region SellItemAns - 4.21

            /// <summary>
            ///     Packet received on selling item.
            /// </summary>
            public class SellItemAns
            {
                public static byte Header = 0xD3;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(2);
                    result.InventorySlot = packet.ReadByte();
                    result.SpellSlot = (SpellSlot) (result.InventorySlot + (byte) SpellSlot.Item1);
                    result.Stack = packet.ReadByte();
                    result.UnknownByte = packet.ReadByte();
                    return result;
                }

                public struct Struct
                {
                    public byte InventorySlot;
                    public int NetworkId;
                    public SpellSlot SpellSlot;
                    public int Stack;
                    public Obj_AI_Hero Unit;
                    public byte UnknownByte;
                }
            }

            #endregion

            #region SwapItemAns - 4.21

            /// <summary>
            ///     Packet received on swapping item.
            /// </summary>
            public class SwapItemAns
            {
                public static byte Header = 0x09;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    
                    result.NetworkId = packet.ReadInteger(2);
                    result.FromInventorySlot = packet.ReadByte();
                    result.FromSpellSlot = (SpellSlot) (result.FromInventorySlot + (byte) SpellSlot.Item1);
                    result.ToInventorySlot = packet.ReadByte();
                    result.ToSpellSlot = (SpellSlot) (result.ToInventorySlot + (byte) SpellSlot.Item1);
                    return result;
                }

                public struct Struct
                {
                    public byte FromInventorySlot;
                    public SpellSlot FromSpellSlot;
                    public int NetworkId;
                    public byte ToInventorySlot;
                    public SpellSlot ToSpellSlot;
                    public Obj_AI_Hero Unit;
                }
            }

            #endregion

            #region ChangeSpellSlot - This packet doesnt seem to exits in 4.21 with this struct

            /// <summary>
            ///     Packet received on spell slot changing.
            /// </summary>
            public class ChangeSpellSlot
            {
                public static byte Header = 0x17;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
                    result.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.NetworkId);
                    result.Slot = (SpellSlot) (packet.ReadByte());
                    result.UnknownByte = packet.ReadByte(); // 0, 1C, 48
                    result.UnknownByte2 = packet.ReadByte(); //usually 2
                    result.SpellString = packet.ReadString(11);
                    return result;
                }

                public static GamePacket Encoded(Struct pStruct)
                {
                    var packet = new GamePacket(Header);
                    packet.WriteInteger(pStruct.NetworkId);
                    packet.WriteByte((byte) pStruct.Slot);
                    packet.WriteByte(pStruct.UnknownByte);
                    packet.WriteByte(2);
                    packet.WriteByte(0, 3);
                    packet.WriteString(pStruct.SpellString);
                    return packet;
                }

                public struct Struct
                {
                    public int NetworkId;
                    public SpellSlot Slot;
                    public string SpellString;
                    public Obj_AI_Base Unit;
                    public byte UnknownByte; // from slot?
                    public byte UnknownByte2;
                }
            }

            #endregion

            #region AddGold

            /// <summary>
            ///     Packet received on gold change.
            /// </summary>
            public class AddGold
            {
                public static byte Header = 0x22;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.ReceivingNetworkId = packet.ReadInteger(5);
                    result.ReceivingUnit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.ReceivingNetworkId);
                    result.SourceNetworkId = packet.ReadInteger();
                    result.SourceUnit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.SourceNetworkId);
                    result.Gold = packet.ReadFloat();
                    return result;
                }

                public static GamePacket Encoded(Struct pStruct)
                {
                    var packet = new GamePacket(Header);
                    packet.WriteInteger(pStruct.ReceivingNetworkId);
                    packet.WriteInteger(pStruct.ReceivingNetworkId);
                    packet.WriteInteger(pStruct.SourceNetworkId);
                    packet.WriteFloat(pStruct.Gold);
                    return packet;
                }

                public struct Struct
                {
                    public float Gold;
                    public int ReceivingNetworkId;
                    public Obj_AI_Base ReceivingUnit;
                    public int SourceNetworkId;
                    public Obj_AI_Base SourceUnit;
                }
            }

            #endregion

            #region LevelUp - 4.21

            /// <summary>
            ///     Received on hero level up.
            /// </summary>
            public class LevelUp
            {
                public static byte Header = 0xCB;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(2);
                    result.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(result.NetworkId);
                    result.Level = packet.ReadByte();
                    result.PointsLeft = packet.ReadByte();

                    return result;
                }

                public struct Struct
                {
                    public int Level;
                    public int NetworkId;
                    public int PointsLeft;
                    public Obj_AI_Hero Unit;
                }
            }

            #endregion

            #region LevelUpSpell - 4.2.1

            /// <summary>
            ///     Received on hero level up spell.
            /// </summary>
            public class LevelUpSpell
            {
                public static byte Header = 0xA9;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(2);
                    result.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(result.NetworkId);
                    result.Slot = (SpellSlot) packet.ReadByte();
                    result.PointsLeft = packet.ReadByte();
                    result.Level = packet.ReadByte();
                    return result;
                }

                public struct Struct
                {
                    public int Level;
                    public int NetworkId;
                    public int PointsLeft;
                    public Obj_AI_Hero Unit;
                    public SpellSlot Slot;
                }
            }

            #endregion

            #region Surrender

            /// <summary>
            ///     Received when someone casts a surrender vote.
            /// </summary>
            public class Surrender
            {
                public static byte Header = 0xC9;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct
                    {
                        NetworkId = packet.ReadInteger(6),
                        YesVotes = packet.ReadByte(10),
                        NoVotes = packet.ReadByte(11),
                        MaxVotes = packet.ReadByte(12),
                        Team = (GameObjectTeam) packet.ReadByte(13)
                    };

                    //byte unknown = packet.ReadByte(5); //Not sure what this is

                    return result;
                }

                public struct Struct
                {
                    public int NetworkId;
                    public int YesVotes;
                    public int NoVotes;
                    public int MaxVotes;
                    public GameObjectTeam Team;
                }
            }

            #endregion

            #region SurrenderResult

            /// <summary>
            ///     Received when surrender voting is over.
            /// </summary>
            public class SurrenderResult
            {
                public static byte Header = 0xA5;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct
                    {
                        TooEarly = Convert.ToBoolean(packet.ReadByte(5)),
                        YesVotes = packet.ReadByte(9),
                        NoVotes = packet.ReadByte(10),
                        Team = (GameObjectTeam) packet.ReadByte(11)
                    };

                    return result;
                }

                public struct Struct
                {
                    public bool TooEarly;
                    public int YesVotes;
                    public int NoVotes;
                    public GameObjectTeam Team;
                }
            }

            #endregion

            #region RefundToken - 4.21

            /// <summary>
            ///     Refund token contains refund amount, when leaving base or casting spell/item it's set to 0.
            /// </summary>
            public static class RefundToken
            {
                public static byte Header = 0xE9;

                public static Struct Decoded(byte[] data)
                {
                    return new Struct { RefundCount = data[6] };
                }

                public static GamePacket Encoded(int undoAmount)
                {
                    var packet = new GamePacket(Header);
                    packet.WriteByte(0);
                    packet.WriteInteger(ObjectManager.Player.NetworkId);
                    packet.WriteInteger(undoAmount);
                    return packet;
                }

                public struct Struct
                {
                    public int RefundCount;
                }
            }

            #endregion

            #region Camera - 4.21 (NO STRUCT)

            /// <summary>
            ///     Received by the server when Camera or Zoom is sent.
            /// </summary>
            public static class Camera
            {
                //94 00 00 00 00 00 27 
                //last byte is probably times camera packet has been sent
                public static byte Header = 0x94;
            }

            #endregion

            #region RefundConfirm - 4.21 (NO STRUCT)

            /// <summary>
            ///     Received by the server when refund is sent.
            /// </summary>
            public static class RefundConfirm
            {
                public static byte Header = 0x49;
            }

            #endregion
        }
    }
}
