using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace LeagueSharp.Common
{
    public static class Packet
    {
        public enum PingType
        {
            Normal = 1,
            Fallback = 5,
            EnemyMissing = 3,
            Danger = 2,
            OnMyWay = 4,
            AssistMe = 6,
        }

        static Packet()
        {
            if (Common.isInitialized == false)
            {
                Common.InitializeCommonLib();
            }
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
                    result.WriteByte((byte)packetStruct.Type);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    packet.Position = 5;
                    return new Struct(packet.ReadFloat(), packet.ReadFloat(), packet.ReadInteger(),
                        (PingType)packet.ReadByte());
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
                    result.WriteFloat(packetStruct.NetworkId);
                    result.WriteByte((byte)packetStruct.Slot);
                    result.WriteByte(0);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    packet.Position = 1;
                    return new Struct(packet.ReadInteger(), (SpellSlot)packet.ReadByte());
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

                    public Struct(float x = 0f, float y = 0f, byte moveType = 2, int targetNetworkId = 0,
                        int unitNetworkId = -1, int sourceNetworkId = -1)
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
                    result.WriteByte((byte)packetStruct.Slot);
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
                    result.Slot = (SpellSlot)packet.ReadByte();
                    result.FromX = packet.ReadFloat();
                    result.FromY = packet.ReadFloat();
                    result.ToX = packet.ReadFloat();
                    result.ToY = packet.ReadFloat();
                    return result;
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

                    public Struct(int targetNetworkId = 0, SpellSlot slot = SpellSlot.Q, int sourceNetworkId = -1,
                        float fromX = 0f, float fromY = 0f, float toX = 0f, float toY = 0f)
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
                    result.WriteByte((byte)packetStruct.Slot);
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
                    result.Slot = (SpellSlot)packet.ReadByte();
                    result.ToX = packet.ReadFloat();
                    result.ToY = packet.ReadFloat();
                    result.ToZ = packet.ReadFloat();
                    return result;
                }

                public struct Struct
                {
                    public int SourceNetworkId;
                    public SpellSlot Slot;
                    public float ToX;
                    public float ToY;
                    public float ToZ;
                    public Struct(SpellSlot slot, float toX = 0f, float toY = 0f, float toZ = 0f, int sourceNetworkId = -1)
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
        }

        public static class S2C
        {
            #region Ping

            /// <summary>
            /// RPing Packet. Received when ally team players send a SPing packet.
            /// </summary>
            public static class Ping
            {
                public static byte Header = 0x3F;

                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(0);
                    result.WriteFloat(packetStruct.X);
                    result.WriteFloat(packetStruct.Y);
                    result.WriteInteger(packetStruct.TargetNetworkId);
                    result.WriteInteger(packetStruct.SourceNetworkId);
                    result.WriteByte((byte)packetStruct.Type);
                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    packet.Position = 5;
                    return new Struct(packet.ReadFloat(), packet.ReadFloat(), packet.ReadInteger(), packet.ReadInteger(),
                        (PingType)packet.ReadByte());
                }

                public struct Struct
                {
                    public int SourceNetworkId;
                    public int TargetNetworkId;
                    public PingType Type;
                    public float X;
                    public float Y;

                    public Struct(float x = 0f, float y = 0f, int targetNetworkId = 0, int sourceNetworkId = 0,
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

                    packet.Position = 1;
                    result.UnitNetworkId = packet.ReadInteger();
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
                    var packet = new GamePacket(data);
                    packet.Position = 1;
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
                public static byte Header = 0xC2;

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
                    packet.Position = 5;
                    result.UnitNetworkId = packet.ReadInteger();
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

                    packet.Position = 12;
                    result.UnitNetworkId = packet.ReadInteger();
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
                    packet.Position = 5;
                    result.Winner = packet.ReadByte();
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
                        if(i < packetStruct.ModelName.Length)
                            result.WriteByte((byte) packetStruct.ModelName[i]);
                        else
                            result.WriteByte(0x00);

                    return result;
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 1;
                    result.NetworkId = packet.ReadInteger();
                    result.Id = packet.ReadInteger();
                    result.BOk = packet.ReadByte() == 0x01;
                    result.SkinId = packet.ReadInteger();

                    return result;
                }

                public struct Struct
                {
                    public int NetworkId;
                    public int Id;
                    public bool BOk;
                    public int SkinId;
                    public string ModelName;

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
                public static byte Header = 0xD8;
                internal static readonly Dictionary<int, int> RecallT = new Dictionary<int, int>();
                internal static readonly Dictionary<int, int> TPT = new Dictionary<int, int>();

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

                public enum ObjectType
                {
                    Player,
                    Turret,
                    Minion,
                    Ward,
                    Object
                }

                public static GamePacket Encoded(Struct packetStruct)
                {
                    //TODO when the packet is fully decoded.
                    return new GamePacket(Header);
                }

                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    packet.Position = 5;
                    result.UnitNetworkId = packet.ReadInteger();
                    packet.Position = 112;
                    var b = packet.ReadByte();
                    packet.Position = 81;
                    var b2 = packet.ReadByte();
                    result.Status = RecallStatus.Unknown;

                    var gObject = ObjectManager.GetUnitByNetworkId<GameObject>(result.UnitNetworkId);
                    if (gObject != null)
                    {
                        if (gObject is Obj_AI_Hero)
                        {
                            var unit = (Obj_AI_Hero)gObject;
                            if (unit.IsValid && unit.Spellbook.GetSpell(SpellSlot.Recall) != null)
                            {
                                result.Type = ObjectType.Player;
                                var rName = unit.Spellbook.GetSpell(SpellSlot.Recall).Name;
                                var duration = 0;
                                switch (rName)
                                {
                                    case "Recall": duration = 8000; break;
                                    case "RecallImproved": duration = 7000; break;
                                    case "OdinRecall": duration = 4500; break;
                                    case "OdinRecallImproved": duration = 4000; break;
                                }
                                result.Duration = duration;

                                if (!RecallT.ContainsKey(result.UnitNetworkId))
                                    RecallT.Add(result.UnitNetworkId, 0);

                                if (!TPT.ContainsKey(result.UnitNetworkId))
                                    TPT.Add(result.UnitNetworkId, 0);


                                if (b2 != 0 || TPT.ContainsKey(result.UnitNetworkId) && Environment.TickCount - TPT[result.UnitNetworkId] < 4500)
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
                                    if (b == 4)
                                    {
                                        if (RecallT.ContainsKey(result.UnitNetworkId))
                                        {
                                            if (Environment.TickCount - RecallT[result.UnitNetworkId] < duration - 1000)
                                                result.Status = RecallStatus.RecallAborted;
                                            else if (Environment.TickCount - RecallT[result.UnitNetworkId] < duration + 1000)
                                                result.Status = RecallStatus.RecallFinished;
                                            RecallT[result.UnitNetworkId] = 0;
                                        }
                                    }
                                    else if (b == 6)
                                    {
                                        result.Status = RecallStatus.RecallStarted;
                                        RecallT[result.UnitNetworkId] = Environment.TickCount;
                                    }
                            }
                        }
                        else if (gObject is Obj_AI_Turret)
                        {
                            if (gObject.IsValid)
                            {
                                result.Type = ObjectType.Turret;

                                if (b2 != 0)
                                {
                                    result.Status = RecallStatus.TeleportStart;
                                }
                                else
                                {
                                    result.Status = RecallStatus.TeleportEnd;
                                }
                            }
                        }
                        else if (gObject is Obj_AI_Minion)
                        {
                            if (gObject.IsValid)
                            {
                                result.Type = ObjectType.Object;
                                if(gObject.Name.Contains("Minion"))
                                    result.Type = ObjectType.Minion;
                                if (gObject.Name.Contains("Ward"))
                                    result.Type = ObjectType.Ward;

                                if (b2 != 0)
                                {
                                    result.Status = RecallStatus.TeleportStart;
                                }
                                else
                                {
                                    result.Status = RecallStatus.TeleportEnd;
                                }
                            }
                        }
                        else
                        {
                            if (gObject.IsValid)
                            {
                                result.Type = ObjectType.Object;
                            }
                        }
                    }
                        
                                           
                    return result;
                }

                public struct Struct
                {
                    public int UnitNetworkId;
                    public RecallStatus Status;
                    public ObjectType Type;
                    public int Duration;
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
        }
    }
}