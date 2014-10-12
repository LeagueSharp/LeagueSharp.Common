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

#endregion

namespace LeagueSharp.Common
{
    public static class Packet
    {
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

        public enum PingType
        {
            Normal = 1,
            Fallback = 5,
            EnemyMissing = 3,
            Danger = 2,
            OnMyWay = 4,
            AssistMe = 6,
            NormalSound = 177,
            DangerSound = 178,
            EnemyMissingSound = 179,
            OnMyWaySound = 180,
            FallbackSound = 181,
            AssistMeSound = 182,
        }

        public static class C2S
        {
            #region Ping

            /// <summary>
            /// Ping Packet. Sent by the client when pings are sent.
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
            /// Packet sent when leveling up a spell.
            /// </summary>
            public static class LevelUpSpell
            {
                public static byte Header = 0x39;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteByte((byte) packetStruct.Slot);
                    result.WriteByte(0x00);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 1 };
                    return new Struct(packet.ReadInteger(), (SpellSlot) packet.ReadByte());
                }

                public struct Struct
                {
                    public int NetworkId;
                    public SpellSlot Slot;

                    public Struct(int networkId = -1, SpellSlot slot = SpellSlot.Q)
                    {
                        NetworkId = (networkId == -1) ? ObjectManager.Player.NetworkId : networkId;
                        Slot = slot;
                    }
                }
            }

            #endregion

            #region Move

            /// <summary>
            /// Packet sent when issuing GameObjectOrder's.
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
                    packet.ReadByte();
                    result.UnitNetworkId = packet.ReadInteger();
                    return result;
                }

                public struct Struct
                {
                    public byte MoveType;
                    public int SourceNetworkId;
                    public int TargetNetworkId;
                    public int UnitNetworkId;
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
                    }
                }
            }

            #endregion

            #region Cast

            /// <summary>
            /// Packet sent when casting spells.
            /// </summary>
            public static class Cast
            {
                public static byte Header = 0x9A;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.SourceNetworkId);
                    result.WriteByte(GetSpellByte(packetStruct.Slot));
                    result.WriteByte((byte) packetStruct.Slot);
                    result.WriteFloat(packetStruct.FromX);
                    result.WriteFloat(packetStruct.FromY);
                    result.WriteFloat(packetStruct.ToX);
                    result.WriteFloat(packetStruct.ToY);
                    result.WriteInteger(packetStruct.TargetNetworkId);
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
                        case (SpellSlot) 0x64:
                            return 0xEF;
                        case (SpellSlot) 0x65:
                            return 0xEF;
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
                    public int TargetNetworkId;
                    public float ToX;
                    public float ToY;

                    public Struct(int targetNetworkId = 0,
                        SpellSlot slot = SpellSlot.Q,
                        int sourceNetworkId = -1,
                        float fromX = 0f,
                        float fromY = 0f,
                        float toX = 0f,
                        float toY = 0f)
                    {
                        SourceNetworkId = (sourceNetworkId == -1) ? ObjectManager.Player.NetworkId : sourceNetworkId;
                        Slot = slot;
                        FromX = fromX;
                        FromY = fromY;
                        ToX = toX;
                        ToY = toY;
                        TargetNetworkId = targetNetworkId;
                    }
                }
            }

            #endregion

            #region ChargedCast

            /// <summary>
            /// Packet sent when casting charged spells second cast.
            /// </summary>
            public static class ChargedCast
            {
                public static byte Header = 0xE6;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.SourceNetworkId);
                    result.WriteByte((byte)(packetStruct.Slot == SpellSlot.Q ? 0xEA : 0x9C));
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
            /// Packet sent when buying items.
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
            /// Packet sent when selling items.
            /// </summary>
            public static class SellItem
            {
                public static byte Header = 0x09;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteByte(packetStruct.SlotByte);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 1;
                    result.NetworkId = packet.ReadInteger();
                    result.SlotByte = packet.ReadByte();
                    result.SlotId = (SpellSlot) (result.SlotByte - 0x80 + (byte) SpellSlot.Item1);
                    return result;
                }

                public struct Struct
                {
                    public int NetworkId;
                    public byte SlotByte;
                    public SpellSlot SlotId;

                    public Struct(SpellSlot slotId, int networkId = -1)
                    {
                        SlotId = slotId;
                        SlotByte = (byte) slotId;
                        if (SlotByte >= (byte) SpellSlot.Item1 && SlotByte <= (byte) SpellSlot.Trinket)
                        {
                            SlotByte = (byte) (0x80 + SlotByte - (byte) SpellSlot.Item1);
                        }
                        NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }

                    public Struct(byte slotByte, int networkId = -1)
                    {
                        SlotByte = slotByte;
                        SlotId = (SpellSlot) slotByte;
                        if (slotByte >= 0x80 && slotByte <= 0x85)
                        {
                            SlotId = (SpellSlot) ((byte) SpellSlot.Item1 + slotByte - 0x80);
                        }
                        NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }
                }
            }

            #endregion

            #region SwapItem

            /// <summary>
            /// Packet sent when swapping items.
            /// </summary>
            public static class SwapItem
            {
                public static byte Header = 0x21;

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
            /// Packet sent when sending emotes.
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
            /// Packet sent when interacting with Thresh Lantern and Dominion capturing.
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
            /// Packet sent when left clicking a target.
            /// </summary>
            public static class SetTarget
            {
                public static byte Header = 0xAF;

                public static Struct Decoded(byte[] data)
                {
                    var result = new Struct { NetworkId = new GamePacket(data).ReadInteger(5) };
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
        }

        public static class S2C
        {
            #region Ping

            /// <summary>
            /// RPing Packet. Received when ally team players send a SPing packet.
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
                    result.WriteByte((byte)packetStruct.Type);
                    result.WriteByte(0xFB);

                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 5 };
                    return new Struct(
                        packet.ReadFloat(), packet.ReadFloat(), packet.ReadInteger(), packet.ReadInteger(),
                        (PingType) packet.ReadByte());
                }

                public struct Struct
                {
                    public int SourceNetworkId;
                    public int TargetNetworkId;
                    public PingType Type;
                    public float X;
                    public float Y;

                    public Struct(float x = 0f,
                        float y = 0f,
                        int targetNetworkId = 0,
                        int sourceNetworkId = 0,
                        PingType type = PingType.Normal)
                    {
                        X = x;
                        Y = y;
                        TargetNetworkId = targetNetworkId;
                        SourceNetworkId = sourceNetworkId;
                        Type = type;
                    }
                }
            }

            #endregion

            #region GainVision

            /// <summary>
            /// Gets received when a unit leaves FOW.
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
            /// Gets received when a unit enters FOW.
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
            /// Gets received when gaining vision of an empty jungle camp.
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

            #region Dash

            /// <summary>
            /// Gets received when a unit dashes.
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
            /// Gets received when the game ends.
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

            ///<summary>
            /// Gets received when a tower starts targeting a unit
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
            /// Gets received when the model changes.
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
            /// Gets received when a unit starts, aborts or finishes recalling.
            /// </summary>
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

                public static byte Header = 0xD8;
                public static readonly Dictionary<int, int> RecallT = new Dictionary<int, int>();
                public static readonly Dictionary<int, int> TPT = new Dictionary<int, int>();

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
                    var b = packet.ReadByte(184);
                    var b2 = packet.ReadByte(81);
                    result.Status = RecallStatus.Unknown;

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
                        var duration = Utility.GetRecallTime(unit);

                        result.Duration = duration;

                        if (!RecallT.ContainsKey(result.UnitNetworkId))
                        {
                            RecallT.Add(result.UnitNetworkId, 0);
                        }

                        if (!TPT.ContainsKey(result.UnitNetworkId))
                        {
                            TPT.Add(result.UnitNetworkId, 0);
                        }


                        if (b2 != 0 ||
                            TPT.ContainsKey(result.UnitNetworkId) &&
                            Environment.TickCount - TPT[result.UnitNetworkId] < 4500)
                        {
                            if (b2 != 0)
                            {
                                TPT[result.UnitNetworkId] = Environment.TickCount;
                                result.Status = RecallStatus.TeleportStart;
                            }

                            else if (Environment.TickCount - TPT[result.UnitNetworkId] < 3500)
                            {
                                result.Status = RecallStatus.TeleportAbort;
                                TPT[result.UnitNetworkId] = 0;
                            }
                            else if (Environment.TickCount - TPT[result.UnitNetworkId] < 4500)
                            {
                                result.Status = RecallStatus.TeleportEnd;
                                TPT[result.UnitNetworkId] = 0;
                            }
                        }
                        else
                        {
                            switch (b)
                            {
                                case 4:
                                    if (RecallT.ContainsKey(result.UnitNetworkId))
                                    {
                                        if (Environment.TickCount - RecallT[result.UnitNetworkId] < duration - 1200)
                                        {
                                            result.Status = RecallStatus.RecallAborted;
                                        }
                                        else if (Environment.TickCount - RecallT[result.UnitNetworkId] < duration + 1000)
                                        {
                                            result.Status = RecallStatus.RecallFinished;
                                        }
                                        RecallT[result.UnitNetworkId] = 0;
                                    }
                                    break;
                                case 6:
                                    result.Status = RecallStatus.RecallStarted;
                                    RecallT[result.UnitNetworkId] = Environment.TickCount;
                                    break;
                            }
                        }
                    }
                    else if (gObject is Obj_AI_Turret)
                    {
                        result.Type = ObjectType.Turret;
                        result.Status = b2 != 0 ? RecallStatus.TeleportStart : RecallStatus.TeleportEnd;
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

                        result.Status = b2 != 0 ? RecallStatus.TeleportStart : RecallStatus.TeleportEnd;
                    }
                    else
                    {
                        result.Type = ObjectType.Object;
                    }


                    return result;
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
            }

            #endregion

            #region PlayEmote

            /// <summary>
            /// Gets received when an unit uses an emote.
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
            /// Packet received when a unit deals damage.
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
                    public short Unknown;
                    public DamageTypePacket Type;

                    public Struct(float damageAmount,
                        int sourceNetworkId,
                        int targetNetworkId,
                        int targetNetworkIdCopy,
                        DamageTypePacket type, short unknown)
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
            /// Packet received print float text.
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
            /// Packet received for debug message.
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
            /// Packet highlights unit.
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
            /// Packet remove highlights unit.
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
            /// Packet received on player disconnect.
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
        }
    }
}
