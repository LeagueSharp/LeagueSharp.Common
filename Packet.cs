namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SharpDX;

    /// <summary>
    ///     Helps in decoding packets. This is not currently updated past 4.21!
    /// </summary>
    [Obsolete("Jodus wears dirty socks", false)]
    public static class Packet
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="Packet" /> class.
        /// </summary>
        static Packet()
        {
            Console.WriteLine(
                @"LeagueSharp.Common.Packet will be removed soon, use LeagueSharp.Network.Packets instead");
        }

        #endregion

        #region Enums

        /// <summary>
        ///     The states of actions.
        /// </summary>
        public enum ActionStates
        {
            /// <summary>
            ///     The begin recall state
            /// </summary>
            BeginRecall = 111207118,

            /// <summary>
            ///     The finish recall state
            /// </summary>
            FinishRecall = 97690254,
        }

        /// <summary>
        ///     The type of attack.
        /// </summary>
        public enum AttackTypePacket
        {
            /// <summary>
            ///     Circular skillshots.
            /// </summary>
            Circular = 0,

            /// <summary>
            ///     Cone and Skillshot spells
            /// </summary>
            ConeSkillShot = 1,

            /// <summary>
            ///     Targeted spells and AAs
            /// </summary>
            TargetedAA = 2,
        }

        /// <summary>
        ///     The type of damage.
        /// </summary>
        public enum DamageTypePacket
        {
            /// <summary>
            ///     Magical Damage (AP)
            /// </summary>
            Magical = 4,

            /// <summary>
            ///     A Critical Attack
            /// </summary>
            CriticalAttack = 11,

            /// <summary>
            ///     Physical Damage (AD)
            /// </summary>
            Physical = 12,

            /// <summary>
            ///     True Damage
            /// </summary>
            True = 36,
        }

        /// <summary>
        ///     Types of emotes.
        /// </summary>
        public enum Emotes
        {
            /// <summary>
            ///     Dance
            /// </summary>
            Dance = 0x00,

            /// <summary>
            ///     Joke
            /// </summary>
            Joke = 0x03,

            /// <summary>
            ///     Taunt
            /// </summary>
            Taunt = 0x01,

            /// <summary>
            ///     Laugh
            /// </summary>
            Laugh = 0x02,
        }

        /// <summary>
        ///     Type of floating text on a hero.
        /// </summary>
        public enum FloatTextPacket
        {
            /// <summary>
            ///     Invulnerable
            /// </summary>
            Invulnerable,

            /// <summary>
            ///     Special
            /// </summary>
            Special,

            /// <summary>
            ///     Heal
            /// </summary>
            Heal,

            /// <summary>
            ///     Mana heal
            /// </summary>
            ManaHeal,

            /// <summary>
            ///     Mana Damage
            /// </summary>
            ManaDmg,

            /// <summary>
            ///     Dodge
            /// </summary>
            Dodge,

            /// <summary>
            ///     Critical
            /// </summary>
            Critical,

            /// <summary>
            ///     Experience
            /// </summary>
            Experience,

            /// <summary>
            ///     Gold
            /// </summary>
            Gold,

            /// <summary>
            ///     Level
            /// </summary>
            Level,

            /// <summary>
            ///     Disable
            /// </summary>
            Disable,

            /// <summary>
            ///     Quest Received
            /// </summary>
            QuestRecv,

            /// <summary>
            ///     Quest Done
            /// </summary>
            QuestDone,

            /// <summary>
            ///     Score
            /// </summary>
            Score,

            /// <summary>
            ///     Physical Damage
            /// </summary>
            PhysDmg,

            /// <summary>
            ///     Magic Damage
            /// </summary>
            MagicDmg,

            /// <summary>
            ///     True Damage
            /// </summary>
            TrueDmg,

            /// <summary>
            ///     Enemy Physical Damage
            /// </summary>
            EnemyPhysDmg,

            /// <summary>
            ///     Enemy Magic Damage
            /// </summary>
            EnemyMagicDmg,

            /// <summary>
            ///     Enemy True Damage
            /// </summary>
            EnemyTrueDmg,

            /// <summary>
            ///     Enemy Critical
            /// </summary>
            EnemyCritical,

            /// <summary>
            ///     Countdown
            /// </summary>
            Countdown,

            /// <summary>
            ///     Legacy
            /// </summary>
            Legacy,

            /// <summary>
            ///     Legacy critical
            /// </summary>
            LegacyCritical,

            /// <summary>
            ///     Debug
            /// </summary>
            Debug
        }

        /// <summary>
        ///     Because riot has run out of headers because they used byte headers, packets have 2 byte headers. This Enum
        ///     represnets them.
        /// </summary>
        public enum MultiPacketType
        {
            /* Confirmed in IDA */

            /// <summary>
            ///     The unknown100
            /// </summary>
            Unknown100 = 0x00,

            /// <summary>
            ///     The unknown101
            /// </summary>
            Unknown101 = 0x01,

            /// <summary>
            ///     The unknown102
            /// </summary>
            Unknown102 = 0x02,

            /// <summary>
            ///     The unknown115
            /// </summary>
            Unknown115 = 0x15,

            /// <summary>
            ///     The unknown116
            /// </summary>
            Unknown116 = 0x16,

            /// <summary>
            ///     The unknown124
            /// </summary>
            Unknown124 = 0x24,

            /// <summary>
            ///     The unknown11 a
            /// </summary>
            Unknown11A = 0x1A,

            /// <summary>
            ///     The unknown11 e (Currently Empty)
            /// </summary>
            Unknown11E = 0x1E,

            /* These others could be packets with a handler */

            /// <summary>
            ///     The unknown104. Somehow related to spell slots.
            /// </summary>
            Unknown104 = 0x04,

            /// <summary>
            ///     The unknown118. (Sion Ult)
            /// </summary>
            Unknown118 = 0x08,

            /// <summary>
            ///     The unknown120
            /// </summary>
            Unknown120 = 0x20, // confirmed in game

            /// <summary>
            ///     The spawn turret packet.
            /// </summary>
            SpawnTurret = 0x23, // confirmed in ida

            /* New List: Confirmed in Game */
            //FE 19 00 00 40 07 01 00 01 00 00 00 02 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00
            /// <summary>
            ///     The initialize spell pack. Could also be the stack count for stackables.
            /// </summary>
            InitSpell = 0x07,

            /// <summary>
            ///     The unknown10 c.
            /// </summary>
            Unknown10C = 0x0C, //this packet is like 0x127

            /// <summary>
            ///     The unknown122
            /// </summary>
            Unknown122 = 0x22,

            /// <summary>
            ///     The unknown125. (Sion Ult)
            /// </summary>
            Unknown125 = 0x25, // sion ult, other stuff
            //FE 05 00 00 40 25 01 03 EC 06 00 00 00 01 <== sion
            //            FE 19 00 00 40 25 01 00 00 07 00 00 00 06 FB 16 00 40 56 04 00 40 B2 04 00 40 B2 04 00 40 56 05 00 40 FB 16 00 40
            //FE 19 00 00 40 25 01 00 00 07 00 00 00 06 FB 16 00 40 56 04 00 40 B2 04 00 40 B2 04 00 40 56 05 00 40 FB 16 00 40
            //FE 19 00 00 40 25 01 00 00 07 00 00 00 06 FB 16 00 40 56 04 00 40 B2 04 00 40 B2 04 00 40 56 05 00 40 FB 16 00 40
            //FE 19 00 00 40 25 01 00 00 07 00 00 00 06 FB 16 00 40 56 04 00 40 B2 04 00 40 B2 04 00 40 56 05 00 40 FB 16 00 40
            //FE 19 00 00 40 25 01 00 00 07 00 00 00 06 56 04 00 40 B2 04 00 40 B2 04 00 40 56 05 00 40 56 05 00 40 FB 16 00 40

            /// <summary>
            ///     The NPC death packet
            /// </summary>
            NPCDeath = 0x26, //confirmed in ida, struct from intwars/ida

            /// <summary>
            ///     The unknown129. Related to spells.
            /// </summary>
            Unknown129 = 0x29, //related to spells (kalista ally unit), add?

            /// <summary>
            ///     The unknown12A. Related to  spells.
            /// </summary>
            Unknown12A = 0x2A, //related to spells (kalist ally unit after 0x129), maybe delete?

            //FE 06 00 00 40 2A 01 3C 00 00 00
            /// <summary>
            ///     The unknown12 c
            /// </summary>
            Unknown12C = 0x2C,

            //FE 00 00 00 00 2C 01 81 00 00 00 00 FF FF FF FF 
            //FE 00 00 00 00 2C 01 80 00 00 00 00 FF FF FF FF 
            /// <summary>
            ///     The unknown12 e
            /// </summary>
            Unknown12E = 0x2E, //confirmed in ida

            /// <summary>
            ///     The unknown12 f
            /// </summary>
            Unknown12F = 0x2F, //FE 05 00 00 40 2F 01 00

            /// <summary>
            ///     The add buff packet.
            /// </summary>
            AddBuff = 0x09, // buff added by towers in new SR

            /// <summary>
            ///     The undo token packet
            /// </summary>
            UndoToken = 0x0B,

            /// <summary>
            ///     The object creation packet. Used for Azir's ult.
            /// </summary>
            ObjectCreation = 0x0D, // azir ult

            /// <summary>
            ///     The surrender state packet
            /// </summary>
            SurrenderState = 0x0E,

            /// <summary>
            ///     The on attack packet.
            /// </summary>
            OnAttack = 0x0F,

            /// <summary>
            ///     The death timer packet.
            /// </summary>
            DeathTimer = 0x17,

            /// <summary>
            ///     The change item packet. (EX: Health Potion to Biscuit)
            /// </summary>
            ChangeItem = 0x1C, //like hpp=>biscuit

            /// <summary>
            ///     The action state packet. Triggers on recall.
            /// </summary>
            ActionState = 0x21, // ?? triggers on recall

            /// <summary>
            ///     The undo confirmation packet.
            /// </summary>
            UndoConfirm = 0x27,

            /// <summary>
            ///     The lock camera packet for Sion's Ult.
            /// </summary>
            LockCamera = 0x2B, // Sion Ult

            /// <summary>
            ///     An unkown packet.
            /// </summary>
            Unknown = 0xFF, // Default, not real packet
        }

        /// <summary>
        ///     The type of ping.
        /// </summary>
        public enum PingType
        {
            /// <summary>
            ///     A normal ping.
            /// </summary>
            Normal = 0,

            /// <summary>
            ///     A fallback ping.
            /// </summary>
            Fallback = 5,

            /// <summary>
            ///     An enemy missing ping.
            /// </summary>
            EnemyMissing = 3,

            /// <summary>
            ///     A danagr ping.
            /// </summary>
            Danger = 2,

            /// <summary>
            ///     An on my way ping.
            /// </summary>
            OnMyWay = 4,

            /// <summary>
            ///     An assist me ping.
            /// </summary>
            AssistMe = 6,
        }

        #endregion

        /// <summary>
        ///     Contains packets that are sent from the client (the game) to the server.
        /// </summary>
        public static class C2S
        {
            /// <summary>
            ///     Packet sent when buying items.
            /// </summary>
            public static class BuyItem
            {
                #region Static Fields

                /// <summary>
                ///     The channel
                /// </summary>
                public static PacketChannel Channel = PacketChannel.C2S;

                /// <summary>
                ///     The flags
                /// </summary>
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xC6;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 2;
                    result.NetworkId = packet.ReadInteger();
                    result.ItemId = packet.ReadInteger();
                    return result;
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteInteger(packetStruct.ItemId);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Reprents the packet sent when an item is bought.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The item identifier
                    /// </summary>
                    public int ItemId;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="itemId">The item identifier.</param>
                    /// <param name="networkId">The network identifier.</param>
                    public Struct(int itemId, int networkId = -1)
                    {
                        this.ItemId = itemId;
                        this.NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Sent by client on center/lock camera.
            /// </summary>
            public static class Camera
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x64;

                #endregion
            }

            /// <summary>
            ///     Packet sent when casting spells.
            /// </summary>
            public static class Cast
            {
                #region Static Fields

                /// <summary>
                ///     The channel
                /// </summary>
                public static PacketChannel Channel = PacketChannel.C2S;

                /// <summary>
                ///     The flags
                /// </summary>
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xDE;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
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

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
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
                    result.WriteByte(0);
                        //packetStruct.SpellFlag == 0xFF ? GetSpellByte(packetStruct.Slot) : packetStruct.SpellFlag
                    return result;
                }

                #endregion

                #region Methods

                /// <summary>
                ///     Gets the spell byte.
                /// </summary>
                /// <param name="spell">The spell.</param>
                /// <returns>System.Byte.</returns>
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

                #endregion

                /// <summary>
                ///     Represents a spell cast packet.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The X position of where the spell came from.
                    /// </summary>
                    public float FromX;

                    /// <summary>
                    ///     The Y position of where the spell came from.
                    /// </summary>
                    public float FromY;

                    /// <summary>
                    ///     The slot
                    /// </summary>
                    public SpellSlot Slot;

                    /// <summary>
                    ///     The source network identifier
                    /// </summary>
                    public int SourceNetworkId;

                    /// <summary>
                    ///     The spell flag
                    /// </summary>
                    public byte SpellFlag;

                    /// <summary>
                    ///     The target network identifier
                    /// </summary>
                    public int TargetNetworkId;

                    /// <summary>
                    ///     The end X position of where the spell is going to.
                    /// </summary>
                    public float ToX;

                    /// <summary>
                    ///     To end Y position of where the spell is going to.
                    /// </summary>
                    public float ToY;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="targetNetworkId">The target network identifier.</param>
                    /// <param name="slot">The slot.</param>
                    /// <param name="sourceNetworkId">The source network identifier.</param>
                    /// <param name="fromX">From x.</param>
                    /// <param name="fromY">From y.</param>
                    /// <param name="toX">To x.</param>
                    /// <param name="toY">To y.</param>
                    /// <param name="spellFlag">The spell flag.</param>
                    public Struct(
                        int targetNetworkId = 0,
                        SpellSlot slot = SpellSlot.Q,
                        int sourceNetworkId = -1,
                        float fromX = 0f,
                        float fromY = 0f,
                        float toX = 0f,
                        float toY = 0f,
                        byte spellFlag = 0xFF)
                    {
                        this.SourceNetworkId = (sourceNetworkId == -1)
                                                   ? ObjectManager.Player.NetworkId
                                                   : sourceNetworkId;
                        this.Slot = slot;
                        this.FromX = fromX;
                        this.FromY = fromY;
                        this.ToX = toX;
                        this.ToY = toY;
                        this.TargetNetworkId = targetNetworkId;
                        this.SpellFlag = spellFlag;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Packet sent when casting charged spells second cast.
            /// </summary>
            public static class ChargedCast
            {
                #region Static Fields

                /// <summary>
                ///     The channel
                /// </summary>
                public static PacketChannel Channel = PacketChannel.C2S;

                /// <summary>
                ///     The flags
                /// </summary>
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x03;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 2;
                    result.SourceNetworkId = packet.ReadInteger();
                    result.Slot = (SpellSlot)packet.ReadByte();
                    result.ToX = packet.ReadFloat();
                    result.ToY = packet.ReadFloat();
                    result.ToZ = packet.ReadFloat();
                    return result;
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteByte(1);
                    result.WriteInteger(packetStruct.SourceNetworkId);
                    result.WriteByte((byte)packetStruct.Slot);
                    result.WriteFloat(packetStruct.ToX);
                    result.WriteFloat(packetStruct.ToY);
                    result.WriteFloat(packetStruct.ToZ);
                    result.WriteByte(2);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents a charged cast packet.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The slot
                    /// </summary>
                    public SpellSlot Slot;

                    /// <summary>
                    ///     The source network identifier
                    /// </summary>
                    public int SourceNetworkId;

                    /// <summary>
                    ///     The X position of where the spell is going to.
                    /// </summary>
                    public float ToX;

                    /// <summary>
                    ///     The Y position of where the spell is going to.
                    /// </summary>
                    public float ToY;

                    /// <summary>
                    ///     The Z position of where the spell is going to.
                    /// </summary>
                    public float ToZ;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="slot">The slot.</param>
                    /// <param name="toX">To x.</param>
                    /// <param name="toY">To y.</param>
                    /// <param name="toZ">To z.</param>
                    /// <param name="sourceNetworkId">The source network identifier.</param>
                    public Struct(
                        SpellSlot slot,
                        float toX = 0f,
                        float toY = 0f,
                        float toZ = 0f,
                        int sourceNetworkId = -1)
                    {
                        this.SourceNetworkId = (sourceNetworkId == -1)
                                                   ? ObjectManager.Player.NetworkId
                                                   : sourceNetworkId;
                        this.Slot = slot;
                        this.ToX = toX;
                        this.ToY = toY;
                        this.ToZ = toZ;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Packet sent when sending emotes.
            /// </summary>
            public static class Emote
            {
                #region Static Fields

                /// <summary>
                ///     The channel
                /// </summary>
                public static PacketChannel Channel = PacketChannel.C2S;

                /// <summary>
                ///     The flags
                /// </summary>
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x14;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 2;
                    result.NetworkId = packet.ReadInteger();
                    result.EmoteId = packet.ReadByte();
                    return result;
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteByte(1);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteByte(packetStruct.EmoteId);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet sent when you cast an emote.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The emote identifier
                    /// </summary>
                    public byte EmoteId;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="emoteId">The emote identifier.</param>
                    /// <param name="networkId">The network identifier.</param>
                    public Struct(byte emoteId, int networkId = -1)
                    {
                        this.EmoteId = emoteId;
                        this.NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Sent by client when the game is over.
            /// </summary>
            public static class EndGame
            {
                #region Static Fields

                //1E 00 00 00 00 00
                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x1E;

                #endregion
            }

            /// <summary>
            ///     Packet sent frequently as heartbeat to servers.
            ///     Related to 0x29 (Recv)
            /// </summary>
            public static class HeartBeat
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x4C;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct { RecvTime = packet.ReadFloat(1), AckTime = packet.ReadFloat(5) };
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents a heart beat packet.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The ack time
                    /// </summary>
                    public float AckTime;

                    /// <summary>
                    ///     The recv time
                    /// </summary>
                    public float RecvTime;

                    #endregion
                }
            }

            /// <summary>
            ///     Packet sent when interacting with Thresh Lantern and Dominion capturing.
            /// </summary>
            public static class InteractObject
            {
                #region Static Fields

                /// <summary>
                ///     The channel
                /// </summary>
                public static PacketChannel Channel = PacketChannel.C2S;

                /// <summary>
                ///     The flags
                /// </summary>
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x86;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 1;
                    result.SourceNetworkId = packet.ReadInteger();
                    result.ObjectNetworkId = packet.ReadInteger();
                    return result;
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteInteger(packetStruct.SourceNetworkId);
                    result.WriteInteger(packetStruct.ObjectNetworkId);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet sent when you interact with an object.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The object network identifier
                    /// </summary>
                    public int ObjectNetworkId;

                    /// <summary>
                    ///     The source network identifier
                    /// </summary>
                    public int SourceNetworkId;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="networkId">The network identifier.</param>
                    /// <param name="objectNetworkId">The object network identifier.</param>
                    public Struct(int networkId, int objectNetworkId)
                    {
                        this.SourceNetworkId = networkId;
                        this.ObjectNetworkId = objectNetworkId;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Sent by client on exiting the game.
            /// </summary>
            public static class LeaveGame
            {
                #region Static Fields

                //75 00 00 00 00 00 5A
                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x75;

                #endregion
            }

            /// <summary>
            ///     Packet sent when leveling up a spell.
            /// </summary>
            public static class LevelUpSpell
            {
                #region Static Fields

                /// <summary>
                ///     The channel
                /// </summary>
                public static PacketChannel Channel = PacketChannel.C2S;

                /// <summary>
                ///     The flags
                /// </summary>
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xEC;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 2 };
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger();
                    packet.Position += 4;
                    result.Slot = (SpellSlot)packet.ReadByte();
                    result.Evolution = packet.ReadByte() == 0x01 ? true : false;

                    return new Struct(packet.ReadInteger(), (SpellSlot)packet.ReadByte());
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteInteger(0);
                    result.WriteByte((byte)packetStruct.Slot);
                    var bit = packetStruct.Evolution ? (byte)0x01 : (byte)0x0;
                    result.WriteByte(bit);
                    result.WriteInteger(0);
                    result.WriteInteger(0);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents a level up packet.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     <c>true</c> if the level up was to evolve a spell.
                    /// </summary>
                    public bool Evolution;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The slot
                    /// </summary>
                    public SpellSlot Slot;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="networkId">The network identifier.</param>
                    /// <param name="slot">The slot.</param>
                    /// <param name="evolve">if set to <c>true</c> [evolve].</param>
                    public Struct(int networkId = -1, SpellSlot slot = SpellSlot.Q, bool evolve = false)
                    {
                        this.NetworkId = (networkId == -1) ? ObjectManager.Player.NetworkId : networkId;
                        this.Slot = slot;
                        this.Evolution = evolve;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Packet sent when issuing GameObjectOrder's.
            /// </summary>
            public static class Move
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x13;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
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

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
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

                #endregion

                /// <summary>
                ///     Represents a move packet.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The move type
                    /// </summary>
                    public byte MoveType;

                    /// <summary>
                    ///     The source network identifier
                    /// </summary>
                    public int SourceNetworkId;

                    /// <summary>
                    ///     The target network identifier
                    /// </summary>
                    public int TargetNetworkId;

                    /// <summary>
                    ///     The unit network identifier
                    /// </summary>
                    public int UnitNetworkId;

                    /// <summary>
                    ///     The waypoint count
                    /// </summary>
                    public int WaypointCount;

                    /// <summary>
                    ///     The x position
                    /// </summary>
                    public float X;

                    /// <summary>
                    ///     The y position
                    /// </summary>
                    public float Y;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="x">The x.</param>
                    /// <param name="y">The y.</param>
                    /// <param name="moveType">Type of the move.</param>
                    /// <param name="targetNetworkId">The target network identifier.</param>
                    /// <param name="unitNetworkId">The unit network identifier.</param>
                    /// <param name="sourceNetworkId">The source network identifier.</param>
                    public Struct(
                        float x = 0f,
                        float y = 0f,
                        byte moveType = 2,
                        int targetNetworkId = 0,
                        int unitNetworkId = -1,
                        int sourceNetworkId = -1)
                    {
                        this.SourceNetworkId = (sourceNetworkId == -1)
                                                   ? ObjectManager.Player.NetworkId
                                                   : sourceNetworkId;
                        this.MoveType = moveType;
                        this.X = x;
                        this.Y = y;
                        this.TargetNetworkId = targetNetworkId;
                        this.UnitNetworkId = (unitNetworkId == -1) ? ObjectManager.Player.NetworkId : unitNetworkId;
                        this.WaypointCount = 1;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Sent by client on pause.
            /// </summary>
            public static class Pause
            {
                #region Static Fields

                //00 01 00 00 00 00 00 00 00 00 00 00 00 00 90 
                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x00;

                #endregion
            }

            /// <summary>
            ///     Ping Packet. Sent by the client when pings are sent.
            /// </summary>
            public static class Ping
            {
                #region Static Fields

                /// <summary>
                ///     The channel
                /// </summary>
                public static PacketChannel Channel = PacketChannel.C2S;

                /// <summary>
                ///     The flags
                /// </summary>
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x1D;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 10 };
                    return new Struct(
                        packet.ReadFloat(),
                        packet.ReadFloat(),
                        packet.ReadInteger(),
                        (PingType)packet.ReadByte());
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteByte(1);
                    result.WriteInteger(0);
                    result.WriteInteger(packetStruct.TargetNetworkId);
                    result.WriteFloat(packetStruct.X);
                    result.WriteFloat(packetStruct.Y);
                    result.WriteByte((byte)packetStruct.Type);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Repreents a ping packet.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The target network identifier
                    /// </summary>
                    public int TargetNetworkId;

                    /// <summary>
                    ///     The ping type
                    /// </summary>
                    public PingType Type;

                    /// <summary>
                    ///     The x position
                    /// </summary>
                    public float X;

                    /// <summary>
                    ///     The y position
                    /// </summary>
                    public float Y;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="x">The x.</param>
                    /// <param name="y">The y.</param>
                    /// <param name="targetNetworkId">The target network identifier.</param>
                    /// <param name="type">The type.</param>
                    public Struct(float x = 0f, float y = 0f, int targetNetworkId = 0, PingType type = PingType.Normal)
                    {
                        this.X = x;
                        this.Y = y;
                        this.TargetNetworkId = targetNetworkId;
                        this.Type = type;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Sent by client on refund.
            /// </summary>
            public static class Refund
            {
                #region Static Fields

                /// <summary>
                ///     The channel
                /// </summary>
                public static PacketChannel Channel = PacketChannel.C2S;

                /// <summary>
                ///     The flags
                /// </summary>
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x54;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    packet.Position = 2;
                    result.NetworkId = packet.ReadInteger();
                    return result;
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.NetworkId);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet sent when refunding an item.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="networkId">The network identifier.</param>
                    public Struct(int networkId = -1)
                    {
                        this.NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Sent by client on resume.
            /// </summary>
            public static class Resume
            {
                #region Static Fields

                //19 00 00 00 00 00 00 00 00 00 90  
                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x19;

                #endregion
            }

            /// <summary>
            ///     Sent by client on opening score screen.
            /// </summary>
            public static class ScoreScreen
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x15;

                #endregion
            }

            /// <summary>
            ///     Packet sent when selling items.
            /// </summary>
            public static class SellItem
            {
                #region Static Fields

                /// <summary>
                ///     The channel
                /// </summary>
                public static PacketChannel Channel = PacketChannel.C2S;

                /// <summary>
                ///     The flags
                /// </summary>
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x72;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 2 };
                    var networkId = packet.ReadInteger();
                    var slot = packet.ReadByte();

                    return new Struct(slot, networkId);
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteByte(packetStruct.InventorySlot);
                    result.WriteByte(1);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Packet sent on selling an item.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The inventory slot
                    /// </summary>
                    public byte InventorySlot;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The spell slot
                    /// </summary>
                    public SpellSlot SpellSlot;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="slot">The slot.</param>
                    /// <param name="networkId">The network identifier.</param>
                    public Struct(byte slot, int networkId = -1)
                    {
                        this.InventorySlot = slot;
                        this.SpellSlot = (SpellSlot)(this.InventorySlot + (byte)SpellSlot.Item1);
                        this.NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="slot">The slot.</param>
                    /// <param name="networkId">The network identifier.</param>
                    public Struct(SpellSlot slot, int networkId = -1)
                    {
                        this.SpellSlot = slot;
                        this.InventorySlot = (byte)((byte)this.SpellSlot - (byte)SpellSlot.Item1);
                        this.NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Packet sent when left clicking a target.
            /// </summary>
            public static class SetTarget
            {
                #region Static Fields

                /// <summary>
                ///     The channel
                /// </summary>
                public static PacketChannel Channel = PacketChannel.C2S;

                /// <summary>
                ///     The flags
                /// </summary>
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x04;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var result = new Struct { NetworkId = new GamePacket(data).ReadInteger(6) };
                    result.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.NetworkId);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet sent when left clicking a target.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The unit
                    /// </summary>
                    public Obj_AI_Base Unit;

                    #endregion
                }
            }

            /// <summary>
            ///     Sent by client on surrendering.
            /// </summary>
            public static class Surrender
            {
                #region Static Fields

                //A2 00 00 00 00 00 AB
                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xA2;

                #endregion
            }

            /// <summary>
            ///     Packet sent when swapping items.
            /// </summary>
            public static class SwapItem
            {
                #region Static Fields

                /// <summary>
                ///     The channel
                /// </summary>
                public static PacketChannel Channel = PacketChannel.C2S;

                /// <summary>
                ///     The flags
                /// </summary>
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x55;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
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

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header, Channel, Flags);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteByte(packetStruct.FromSlotByte);
                    result.WriteByte(packetStruct.ToSlotByte);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet sent when you swap an item in your inventory.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     From slot byte
                    /// </summary>
                    public byte FromSlotByte;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     To slot byte
                    /// </summary>
                    public byte ToSlotByte;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="fromSlotByte">From slot byte.</param>
                    /// <param name="toSlotByte">To slot byte.</param>
                    /// <param name="networkId">The network identifier.</param>
                    public Struct(byte fromSlotByte, byte toSlotByte, int networkId = -1)
                    {
                        this.FromSlotByte = fromSlotByte;
                        this.ToSlotByte = toSlotByte;
                        this.NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Packet sent to acknowledge the received update packet.
            /// </summary>
            public static class UpdateConfirm
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xA8;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct { SequenceId = packet.ReadInteger(5) };
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet sent to acknowledge the received update packet
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The sequence identifier
                    /// </summary>
                    public int SequenceId;

                    #endregion
                }
            }

            /// <summary>
            ///     Sent by client to confirm receiving waypoints.
            /// </summary>
            public static class WaypointConfirm
            {
                #region Static Fields

                //36 00 00 00 00 00 83 7F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 
                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x36;

                #endregion
            }

            /// <summary>
            ///     Sent by client on zoom level change or move camera.
            /// </summary>
            public static class Zoom
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xDC;

                #endregion
            }
        }

        /// <summary>
        ///     Contains packet that are sent from the server to the client (the game).
        /// </summary>
        public static class S2C
        {
            /// <summary>
            ///     Received by the server when Camera or Zoom is sent.
            /// </summary>
            public static class Camera
            {
                #region Static Fields

                //94 00 00 00 00 00 27 
                //last byte is probably times camera packet has been sent
                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x94;

                #endregion
            }

            /// <summary>
            ///     Received when a unit casts a spell.
            /// </summary>
            public static class CastAns
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xB5;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
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

                    result.SpellSlot = (SpellSlot)packet.ReadByte();
                    result.SpellFlag2 = packet.ReadByte();
                    result.ManaCost = packet.ReadFloat();

                    p = packet.Position + 8;
                    result.FromPosition = new Vector2(packet.ReadFloat(), packet.ReadFloat(p));

                    return result;
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    //TODO when the packet is fully decoded.
                    return new GamePacket(Header);
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when a spell is casted.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The channel time
                    /// </summary>
                    public float ChannelTime;

                    /// <summary>
                    ///     The cooldown. Maybe 0.
                    /// </summary>
                    public float Cooldown; // sometimes 0

                    /// <summary>
                    ///     The delay
                    /// </summary>
                    public float Delay;

                    /// <summary>
                    ///     From position
                    /// </summary>
                    public Vector2 FromPosition;

                    /// <summary>
                    ///     <c>true</c> if the spell cast is visible
                    /// </summary>
                    public bool IsVisible;

                    /// <summary>
                    ///     The mana cost
                    /// </summary>
                    public float ManaCost;

                    /// <summary>
                    ///     The missile hash
                    /// </summary>
                    public int MissileHash;

                    /// <summary>
                    ///     The missile network identifier
                    /// </summary>
                    public int MissileNetworkId;

                    /// <summary>
                    ///     The source network identifier
                    /// </summary>
                    public int SourceNetworkId;

                    /// <summary>
                    ///     The source unit
                    /// </summary>
                    public Obj_AI_Base SourceUnit;

                    /// <summary>
                    ///     The speed
                    /// </summary>
                    public float Speed;

                    /// <summary>
                    ///     The spell flag
                    /// </summary>
                    public short SpellFlag;

                    /// <summary>
                    ///     The spell flag2
                    /// </summary>
                    public byte SpellFlag2;

                    /// <summary>
                    ///     The spell hash
                    /// </summary>
                    public int SpellHash;

                    /// <summary>
                    ///     The spell network identifier
                    /// </summary>
                    public int SpellNetworkId;

                    /// <summary>
                    ///     The spell slot
                    /// </summary>
                    public SpellSlot SpellSlot;

                    /// <summary>
                    ///     The target network identifier
                    /// </summary>
                    public int TargetNetworkId;

                    /// <summary>
                    ///     The target unit
                    /// </summary>
                    public Obj_AI_Base TargetUnit;

                    /// <summary>
                    ///     To position
                    /// </summary>
                    public Vector2 ToPosition;

                    /// <summary>
                    ///     If this value is greater than 0, the spell cast is visible.
                    /// </summary>
                    public float Visible; // >0 visible

                    #endregion
                }
            }

            /// <summary>
            ///     Gets received when a unit dashes.
            /// </summary>
            public static class Dash
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xD7;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.UnitNetworkId = packet.ReadInteger(12);
                    result.Speed = 900; //packet.ReadFloat();

                    return result;
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    //TODO when the packet is fully decoded.
                    return new GamePacket(Header);
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when a unit dashes.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The speed
                    /// </summary>
                    public float Speed;

                    /// <summary>
                    ///     The unit network identifier
                    /// </summary>
                    public int UnitNetworkId;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="unitNetworkId">The unit network identifier.</param>
                    /// <param name="speed">The speed.</param>
                    public Struct(int unitNetworkId = 0, float speed = 0)
                    {
                        this.UnitNetworkId = unitNetworkId;
                        this.Speed = speed;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Gets received when gaining vision of an empty jungle camp. Partially implemented.
            /// </summary>
            public static class EmptyJungleCamp
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x93;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
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

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
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

                #endregion

                /// <summary>
                ///     Represents the packet received when a jungle camp is empty.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The camp identifier
                    /// </summary>
                    public int CampId;

                    /// <summary>
                    ///     The empty type
                    /// </summary>
                    public byte EmptyType;

                    /// <summary>
                    ///     The unit network identifier
                    /// </summary>
                    public int UnitNetworkId;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="unitNetworkId">The unit network identifier.</param>
                    /// <param name="campId">The camp identifier.</param>
                    /// <param name="emptyType">The empty type.</param>
                    public Struct(int unitNetworkId = 0, int campId = 0, byte emptyType = 0)
                    {
                        this.UnitNetworkId = unitNetworkId;
                        this.CampId = campId;
                        this.EmptyType = emptyType;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Gets received when a unit enters FOW.
            /// </summary>
            public static class GainVision
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xFC;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    result.UnitNetworkId = packet.ReadInteger(2);
                    return result;
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    //Not fully encoded
                    var result = new GamePacket(Header);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.UnitNetworkId);
                    result.WriteShort(0);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when the team gains vision of a unit.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The unit network identifier
                    /// </summary>
                    public int UnitNetworkId;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="unitNetworkId">The unit network identifier.</param>
                    public Struct(int unitNetworkId = 0)
                    {
                        this.UnitNetworkId = unitNetworkId;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Gets received when the game ends.
            /// </summary>
            public static class GameEnd
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xED;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.Winner = packet.ReadByte(5);
                    return result;
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(0);
                    result.WriteByte(packetStruct.Winner);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when the game ends.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The winner
                    /// </summary>
                    public byte Winner;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="winner">The winner.</param>
                    public Struct(byte winner = 1)
                    {
                        this.Winner = winner;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Gets received when a unit leaves FOW.
            /// </summary>
            public static class LoseVision
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xCD;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 2 };
                    return new Struct(packet.ReadInteger());
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.UnitNetworkId);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when the team loses vision of an unit.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The unit network identifier
                    /// </summary>
                    public int UnitNetworkId;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="unitNetworkId">The unit network identifier.</param>
                    public Struct(int unitNetworkId = 0)
                    {
                        this.UnitNetworkId = unitNetworkId;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     RPing Packet. Received when ally team players send a SPing packet.
            /// </summary>
            public static class Ping
            {
                #region Static Fields

                /// <summary>
                ///     The channel
                /// </summary>
                public static PacketChannel Channel = PacketChannel.C2S;

                /// <summary>
                ///     The flags
                /// </summary>
                public static PacketProtocolFlags Flags = PacketProtocolFlags.Reliable;

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x60;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data) { Position = 6 };
                    var targetNetworkId = packet.ReadInteger();
                    var type = packet.ReadByte();
                    var sourceNetworkId = packet.ReadInteger();
                    var x = packet.ReadFloat();
                    var y = packet.ReadFloat();
                    var silent = (packet.ReadByte() & 1) != 1;
                    return new Struct(x, y, targetNetworkId, sourceNetworkId, (PingType)type, silent);
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
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
                    var bit = packetStruct.Silent ? (byte)0x14 : (byte)0x1B;
                    result.WriteByte(bit);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when an ally sends a ping.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     <c>true</c> if the ping is silent, meaning that the ping produces no sound.
                    /// </summary>
                    public bool Silent;

                    /// <summary>
                    ///     The source network identifier
                    /// </summary>
                    public int SourceNetworkId;

                    /// <summary>
                    ///     The target network identifier
                    /// </summary>
                    public int TargetNetworkId;

                    /// <summary>
                    ///     The ping type
                    /// </summary>
                    public PingType Type;

                    /// <summary>
                    ///     The x position
                    /// </summary>
                    public float X;

                    /// <summary>
                    ///     The y position
                    /// </summary>
                    public float Y;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="x">The x.</param>
                    /// <param name="y">The y.</param>
                    /// <param name="targetNetworkId">The target network identifier.</param>
                    /// <param name="sourceNetworkId">The source network identifier.</param>
                    /// <param name="type">The type.</param>
                    /// <param name="silent">if set to <c>true</c> [silent].</param>
                    public Struct(
                        float x = 0f,
                        float y = 0f,
                        int targetNetworkId = 0,
                        int sourceNetworkId = 0,
                        PingType type = PingType.Normal,
                        bool silent = false)
                    {
                        this.X = x;
                        this.Y = y;
                        this.TargetNetworkId = targetNetworkId;
                        this.SourceNetworkId = sourceNetworkId;
                        this.Type = type;
                        this.Silent = silent;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Gets received when an unit uses an emote.
            /// </summary>
            public static class PlayEmote
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xAA;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();
                    result.NetworkId = packet.ReadInteger(2);
                    result.EmoteId = packet.ReadByte();
                    return result;
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteByte(packetStruct.EmoteId);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when a unit uses an emote.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The emote identifier
                    /// </summary>
                    public byte EmoteId;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="emoteId">The emote identifier.</param>
                    /// <param name="networkId">The network identifier.</param>
                    public Struct(byte emoteId, int networkId = -1)
                    {
                        this.EmoteId = emoteId;
                        this.NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Received by the server when refund is sent.
            /// </summary>
            public static class RefundConfirm
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x49;

                #endregion
            }

            /// <summary>
            ///     Refund token contains refund amount, when leaving base or casting spell/item it's set to 0.
            /// </summary>
            public static class RefundToken
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xE9;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    return new Struct { RefundCount = data[6] };
                }

                /// <summary>
                ///     Encodes the specified undo amount.
                /// </summary>
                /// <param name="undoAmount">The undo amount.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(int undoAmount)
                {
                    var packet = new GamePacket(Header);
                    packet.WriteByte(0);
                    packet.WriteInteger(ObjectManager.Player.NetworkId);
                    packet.WriteInteger(undoAmount);
                    return packet;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when refunding an item.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The refund count
                    /// </summary>
                    public int RefundCount;

                    #endregion
                }
            }

            /// <summary>
            ///     Gets received when a unit starts, aborts or finishes a teleport (such as recall, teleport, twisted fate ulti, shen
            ///     ulti,...)
            /// </summary>
            public static class Teleport
            {
                #region Constants

                /// <summary>
                ///     The error gap in ticks.
                /// </summary>
                private const int ErrorGap = 100; //in ticks

                #endregion

                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x44;

                /// <summary>
                ///     The recall data by network identifier
                /// </summary>
                private static readonly IDictionary<int, TeleportData> RecallDataByNetworkId =
                    new Dictionary<int, TeleportData>();

                /// <summary>
                ///     The type by string
                /// </summary>
                private static readonly IDictionary<string, ITeleport> TypeByString =
                    new Dictionary<string, ITeleport>
                        {
                            { "Recall", new RecallTeleport() }, { "Teleport", new TeleportTeleport() },
                            { "Gate", new TwistedFateTeleport() }, { "Shen", new ShenTeleport() },
                        };

                #endregion

                #region Enums

                /// <summary>
                ///     The status of the teleport.
                /// </summary>
                public enum Status
                {
                    /// <summary>
                    ///     The teleport has been started.
                    /// </summary>
                    Start,

                    /// <summary>
                    ///     The teleport has been aborted.
                    /// </summary>
                    Abort,

                    /// <summary>
                    ///     The teleport has finished.
                    /// </summary>
                    Finish,

                    /// <summary>
                    ///     The status of the teleport is unknown.
                    /// </summary>
                    Unknown
                }

                /// <summary>
                ///     The type of teleport.
                /// </summary>
                public enum Type
                {
                    /// <summary>
                    ///     The unit is recalling back to base.
                    /// </summary>
                    Recall,

                    /// <summary>
                    ///     The unit is teleporting to another unit with the Teleport summoner spell.
                    /// </summary>
                    Teleport,

                    /// <summary>
                    ///     The unit is teleporting to a location with Twisted Fate's Ultimate.
                    /// </summary>
                    TwistedFate,

                    /// <summary>
                    ///     The unit is teleporting to a unit with Shen's Ultimate.
                    /// </summary>
                    Shen,

                    /// <summary>
                    ///     The type of teleportation is unknown.
                    /// </summary>
                    Unknown
                }

                #endregion

                #region Interfaces

                /// <summary>
                ///     An interface for different types of teleports to get durations and type of teleport.
                /// </summary>
                internal interface ITeleport
                {
                    #region Public Properties

                    /// <summary>
                    ///     Gets the type.
                    /// </summary>
                    /// <value>The type.</value>
                    Type Type { get; }

                    #endregion

                    #region Public Methods and Operators

                    /// <summary>
                    ///     Gets the duration.
                    /// </summary>
                    /// <param name="packetData">The <see cref="GameObjectTeleportEventArgs" /> instance containing the event data.</param>
                    /// <returns>System.Int32.</returns>
                    int GetDuration(GameObjectTeleportEventArgs packetData);

                    #endregion
                }

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified sender.
                /// </summary>
                /// <param name="sender">The sender.</param>
                /// <param name="args">The <see cref="GameObjectTeleportEventArgs" /> instance containing the event data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(GameObject sender, GameObjectTeleportEventArgs args) //
                {
                    var result = new Struct { Status = Status.Unknown, Type = Type.Unknown };

                    if (sender == null || !sender.IsValid || !(sender is Obj_AI_Hero))
                    {
                        return result;
                    }

                    result.UnitNetworkId = sender.NetworkId;

                    var hero = sender as Obj_AI_Hero;

                    if (!RecallDataByNetworkId.ContainsKey(result.UnitNetworkId))
                    {
                        RecallDataByNetworkId[result.UnitNetworkId] = new TeleportData { Type = Type.Unknown };
                    }

                    if (!string.IsNullOrEmpty(args.RecallType))
                    {
                        if (TypeByString.ContainsKey(args.RecallType))
                        {
                            ITeleport teleportMethod = TypeByString[args.RecallType];

                            var duration = teleportMethod.GetDuration(args);
                            var type = teleportMethod.Type;
                            var time = Utils.TickCount;

                            RecallDataByNetworkId[result.UnitNetworkId] = new TeleportData
                                                                              {
                                                                                  Duration = duration, Type = type,
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
                        var shorter = Utils.TickCount - RecallDataByNetworkId[result.UnitNetworkId].Start
                                      < RecallDataByNetworkId[result.UnitNetworkId].Duration - ErrorGap;
                        result.Status = shorter ? Status.Abort : Status.Finish;
                        result.Type = RecallDataByNetworkId[result.UnitNetworkId].Type;
                        result.Duration = 0;
                        result.Start = 0;
                    }
                    return result;
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    //TODO when the packet is fully decoded.
                    return new GamePacket(Header);
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when a unit teleports.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The duration
                    /// </summary>
                    public int Duration;

                    /// <summary>
                    ///     The start
                    /// </summary>
                    public int Start;

                    /// <summary>
                    ///     The status
                    /// </summary>
                    public Status Status;

                    /// <summary>
                    ///     The type
                    /// </summary>
                    public Type Type;

                    /// <summary>
                    ///     The unit network identifier
                    /// </summary>
                    public int UnitNetworkId;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="unitNetworkId">The unit network identifier.</param>
                    /// <param name="status">The status.</param>
                    /// <param name="type">The type.</param>
                    /// <param name="duration">The duration.</param>
                    /// <param name="start">The start.</param>
                    public Struct(int unitNetworkId, Status status, Type type, int duration, int start = 0)
                    {
                        this.UnitNetworkId = unitNetworkId;
                        this.Status = status;
                        this.Type = type;
                        this.Duration = duration;
                        this.Start = start;
                    }

                    #endregion
                }

                /// <summary>
                ///     Contains data about the teleport.
                /// </summary>
                internal struct TeleportData
                {
                    #region Public Properties

                    /// <summary>
                    ///     Gets or sets the duration.
                    /// </summary>
                    /// <value>The duration.</value>
                    public int Duration { get; set; }

                    /// <summary>
                    ///     Gets or sets the start.
                    /// </summary>
                    /// <value>The start.</value>
                    public int Start { get; set; }

                    /// <summary>
                    ///     Gets or sets the type.
                    /// </summary>
                    /// <value>The type.</value>
                    public Type Type { get; set; }

                    #endregion
                }

                /// <summary>
                ///     A recall teleport.
                /// </summary>
                internal class RecallTeleport : ITeleport
                {
                    #region Public Properties

                    /// <summary>
                    ///     Gets the type.
                    /// </summary>
                    /// <value>The type.</value>
                    public Type Type
                    {
                        get
                        {
                            return Type.Recall;
                        }
                    }

                    #endregion

                    #region Public Methods and Operators

                    /// <summary>
                    ///     Gets the duration.
                    /// </summary>
                    /// <param name="args">The <see cref="GameObjectTeleportEventArgs" /> instance containing the event data.</param>
                    /// <returns>System.Int32.</returns>
                    public int GetDuration(GameObjectTeleportEventArgs args)
                    {
                        return Utility.GetRecallTime(args.RecallName);
                    }

                    #endregion
                }

                /// <summary>
                ///     A Shen's Ultimate teleport.
                /// </summary>
                internal class ShenTeleport : ITeleport
                {
                    #region Public Properties

                    /// <summary>
                    ///     Gets the type.
                    /// </summary>
                    /// <value>The type.</value>
                    public Type Type
                    {
                        get
                        {
                            return Type.Shen;
                        }
                    }

                    #endregion

                    #region Public Methods and Operators

                    /// <summary>
                    ///     Gets the duration.
                    /// </summary>
                    /// <param name="args">The <see cref="GameObjectTeleportEventArgs" /> instance containing the event data.</param>
                    /// <returns>System.Int32.</returns>
                    public int GetDuration(GameObjectTeleportEventArgs args)
                    {
                        return 3000;
                    }

                    #endregion
                }

                /// <summary>
                ///     A teleport summoner spell teleport.
                /// </summary>
                internal class TeleportTeleport : ITeleport
                {
                    #region Public Properties

                    /// <summary>
                    ///     Gets the type.
                    /// </summary>
                    /// <value>The type.</value>
                    public Type Type
                    {
                        get
                        {
                            return Type.Teleport;
                        }
                    }

                    #endregion

                    #region Public Methods and Operators

                    /// <summary>
                    ///     Gets the duration.
                    /// </summary>
                    /// <param name="args">The <see cref="GameObjectTeleportEventArgs" /> instance containing the event data.</param>
                    /// <returns>System.Int32.</returns>
                    public int GetDuration(GameObjectTeleportEventArgs args)
                    {
                        return 3500;
                    }

                    #endregion
                }

                /// <summary>
                ///     A Twisted Fate's Ultimate Teleport.
                /// </summary>
                internal class TwistedFateTeleport : ITeleport
                {
                    #region Public Properties

                    /// <summary>
                    ///     Gets the type.
                    /// </summary>
                    /// <value>The type.</value>
                    public Type Type
                    {
                        get
                        {
                            return Type.TwistedFate;
                        }
                    }

                    #endregion

                    #region Public Methods and Operators

                    /// <summary>
                    ///     Gets the duration.
                    /// </summary>
                    /// <param name="args">The <see cref="GameObjectTeleportEventArgs" /> instance containing the event data.</param>
                    /// <returns>System.Int32.</returns>
                    public int GetDuration(GameObjectTeleportEventArgs args)
                    {
                        return 1500;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Gets received when a tower starts targeting a unit
            /// </summary>
            public static class TowerAggro
            {
                #region Static Fields

                /// <summary>
                ///     The aggro list
                /// </summary>
                public static readonly Dictionary<int, int> AggroList = new Dictionary<int, int>();

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x6A;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
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

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteInteger(packetStruct.TurretNetworkId);
                    result.WriteInteger(packetStruct.TargetNetworkId);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when a tower focuses a target.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The target network identifier
                    /// </summary>
                    public int TargetNetworkId;

                    /// <summary>
                    ///     The turret network identifier
                    /// </summary>
                    public int TurretNetworkId;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="turretNetworkId">The turret network identifier.</param>
                    /// <param name="targetNetworkId">The target network identifier.</param>
                    public Struct(int turretNetworkId, int targetNetworkId)
                    {
                        this.TurretNetworkId = turretNetworkId;
                        this.TargetNetworkId = targetNetworkId;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Gets received when the model changes.
            /// </summary>
            public static class UpdateModel
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x1A;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
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

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var result = new GamePacket(Header);
                    result.WriteByte(0);
                    result.WriteInteger(packetStruct.NetworkId);
                    result.WriteInteger(packetStruct.NetworkId & ~0x40000000);
                    result.WriteByte((byte)(packetStruct.BOk ? 1 : 0));
                    result.WriteInteger(packetStruct.SkinId);
                    for (var i = 0; i < 32; i++)
                    {
                        if (i < packetStruct.ModelName.Length)
                        {
                            result.WriteByte((byte)packetStruct.ModelName[i]);
                        }
                        else
                        {
                            result.WriteByte(0x00);
                        }
                    }

                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when the model of a unit is updated.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The b ok
                    /// </summary>
                    public bool BOk;

                    /// <summary>
                    ///     The identifier
                    /// </summary>
                    public int Id;

                    /// <summary>
                    ///     The model name
                    /// </summary>
                    public string ModelName;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The skin identifier
                    /// </summary>
                    public int SkinId;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="networkId">The network identifier.</param>
                    /// <param name="skinId">The skin identifier.</param>
                    /// <param name="modelName">Name of the model.</param>
                    /// <param name="bOk">if set to <c>true</c> [b ok].</param>
                    /// <param name="id">The identifier.</param>
                    public Struct(int networkId, int skinId, string modelName, bool bOk = true, int id = -1)
                    {
                        this.NetworkId = networkId;
                        this.Id = id != -1 ? id : this.NetworkId & ~0x40000000;
                        this.BOk = bOk;
                        this.SkinId = skinId;
                        this.ModelName = modelName;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Packet received on gold change.
            /// </summary>
            public class AddGold
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x22;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
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

                /// <summary>
                ///     Encodes the specified p structure.
                /// </summary>
                /// <param name="pStruct">The p structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct pStruct)
                {
                    var packet = new GamePacket(Header);
                    packet.WriteInteger(pStruct.ReceivingNetworkId);
                    packet.WriteInteger(pStruct.ReceivingNetworkId);
                    packet.WriteInteger(pStruct.SourceNetworkId);
                    packet.WriteFloat(pStruct.Gold);
                    return packet;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when a unit's gold is changed.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The gold
                    /// </summary>
                    public float Gold;

                    /// <summary>
                    ///     The receiving network identifier
                    /// </summary>
                    public int ReceivingNetworkId;

                    /// <summary>
                    ///     The receiving unit
                    /// </summary>
                    public Obj_AI_Base ReceivingUnit;

                    /// <summary>
                    ///     The source network identifier
                    /// </summary>
                    public int SourceNetworkId;

                    /// <summary>
                    ///     The source unit
                    /// </summary>
                    public Obj_AI_Base SourceUnit;

                    #endregion
                }
            }

            /// <summary>
            ///     Packet received on buying item.
            /// </summary>
            public class BuyItemAns
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x6F;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
                    result.Item = new Items.Item(packet.ReadShort(), 0);
                    packet.Position += 2;
                    result.InventorySlot = packet.ReadByte();
                    result.SpellSlot = (SpellSlot)(result.InventorySlot + (byte)SpellSlot.Item1);
                    result.Stack = packet.ReadByte();
                    result.Charge = packet.ReadByte();
                    result.ReplaceItem = packet.ReadByte();

                    return result;
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="pStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct pStruct)
                {
                    var packet = new GamePacket(Header);
                    packet.WriteInteger(pStruct.NetworkId);
                    packet.WriteShort((short)pStruct.Item.Id);
                    packet.WriteByte(0, 2);
                    packet.WriteByte(pStruct.InventorySlot);
                    packet.WriteByte((byte)pStruct.Stack);
                    packet.WriteByte((byte)pStruct.Charge);
                    packet.WriteByte(pStruct.ReplaceItem);

                    return packet;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when a unit buys an item.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The charges of the item.
                    /// </summary>
                    public int Charge;

                    /// <summary>
                    ///     The inventory slot
                    /// </summary>
                    public byte InventorySlot;

                    /// <summary>
                    ///     The item
                    /// </summary>
                    public Items.Item Item;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The replace item
                    /// </summary>
                    public byte ReplaceItem;

                    /// <summary>
                    ///     The spell slot
                    /// </summary>
                    public SpellSlot SpellSlot;

                    /// <summary>
                    ///     The stacks of the item.
                    /// </summary>
                    public int Stack;

                    /// <summary>
                    ///     The unit
                    /// </summary>
                    public Obj_AI_Hero Unit;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="id">The identifier.</param>
                    /// <param name="slot">The slot.</param>
                    /// <param name="replace">The replace.</param>
                    /// <param name="stack">The stack.</param>
                    /// <param name="charge">The charge.</param>
                    /// <param name="networkId">The network identifier.</param>
                    public Struct(
                        int id,
                        byte slot,
                        byte replace = 0x7B,
                        int stack = 1,
                        int charge = 0,
                        int networkId = -1)
                    {
                        this.Item = new Items.Item(id, 0);
                        this.InventorySlot = slot;
                        this.SpellSlot = (SpellSlot)(this.InventorySlot + (byte)SpellSlot.Item1);
                        this.ReplaceItem = replace;
                        this.Stack = stack;
                        this.Charge = charge;
                        this.NetworkId = networkId == -1 ? ObjectManager.Player.NetworkId : networkId;
                        this.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(this.NetworkId);
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Packet received on spell slot changing.
            /// </summary>
            public class ChangeSpellSlot
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x17;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
                    result.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.NetworkId);
                    result.Slot = (SpellSlot)(packet.ReadByte());
                    result.UnknownByte = packet.ReadByte(); // 0, 1C, 48
                    result.UnknownByte2 = packet.ReadByte(); //usually 2
                    result.SpellString = packet.ReadString(11);
                    return result;
                }

                /// <summary>
                ///     Encodes the specified p structure.
                /// </summary>
                /// <param name="pStruct">The p structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct pStruct)
                {
                    var packet = new GamePacket(Header);
                    packet.WriteInteger(pStruct.NetworkId);
                    packet.WriteByte((byte)pStruct.Slot);
                    packet.WriteByte(pStruct.UnknownByte);
                    packet.WriteByte(2);
                    packet.WriteByte(0, 3);
                    packet.WriteString(pStruct.SpellString);
                    return packet;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when a spell slot is changed.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The slot
                    /// </summary>
                    public SpellSlot Slot;

                    /// <summary>
                    ///     The spell string
                    /// </summary>
                    public string SpellString;

                    /// <summary>
                    ///     The unit
                    /// </summary>
                    public Obj_AI_Base Unit;

                    /// <summary>
                    ///     An unknown byte. Possibly the previous slot.
                    /// </summary>
                    public byte UnknownByte; // from slot?

                    /// <summary>
                    ///     An unknown byte2
                    /// </summary>
                    public byte UnknownByte2;

                    #endregion
                }
            }

            /// <summary>
            ///     Packet received when a unit deals damage.
            /// </summary>
            public class Damage
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x23;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    packet.Position = 2;
                    result.TargetNetworkId = packet.ReadInteger();
                    result.TargetNetworkIdCopy = packet.ReadInteger();
                    result.SourceNetworkId = packet.ReadInteger();
                    //wrong:
                    result.Type = (DamageTypePacket)packet.ReadByte();
                    result.Unknown = packet.ReadShort();
                    result.DamageAmount = packet.ReadFloat();
                    return result;
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    //23 00 05 00 00 40 05 00 00 40 06 00 00 40 04 F7 33 B4 3F 01 66
                    var packet = new GamePacket(Header);
                    packet.WriteByte(0);
                    packet.WriteInteger(packetStruct.TargetNetworkId);
                    packet.WriteByte((byte)packetStruct.Type);
                    packet.WriteShort(packetStruct.Unknown); // Unknown value
                    packet.WriteFloat(packetStruct.DamageAmount);
                    packet.WriteInteger(packetStruct.TargetNetworkIdCopy);
                    packet.WriteInteger(packetStruct.SourceNetworkId);

                    return packet;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when unit deals damage.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The damage amount
                    /// </summary>
                    public float DamageAmount;

                    /// <summary>
                    ///     The source network identifier
                    /// </summary>
                    public int SourceNetworkId;

                    /// <summary>
                    ///     The target network identifier
                    /// </summary>
                    public int TargetNetworkId;

                    /// <summary>
                    ///     The target network identifier copy
                    /// </summary>
                    public int TargetNetworkIdCopy;

                    /// <summary>
                    ///     The type
                    /// </summary>
                    public DamageTypePacket Type;

                    /// <summary>
                    ///     Unknown short value.
                    /// </summary>
                    public short Unknown;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="damageAmount">The damage amount.</param>
                    /// <param name="sourceNetworkId">The source network identifier.</param>
                    /// <param name="targetNetworkId">The target network identifier.</param>
                    /// <param name="targetNetworkIdCopy">The target network identifier copy.</param>
                    /// <param name="type">The type.</param>
                    /// <param name="unknown">The unknown.</param>
                    public Struct(
                        float damageAmount,
                        int sourceNetworkId,
                        int targetNetworkId,
                        int targetNetworkIdCopy,
                        DamageTypePacket type,
                        short unknown)
                    {
                        this.DamageAmount = damageAmount;
                        this.SourceNetworkId = sourceNetworkId;
                        this.TargetNetworkId = targetNetworkId;
                        this.TargetNetworkIdCopy = targetNetworkIdCopy;
                        this.Type = type;
                        this.Unknown = unknown;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Packet received for a debug message.
            /// </summary>
            public class DebugMessage
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xF7;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Encodes the specified debug string.
                /// </summary>
                /// <param name="debugString">The debug string.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(String debugString)
                {
                    var packet = new GamePacket(Header);

                    packet.WriteByte(0, 8);
                    packet.WriteString(debugString);
                    packet.WriteByte(0);
                    return packet;
                }

                #endregion
            }

            /// <summary>
            ///     Packet received to print floating text.
            /// </summary>
            public class FloatText
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x19;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
                    result.Type = (FloatTextPacket)packet.ReadByte();
                    //result.Text = packet.ReadString();

                    return result;
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var packet = new GamePacket(Header);

                    packet.WriteInteger(0);
                    packet.WriteInteger(packetStruct.NetworkId);
                    packet.WriteByte((byte)packetStruct.Type);
                    packet.WriteInteger(packetStruct.NetworkId);
                    packet.WriteString(packetStruct.Text);
                    packet.WriteByte(0);

                    return packet;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when text is to be placed on a unit.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The text
                    /// </summary>
                    public string Text;

                    /// <summary>
                    ///     The type
                    /// </summary>
                    public FloatTextPacket Type;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="text">The text.</param>
                    /// <param name="type">The type.</param>
                    /// <param name="networkId">The network identifier.</param>
                    public Struct(string text, FloatTextPacket type, int networkId = 0)
                    {
                        this.NetworkId = networkId == 0 ? ObjectManager.Player.NetworkId : networkId;
                        this.Text = text;
                        this.Type = type;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     Packet received on gaining buff.
            /// </summary>
            public class GainBuff
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xB7;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
                    result.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.NetworkId);

                    result.BuffSlot = packet.ReadByte();
                    result.Type = (BuffType)packet.ReadByte();
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

                #endregion

                /// <summary>
                ///     Represents the packet received when a unit gains a buff.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The buff identifier
                    /// </summary>
                    public int BuffId;

                    /// <summary>
                    ///     The buff slot
                    /// </summary>
                    public byte BuffSlot;

                    /// <summary>
                    ///     The duration of the buff
                    /// </summary>
                    public float Duration;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The source
                    /// </summary>
                    public Obj_AI_Base Source;

                    /// <summary>
                    ///     The source network identifier
                    /// </summary>
                    public int SourceNetworkId;

                    /// <summary>
                    ///     The buff stacks
                    /// </summary>
                    public int Stack;

                    /// <summary>
                    ///     The target
                    /// </summary>
                    public Obj_AI_Base Target;

                    /// <summary>
                    ///     The target network identifier
                    /// </summary>
                    public int TargetNetworkId;

                    /// <summary>
                    ///     The buff type
                    /// </summary>
                    public BuffType Type;

                    /// <summary>
                    ///     The unit
                    /// </summary>
                    public Obj_AI_Base Unit;

                    /// <summary>
                    ///     <c>true</c> if the buff is visible.
                    /// </summary>
                    public bool Visible;

                    #endregion
                }
            }

            /// <summary>
            ///     Packet received to highlight a unit.
            /// </summary>
            public class HighlightUnit
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x59;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Encodes the specified network identifier.
                /// </summary>
                /// <param name="networkId">The network identifier.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(int networkId)
                {
                    var packet = new GamePacket(Header);

                    packet.WriteInteger(0);
                    packet.WriteInteger(networkId);

                    return packet;
                }

                #endregion
            }

            /// <summary>
            ///     Received on hero level up.
            /// </summary>
            public class LevelUp
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xCB;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
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

                #endregion

                /// <summary>
                ///     Represents the packet received when a unit levels up.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The new level
                    /// </summary>
                    public int Level;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The amount points left to level up other spells.
                    /// </summary>
                    public int PointsLeft;

                    /// <summary>
                    ///     The unit
                    /// </summary>
                    public Obj_AI_Hero Unit;

                    #endregion
                }
            }

            /// <summary>
            ///     Received on hero level up spell.
            /// </summary>
            public class LevelUpSpell
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xA9;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(2);
                    result.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(result.NetworkId);
                    result.Slot = (SpellSlot)packet.ReadByte();
                    result.PointsLeft = packet.ReadByte();
                    result.Level = packet.ReadByte();
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when a unit levels up a spell.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The new level
                    /// </summary>
                    public int Level;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The amount points left to level up other spells.
                    /// </summary>
                    public int PointsLeft;

                    /// <summary>
                    ///     The slot
                    /// </summary>
                    public SpellSlot Slot;

                    /// <summary>
                    ///     The unit
                    /// </summary>
                    public Obj_AI_Hero Unit;

                    #endregion
                }
            }

            /// <summary>
            ///     Packet received on losing buff.
            /// </summary>
            public class LoseBuff
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x7B;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
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

                #endregion

                /// <summary>
                ///     Represents the packet received when a unit loses a buff.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The buff identifier
                    /// </summary>
                    public int BuffId;

                    /// <summary>
                    ///     The buff slot
                    /// </summary>
                    public byte BuffSlot;

                    /// <summary>
                    ///     The duration of the buff
                    /// </summary>
                    public float Duration;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The unit
                    /// </summary>
                    public Obj_AI_Base Unit;

                    #endregion
                }
            }

            /// <summary>
            ///     Packet received on player disconnect.
            /// </summary>
            public class PlayerDisconnect
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xFE;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    packet.Position = 6;
                    result.NetworkId = packet.ReadInteger();
                    result.Player = ObjectManager.GetUnitByNetworkId<Obj_AI_Hero>(result.NetworkId);

                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when a player disconnects from the game.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The player
                    /// </summary>
                    public Obj_AI_Hero Player;

                    #endregion
                }
            }

            /// <summary>
            ///     Packet received when a player presses the "Reconnect" Button.
            /// </summary>
            public class PlayerReconnect
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x0;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    packet.Position = 4;
                    result.ClientId = packet.ReadInteger();
                    result.Player = ObjectManager.Get<Obj_AI_Hero>().ElementAt(result.ClientId);

                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when a player reconects to the game.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The client identifier
                    /// </summary>
                    public int ClientId;

                    /// <summary>
                    ///     The player
                    /// </summary>
                    public Obj_AI_Hero Player;

                    #endregion
                }
            }

            /// <summary>
            ///     Packet received when a player reconnected.
            /// </summary>
            public class PlayerReconnected
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xF;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    packet.Position = 5;
                    result.ClientId = packet.ReadInteger();
                    result.Player = ObjectManager.Get<Obj_AI_Hero>().ElementAt(result.ClientId);

                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when a player reconnectes to the game.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The client identifier
                    /// </summary>
                    public int ClientId;

                    /// <summary>
                    ///     The player
                    /// </summary>
                    public Obj_AI_Hero Player;

                    #endregion
                }
            }

            /// <summary>
            ///     Packet received to remove a unit's highlight.
            /// </summary>
            public class RemoveHighlightUnit
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xB4;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Encodes the specified network identifier.
                /// </summary>
                /// <param name="networkId">The network identifier.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(int networkId)
                {
                    var packet = new GamePacket(Header);

                    packet.WriteInteger(0);
                    packet.WriteInteger(networkId);

                    return packet;
                }

                #endregion
            }

            /// <summary>
            ///     Packet received on selling item.
            /// </summary>
            public class SellItemAns
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xD3;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(2);
                    result.InventorySlot = packet.ReadByte();
                    result.SpellSlot = (SpellSlot)(result.InventorySlot + (byte)SpellSlot.Item1);
                    result.Stack = packet.ReadByte();
                    result.UnknownByte = packet.ReadByte();
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when a unit sells an item.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The inventory slot
                    /// </summary>
                    public byte InventorySlot;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The spell slot
                    /// </summary>
                    public SpellSlot SpellSlot;

                    /// <summary>
                    ///     The stack
                    /// </summary>
                    public int Stack;

                    /// <summary>
                    ///     The unit
                    /// </summary>
                    public Obj_AI_Hero Unit;

                    /// <summary>
                    ///     An unknown byte
                    /// </summary>
                    public byte UnknownByte;

                    #endregion
                }
            }

            /// <summary>
            ///     A packet that sets cooldown of a spell/item.
            /// </summary>
            public class SetCooldown
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x85;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
                    result.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.NetworkId);
                    result.Slot = (SpellSlot)packet.ReadByte();
                    packet.Position += 1;
                    result.TotalCooldown = packet.ReadFloat();
                    result.CurrentCooldown = packet.ReadFloat();

                    return result;
                }

                /// <summary>
                ///     Encodes the specified packet structure.
                /// </summary>
                /// <param name="packetStruct">The packet structure.</param>
                /// <returns>GamePacket.</returns>
                public static GamePacket Encoded(Struct packetStruct)
                {
                    var packet = new GamePacket(Header);
                    packet.WriteInteger(packetStruct.NetworkId);
                    packet.WriteByte((byte)packetStruct.Slot);
                    packet.WriteByte(0xF8);
                    packet.WriteFloat(packetStruct.TotalCooldown);
                    packet.WriteFloat(packetStruct.CurrentCooldown);

                    return packet;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received to set the cooldown of a spell/item.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The current cooldown
                    /// </summary>
                    public float CurrentCooldown;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The slot
                    /// </summary>
                    public SpellSlot Slot;

                    /// <summary>
                    ///     The total cooldown
                    /// </summary>
                    public float TotalCooldown;

                    /// <summary>
                    ///     The unit
                    /// </summary>
                    public Obj_AI_Base Unit;

                    #endregion

                    #region Constructors and Destructors

                    /// <summary>
                    ///     Initializes a new instance of the <see cref="Struct" /> struct.
                    /// </summary>
                    /// <param name="networkId">The network identifier.</param>
                    /// <param name="slot">The slot.</param>
                    /// <param name="totalCd">The total cd.</param>
                    /// <param name="currentCd">The current cd.</param>
                    public Struct(int networkId, SpellSlot slot, float totalCd, float currentCd)
                    {
                        this.NetworkId = networkId;
                        this.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(this.NetworkId);
                        this.Slot = slot;
                        this.TotalCooldown = totalCd;
                        this.CurrentCooldown = currentCd;
                    }

                    #endregion
                }
            }

            /// <summary>
            ///     One packet that starts cooldown (mostly for items).
            /// </summary>
            public class StartItemCooldown
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x9F;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(1);
                    result.Unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(result.NetworkId);
                    result.InventorySlot = packet.ReadByte();
                    result.SpellSlot = (SpellSlot)(result.InventorySlot + (byte)SpellSlot.Item1);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received to start the cooldown of an item.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The inventory slot
                    /// </summary>
                    public byte InventorySlot;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The spell slot
                    /// </summary>
                    public SpellSlot SpellSlot;

                    /// <summary>
                    ///     The unit
                    /// </summary>
                    public Obj_AI_Base Unit;

                    #endregion
                }
            }

            /// <summary>
            ///     Received when someone casts a surrender vote.
            /// </summary>
            public class Surrender
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xC9;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct
                                     {
                                         NetworkId = packet.ReadInteger(6), YesVotes = packet.ReadByte(10),
                                         NoVotes = packet.ReadByte(11), MaxVotes = packet.ReadByte(12),
                                         Team = (GameObjectTeam)packet.ReadByte(13)
                                     };

                    //byte unknown = packet.ReadByte(5); //Not sure what this is

                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received a player votes on a surrender.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The maximum votes
                    /// </summary>
                    public int MaxVotes;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The amount of no votes
                    /// </summary>
                    public int NoVotes;

                    /// <summary>
                    ///     The team that is surrendering
                    /// </summary>
                    public GameObjectTeam Team;

                    /// <summary>
                    ///     The amount of yes votes
                    /// </summary>
                    public int YesVotes;

                    #endregion
                }
            }

            /// <summary>
            ///     Received when surrender voting is over.
            /// </summary>
            public class SurrenderResult
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0xA5;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct
                                     {
                                         TooEarly = Convert.ToBoolean(packet.ReadByte(5)), YesVotes = packet.ReadByte(9),
                                         NoVotes = packet.ReadByte(10), Team = (GameObjectTeam)packet.ReadByte(11)
                                     };

                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when a team finishes their decision on a surrender.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The amount of no votes
                    /// </summary>
                    public int NoVotes;

                    /// <summary>
                    ///     The team that is surrendering.
                    /// </summary>
                    public GameObjectTeam Team;

                    /// <summary>
                    ///     <c>true</c> if the request to surrender was denied because it was too early in the game.
                    /// </summary>
                    public bool TooEarly;

                    /// <summary>
                    ///     The amount of yes votes
                    /// </summary>
                    public int YesVotes;

                    #endregion
                }
            }

            /// <summary>
            ///     Packet received on swapping item.
            /// </summary>
            public class SwapItemAns
            {
                #region Static Fields

                /// <summary>
                ///     The header
                /// </summary>
                public static byte Header = 0x09;

                #endregion

                #region Public Methods and Operators

                /// <summary>
                ///     Decodes the specified data.
                /// </summary>
                /// <param name="data">The data.</param>
                /// <returns>Struct.</returns>
                public static Struct Decoded(byte[] data)
                {
                    var packet = new GamePacket(data);
                    var result = new Struct();

                    result.NetworkId = packet.ReadInteger(2);
                    result.FromInventorySlot = packet.ReadByte();
                    result.FromSpellSlot = (SpellSlot)(result.FromInventorySlot + (byte)SpellSlot.Item1);
                    result.ToInventorySlot = packet.ReadByte();
                    result.ToSpellSlot = (SpellSlot)(result.ToInventorySlot + (byte)SpellSlot.Item1);
                    return result;
                }

                #endregion

                /// <summary>
                ///     Represents the packet received when a unit swaps an item.
                /// </summary>
                public struct Struct
                {
                    #region Fields

                    /// <summary>
                    ///     The previous inventory slot
                    /// </summary>
                    public byte FromInventorySlot;

                    /// <summary>
                    ///     The previous spell slot
                    /// </summary>
                    public SpellSlot FromSpellSlot;

                    /// <summary>
                    ///     The network identifier
                    /// </summary>
                    public int NetworkId;

                    /// <summary>
                    ///     The new inventory slot
                    /// </summary>
                    public byte ToInventorySlot;

                    /// <summary>
                    ///     The new spell slot
                    /// </summary>
                    public SpellSlot ToSpellSlot;

                    /// <summary>
                    ///     The unit
                    /// </summary>
                    public Obj_AI_Hero Unit;

                    #endregion
                }
            }
        }
    }
}