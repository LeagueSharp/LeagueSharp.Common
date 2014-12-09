#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Orbwalking.cs is part of LeagueSharp.Common.
 
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
    public static class Packet
    {
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
            #region Ping

            /// <summary>
            ///     Ping Packet. Sent by the client when pings are sent.
            /// </summary>
            public static class Ping
            {
                public static byte Header = 0x57;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(0);
                    result.WriteFloat(packetStruct.X);
                    result.WriteFloat(packetStruct.Y);
                    result.WriteInteger(packetStruct.TargetNetworkId);
                    result.WriteByte((byte) packetStruct.Type);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 5 };
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

            #region LevelUpSpell

            /// <summary>
            ///     Packet sent when leveling up a spell.
            /// </summary>
            public static class LevelUpSpell
            {
                public static byte Header = 0x39;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteByte((byte) packetStruct.Slot);
                    var bit = packetStruct.Evolution ? (byte) 0x01 : (byte) 0x0;
                    result.WriteByte(bit);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 1 };
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
                public static byte Header = 0x72;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.SourceNetworkId);
                    result.WriteByte(packetStruct.MoveType);
                    result.WriteFloat(packetStruct.X);
                    result.WriteFloat(packetStruct.Y);
                    result.WriteInteger(packetStruct.TargetNetworkId);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.UnitNetworkId);
                    result.WriteByte(0);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 1;
                    result.SourceNetworkId = packet.ReadInteger();
                    result.MoveType = packet.ReadByte();
                    result.X = packet.ReadFloat();
                    result.Y = packet.ReadFloat();
                    result.TargetNetworkId = packet.ReadInteger();
                    result.WaypointCount = packet.ReadByte() / 2;
                    result.UnitNetworkId = packet.ReadInteger();
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

            #region Cast

            /// <summary>
            ///     Packet sent when casting spells.
            /// </summary>
            public static class Cast
            {
                public static byte Header = 0x9A;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.SourceNetworkId);
                    result.WriteByte(
                        packetStruct.SpellFlag == 0xFF ? GetSpellByte(packetStruct.Slot) : packetStruct.SpellFlag);
                    result.WriteByte((byte) packetStruct.Slot);
                    result.WriteFloat(packetStruct.FromX);
                    result.WriteFloat(packetStruct.FromY);
                    result.WriteFloat(packetStruct.ToX);
                    result.WriteFloat(packetStruct.ToY);
                    result.WriteInteger(packetStruct.TargetNetworkId);

                    result.Block = !SpellHumanizer.Check(result);

                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 1;
                    result.SourceNetworkId = packet.ReadInteger();
                    result.SpellFlag = packet.ReadByte();
                    result.Slot = (SpellSlot) packet.ReadByte();
                    result.FromX = packet.ReadFloat();
                    result.FromY = packet.ReadFloat();
                    result.ToX = packet.ReadFloat();
                    result.ToY = packet.ReadFloat();
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

            #region ChargedCast

            /// <summary>
            ///     Packet sent when casting charged spells second cast.
            /// </summary>
            public static class ChargedCast
            {
                public static byte Header = 0xE6;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.SourceNetworkId);
                    result.WriteByte((byte) (packetStruct.Slot == SpellSlot.Q ? 0xEA : 0x9C));
                    result.WriteByte((byte) packetStruct.Slot);
                    result.WriteFloat(packetStruct.ToX);
                    result.WriteFloat(packetStruct.ToY);
                    result.WriteFloat(packetStruct.ToZ);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 1;
                    result.SourceNetworkId = packet.ReadInteger();
                    packet.Position += 1;
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

            #region BuyItem

            /// <summary>
            ///     Packet sent when buying items.
            /// </summary>
            public static class BuyItem
            {
                public static byte Header = 0x82;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteInteger(packetStruct.ItemId);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 1;
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

            #region SellItem

            /// <summary>
            ///     Packet sent when selling items.
            /// </summary>
            public static class SellItem
            {
                public static byte Header = 0x09;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteByte(packetStruct.InventorySlot);
                    result.WriteByte(1);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 1 };
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

            #region SwapItem

            /// <summary>
            ///     Packet sent when swapping items.
            /// </summary>
            public static class SwapItem
            {
                public static byte Header = 0x20;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteByte(packetStruct.FromSlotByte);
                    result.WriteByte(packetStruct.ToSlotByte);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 1;
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

            #region Emote

            /// <summary>
            ///     Packet sent when sending emotes.
            /// </summary>
            public static class Emote
            {
                public static byte Header = 0x48;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteByte(packetStruct.EmoteId);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 1;
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

            #region InteractObject

            /// <summary>
            ///     Packet sent when interacting with Thresh Lantern and Dominion capturing.
            /// </summary>
            public static class InteractObject
            {
                public static byte Header = 0x3A;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
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

            #region SetTarget

            /// <summary>
            ///     Packet sent when left clicking a target.
            /// </summary>
            public static class SetTarget
            {
                public static byte Header = 0xAF;

                public static Struct Decoded(byte[] data)
                {
                    var result = new Struct { NetworkId = new GamePacket(data).ReadInteger(9) };
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

            #region Heartbeat

            /// <summary>
            ///     Packet sent frequently as heartbeat to servers.
            /// </summary>
            public static class HeartBeat
            {
                public static byte Header = 0x08;

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

            #region Undo

            /// <summary>
            ///     Sent by client on undo.
            /// </summary>
            public static class Undo
            {
                public static byte Header = MultiPacket.Header;

                public static GamePacket Encoded()
                {
                    return MultiPacket.BasePacket(0xA);
                }
            }

            #endregion
        }

        public static class MultiPacket
        {
            #region INFO

            /*
             * This is the MultiPacket packet. 
             * Packets in LoL are restricted to one byte i.e. (0-254) or (0 - 0xFE). 
             * Riot needed to add more packets, but couldn't add anymore headers.
             * The new packets are being added to 0xFE with a sub-header in order to be identified. 
             */

            #endregion

            public static byte Header = 0xFE;

            public static GamePacket BasePacket(byte subHeader)
            {
                var p = new GamePacket(Header);
                p.WriteInteger(ObjectManager.Player.NetworkId);
                p.WriteByte(subHeader);
                p.WriteByte(1);
                return p;
            }

            public static Struct DecodeHeader(byte[] data)
            {
                var packet = new GamePacket(data);

                var networkId = packet.ReadInteger(1);
                var subHeader = packet.ReadByte(5);

                return Enum.GetName(typeof(MultiPacketType), subHeader) == null
                    ? new Struct(networkId, MultiPacketType.Unknown, subHeader)
                    : new Struct(networkId, (MultiPacketType) subHeader);
            }

            public struct Struct
            {
                public int NetworkId;
                public byte SubHeader;
                public MultiPacketType Type;

                public Struct(int networkId, MultiPacketType type, byte subHeader = 0xFF)
                {
                    NetworkId = networkId;
                    Type = type;
                    SubHeader = subHeader == 0xFF ? (byte) type : subHeader;
                }
            }

            #region ActionState

            /// <summary>
            ///     Not sure what this does, recv on recall.
            /// </summary>
            public static class ActionState
            {
                public static byte SubHeader = (byte) MultiPacketType.ActionState;

                public static ReturnStruct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    return new ReturnStruct { Action = packet.ReadInteger(7) };
                }

                public static GamePacket Encoded(ReturnStruct pStruct)
                {
                    var packet = BasePacket(SubHeader);
                    packet.WriteInteger(pStruct.Action);
                    return packet;
                }

                public struct ReturnStruct
                {
                    public int Action;
                }
            }

            #endregion

            #region ChangeItem

            /// <summary>
            ///     Currently this packet only transforms HP pots to biscuits.
            /// </summary>
            public static class ChangeItem
            {
                public static byte SubHeader = (byte) MultiPacketType.ChangeItem;

                public static ReturnStruct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    return new ReturnStruct
                    {
                        InitialItemId = packet.ReadInteger(7),
                        FinalItemId = packet.ReadInteger()
                    };
                    //Utility.DumpPacket(data);
                }

                public static GamePacket Encoded(ReturnStruct pStruct)
                {
                    var packet = BasePacket(SubHeader);
                    packet.WriteInteger(pStruct.InitialItemId);
                    packet.WriteInteger(pStruct.FinalItemId);
                    return packet;
                }

                public struct ReturnStruct
                {
                    public int FinalItemId;
                    public int InitialItemId;
                }
            }

            #endregion

            #region OnAttack

            /// <summary>
            ///     Received when attacking or casting a spell on a unit.'
            ///     This packet comes before 0xB5 (S2C.Cast)
            ///     The attack can be cancelled and the packet will still be received.
            /// </summary>
            public static class OnAttack
            {
                public static byte SubHeader = (byte) MultiPacketType.OnAttack;

                public static ReturnStruct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var returnStruct = new ReturnStruct
                    {
                        Type = (AttackTypePacket) packet.ReadByte(7),
                        Position = new Vector3(packet.ReadFloat(), packet.ReadFloat(16), packet.ReadFloat(12)),
                        TargetNetworkId = packet.ReadInteger(20)
                    };

                    return returnStruct;
                }

                public struct ReturnStruct
                {
                    public Vector3 Position;
                    public int TargetNetworkId;
                    public AttackTypePacket Type;
                }
            }

            #endregion

            #region SurrenderState

            /// <summary>
            ///     Received when surrender is available.
            /// </summary>
            public static class SurrenderState
            {
                public static byte SubHeader = (byte) MultiPacketType.SurrenderState;

                public static ReturnStruct Decoded(byte[] data)
                {
                    return new ReturnStruct(data[7] == 0x01);
                }

                public struct ReturnStruct
                {
                    public bool CanSurrender;

                    public ReturnStruct(bool canSurrender)
                    {
                        CanSurrender = canSurrender;
                    }
                }
            }

            #endregion

            #region UndoToken

            /// <summary>
            ///     Undo token contains undo amount, when leaving base or casting spell/item it's set to 0.
            /// </summary>
            public static class UndoToken
            {
                public static byte SubHeader = (byte) MultiPacketType.UndoToken;

                public static ReturnStruct Decoded(byte[] data)
                {
                    return new ReturnStruct { UndoAmount = data[7] };
                }

                public static GamePacket Encoded(int undoAmount)
                {
                    var packet = BasePacket(SubHeader);
                    packet.WriteByte((byte) undoAmount);

                    return packet;
                }

                public struct ReturnStruct
                {
                    public int UndoAmount;
                }
            }

            #endregion

            #region UndoConfirm

            /// <summary>
            ///     Undo confirm comes after undoing.
            /// </summary>
            public static class UndoConfirm
            {
                public static byte SubHeader = (byte) MultiPacketType.UndoConfirm;

                public static List<RefundItem> Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 7 };
                    var itemList = new List<RefundItem>();

                    for (var i = 0; i < 7; i++)
                    {
                        var itemId = packet.ReadShort();
                        packet.Position += 2;
                        var inventorySlot = packet.ReadByte();
                        var spellSlot = inventorySlot + SpellSlot.Item1;
                        var stack = packet.ReadByte();
                        var charge = packet.ReadByte();
                        var pos = packet.Position;

                        if (itemId == 0)
                        {
                            continue;
                        }

                        var cd = packet.ReadFloat(packet.Position + 63 - 3 * inventorySlot);
                        var totalCd = packet.ReadFloat(packet.Position + 36);
                        packet.Position = pos;

                        itemList.Add(new RefundItem(itemId, inventorySlot, spellSlot, stack, charge, cd, totalCd));
                    }

                    return itemList;
                }

                public class RefundItem
                {
                    public int Charge;
                    public float CurrentCooldown;
                    public byte InventorySlot;
                    public int ItemId;
                    public SpellSlot SpellSlot;
                    public int Stack;
                    public float TotalCooldown;

                    public RefundItem(short itemId,
                        byte inventorySlot,
                        SpellSlot slot,
                        int stack,
                        int charge,
                        float cd,
                        float totalCd)
                    {
                        ItemId = itemId;
                        InventorySlot = inventorySlot;
                        SpellSlot = slot;
                        Stack = stack;
                        Charge = charge;
                        CurrentCooldown = cd;
                        TotalCooldown = totalCd;
                    }
                }
            }

            #endregion

            #region DeathTimer

            /// <summary>
            ///     Contains death timer for hero.
            /// </summary>
            public static class DeathTimer
            {
                public static byte SubHeader = (byte) MultiPacketType.DeathTimer;

                public static ReturnStruct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var hero = ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(packet.ReadInteger(1));
                    return new ReturnStruct { Time = packet.ReadFloat(7), Hero = hero };
                }

                public struct ReturnStruct
                {
                    public Obj_AI_Hero Hero;
                    public float Time;
                }
            }

            #endregion

            #region ObjectCreation

            /// <summary>
            ///     Object creation.
            ///     FE 2E 17 00 40 0D 01 00 00 16 44 00 00 00 00
            ///     FE 6B 17 00 40 0D 01 00 00 C8 43 00 00 00 00
            ///     FE 6D 17 00 40 0D 01 00 00 C8 43 00 00 00 00
            ///     FE 6F 17 00 40 0D 01 00 00 C8 43 00 00 00 00
            ///     FE 71 17 00 40 0D 01 00 00 C8 43 00 00 00 00
            /// </summary>
            public static class ObjectCreation
            {
                public static byte SubHeader = (byte) MultiPacketType.ObjectCreation;

                public static ReturnStruct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var pStruct = new ReturnStruct();
                    pStruct.NetworkId = packet.ReadInteger(1);

                    pStruct.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(pStruct.NetworkId);
                    pStruct.UnknownFloat = packet.ReadFloat(7);
                    pStruct.UnknownFloat2 = packet.ReadFloat();

                    return pStruct;
                }

                public struct ReturnStruct
                {
                    public int NetworkId;
                    public Obj_AI_Base Unit;
                    public float UnknownFloat;
                    public float UnknownFloat2;
                }
            }

            #endregion

            #region AddBuff

            /// <summary>
            ///     Buff given by new turrets on SR.
            /// </summary>
            public static class AddBuff
            {
                public static byte SubHeader = (byte) MultiPacketType.AddBuff;

                public static ReturnStruct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var pStruct = new ReturnStruct();
                    pStruct.NetworkId = packet.ReadInteger(1);

                    pStruct.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(pStruct.NetworkId);
                    pStruct.BuffType = packet.ReadByte(7);
                    pStruct.State = packet.ReadByte();

                    return pStruct;
                }

                public struct ReturnStruct
                {
                    public byte BuffType; //or slot
                    public int NetworkId;
                    public byte State; //0,1,2,3
                    public Obj_AI_Base Unit;
                }
            }

            #endregion

            #region Unknown

            #region Unknown100

            /// <summary>
            ///     Unknown, ?? "Marker"
            ///     Struct from ida
            /// </summary>
            public static class Unknown100
            {
                public static byte SubHeader = (byte) MultiPacketType.Unknown100;

                public static GamePacket Encoded(ReturnStruct pStruct)
                {
                    var packet = BasePacket(SubHeader);
                    packet.WriteInteger(pStruct.UnknownNetworkId);
                    packet.WriteByte(0);
                    packet.WriteFloat(pStruct.UnknownFloats[0]);
                    packet.WriteFloat(pStruct.UnknownFloats[1]);
                    packet.WriteFloat(pStruct.UnknownFloats[2]);

                    return packet;
                }

                public static ReturnStruct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var unknownNetworkId = packet.ReadInteger(7);
                    packet.Position = 12;
                    // these floats are probably a position
                    var unknownFloat1 = packet.ReadFloat();
                    var unknownFloat2 = packet.ReadFloat();
                    var unknownFloat3 = packet.ReadFloat();
                    return new ReturnStruct
                    {
                        UnknownNetworkId = unknownNetworkId,
                        UnknownFloats = new[] { unknownFloat1, unknownFloat2, unknownFloat3 }
                    };
                }

                public struct ReturnStruct
                {
                    public float[] UnknownFloats;
                    public int UnknownNetworkId;

                    public ReturnStruct(int networkId, float[] floats)
                    {
                        UnknownNetworkId = networkId;
                        UnknownFloats = floats;
                    }
                }
            }

            #endregion

            #region Unknown101

            /// <summary>
            ///     Unknown
            ///     Struct from ida, this packet almost identical to 0xD7/0x102
            /// </summary>
            public static class Unknown101
            {
                public static byte SubHeader = (byte) MultiPacketType.Unknown101;

                public static ReturnStruct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var unknownNetworkId = packet.ReadInteger(7);
                    var unknownByte = packet.ReadByte();
                    return new ReturnStruct { UnknownNetworkId = unknownNetworkId, UnknownByte = unknownByte };
                }

                public struct ReturnStruct
                {
                    public byte UnknownByte;
                    public int UnknownNetworkId;
                }
            }

            #endregion

            #region Unknown102

            /// <summary>
            ///     Unknown
            ///     Struct from ida, this packet almost identical to 0xD7/0x101
            /// </summary>
            public static class Unknown102
            {
                public static byte SubHeader = (byte) MultiPacketType.Unknown102;

                public static ReturnStruct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var unknownNetworkId = packet.ReadInteger(7);
                    var unknownByte = packet.ReadByte();
                    return new ReturnStruct { UnknownNetworkId = unknownNetworkId, UnknownByte = unknownByte };
                }

                public struct ReturnStruct
                {
                    public byte UnknownByte;
                    public int UnknownNetworkId;
                }
            }

            #endregion

            #region Unknown104

            /// <summary>
            ///     Unknown
            ///     Struct from ida, this packet is related to spell slots.
            ///     FE 05 00 00 40 04 01 01 03 00 00 00 00 00 16 C3
            ///     FE 05 00 00 40 04 01 01 03 00 00 00 00 00 00 00
            /// </summary>
            public static class Unknown104
            {
                public static byte SubHeader = (byte) MultiPacketType.Unknown104;

                public static ReturnStruct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var unknownByte = packet.ReadByte(7);
                    var slot = packet.ReadByte(8);
                    return new ReturnStruct { UnknownByte = unknownByte, Slot = (SpellSlot) slot };
                }

                public struct ReturnStruct
                {
                    public SpellSlot Slot;
                    public byte UnknownByte;
                    public int UnknownNetworkId;
                }
            }

            #endregion

            #region Unknown10C

            /// <summary>
            ///     Like undo.
            /// </summary>
            public static class Unknown10C
            {
                public static byte SubHeader = (byte) MultiPacketType.Unknown10C;

                public static List<RefundItem> Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 11 };
                    var itemList = new List<RefundItem>();

                    for (var i = 0; i <= 7; i++)
                    {
                        var inventorySlot = packet.ReadByte();
                        var spellSlot = inventorySlot + SpellSlot.Item1;
                        var stack = packet.ReadByte();
                        var charge = packet.ReadByte();
                        var itemId = packet.ReadShort();
                        packet.Position += 2;
                        var pos = packet.Position;

                        if (itemId == 0)
                        {
                            continue;
                        }

                        var cd = packet.ReadFloat(packet.Position + 59); //this is prob wrong
                        var totalCd = packet.ReadFloat(packet.Position + 29); //this is prob wrong
                        packet.Position = pos;

                        itemList.Add(new RefundItem(itemId, inventorySlot, spellSlot, stack, charge, cd, totalCd));
                    }

                    return itemList;
                }

                public class RefundItem
                {
                    public int Charge;
                    public float CurrentCooldown;
                    public byte InventorySlot;
                    public int ItemId;
                    public SpellSlot SpellSlot;
                    public int Stack;
                    public float TotalCooldown;

                    public RefundItem(short itemId,
                        byte inventorySlot,
                        SpellSlot slot,
                        int stack,
                        int charge,
                        float cd,
                        float totalCd)
                    {
                        ItemId = itemId;
                        InventorySlot = inventorySlot;
                        SpellSlot = slot;
                        Stack = stack;
                        Charge = charge;
                        CurrentCooldown = cd;
                        TotalCooldown = totalCd;
                    }
                }
            }

            #endregion

            #region Unknown115

            /// <summary>
            ///     Unknown
            ///     Struct from ida
            /// </summary>
            public static class Unknown115
            {
                public static byte SubHeader = (byte) MultiPacketType.Unknown115;

                public static ReturnStruct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var unknownNetworkId = packet.ReadInteger(7);
                    var unknownByte = packet.ReadByte();
                    return new ReturnStruct { UnknownNetworkId = unknownNetworkId, UnknownByte = unknownByte };
                }

                public struct ReturnStruct
                {
                    public byte UnknownByte;
                    public int UnknownNetworkId;
                }
            }

            #endregion

            #region Unknown122

            /// <summary>
            ///     Unknown
            ///     Struct from ida, "timers"
            /// </summary>
            public static class Unknown122
            {
                public static byte SubHeader = (byte) MultiPacketType.Unknown122;

                public static ReturnStruct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new ReturnStruct();

                    var Int = packet.ReadInteger(7); // this is either a hash or two shorts
                    var Float = packet.ReadFloat(); // usually 60
                    result.Name = packet.ReadString(19);
                    var unknownByte1 = packet.ReadByte(83); //camp id?
                    var unknownByte2 = packet.ReadByte(84);
                    result.CampId = packet.ReadByte(85);
                    result.UnknownNetworkId = packet.ReadInteger(86);
                    result.UnknownFloat = packet.ReadFloat(90);

                    return result;
                }

                public struct ReturnStruct
                {
                    public byte CampId;
                    public string Name;
                    public float UnknownFloat;
                    public int UnknownNetworkId;
                }
            }

            #endregion

            #region SpawnTurret

            /// <summary>
            ///     SpawnTurret
            ///     Struct from ida
            ///     FE 00 00 00 00 22 01 6E F7 9C 45 00 00 70 42 28 12 F3 45 53 68 72 69 6E 65 00 00 00 00 00 00 00 00 00 00 00 00 00
            ///     00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
            ///     00 00 00 00 00 00 00 66 4A 00 00 00 00 00 00 00 00 00
            ///     FE 00 00 00 00 22 01 97 6F 0A 46 00 00 70 42 DA 60 F3 45 53 68 72 69 6E 65 00 00 00 00 00 00 00 00 00 00 00 00 00
            ///     00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
            ///     00 00 00 00 00 00 00 67 4A 00 00 00 00 00 00 00 00 00
            ///     FE 00 00 00 00 22 01 8C 95 D9 45 00 00 70 42 AE 97 7F 45 53 68 72 69 6E 65 00 00 00 00 00 00 00 00 00 00 00 00 00
            ///     00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
            ///     00 00 00 00 00 00 00 68 4A 00 00 00 00 00 00 00 00 00
            /// </summary>
            public static class SpawnTurret
            {
                public static byte SubHeader = (byte) MultiPacketType.SpawnTurret;

                public static void Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var unknownNetworkId = packet.ReadInteger(7); // Node ID??
                    var unknownNetworkId2 = packet.ReadInteger(); // Object ID??
                    var unknownByte = packet.ReadByte();
                    var name = packet.ReadString();
                    var skin = packet.ReadString(80);
                    var skinID = packet.ReadInteger(144);
                    var unknownShort = packet.ReadShort(165);
                }

                public struct ReturnStruct
                {
                    public float[] UnknownFloats;
                    public int UnknownNetworkId;
                }
            }

            #endregion

            #region NPCDeath

            /// <summary>
            ///     NPCDeath
            ///     Struct from intwars/ida
            /// </summary>
            public static class NPCDeath
            {
                public static byte SubHeader = (byte) MultiPacketType.NPCDeath;

                public static void Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var KilledNetworkId = packet.ReadInteger(1);
                    var KilledUnit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(KilledNetworkId);

                    var KillerNetworkId = packet.ReadInteger(12);
                    var KillerUnit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(KillerNetworkId);

                    var unknownByte = packet.ReadByte();
                    var unknownByte2 = packet.ReadByte();
                    var unknownInt = packet.ReadInteger(); // intwars says flags
                }
            }

            #endregion

            #endregion
        }

        public static class S2C
        {
            #region Ping

            /// <summary>
            ///     RPing Packet. Received when ally team players send a SPing packet.
            /// </summary>
            public static class Ping
            {
                public static byte Header = 0x40;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(0);
                    result.WriteFloat(packetStruct.X);
                    result.WriteFloat(packetStruct.Y);
                    result.WriteInteger(packetStruct.TargetNetworkId);
                    result.WriteInteger(packetStruct.SourceNetworkId);
                    result.WriteByte((byte) packetStruct.Type);
                    var bit = packetStruct.Silent ? (byte) 0 : (byte) 0xFB;
                    result.WriteByte(bit);

                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 5 };
                    return new Struct(
                        packet.ReadFloat(), packet.ReadFloat(), packet.ReadInteger(), packet.ReadInteger(),
                        (PingType) packet.ReadByte(), (packet.ReadByte() & 1) != 1);
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

            #region GainVision

            /// <summary>
            ///     Gets received when a unit leaves FOW.
            /// </summary>
            public static class GainVision
            {
                public static byte Header = 0xAD;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.UnitNetworkId);
                    result.WriteShort(0);
                    result.WriteFloat(packetStruct.MaxHealth);
                    result.WriteFloat(packetStruct.CurrentHealth);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.UnitNetworkId = packet.ReadInteger(1);
                    packet.ReadShort();
                    result.MaxHealth = packet.ReadFloat();
                    result.CurrentHealth = packet.ReadFloat();
                    return result;
                }

                public struct Struct
                {
                    public float CurrentHealth;
                    public float MaxHealth;
                    public int UnitNetworkId;

                    public Struct(int unitNetworkId = 0, float maxHealth = 0, float currentHealth = 0)
                    {
                        UnitNetworkId = unitNetworkId;
                        MaxHealth = maxHealth;
                        CurrentHealth = currentHealth;
                    }
                }
            }

            #endregion

            #region LoseVision

            /// <summary>
            ///     Gets received when a unit enters FOW.
            /// </summary>
            public static class LoseVision
            {
                public static byte Header = 0x51;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.UnitNetworkId);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 1 };
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

            #region EmptyJungleCamp

            /// <summary>
            ///     Gets received when gaining vision of an empty jungle camp.
            /// </summary>
            public static class EmptyJungleCamp
            {
                public static byte Header = 0xC3;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(0);
                    result.WriteInteger(packetStruct.UnitNetworkId);
                    result.WriteInteger(packetStruct.CampId);
                    result.WriteByte(packetStruct.EmptyType);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.UnitNetworkId = packet.ReadInteger(5);
                    result.CampId = packet.ReadInteger();
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

            #region Dash

            /// <summary>
            ///     Gets received when a unit dashes.
            /// </summary>
            public static class Dash
            {
                public static byte Header = 0x64;

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
                    result.Speed = packet.ReadFloat();

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

            #region GameEnd

            /// <summary>
            ///     Gets received when the game ends.
            /// </summary>
            public static class GameEnd
            {
                public static byte Header = 0xC6;

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
                public static byte Header = 0x97;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
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

                    result.NetworkId = packet.ReadInteger(1);
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

            #region Recall

            /// <summary>
            ///     Gets received when a unit starts, aborts or finishes recalling.
            /// </summary>
            [Obsolete("Use Packet.S2C.Teleport class instead.")]
            public static class Recall
            {
                public enum ObjectType
                {
                    Player,
                    Turret,
                    Minion,
                    Ward,
                    Object
                }

                public enum RecallStatus
                {
                    RecallStarted,
                    RecallAborted,
                    RecallFinished,
                    Unknown,
                    TeleportStart,
                    TeleportAbort,
                    TeleportEnd,
                }

                private const int ErrorGap = 100; //in ticks

                public static byte Header = 0xD8;

                private static readonly IDictionary<string, Type> TypeByString = new Dictionary<string, Type>
                {
                    { "Recall", Type.Recall },
                    { "Teleport", Type.Teleport }
                };

                private static readonly Dictionary<int, RecallData> RecallDataByNetworkId =
                    new Dictionary<int, RecallData>();

                public static GamePacket Encoded(Struct packetStruct)
                {
                    //TODO when the packet is fully decoded.
                    return new GamePacket(Header);
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.UnitNetworkId = packet.ReadInteger(5);
                    result.Status = RecallStatus.Unknown;

                    var typeAsString = packet.ReadString(75);
                    var gObject = ObjectManager.GetUnitByNetworkId<GameObject>(result.UnitNetworkId);

                    if (gObject == null || !gObject.IsValid)
                    {
                        return result;
                    }

                    if (gObject is Obj_AI_Hero)
                    {
                        var unit = (Obj_AI_Hero) gObject;

                        if (!unit.IsValid || unit.Spellbook.GetSpell(SpellSlot.Recall) == null)
                        {
                            return result;
                        }

                        result.Type = ObjectType.Player;

                        if (!RecallDataByNetworkId.ContainsKey(result.UnitNetworkId))
                        {
                            RecallDataByNetworkId[result.UnitNetworkId] = new RecallData { Type = Type.Unknown };
                        }

                        if (string.IsNullOrEmpty(typeAsString))
                        {
                            switch (RecallDataByNetworkId[result.UnitNetworkId].Type)
                            {
                                case Type.Recall:
                                    if (Environment.TickCount - RecallDataByNetworkId[result.UnitNetworkId].Start <
                                        RecallDataByNetworkId[result.UnitNetworkId].Duration - ErrorGap)
                                    {
                                        result.Status = RecallStatus.RecallAborted;
                                    }
                                    else
                                    {
                                        result.Status = RecallStatus.RecallFinished;
                                    }
                                    break;
                                case Type.Teleport:
                                    if (Environment.TickCount - RecallDataByNetworkId[result.UnitNetworkId].Start <
                                        RecallDataByNetworkId[result.UnitNetworkId].Duration - ErrorGap)
                                    {
                                        result.Status = RecallStatus.TeleportAbort;
                                    }
                                    else
                                    {
                                        result.Status = RecallStatus.TeleportEnd;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            if (TypeByString.ContainsKey(typeAsString))
                            {
                                switch (TypeByString[typeAsString])
                                {
                                    case Type.Recall:
                                        result.Status = RecallStatus.RecallStarted;
                                        result.Duration = Utility.GetRecallTime(packet.ReadString(139));
                                        RecallDataByNetworkId[result.UnitNetworkId] = new RecallData
                                        {
                                            Type = Type.Recall,
                                            Duration = result.Duration,
                                            Start = Environment.TickCount
                                        };
                                        break;
                                    case Type.Teleport:
                                        result.Status = RecallStatus.TeleportStart;
                                        result.Duration = 3500;
                                        RecallDataByNetworkId[result.UnitNetworkId] = new RecallData
                                        {
                                            Type = Type.Teleport,
                                            Duration = result.Duration,
                                            Start = Environment.TickCount
                                        };
                                        break;
                                }
                            }
                        }
                    }
                    else if (gObject is Obj_AI_Turret)
                    {
                        result.Type = ObjectType.Turret;
                        result.Status = string.IsNullOrEmpty(typeAsString)
                            ? RecallStatus.TeleportEnd
                            : RecallStatus.TeleportStart;
                    }
                    else if (gObject is Obj_AI_Minion)
                    {
                        result.Type = ObjectType.Object;

                        if (gObject.Name.Contains("Minion"))
                        {
                            result.Type = ObjectType.Minion;
                        }
                        if (gObject.Name.Contains("Ward"))
                        {
                            result.Type = ObjectType.Ward;
                        }

                        result.Status = string.IsNullOrEmpty(typeAsString)
                            ? RecallStatus.TeleportEnd
                            : RecallStatus.TeleportStart;
                    }
                    else
                    {
                        result.Type = ObjectType.Object;
                    }

                    return result;
                }

                internal struct RecallData
                {
                    public Type Type { get; set; }
                    public int Start { get; set; }
                    public int Duration { get; set; }
                }

                public struct Struct
                {
                    public int Duration;
                    public RecallStatus Status;
                    public ObjectType Type;
                    public int UnitNetworkId;

                    public Struct(int unitNetworkId, RecallStatus status, ObjectType type, int duration)
                    {
                        UnitNetworkId = unitNetworkId;
                        Status = status;
                        Type = type;
                        Duration = duration;
                    }
                }

                internal enum Type
                {
                    Recall,
                    Teleport,
                    Unknown
                }
            }

            #endregion

            #region Teleport

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
                    int GetDuration(byte[] packetData);
                }

                internal class RecallTeleport : ITeleport
                {
                    public Type Type
                    {
                        get { return Type.Recall; }
                    }

                    public int GetDuration(byte[] packetData)
                    {
                        var p = new GamePacket(packetData);
                        return Utility.GetRecallTime(p.ReadString(139));
                    }
                }

                internal class TeleportTeleport : ITeleport
                {
                    public Type Type
                    {
                        get { return Type.Teleport; }
                    }

                    public int GetDuration(byte[] packetData)
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

                    public int GetDuration(byte[] packetData)
                    {
                        return 1500;
                    }
                }

                internal class ShenTeleport : ITeleport
                {
                    public Type Type
                    {
                        get { return Type.TwistedFate; }
                    }

                    public int GetDuration(byte[] packetData)
                    {
                        return 3000;
                    }
                }


                public static byte Header = 0xD8;

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

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct
                    {
                        UnitNetworkId = packet.ReadInteger(5),
                        Status = Status.Unknown,
                        Type = Type.Unknown
                    };

                    string typeAsString = packet.ReadString(75);
                    var gameObject = ObjectManager.GetUnitByNetworkId<GameObject>(result.UnitNetworkId);

                    if (gameObject == null)
                    {
                        return result;
                    }

                    var hero = gameObject as Obj_AI_Hero;
                    if (hero == null || !hero.IsValid)
                    {
                        return result;
                    }

                    if (!RecallDataByNetworkId.ContainsKey(result.UnitNetworkId))
                    {
                        RecallDataByNetworkId[result.UnitNetworkId] = new TeleportData {Type = Type.Unknown};
                    }


                    if (!string.IsNullOrEmpty(typeAsString))
                    {
                        if (TypeByString.ContainsKey(typeAsString))
                        {
                            ITeleport teleportMethod = TypeByString[typeAsString];

                            int duration = teleportMethod.GetDuration(data);
                            Type type = teleportMethod.Type;
                            int time = Environment.TickCount;

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
                        bool shorter = Environment.TickCount - RecallDataByNetworkId[result.UnitNetworkId].Start <
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

            #region PlayEmote

            /// <summary>
            ///     Gets received when an unit uses an emote.
            /// </summary>
            public static class PlayEmote
            {
                public static byte Header = 0x42;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteByte(packetStruct.EmoteId);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    result.NetworkId = packet.ReadInteger(1);
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

            #region Damage

            /// <summary>
            ///     Packet received when a unit deals damage.
            /// </summary>
            public class Damage
            {
                public static byte Header = 0x65;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var packet = new GamePacket(Header);

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

                    packet.Position = 1;
                    result.TargetNetworkId = packet.ReadInteger();
                    result.Type = (DamageTypePacket) packet.ReadByte();
                    result.Unknown = packet.ReadShort();
                    result.DamageAmount = packet.ReadFloat();
                    result.TargetNetworkIdCopy = packet.ReadInteger();
                    result.SourceNetworkId = packet.ReadInteger();

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

            #region PlayerDisconnect

            /// <summary>
            ///     Packet received on player disconnect.
            /// </summary>
            public class PlayerDisconnect
            {
                public static byte Header = 0x98;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();


                    packet.Position = 5;
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

            #region SetCoodlown

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

            #region SellItemAns

            /// <summary>
            ///     Packet received on selling item.
            /// </summary>
            public class SellItemAns
            {
                public static byte Header = 0x0B;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
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

            #region SwapItemAns

            /// <summary>
            ///     Packet received on swapping item.
            /// </summary>
            public class SwapItemAns
            {
                public static byte Header = 0x3E;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
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

            #region ChangeSpellSlot

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

            #region LevelUp

            /// <summary>
            ///     Received on hero level up.
            /// </summary>
            public class LevelUp
            {
                public static byte Header = 0x3F;

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
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
        }
    }
}
