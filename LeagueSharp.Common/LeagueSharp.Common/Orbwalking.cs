#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    ///     This class offers everything related to auto-attacks and orbwalking.
    /// </summary>
    public static class Orbwalking
    {
        public delegate void AfterAttackEvenH(Obj_AI_Base unit, Obj_AI_Base target);

        public delegate void OnAttackEvenH(Obj_AI_Base unit, Obj_AI_Base target);

        private static readonly Dictionary<String, float> AttackDelayCastOffsetPercent =
            new Dictionary<String, float>(StringComparer.InvariantCultureIgnoreCase);

        private static readonly Dictionary<String, float> AttackDelayOffsetPercent =
            new Dictionary<String, float>(StringComparer.InvariantCultureIgnoreCase);

        private static readonly List<string> AttackResets = new List<string>();
        private static readonly List<string> NoAttacks = new List<string>();
        private static readonly List<string> Attacks = new List<string>();
        private static readonly List<AttackPassive> AttackPassives = new List<AttackPassive>();

        private static int LastAATick;

        public static bool Attack = true;
        public static bool Move = true;

        static Orbwalking()
        {
            LoadTheData();
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
            Game.OnGameProcessPacket += OnProcessPacket;
        }

        /// <summary>
        ///     This event is fired when a unit is about to auto-attack another unit.
        /// </summary>
        public static event OnAttackEvenH OnAttack;

        /// <summary>
        ///     This event is fired after a unit finishes auto-attacking another unit (Only works with player for now).
        /// </summary>
        public static event AfterAttackEvenH AfterAttack;

        private static void FireOnAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if (OnAttack != null)
                OnAttack(unit, target);
        }

        private static void FireAfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if (AfterAttack != null)
                AfterAttack(unit, target);
        }

        /// <summary>
        ///     Returns the auto-attack passive damage.
        /// </summary>
        public static float GetAutoAttackPassiveDamage(Obj_AI_Minion minion)
        {
            var totaldamage = 0f;

            foreach (var passive in AttackPassives)
            {
                if (ObjectManager.Player.HasBuff(passive.BuffName))
                {
                    totaldamage += passive.CalcExtraDamage(minion);
                }
            }

            return totaldamage;
        }

        /// <summary>
        ///     Returns true if the spellname resets the attack timer.
        /// </summary>
        public static bool IsAutoAttackReset(string name)
        {
            return AttackResets.Contains(name.ToLower());
        }

        /// <summary>
        ///     Returns true if the spellname is an auto-attack.
        /// </summary>
        public static bool IsAutoAttack(string name)
        {
            return (name.ToLower().Contains("attack") && !NoAttacks.Contains(name.ToLower())) ||
                   Attacks.Contains(name.ToLower());
        }

        /// <summary>
        ///     Returns the auto-attack range.
        /// </summary>
        public static float GetRealAutoAttackRange(Obj_AI_Base target)
        {
            var result = ObjectManager.Player.AttackRange + ObjectManager.Player.BoundingRadius;
            if (target != null && target.IsValidTarget())
            {
                return result + target.BoundingRadius - ((target.Path.Length > 0) ? 20 : 0);
            }
            return result;
        }

        /// <summary>
        ///     Returns true if the target is in auto-attack range.
        /// </summary>
        public static bool InAutoAttackRange(Obj_AI_Base target)
        {
            if (target != null)
            {
                var myRange = GetRealAutoAttackRange(target);
                return
                    Vector2.DistanceSquared(target.ServerPosition.To2D(),
                        ObjectManager.Player.ServerPosition.To2D()) <= myRange * myRange;
            }
            return false;
        }

        /// <summary>
        ///     Returns player auto-attack missile speed.
        /// </summary>
        public static float GetMyProjectileSpeed()
        {
            return ObjectManager.Player.BasicAttack.MissileSpeed;
        }


        /// <summary>
        ///     Returns the WindUp time in milliseconds. (The time a unit has to wait before starting to move again after an
        ///     auto-attack in order to not cancel it).
        /// </summary>
        public static float GetWindupTime(this Obj_AI_Base unit, bool Extra = true, float ExtraWindup = 40)
        {
            if (AttackDelayCastOffsetPercent.ContainsKey(unit.BaseSkinName))
            {
                return (300 / (unit.AttackSpeedMod * (1 - AttackDelayCastOffsetPercent[unit.BaseSkinName]))) +
                       (Extra ? ExtraWindup : 0);
            }
            //Game.PrintChat(unit.BaseSkinName + " Is not in the Windup times database");
            return (300 / (unit.AttackSpeedMod * (1 - AttackDelayCastOffsetPercent["Caitlyn"]))) + (Extra ? ExtraWindup : 0);
        }

        /// <summary>
        ///     Returns if the player's auto-attack is ready.
        /// </summary>
        public static bool CanAttack()
        {
            if (LastAATick <= Environment.TickCount)
            {
                return Environment.TickCount + Game.Ping / 2 + 25 >=
                       LastAATick + ObjectManager.Player.AttackDelay * 1000 &&
                       Attack;
            }

            return false;
        }

        /// <summary>
        ///     Returns true if moving won't cancel the auto-attack.
        /// </summary>
        public static bool CanMove(float ExtraWindup)
        {
            if (LastAATick <= Environment.TickCount)
            {
                return Environment.TickCount + Game.Ping / 2 >=
                       LastAATick + ObjectManager.Player.AttackCastDelay * 1000 + ExtraWindup && Move;
            }

            return false;
        }

        private static void MoveTo(Vector3 position)
        {
            var point = ObjectManager.Player.ServerPosition +
                        (position.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized().To3D() * 300;
            ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, point);
        }

        /// <summary>
        ///     Orbwalk a target while moving to Position.
        /// </summary>
        public static void Orbwalk(Obj_AI_Base target, Vector3 Position, float ExtraWindup = 90)
        {
            if (target != null)
            {
                if (CanAttack())
                {
                    ObjectManager.Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                    if (target.Type != ObjectManager.Player.Type)
                        LastAATick = Environment.TickCount + 0 /*Game.Ping / 2*/;
                    return;
                }

                if (CanMove(ExtraWindup))
                {
                    MoveTo(Position);
                }
            }
            else if (CanMove(ExtraWindup))
            {
                MoveTo(Position);
            }
        }

        private static void OnProcessPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] == 0x33)
            {
                var stream = new MemoryStream(args.PacketData);
                var b = new BinaryReader(stream);
                b.BaseStream.Position = b.BaseStream.Position + 1;
                var unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(BitConverter.ToInt32(b.ReadBytes(4), 0));

                if (args.PacketData[9] == 17)
                {
                    if (unit.IsMe)
                    {
                        LastAATick = 0;
                    }
                }
                else
                {
                    if (unit.IsMe && unit.AttackRange < 400)
                    {
                        FireAfterAttack(unit, unit);
                    }
                }
            }
        }

        private static void OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs Spell)
        {
            if (IsAutoAttackReset(Spell.SData.Name) && unit.IsMe)
            {
                new Timer(obj => { LastAATick = 0; }, null, 25, Timeout.Infinite);
            }

            if (IsAutoAttack(Spell.SData.Name))
            {
                if (unit.IsMe)
                {
                    LastAATick = Environment.TickCount - Game.Ping / 2;
                }

                FireOnAttack(unit, unit);
            }
        }

        private static void LoadTheData()
        {
            /*Passive list*/

            /*Caitlyn*/
            var PassiveToAdd = new AttackPassive("Caitlyn", "CaitlynHeadshotReady");
            PassiveToAdd.TotalDamageMultiplicator = 1.5f;
            PassiveToAdd.DamageType = 2;
            AttackPassives.Add(PassiveToAdd);

            /*Draven*/
            PassiveToAdd = new AttackPassive("Draven", "dravenspinning");
            PassiveToAdd.TotalDamageMultiplicator = 0.45f;
            PassiveToAdd.DamageType = 2;
            AttackPassives.Add(PassiveToAdd);

            /*Vayne*/
            PassiveToAdd = new AttackPassive("Vayne", "VayneTumble");
            PassiveToAdd.TotalDamageMultiplicator = 0.3f;
            PassiveToAdd.DamageType = 2;
            AttackPassives.Add(PassiveToAdd);

            /*Corki*/
            PassiveToAdd = new AttackPassive("Corki", "RapidReload");
            PassiveToAdd.TotalDamageMultiplicator = 0.1f;
            PassiveToAdd.DamageType = 0;
            AttackPassives.Add(PassiveToAdd);

            /* Teemo*/
            PassiveToAdd = new AttackPassive("Teemo", "Toxic Attack");
            PassiveToAdd.APScaling = 0.3f;
            PassiveToAdd.slot = SpellSlot.E;
            PassiveToAdd.SpellBaseDamage = 0;
            PassiveToAdd.SpellDamagePerLevel = 10;
            PassiveToAdd.DamageType = 1;
            AttackPassives.Add(PassiveToAdd);

            /* Varus*/
            PassiveToAdd = new AttackPassive("Varus", "VarusW");
            PassiveToAdd.APScaling = 0.25f;
            PassiveToAdd.slot = SpellSlot.W;
            PassiveToAdd.SpellBaseDamage = 6;
            PassiveToAdd.SpellDamagePerLevel = 4;
            PassiveToAdd.DamageType = 1;
            AttackPassives.Add(PassiveToAdd);

            /* MissFortune*/
            PassiveToAdd = new AttackPassive("MissFortune", "MissFortunePassive");
            PassiveToAdd.TotalDamageMultiplicator = 0.06f;
            PassiveToAdd.DamageType = 1;
            AttackPassives.Add(PassiveToAdd);

            /* Twisted Fate W*/
            PassiveToAdd = new AttackPassive("TwistedFate", "Pick A Card Blue");
            PassiveToAdd.APScaling = 0.5f;
            PassiveToAdd.slot = SpellSlot.E;
            PassiveToAdd.SpellBaseDamage = 20;
            PassiveToAdd.SpellDamagePerLevel = 20;
            PassiveToAdd.DamageType = 1;
            AttackPassives.Add(PassiveToAdd);

            /* Twisted Fate E*/
            PassiveToAdd = new AttackPassive("TwistedFate", "CardMasterStackParticle");
            PassiveToAdd.APScaling = 0.5f;
            PassiveToAdd.slot = SpellSlot.E;
            PassiveToAdd.SpellBaseDamage = 30;
            PassiveToAdd.SpellDamagePerLevel = 25;
            PassiveToAdd.DamageType = 1;
            AttackPassives.Add(PassiveToAdd);

            /* Oriannas Passive*/
            PassiveToAdd = new AttackPassive("Orianna", "OrianaSpellSword");
            PassiveToAdd.APScaling = 0.15f;
            PassiveToAdd.LevelDamageArray = new float[] { 10, 10, 10, 18, 18, 18, 26, 26, 26, 34, 34, 34, 42, 42, 42, 50, 50, 50 };
            PassiveToAdd.DamageType = 1;
            AttackPassives.Add(PassiveToAdd);


            AttackDelayOffsetPercent.Add("XerathArcaneBarrageLauncher", -0.375f);
            AttackDelayOffsetPercent.Add("Caitlyn", 0f);
            AttackDelayOffsetPercent.Add("TestCube", -0.375f);
            AttackDelayOffsetPercent.Add("ARAMOrderTurretNexus", -0.25f);
            AttackDelayOffsetPercent.Add("ShopKeeper", 0f);
            AttackDelayOffsetPercent.Add("Mordekaiser", -0.1f);
            AttackDelayOffsetPercent.Add("OdinQuestBuff", -0.375f);
            AttackDelayOffsetPercent.Add("Lizard", -0.02f);
            AttackDelayOffsetPercent.Add("GolemOdin", 0.02f);
            AttackDelayOffsetPercent.Add("OdinOpeningBarrier", 0f);
            AttackDelayOffsetPercent.Add("TT_ChaosTurret2", -0.25f);
            AttackDelayOffsetPercent.Add("TT_ChaosTurret3", -0.25f);
            AttackDelayOffsetPercent.Add("TT_ChaosTurret1", -0.25f);
            AttackDelayOffsetPercent.Add("OdinShieldRelic", 0f);
            AttackDelayOffsetPercent.Add("Tutorial_Red_Minion_Wizard", -0.067f);
            AttackDelayOffsetPercent.Add("Renekton", -0.06f);
            AttackDelayOffsetPercent.Add("Anivia", 0f);
            AttackDelayOffsetPercent.Add("ChaosTurretGiant", -0.25f);
            AttackDelayOffsetPercent.Add("Dragon", -0.05f);
            AttackDelayOffsetPercent.Add("SmallGolem", 0.02f);
            AttackDelayOffsetPercent.Add("ARAMOrderTurretFront", -0.25f);
            AttackDelayOffsetPercent.Add("TestCubeRender", -0.375f);
            AttackDelayOffsetPercent.Add("Worm", 2.0f);
            AttackDelayOffsetPercent.Add("ChaosTurretTutorial", -0.375f);
            AttackDelayOffsetPercent.Add("redDragon", -0.05f);
            AttackDelayOffsetPercent.Add("ChaosTurretWorm", -.25f);
            AttackDelayOffsetPercent.Add("Darius", -0.08f);
            AttackDelayOffsetPercent.Add("ChaosInhibitor_D", 0f);
            AttackDelayOffsetPercent.Add("OdinChaosTurretShrine", 65535f);
            AttackDelayOffsetPercent.Add("OdinCenterRelic", 0f);
            AttackDelayOffsetPercent.Add("Vladimir", -0.05f);
            AttackDelayOffsetPercent.Add("DestroyedNexus", 0f);
            AttackDelayOffsetPercent.Add("Wraith", -0.02f);
            AttackDelayOffsetPercent.Add("ARAMOrderNexus", 0f);
            AttackDelayOffsetPercent.Add("OrderInhibitor_D", 0f);
            AttackDelayOffsetPercent.Add("SyndraOrbs", 0f);
            AttackDelayOffsetPercent.Add("Nidalee", -0.0672f);
            AttackDelayOffsetPercent.Add("Ziggs", -0.04734f);
            AttackDelayOffsetPercent.Add("TT_ChaosInhibitor", 0f);
            AttackDelayOffsetPercent.Add("ChaosTurretNormal", -0.25f);
            AttackDelayOffsetPercent.Add("CaitlynTrap", 0f);
            AttackDelayOffsetPercent.Add("ChaosInhibitor", 0f);
            AttackDelayOffsetPercent.Add("ARAMChaosTurretShrine", 65535f);
            AttackDelayOffsetPercent.Add("AncientGolem", 0.02f);
            AttackDelayOffsetPercent.Add("DestroyedInhibitor", 0f);
            AttackDelayOffsetPercent.Add("Tutorial_Red_Minion_Basic", -0.5f);
            AttackDelayOffsetPercent.Add("OdinMinionGraveyardPortal", 0f);
            AttackDelayOffsetPercent.Add("TestCubeRenderwCollision", -0.375f);
            AttackDelayOffsetPercent.Add("Summoner_Rider_Order", 0f);
            AttackDelayOffsetPercent.Add("TT_Relic", 0f);
            AttackDelayOffsetPercent.Add("OrderTurretNormal2", -0.25f);
            AttackDelayOffsetPercent.Add("Wolf", -0.05f);
            AttackDelayOffsetPercent.Add("Veigar", 0f);
            AttackDelayOffsetPercent.Add("OdinOrderTurretShrine", 65535f);
            AttackDelayOffsetPercent.Add("KogMaw", -0.06f);
            AttackDelayOffsetPercent.Add("Alistar", 0f);
            AttackDelayOffsetPercent.Add("TT_ChaosInhibitor_D", 0f);
            AttackDelayOffsetPercent.Add("TrundleWall", 0f);
            AttackDelayOffsetPercent.Add("HA_AP_HealthRelic", 0f);
            AttackDelayOffsetPercent.Add("HA_FB_HealthRelic", 0f);
            AttackDelayOffsetPercent.Add("TT_OrderInhibitor", 0f);
            AttackDelayOffsetPercent.Add("Galio", -0.02f);
            AttackDelayOffsetPercent.Add("Nasus", -0.02f);
            AttackDelayOffsetPercent.Add("Golem", 0.02f);
            AttackDelayOffsetPercent.Add("OrderTurretDragon", -0.25f);
            AttackDelayOffsetPercent.Add("OrderTurretNormal", -0.25f);
            AttackDelayOffsetPercent.Add("Kayle", -0.02f);
            AttackDelayOffsetPercent.Add("Brand", 0f);
            AttackDelayOffsetPercent.Add("Amumu", -0.02f);
            AttackDelayOffsetPercent.Add("Annie", 0.08f);
            AttackDelayOffsetPercent.Add("LizardElder", 0f);
            AttackDelayOffsetPercent.Add("ARAMChaosTurretFront", -.25f);
            AttackDelayOffsetPercent.Add("SummonerBeacon", 0.02f);
            AttackDelayOffsetPercent.Add("MissFortune", -0.04734f);
            AttackDelayOffsetPercent.Add("ARAMOrderTurretInhib", -0.25f);
            AttackDelayOffsetPercent.Add("Shop", 0f);
            AttackDelayOffsetPercent.Add("Cassiopeia", -0.034f);
            AttackDelayOffsetPercent.Add("OrderInhibitor", 0f);
            AttackDelayOffsetPercent.Add("Tutorial_Blue_Minion_Wizard", -0.067f);
            AttackDelayOffsetPercent.Add("DrMundo", 0f);
            AttackDelayOffsetPercent.Add("AramSpeedShrine", -0.375f);
            AttackDelayOffsetPercent.Add("DestroyedTower", 0f);
            AttackDelayOffsetPercent.Add("GiantWolf", -0.08f);
            AttackDelayOffsetPercent.Add("Summoner_Rider_Chaos", 0f);
            AttackDelayOffsetPercent.Add("OdinSpeedShrine", -0.375f);
            AttackDelayOffsetPercent.Add("YoungLizard", -0.08f);
            AttackDelayOffsetPercent.Add("SightWard", 0f);
            AttackDelayOffsetPercent.Add("TT_SpeedShrine", -0.375f);
            AttackDelayOffsetPercent.Add("Irelia", -0.06f);
            AttackDelayOffsetPercent.Add("Lucian", -0.02f);
            AttackDelayOffsetPercent.Add("OdinMinionSpawnPortal", 0f);
            AttackDelayOffsetPercent.Add("RammusPB", 0f);
            AttackDelayOffsetPercent.Add("JarvanIV", -0.05f);
            AttackDelayOffsetPercent.Add("Jax", -0.02f);
            AttackDelayOffsetPercent.Add("LesserWraith", -0.02f);
            AttackDelayOffsetPercent.Add("Udyr", -0.05f);
            AttackDelayOffsetPercent.Add("Tutorial_Blue_Minion_Basic", -0.5f);
            AttackDelayOffsetPercent.Add("TestCubeRender10Vision", -0.375f);
            AttackDelayOffsetPercent.Add("GhostWard", 0f);
            AttackDelayOffsetPercent.Add("Ryze", 0f);
            AttackDelayOffsetPercent.Add("AniviaIceBlock", 0f);
            AttackDelayOffsetPercent.Add("TT_OrderInhibitor_D", 0f);
            AttackDelayOffsetPercent.Add("Blitzcrank", 0f);
            AttackDelayOffsetPercent.Add("OdinTestCubeRender", 0f);
            AttackDelayOffsetPercent.Add("VisionWard", 0f);
            AttackDelayOffsetPercent.Add("ARAMOrderTurretShrine", 65535f);
            AttackDelayOffsetPercent.Add("ChaosTurretWorm2", -0.25f);
            AttackDelayOffsetPercent.Add("ARAMChaosNexus", 0f);
            AttackDelayOffsetPercent.Add("TT_OrderTurret1", -0.25f);
            AttackDelayOffsetPercent.Add("TT_OrderTurret2", -0.25f);
            AttackDelayOffsetPercent.Add("ARAMChaosTurretInhib", -0.25f);
            AttackDelayOffsetPercent.Add("Ezreal", 0f);
            AttackDelayOffsetPercent.Add("TT_OrderTurret3", -0.25f);
            AttackDelayOffsetPercent.Add("OdinNeutralGuardian", 0f);
            AttackDelayOffsetPercent.Add("Sion", 0f);
            AttackDelayOffsetPercent.Add("Pantheon", -0.08f);
            AttackDelayOffsetPercent.Add("OdinQuestIndicator", 0f);
            AttackDelayOffsetPercent.Add("Zyra", 0f);
            AttackDelayOffsetPercent.Add("Karthus", 0f);
            AttackDelayOffsetPercent.Add("Sona", -0.03f);
            AttackDelayOffsetPercent.Add("Rammus", 0f);
            AttackDelayOffsetPercent.Add("TT_DummyPusher", -0.375f);
            AttackDelayOffsetPercent.Add("ARAMChaosTurretNexus", -0.25f);
            AttackDelayOffsetPercent.Add("Zilean", 0f);
            AttackDelayOffsetPercent.Add("OrderTurretAngel", -0.25f);
            AttackDelayOffsetPercent.Add("OrderTurretTutorial", -0.375f);
            AttackDelayOffsetPercent.Add("RecItemsODIN", 0f);
            AttackDelayOffsetPercent.Add("RecItemsARAM", 0f);
            AttackDelayOffsetPercent.Add("RecItemsCLASSIC", 0f);
            AttackDelayOffsetPercent.Add("Braum", -0.03f);
            AttackDelayOffsetPercent.Add("RecItemsCLASSICMap10", 0f);
            AttackDelayOffsetPercent.Add("Syndra", 0f);
            AttackDelayOffsetPercent.Add("Jinx", 0f);
            AttackDelayOffsetPercent.Add("Trundle", -0.0672f);
            AttackDelayOffsetPercent.Add("MasterYi", -0.08f);
            AttackDelayOffsetPercent.Add("Lissandra", 0f);
            AttackDelayOffsetPercent.Add("Malphite", -0.02f);
            AttackDelayOffsetPercent.Add("HA_AP_Poro", -0.05f);
            AttackDelayOffsetPercent.Add("TT_NWolf", -0.08f);
            AttackDelayOffsetPercent.Add("Vi", -0.03f);
            AttackDelayOffsetPercent.Add("Fizz", -0.05f);
            AttackDelayOffsetPercent.Add("Heimerdinger", 0f);
            AttackDelayOffsetPercent.Add("Draven", -0.08f);
            AttackDelayOffsetPercent.Add("FiddleSticks", 0f);
            AttackDelayOffsetPercent.Add("Evelynn", 0f);
            AttackDelayOffsetPercent.Add("Rumble", -0.03f);
            AttackDelayOffsetPercent.Add("Xerath", 0f);
            AttackDelayOffsetPercent.Add("Kassadin", -0.023f);
            AttackDelayOffsetPercent.Add("Leblanc", 0f);
            AttackDelayOffsetPercent.Add("Rengar", -0.08f);
            AttackDelayOffsetPercent.Add("Viktor", 0f);
            AttackDelayOffsetPercent.Add("XinZhao", -0.07f);
            AttackDelayOffsetPercent.Add("Orianna", -0.05f);
            AttackDelayOffsetPercent.Add("Ezreal_cyber_1", 0f);
            AttackDelayOffsetPercent.Add("Ezreal_cyber_3", 0f);
            AttackDelayOffsetPercent.Add("Ezreal_cyber_2", 0f);
            AttackDelayOffsetPercent.Add("Thresh", 0f);
            AttackDelayOffsetPercent.Add("Maokai", -0.1f);
            AttackDelayOffsetPercent.Add("TT_NWolf2", -0.05f);
            AttackDelayOffsetPercent.Add("Tryndamere", -0.0672f);
            AttackDelayOffsetPercent.Add("Zac", -0.02f);
            AttackDelayOffsetPercent.Add("Olaf", -0.1f);
            AttackDelayOffsetPercent.Add("Twitch", -0.08f);
            AttackDelayOffsetPercent.Add("Singed", 0.02f);
            AttackDelayOffsetPercent.Add("Akali", -0.1f);
            AttackDelayOffsetPercent.Add("Diana", 0f);
            AttackDelayOffsetPercent.Add("Urgot", -0.03f);
            AttackDelayOffsetPercent.Add("Leona", 0f);
            AttackDelayOffsetPercent.Add("Sivir", -0.05f);
            AttackDelayOffsetPercent.Add("Talon", -0.065f);
            AttackDelayOffsetPercent.Add("Corki", 0f);
            AttackDelayOffsetPercent.Add("Janna", 0f);
            AttackDelayOffsetPercent.Add("Karma", 0f);
            AttackDelayOffsetPercent.Add("Jayce", -0.05f);
            AttackDelayOffsetPercent.Add("Shaco", -0.1f);
            AttackDelayOffsetPercent.Add("Taric", 0f);
            AttackDelayOffsetPercent.Add("TwistedFate", -0.04f);
            AttackDelayOffsetPercent.Add("Varus", -0.05f);
            AttackDelayOffsetPercent.Add("Yasuo", -0.05f);
            AttackDelayOffsetPercent.Add("Garen", 0f);
            AttackDelayOffsetPercent.Add("Swain", 0f);
            AttackDelayOffsetPercent.Add("Vayne", -0.05f);
            AttackDelayOffsetPercent.Add("Fiora", -0.07f);
            AttackDelayOffsetPercent.Add("Quinn", -0.065f);
            AttackDelayOffsetPercent.Add("Teemo", -0.0947f);
            AttackDelayOffsetPercent.Add("Elise", 0f);
            AttackDelayOffsetPercent.Add("Nami", -0.03f);
            AttackDelayOffsetPercent.Add("Poppy", -0.02f);
            AttackDelayOffsetPercent.Add("Ahri", -0.065f);
            AttackDelayOffsetPercent.Add("Tristana", -0.04734f);
            AttackDelayOffsetPercent.Add("TT_NWraith2", -0.02f);
            AttackDelayOffsetPercent.Add("Graves", 0f);
            AttackDelayOffsetPercent.Add("Morgana", 0f);
            AttackDelayOffsetPercent.Add("Gragas", -0.04f);
            AttackDelayOffsetPercent.Add("Skarner", 0f);
            AttackDelayOffsetPercent.Add("Katarina", -0.05f);
            AttackDelayOffsetPercent.Add("Riven", 0f);
            AttackDelayOffsetPercent.Add("Velkoz", 0f);
            AttackDelayOffsetPercent.Add("TT_NGolem", 0.02f);
            AttackDelayOffsetPercent.Add("LeeSin", -0.04f);
            AttackDelayOffsetPercent.Add("Warwick", -0.08f);
            AttackDelayOffsetPercent.Add("Volibear", -0.05f);
            AttackDelayOffsetPercent.Add("OriannaNoBall", -0.05f);
            AttackDelayOffsetPercent.Add("Yorick", 0f);
            AttackDelayOffsetPercent.Add("MonkeyKing", -0.05f);
            AttackDelayOffsetPercent.Add("QuinnValor", -0.065f);
            AttackDelayOffsetPercent.Add("Kennen", -0.0947f);
            AttackDelayOffsetPercent.Add("Lulu", 0f);
            AttackDelayOffsetPercent.Add("Nunu", 0f);
            AttackDelayOffsetPercent.Add("Ashe", -0.05f);
            AttackDelayOffsetPercent.Add("Zed", -0.05f);
            AttackDelayOffsetPercent.Add("Nautilus", 0.02f);
            AttackDelayOffsetPercent.Add("TT_NGolem2", 0.02f);
            AttackDelayOffsetPercent.Add("TT_NWraith", -0.02f);
            AttackDelayOffsetPercent.Add("Gangplank", -0.04f);
            AttackDelayOffsetPercent.Add("Lux", 0f);
            AttackDelayOffsetPercent.Add("Sejuani", -0.0672f);
            AttackDelayOffsetPercent.Add("Khazix", -0.065f);
            AttackDelayOffsetPercent.Add("Shen", -0.04f);
            AttackDelayOffsetPercent.Add("Aatrox", -0.04f);
            AttackDelayOffsetPercent.Add("Hecarim", -0.0672f);
            AttackDelayOffsetPercent.Add("Nocturne", -0.065f);
            AttackDelayOffsetPercent.Add("Shyvana", -0.05f);
            AttackDelayOffsetPercent.Add("Soraka", 0f);
            AttackDelayOffsetPercent.Add("Chogath", 0f);
            AttackDelayOffsetPercent.Add("Malzahar", 0f);
            AttackDelayOffsetPercent.Add("ShyvanaDragon", -.05f);
            AttackDelayOffsetPercent.Add("UdyrPhoenix", -0.05f);
            AttackDelayOffsetPercent.Add("UdyrTigerUlt", -0.05f);
            AttackDelayOffsetPercent.Add("MonkeyKingClone", -0.1f);
            AttackDelayOffsetPercent.Add("NasusUlt", -0.02f);
            AttackDelayOffsetPercent.Add("SwainNoBird", 0f);
            AttackDelayOffsetPercent.Add("SwainBeam", 0f);
            AttackDelayOffsetPercent.Add("EliseSpider", 0f);
            AttackDelayOffsetPercent.Add("AniviaEgg", 0f);
            AttackDelayOffsetPercent.Add("UdyrTurtleUlt", -0.05f);
            AttackDelayOffsetPercent.Add("UdyrTurtle", -0.05f);
            AttackDelayOffsetPercent.Add("nidalee_cougar", -0.0672f);
            AttackDelayOffsetPercent.Add("UdyrUlt", -0.05f);
            AttackDelayOffsetPercent.Add("UdyrPhoenixUlt", -0.05f);
            AttackDelayOffsetPercent.Add("SwainRaven", 0f);
            AttackDelayOffsetPercent.Add("RammusDBC", 0f);
            AttackDelayOffsetPercent.Add("FizzShark", -0.05f);
            AttackDelayOffsetPercent.Add("UdyrTiger", -0.05f);
            AttackDelayOffsetPercent.Add("TT_Spiderboss", -0.05f);
            AttackDelayOffsetPercent.Add("ZyraGraspingPlant", -0.219f);
            AttackDelayOffsetPercent.Add("HeimerTBlue", -0.5f);
            AttackDelayOffsetPercent.Add("HeimerTYellow", -0.5f);
            AttackDelayOffsetPercent.Add("TeemoMushroom", 0f);
            AttackDelayOffsetPercent.Add("RecItemsTUTORIAL", 0f);
            AttackDelayOffsetPercent.Add("ZyraPassive", -0.219f);
            AttackDelayOffsetPercent.Add("ZyraThornPlant", -0.219f);
            AttackDelayOffsetPercent.Add("ChaosNexus", 0f);
            AttackDelayOffsetPercent.Add("ThreshLantern", 0f);
            AttackDelayOffsetPercent.Add("OrderNexus", 0f);
            AttackDelayOffsetPercent.Add("Red_Minion_Wizard", -0.067f);
            AttackDelayOffsetPercent.Add("Blue_Minion_Wizard", -0.067f);
            AttackDelayOffsetPercent.Add("LuluSnowman", -0.05f);
            AttackDelayOffsetPercent.Add("HA_AP_OrderTurret3", -0.25f);
            AttackDelayOffsetPercent.Add("HA_AP_OrderTurret2", -0.25f);
            AttackDelayOffsetPercent.Add("GreatWraith", -0.02f);
            AttackDelayOffsetPercent.Add("HA_AP_ChaosTurret", -0.25f);
            AttackDelayOffsetPercent.Add("HA_AP_ChaosTurret3", -0.25f);
            AttackDelayOffsetPercent.Add("HA_AP_ChaosTurret2", -0.25f);
            AttackDelayOffsetPercent.Add("LuluFaerie", -0.065f);
            AttackDelayOffsetPercent.Add("HA_AP_OrderTurret", -0.25f);
            AttackDelayOffsetPercent.Add("YellowTrinketUpgrade", 0f);
            AttackDelayOffsetPercent.Add("HA_AP_ChaosTurretTutorial", -0.375f);
            AttackDelayOffsetPercent.Add("HA_AP_OrderTurretTutorial", -0.375f);
            AttackDelayOffsetPercent.Add("brush_D_SR", 0f);
            AttackDelayOffsetPercent.Add("brush_E_SR", 0f);
            AttackDelayOffsetPercent.Add("brush_F_SR", 0f);
            AttackDelayOffsetPercent.Add("brush_C_SR", 0f);
            AttackDelayOffsetPercent.Add("brush_A_SR", 0f);
            AttackDelayOffsetPercent.Add("brush_B_SR", 0f);
            AttackDelayOffsetPercent.Add("YellowTrinket", 0f);
            AttackDelayOffsetPercent.Add("ZyraSeed", 0f);
            AttackDelayOffsetPercent.Add("OlafAxe", -0.375f);
            AttackDelayOffsetPercent.Add("ZedShadow", -0.1f);
            AttackDelayOffsetPercent.Add("Blue_Minion_Basic", -0.5f);
            AttackDelayOffsetPercent.Add("Odin_Blue_Minion_caster", -0.067f);
            AttackDelayOffsetPercent.Add("Odin_Red_Minion_Caster", -0.067f);
            AttackDelayOffsetPercent.Add("Red_Minion_Basic", -0.5f);
            AttackDelayOffsetPercent.Add("shopevo", 0f);
            AttackDelayOffsetPercent.Add("YorickRavenousGhoul", -0.0672f);
            AttackDelayOffsetPercent.Add("YorickSpectralGhoul", -0.0672f);
            AttackDelayOffsetPercent.Add("JinxMine", 0f);
            AttackDelayOffsetPercent.Add("YorickDecayedGhoul", -0.0672f);
            AttackDelayOffsetPercent.Add("Odin_SOG_Order_Crystal", 0f);
            AttackDelayOffsetPercent.Add("FizzBait", -0.05f);
            AttackDelayOffsetPercent.Add("Blue_Minion_MechMelee", -.1f);
            AttackDelayOffsetPercent.Add("TT_Buffplat_L", 0f);
            AttackDelayOffsetPercent.Add("TT_Buffplat_R", 0f);
            AttackDelayOffsetPercent.Add("KogMawDead", -0.06f);
            AttackDelayOffsetPercent.Add("TempMovableChar", 0f);
            AttackDelayOffsetPercent.Add("TT_ChaosTurret4", 65535f);
            AttackDelayOffsetPercent.Add("TT_Flytrap_A", 0f);
            AttackDelayOffsetPercent.Add("TT_Chains_Order_Periph", 0f);
            AttackDelayOffsetPercent.Add("ShopMale", 0f);
            AttackDelayOffsetPercent.Add("TT_Chains_Xaos_Base", 0f);
            AttackDelayOffsetPercent.Add("LuluSquill", -0.05f);
            AttackDelayOffsetPercent.Add("TT_Shopkeeper", 0f);
            AttackDelayOffsetPercent.Add("Odin_skeleton", 0f);
            AttackDelayOffsetPercent.Add("Cassiopeia_Death", -0.034f);
            AttackDelayOffsetPercent.Add("OdinRedSuperminion", -.1f);
            AttackDelayOffsetPercent.Add("TT_Speedshrine_Gears", 0f);
            AttackDelayOffsetPercent.Add("JarvanIVWall", 0f);
            AttackDelayOffsetPercent.Add("Red_Minion_MechCannon", -0.375f);
            AttackDelayOffsetPercent.Add("OdinBlueSuperminion", -.1f);
            AttackDelayOffsetPercent.Add("LuluKitty", -0.05f);
            AttackDelayOffsetPercent.Add("LuluLadybug", -0.05f);
            AttackDelayOffsetPercent.Add("TT_Shroom_A", 0f);
            AttackDelayOffsetPercent.Add("Odin_Windmill_Propellers", 0f);
            AttackDelayOffsetPercent.Add("odin_lifts_crystal", 0f);
            AttackDelayOffsetPercent.Add("SpellBook1", -0.375f);
            AttackDelayOffsetPercent.Add("Blue_Minion_MechCannon", -0.375f);
            AttackDelayOffsetPercent.Add("Odin_SoG_Chaos", 0f);
            AttackDelayOffsetPercent.Add("OrderTurretShrine", 65535f);
            AttackDelayOffsetPercent.Add("OriannaBall", -0.05f);
            AttackDelayOffsetPercent.Add("ChaosTurretShrine", 65535f);
            AttackDelayOffsetPercent.Add("LuluCupcake", -0.05f);
            AttackDelayOffsetPercent.Add("HA_AP_ChaosTurretShrine", 65535f);
            AttackDelayOffsetPercent.Add("TT_Chains_Bot_Lane", 0f);
            AttackDelayOffsetPercent.Add("TT_Tree_A", 0f);
            AttackDelayOffsetPercent.Add("Odin_Drill", 0f);
            AttackDelayOffsetPercent.Add("Odin_Minecart", 0f);
            AttackDelayOffsetPercent.Add("TT_Brazier", 0f);
            AttackDelayOffsetPercent.Add("odin_lifts_buckets", 0f);
            AttackDelayOffsetPercent.Add("OdinRockSaw", 0f);
            AttackDelayOffsetPercent.Add("SyndraSphere", 0f);
            AttackDelayOffsetPercent.Add("TT_Nexus_Gears", 0f);
            AttackDelayOffsetPercent.Add("Red_Minion_MechMelee", -.1f);
            AttackDelayOffsetPercent.Add("crystal_platform", 0f);
            AttackDelayOffsetPercent.Add("MaokaiSproutling", 0f);
            AttackDelayOffsetPercent.Add("Urf", -0.05f);
            AttackDelayOffsetPercent.Add("MalzaharVoidling", -0.06f);
            AttackDelayOffsetPercent.Add("MonkeyKingFlying", -0.1f);
            AttackDelayOffsetPercent.Add("LuluPig", -0.05f);
            AttackDelayOffsetPercent.Add("yonkey", 0f);
            AttackDelayOffsetPercent.Add("Odin_SoG_Order", 0f);
            AttackDelayOffsetPercent.Add("LuluDragon", -0.05f);
            AttackDelayOffsetPercent.Add("OdinCrane", 0f);
            AttackDelayOffsetPercent.Add("TT_Tree1", 0f);
            AttackDelayOffsetPercent.Add("TT_Chains_Order_Base", 0f);
            AttackDelayOffsetPercent.Add("Odin_Windmill_Gears", 0f);
            AttackDelayOffsetPercent.Add("TT_OrderTurret4", 65535f);
            AttackDelayOffsetPercent.Add("Odin_SOG_Chaos_Crystal", 0f);
            AttackDelayOffsetPercent.Add("TT_SpiderLayer_Web", 0f);
            AttackDelayOffsetPercent.Add("JarvanIVStandard", 0.02f);
            AttackDelayOffsetPercent.Add("OdinClaw", 0f);
            AttackDelayOffsetPercent.Add("EliseSpiderling", -0.06f);
            AttackDelayOffsetPercent.Add("ShacoBox", -0.54f);
            AttackDelayOffsetPercent.Add("AnnieTibbers", 0f);
            AttackDelayOffsetPercent.Add("HA_AP_OrderShrineTurret", 65535f);
            AttackDelayOffsetPercent.Add("HA_AP_OrderTurretRubble", 0f);
            AttackDelayOffsetPercent.Add("HA_AP_Chains_Long", 0f);
            AttackDelayOffsetPercent.Add("HA_AP_OrderCloth", 0f);
            AttackDelayOffsetPercent.Add("HA_AP_PeriphBridge", 0f);
            AttackDelayOffsetPercent.Add("HA_AP_BridgeLaneStatue", 0f);
            AttackDelayOffsetPercent.Add("HA_AP_ChaosTurretRubble", -0.25f);
            AttackDelayOffsetPercent.Add("HA_AP_BannerMidBridge", 0f);
            AttackDelayOffsetPercent.Add("HA_AP_PoroSpawner", -0.05f);
            AttackDelayOffsetPercent.Add("HA_AP_Cutaway", 0f);
            AttackDelayOffsetPercent.Add("HA_AP_Chains", 0f);
            AttackDelayOffsetPercent.Add("HA_AP_ShpSouth", 0f);
            AttackDelayOffsetPercent.Add("HA_AP_HeroTower", 0f);
            AttackDelayOffsetPercent.Add("HA_AP_ShpNorth", 0f);
            AttackDelayOffsetPercent.Add("ZacRebirthBloblet", 0f);
            AttackDelayOffsetPercent.Add("Nidalee_Spear", 0f);
            AttackDelayOffsetPercent.Add("TT_Buffplat_Chain", 0f);
            AttackDelayOffsetPercent.Add("WriggleLantern", 0f);
            AttackDelayOffsetPercent.Add("TwistedLizardElder", 0f);
            AttackDelayOffsetPercent.Add("RabidWolf", -0.1f);
            AttackDelayOffsetPercent.Add("HeimerTGreen", -0.5f);
            AttackDelayOffsetPercent.Add("HeimerTRed", -0.5f);
            AttackDelayOffsetPercent.Add("ViktorFF", -0.05f);
            AttackDelayOffsetPercent.Add("TwistedGolem", 0.02f);
            AttackDelayOffsetPercent.Add("TwistedSmallWolf", -0.05f);
            AttackDelayOffsetPercent.Add("TwistedGiantWolf", -0.08f);
            AttackDelayOffsetPercent.Add("TwistedTinyWraith", -0.02f);
            AttackDelayOffsetPercent.Add("TwistedBlueWraith", -0.02f);
            AttackDelayOffsetPercent.Add("TwistedYoungLizard", -0.08f);
            AttackDelayOffsetPercent.Add("Red_Minion_Melee", -0.5f);
            AttackDelayOffsetPercent.Add("Blue_Minion_Melee", -0.5f);
            AttackDelayOffsetPercent.Add("Blue_Minion_Healer", -0.067f);
            AttackDelayOffsetPercent.Add("Ghast", -0.02f);
            AttackDelayOffsetPercent.Add("blueDragon", 0f);
            AttackDelayOffsetPercent.Add("Red_Minion_MechRange", 65535f);
            AttackDelayOffsetPercent.Add("Test_CubeSphere", 0f);


            AttackDelayCastOffsetPercent.Add("XerathArcaneBarrageLauncher", 0.0595f);
            AttackDelayCastOffsetPercent.Add("Caitlyn", -0.122916667f);
            AttackDelayCastOffsetPercent.Add("TestCube", 0.0595f);
            AttackDelayCastOffsetPercent.Add("ARAMOrderTurretNexus", -.161f);
            AttackDelayCastOffsetPercent.Add("ShopKeeper", 0f);
            AttackDelayCastOffsetPercent.Add("Mordekaiser", -0.057142857f);
            AttackDelayCastOffsetPercent.Add("OdinQuestBuff", 0.0595f);
            AttackDelayCastOffsetPercent.Add("Lizard", 0.207f);
            AttackDelayCastOffsetPercent.Add("GolemOdin", 0.11f);
            AttackDelayCastOffsetPercent.Add("OdinOpeningBarrier", 0f);
            AttackDelayCastOffsetPercent.Add("TT_ChaosTurret2", -0.161f);
            AttackDelayCastOffsetPercent.Add("TT_ChaosTurret3", -0.161f);
            AttackDelayCastOffsetPercent.Add("TT_ChaosTurret1", -.161f);
            AttackDelayCastOffsetPercent.Add("OdinShieldRelic", 0.121f);
            AttackDelayCastOffsetPercent.Add("Tutorial_Red_Minion_Wizard", 0.0087f);
            AttackDelayCastOffsetPercent.Add("Renekton", -0.122695035f);
            AttackDelayCastOffsetPercent.Add("Anivia", -0.00833f);
            AttackDelayCastOffsetPercent.Add("ChaosTurretGiant", -0.161f);
            AttackDelayCastOffsetPercent.Add("Dragon", 0.3118f);
            AttackDelayCastOffsetPercent.Add("SmallGolem", 0.11f);
            AttackDelayCastOffsetPercent.Add("ARAMOrderTurretFront", -.161f);
            AttackDelayCastOffsetPercent.Add("TestCubeRender", 0.0595f);
            AttackDelayCastOffsetPercent.Add("Worm", -0.16267f);
            AttackDelayCastOffsetPercent.Add("ChaosTurretTutorial", 0f);
            AttackDelayCastOffsetPercent.Add("redDragon", 0.3118f);
            AttackDelayCastOffsetPercent.Add("ChaosTurretWorm", -.161f);
            AttackDelayCastOffsetPercent.Add("Darius", -0.050905797f);
            AttackDelayCastOffsetPercent.Add("ChaosInhibitor_D", 0f);
            AttackDelayCastOffsetPercent.Add("OdinChaosTurretShrine", 0.1f);
            AttackDelayCastOffsetPercent.Add("OdinCenterRelic", 0.01219f);
            AttackDelayCastOffsetPercent.Add("Vladimir", -0.07211f);
            AttackDelayCastOffsetPercent.Add("DestroyedNexus", 0f);
            AttackDelayCastOffsetPercent.Add("Wraith", -0.12f);
            AttackDelayCastOffsetPercent.Add("ARAMOrderNexus", 0f);
            AttackDelayCastOffsetPercent.Add("OrderInhibitor_D", 0f);
            AttackDelayCastOffsetPercent.Add("SyndraOrbs", -0.1125f);
            AttackDelayCastOffsetPercent.Add("Nidalee", -0.092292024f);
            AttackDelayCastOffsetPercent.Add("Ziggs", -0.091666667f);
            AttackDelayCastOffsetPercent.Add("TT_ChaosInhibitor", 0f);
            AttackDelayCastOffsetPercent.Add("ChaosTurretNormal", -0.161f);
            AttackDelayCastOffsetPercent.Add("CaitlynTrap", 0f);
            AttackDelayCastOffsetPercent.Add("ChaosInhibitor", 0f);
            AttackDelayCastOffsetPercent.Add("ARAMChaosTurretShrine", 0.1f);
            AttackDelayCastOffsetPercent.Add("AncientGolem", 0.161f);
            AttackDelayCastOffsetPercent.Add("DestroyedInhibitor", 0f);
            AttackDelayCastOffsetPercent.Add("Tutorial_Red_Minion_Basic", 0.13167f);
            AttackDelayCastOffsetPercent.Add("OdinMinionGraveyardPortal", 0.01219f);
            AttackDelayCastOffsetPercent.Add("TestCubeRenderwCollision", 0.0595f);
            AttackDelayCastOffsetPercent.Add("Summoner_Rider_Order", 0.121f);
            AttackDelayCastOffsetPercent.Add("TT_Relic", 0.121f);
            AttackDelayCastOffsetPercent.Add("OrderTurretNormal2", -.161f);
            AttackDelayCastOffsetPercent.Add("Wolf", 0.072f);
            AttackDelayCastOffsetPercent.Add("Veigar", -0.10906f);
            AttackDelayCastOffsetPercent.Add("OdinOrderTurretShrine", 0.1f);
            AttackDelayCastOffsetPercent.Add("KogMaw", -0.133776596f);
            AttackDelayCastOffsetPercent.Add("Alistar", -0.110855263f);
            AttackDelayCastOffsetPercent.Add("TT_ChaosInhibitor_D", 0f);
            AttackDelayCastOffsetPercent.Add("TrundleWall", 0.0958f);
            AttackDelayCastOffsetPercent.Add("HA_AP_HealthRelic", 0.121f);
            AttackDelayCastOffsetPercent.Add("HA_FB_HealthRelic", 0.121f);
            AttackDelayCastOffsetPercent.Add("TT_OrderInhibitor", 0f);
            AttackDelayCastOffsetPercent.Add("Galio", -0.087414966f);
            AttackDelayCastOffsetPercent.Add("Nasus", -0.098603652f);
            AttackDelayCastOffsetPercent.Add("Golem", 0.11f);
            AttackDelayCastOffsetPercent.Add("OrderTurretDragon", -.161f);
            AttackDelayCastOffsetPercent.Add("OrderTurretNormal", -.161f);
            AttackDelayCastOffsetPercent.Add("Kayle", -0.087414966f);
            AttackDelayCastOffsetPercent.Add("Brand", -0.1125f);
            AttackDelayCastOffsetPercent.Add("Amumu", -0.066156463f);
            AttackDelayCastOffsetPercent.Add("Annie", -0.104205247f);
            AttackDelayCastOffsetPercent.Add("LizardElder", 0.116666667f);
            AttackDelayCastOffsetPercent.Add("ARAMChaosTurretFront", -.161f);
            AttackDelayCastOffsetPercent.Add("SummonerBeacon", 0f);
            AttackDelayCastOffsetPercent.Add("MissFortune", -0.151993366f);
            AttackDelayCastOffsetPercent.Add("ARAMOrderTurretInhib", -.161f);
            AttackDelayCastOffsetPercent.Add("Shop", 0f);
            AttackDelayCastOffsetPercent.Add("Cassiopeia", -0.108f);
            AttackDelayCastOffsetPercent.Add("OrderInhibitor", 0f);
            AttackDelayCastOffsetPercent.Add("Tutorial_Blue_Minion_Wizard", 0.0087f);
            AttackDelayCastOffsetPercent.Add("DrMundo", -0.13969f);
            AttackDelayCastOffsetPercent.Add("AramSpeedShrine", 0.0595f);
            AttackDelayCastOffsetPercent.Add("DestroyedTower", 0f);
            AttackDelayCastOffsetPercent.Add("GiantWolf", 0.08417f);
            AttackDelayCastOffsetPercent.Add("Summoner_Rider_Chaos", 0.121f);
            AttackDelayCastOffsetPercent.Add("OdinSpeedShrine", 0.0595f);
            AttackDelayCastOffsetPercent.Add("YoungLizard", 0.10865f);
            AttackDelayCastOffsetPercent.Add("SightWard", 0f);
            AttackDelayCastOffsetPercent.Add("TT_SpeedShrine", 0.0595f);
            AttackDelayCastOffsetPercent.Add("Irelia", -0.03404f);
            AttackDelayCastOffsetPercent.Add("Lucian", -0.15f);
            AttackDelayCastOffsetPercent.Add("OdinMinionSpawnPortal", 0.01219f);
            AttackDelayCastOffsetPercent.Add("RammusPB", 0f);
            AttackDelayCastOffsetPercent.Add("JarvanIV", -0.12456f);
            AttackDelayCastOffsetPercent.Add("Jax", -0.05255f);
            AttackDelayCastOffsetPercent.Add("LesserWraith", 0.06065f);
            AttackDelayCastOffsetPercent.Add("Udyr", -0.10158f);
            AttackDelayCastOffsetPercent.Add("Tutorial_Blue_Minion_Basic", 0.1167f);
            AttackDelayCastOffsetPercent.Add("TestCubeRender10Vision", 0.0595f);
            AttackDelayCastOffsetPercent.Add("GhostWard", 0f);
            AttackDelayCastOffsetPercent.Add("Ryze", -0.1f);
            AttackDelayCastOffsetPercent.Add("AniviaIceBlock", 0.0958f);
            AttackDelayCastOffsetPercent.Add("TT_OrderInhibitor_D", 0f);
            AttackDelayCastOffsetPercent.Add("Blitzcrank", -0.03f);
            AttackDelayCastOffsetPercent.Add("OdinTestCubeRender", 0f);
            AttackDelayCastOffsetPercent.Add("VisionWard", 0f);
            AttackDelayCastOffsetPercent.Add("ARAMOrderTurretShrine", 0.1f);
            AttackDelayCastOffsetPercent.Add("ChaosTurretWorm2", -.161f);
            AttackDelayCastOffsetPercent.Add("ARAMChaosNexus", 0f);
            AttackDelayCastOffsetPercent.Add("TT_OrderTurret1", -.161f);
            AttackDelayCastOffsetPercent.Add("TT_OrderTurret2", -.161f);
            AttackDelayCastOffsetPercent.Add("ARAMChaosTurretInhib", -0.161f);
            AttackDelayCastOffsetPercent.Add("Ezreal", -0.111613475f);
            AttackDelayCastOffsetPercent.Add("TT_OrderTurret3", -.161f);
            AttackDelayCastOffsetPercent.Add("OdinNeutralGuardian", -0.0917f);
            AttackDelayCastOffsetPercent.Add("Sion", -0.091666667f);
            AttackDelayCastOffsetPercent.Add("Pantheon", -0.09687f);
            AttackDelayCastOffsetPercent.Add("OdinQuestIndicator", 0.01219f);
            AttackDelayCastOffsetPercent.Add("Zyra", -0.154166667f);
            AttackDelayCastOffsetPercent.Add("Karthus", 0.04375f);
            AttackDelayCastOffsetPercent.Add("Sona", -0.128178694f);
            AttackDelayCastOffsetPercent.Add("Rammus", -0.070833333f);
            AttackDelayCastOffsetPercent.Add("TT_DummyPusher", 0.0595f);
            AttackDelayCastOffsetPercent.Add("ARAMChaosTurretNexus", -0.161f);
            AttackDelayCastOffsetPercent.Add("Zilean", -0.02826f);
            AttackDelayCastOffsetPercent.Add("OrderTurretAngel", -.161f);
            AttackDelayCastOffsetPercent.Add("OrderTurretTutorial", 0f);
            AttackDelayCastOffsetPercent.Add("RecItemsODIN", 0f);
            AttackDelayCastOffsetPercent.Add("RecItemsARAM", 0f);
            AttackDelayCastOffsetPercent.Add("RecItemsCLASSIC", 0f);
            AttackDelayCastOffsetPercent.Add("Braum", -0.07f);
            AttackDelayCastOffsetPercent.Add("RecItemsCLASSICMap10", 0f);
            AttackDelayCastOffsetPercent.Add("Syndra", -0.1125f);
            AttackDelayCastOffsetPercent.Add("Jinx", -0.122916667f);
            AttackDelayCastOffsetPercent.Add("Trundle", -0.091666667f);
            AttackDelayCastOffsetPercent.Add("MasterYi", -0.05625f);
            AttackDelayCastOffsetPercent.Add("Lissandra", -0.1125f);
            AttackDelayCastOffsetPercent.Add("Malphite", -0.050318878f);
            AttackDelayCastOffsetPercent.Add("HA_AP_Poro", 0.072f);
            AttackDelayCastOffsetPercent.Add("TT_NWolf", 0.08417f);
            AttackDelayCastOffsetPercent.Add("Vi", -0.07f);
            AttackDelayCastOffsetPercent.Add("Fizz", -0.09687f);
            AttackDelayCastOffsetPercent.Add("Heimerdinger", -0.09922f);
            AttackDelayCastOffsetPercent.Add("Draven", -0.11884058f);
            AttackDelayCastOffsetPercent.Add("FiddleSticks", -0.070833333f);
            AttackDelayCastOffsetPercent.Add("Evelynn", -0.070833333f);
            AttackDelayCastOffsetPercent.Add("Rumble", -0.070833333f);
            AttackDelayCastOffsetPercent.Add("Xerath", -0.04926f);
            AttackDelayCastOffsetPercent.Add("Kassadin", -0.15f);
            AttackDelayCastOffsetPercent.Add("Leblanc", -0.133333333f);
            AttackDelayCastOffsetPercent.Add("Rengar", -0.11f);
            AttackDelayCastOffsetPercent.Add("Viktor", -0.091666667f);
            AttackDelayCastOffsetPercent.Add("XinZhao", -0.09687f);
            AttackDelayCastOffsetPercent.Add("Orianna", -0.124561404f);
            AttackDelayCastOffsetPercent.Add("Ezreal_cyber_1", 0f);
            AttackDelayCastOffsetPercent.Add("Ezreal_cyber_3", 0f);
            AttackDelayCastOffsetPercent.Add("Ezreal_cyber_2", 0f);
            AttackDelayCastOffsetPercent.Add("Thresh", -0.060416667f);
            AttackDelayCastOffsetPercent.Add("Maokai", 0.024074074f);
            AttackDelayCastOffsetPercent.Add("TT_NWolf2", 0.072f);
            AttackDelayCastOffsetPercent.Add("Tryndamere", -0.11f);
            AttackDelayCastOffsetPercent.Add("Zac", -0.050318878f);
            AttackDelayCastOffsetPercent.Add("Olaf", -0.06562f);
            AttackDelayCastOffsetPercent.Add("Twitch", -0.098084019f);
            AttackDelayCastOffsetPercent.Add("Singed", -0.063851264f);
            AttackDelayCastOffsetPercent.Add("Akali", -0.080701754f);
            AttackDelayCastOffsetPercent.Add("Diana", -0.091666667f);
            AttackDelayCastOffsetPercent.Add("Urgot", -0.171134021f);
            AttackDelayCastOffsetPercent.Add("Leona", -0.07083f);
            AttackDelayCastOffsetPercent.Add("Sivir", -0.18f);
            AttackDelayCastOffsetPercent.Add("Talon", -0.099465241f);
            AttackDelayCastOffsetPercent.Add("Corki", -0.2f);
            AttackDelayCastOffsetPercent.Add("Janna", -0.08f);
            AttackDelayCastOffsetPercent.Add("Karma", -0.138541667f);
            AttackDelayCastOffsetPercent.Add("Jayce", -0.1f);
            AttackDelayCastOffsetPercent.Add("Shaco", -0.078496169f);
            AttackDelayCastOffsetPercent.Add("Taric", -0.091666667f);
            AttackDelayCastOffsetPercent.Add("TwistedFate", -0.055964382f);
            AttackDelayCastOffsetPercent.Add("Varus", -0.124561404f);
            AttackDelayCastOffsetPercent.Add("Yasuo", -0.08f);
            AttackDelayCastOffsetPercent.Add("Garen", -0.091666667f);
            AttackDelayCastOffsetPercent.Add("Swain", -0.173333333f);
            AttackDelayCastOffsetPercent.Add("Vayne", -0.124561404f);
            AttackDelayCastOffsetPercent.Add("Fiora", -0.080701754f);
            AttackDelayCastOffsetPercent.Add("Quinn", -0.124561404f);
            AttackDelayCastOffsetPercent.Add("Teemo", -0.08426f);
            AttackDelayCastOffsetPercent.Add("Elise", -0.1125f);
            AttackDelayCastOffsetPercent.Add("Nami", -0.12f);
            AttackDelayCastOffsetPercent.Add("Poppy", -0.08316f);
            AttackDelayCastOffsetPercent.Add("Ahri", -0.09946f);
            AttackDelayCastOffsetPercent.Add("Tristana", -0.151993366f);
            AttackDelayCastOffsetPercent.Add("TT_NWraith2", 0.06065f);
            AttackDelayCastOffsetPercent.Add("Graves", -0.154166667f);
            AttackDelayCastOffsetPercent.Add("Morgana", -0.16f);
            AttackDelayCastOffsetPercent.Add("Gragas", -0.05f);
            AttackDelayCastOffsetPercent.Add("Skarner", -0.091f);
            AttackDelayCastOffsetPercent.Add("Katarina", -0.080701754f);
            AttackDelayCastOffsetPercent.Add("Riven", -0.133333333f);
            AttackDelayCastOffsetPercent.Add("Velkoz", -0.1f);
            AttackDelayCastOffsetPercent.Add("TT_NGolem", 0.11f);
            AttackDelayCastOffsetPercent.Add("LeeSin", -0.10469f);
            AttackDelayCastOffsetPercent.Add("Warwick", -0.0047f);
            AttackDelayCastOffsetPercent.Add("Volibear", -0.07f);
            AttackDelayCastOffsetPercent.Add("OriannaNoBall", -0.124561404f);
            AttackDelayCastOffsetPercent.Add("Yorick", -0.091666667f);
            AttackDelayCastOffsetPercent.Add("MonkeyKing", -0.091666667f);
            AttackDelayCastOffsetPercent.Add("QuinnValor", -0.124561404f);
            AttackDelayCastOffsetPercent.Add("Kennen", -0.1f);
            AttackDelayCastOffsetPercent.Add("Lulu", -0.1125f);
            AttackDelayCastOffsetPercent.Add("Nunu", -0.10641f);
            AttackDelayCastOffsetPercent.Add("Ashe", -0.080701754f);
            AttackDelayCastOffsetPercent.Add("Zed", -0.098245614f);
            AttackDelayCastOffsetPercent.Add("Nautilus", 0.00637f);
            AttackDelayCastOffsetPercent.Add("TT_NGolem2", 0.11f);
            AttackDelayCastOffsetPercent.Add("TT_NWraith", -0.12f);
            AttackDelayCastOffsetPercent.Add("Gangplank", -0.110980903f);
            AttackDelayCastOffsetPercent.Add("Lux", -0.14375f);
            AttackDelayCastOffsetPercent.Add("Sejuani", -0.05f);
            AttackDelayCastOffsetPercent.Add("Khazix", -0.099465241f);
            AttackDelayCastOffsetPercent.Add("Shen", -0.126388889f);
            AttackDelayCastOffsetPercent.Add("Aatrox", -0.095f);
            AttackDelayCastOffsetPercent.Add("Hecarim", -0.05f);
            AttackDelayCastOffsetPercent.Add("Nocturne", -0.099465241f);
            AttackDelayCastOffsetPercent.Add("Shyvana", -0.102631579f);
            AttackDelayCastOffsetPercent.Add("Soraka", -0.1125f);
            AttackDelayCastOffsetPercent.Add("Chogath", -0.081f);
            AttackDelayCastOffsetPercent.Add("Malzahar", -0.02926f);
            AttackDelayCastOffsetPercent.Add("ShyvanaDragon", -0.05877f);
            AttackDelayCastOffsetPercent.Add("UdyrPhoenix", -0.10158f);
            AttackDelayCastOffsetPercent.Add("UdyrTigerUlt", -0.10158f);
            AttackDelayCastOffsetPercent.Add("MonkeyKingClone", -0.060536398f);
            AttackDelayCastOffsetPercent.Add("NasusUlt", -0.098603652f);
            AttackDelayCastOffsetPercent.Add("SwainNoBird", -0.173333333f);
            AttackDelayCastOffsetPercent.Add("SwainBeam", -0.0917f);
            AttackDelayCastOffsetPercent.Add("EliseSpider", -0.091f);
            AttackDelayCastOffsetPercent.Add("AniviaEgg", -0.0917f);
            AttackDelayCastOffsetPercent.Add("UdyrTurtleUlt", -0.10158f);
            AttackDelayCastOffsetPercent.Add("UdyrTurtle", -0.10158f);
            AttackDelayCastOffsetPercent.Add("nidalee_cougar", -0.132493568f);
            AttackDelayCastOffsetPercent.Add("UdyrUlt", -0.10158f);
            AttackDelayCastOffsetPercent.Add("UdyrPhoenixUlt", -0.10158f);
            AttackDelayCastOffsetPercent.Add("SwainRaven", -0.0822222222f);
            AttackDelayCastOffsetPercent.Add("RammusDBC", -0.070833333f);
            AttackDelayCastOffsetPercent.Add("FizzShark", -0.09687f);
            AttackDelayCastOffsetPercent.Add("UdyrTiger", -0.10158f);
            AttackDelayCastOffsetPercent.Add("TT_Spiderboss", 0.314035088f);
            AttackDelayCastOffsetPercent.Add("ZyraGraspingPlant", -0.12456f);
            AttackDelayCastOffsetPercent.Add("HeimerTBlue", -0.01198f);
            AttackDelayCastOffsetPercent.Add("HeimerTYellow", -0.01198f);
            AttackDelayCastOffsetPercent.Add("TeemoMushroom", 0f);
            AttackDelayCastOffsetPercent.Add("RecItemsTUTORIAL", 0f);
            AttackDelayCastOffsetPercent.Add("ZyraPassive", -0.080701754f);
            AttackDelayCastOffsetPercent.Add("ZyraThornPlant", -0.080701754f);
            AttackDelayCastOffsetPercent.Add("ChaosNexus", 0f);
            AttackDelayCastOffsetPercent.Add("ThreshLantern", -0.0917f);
            AttackDelayCastOffsetPercent.Add("OrderNexus", 0f);
            AttackDelayCastOffsetPercent.Add("Red_Minion_Wizard", 0.0087f);
            AttackDelayCastOffsetPercent.Add("Blue_Minion_Wizard", 0.0087f);
            AttackDelayCastOffsetPercent.Add("LuluSnowman", 0.072f);
            AttackDelayCastOffsetPercent.Add("HA_AP_OrderTurret3", -.161f);
            AttackDelayCastOffsetPercent.Add("HA_AP_OrderTurret2", -.161f);
            AttackDelayCastOffsetPercent.Add("GreatWraith", -0.12f);
            AttackDelayCastOffsetPercent.Add("HA_AP_ChaosTurret", -.161f);
            AttackDelayCastOffsetPercent.Add("HA_AP_ChaosTurret3", -.161f);
            AttackDelayCastOffsetPercent.Add("HA_AP_ChaosTurret2", -.161f);
            AttackDelayCastOffsetPercent.Add("LuluFaerie", -.099465f);
            AttackDelayCastOffsetPercent.Add("HA_AP_OrderTurret", -.161f);
            AttackDelayCastOffsetPercent.Add("YellowTrinketUpgrade", 0f);
            AttackDelayCastOffsetPercent.Add("HA_AP_ChaosTurretTutorial", 0f);
            AttackDelayCastOffsetPercent.Add("HA_AP_OrderTurretTutorial", 0f);
            AttackDelayCastOffsetPercent.Add("brush_D_SR", 0f);
            AttackDelayCastOffsetPercent.Add("brush_E_SR", 0f);
            AttackDelayCastOffsetPercent.Add("brush_F_SR", 0f);
            AttackDelayCastOffsetPercent.Add("brush_C_SR", 0f);
            AttackDelayCastOffsetPercent.Add("brush_A_SR", 0f);
            AttackDelayCastOffsetPercent.Add("brush_B_SR", 0f);
            AttackDelayCastOffsetPercent.Add("YellowTrinket", 0f);
            AttackDelayCastOffsetPercent.Add("ZyraSeed", -0.0917f);
            AttackDelayCastOffsetPercent.Add("OlafAxe", 0.0595f);
            AttackDelayCastOffsetPercent.Add("ZedShadow", -0.080701754f);
            AttackDelayCastOffsetPercent.Add("Blue_Minion_Basic", 0.1167f);
            AttackDelayCastOffsetPercent.Add("Odin_Blue_Minion_caster", 0.0087f);
            AttackDelayCastOffsetPercent.Add("Odin_Red_Minion_Caster", 0.0087f);
            AttackDelayCastOffsetPercent.Add("Red_Minion_Basic", 0.13167f);
            AttackDelayCastOffsetPercent.Add("shopevo", 0f);
            AttackDelayCastOffsetPercent.Add("YorickRavenousGhoul", -0.091666667f);
            AttackDelayCastOffsetPercent.Add("YorickSpectralGhoul", -0.091666667f);
            AttackDelayCastOffsetPercent.Add("JinxMine", 0f);
            AttackDelayCastOffsetPercent.Add("YorickDecayedGhoul", -0.091666667f);
            AttackDelayCastOffsetPercent.Add("Odin_SOG_Order_Crystal", 0f);
            AttackDelayCastOffsetPercent.Add("FizzBait", -0.09687f);
            AttackDelayCastOffsetPercent.Add("Blue_Minion_MechMelee", 0.0854f);
            AttackDelayCastOffsetPercent.Add("TT_Buffplat_L", -0.0917f);
            AttackDelayCastOffsetPercent.Add("TT_Buffplat_R", -0.0917f);
            AttackDelayCastOffsetPercent.Add("KogMawDead", -0.02f);
            AttackDelayCastOffsetPercent.Add("TempMovableChar", 0.01219f);
            AttackDelayCastOffsetPercent.Add("TT_ChaosTurret4", 0.1f);
            AttackDelayCastOffsetPercent.Add("TT_Flytrap_A", 0f);
            AttackDelayCastOffsetPercent.Add("TT_Chains_Order_Periph", 0f);
            AttackDelayCastOffsetPercent.Add("ShopMale", 0f);
            AttackDelayCastOffsetPercent.Add("TT_Chains_Xaos_Base", 0f);
            AttackDelayCastOffsetPercent.Add("LuluSquill", 0.072f);
            AttackDelayCastOffsetPercent.Add("TT_Shopkeeper", 0f);
            AttackDelayCastOffsetPercent.Add("Odin_skeleton", 0f);
            AttackDelayCastOffsetPercent.Add("Cassiopeia_Death", -.108f);
            AttackDelayCastOffsetPercent.Add("OdinRedSuperminion", 0.0889f);
            AttackDelayCastOffsetPercent.Add("TT_Speedshrine_Gears", 0f);
            AttackDelayCastOffsetPercent.Add("JarvanIVWall", 0.0958f);
            AttackDelayCastOffsetPercent.Add("Red_Minion_MechCannon", 0.25f);
            AttackDelayCastOffsetPercent.Add("OdinBlueSuperminion", 0.0854f);
            AttackDelayCastOffsetPercent.Add("LuluKitty", 0.072f);
            AttackDelayCastOffsetPercent.Add("LuluLadybug", 0.072f);
            AttackDelayCastOffsetPercent.Add("TT_Shroom_A", 0f);
            AttackDelayCastOffsetPercent.Add("Odin_Windmill_Propellers", 0f);
            AttackDelayCastOffsetPercent.Add("odin_lifts_crystal", 0f);
            AttackDelayCastOffsetPercent.Add("SpellBook1", 0.0595f);
            AttackDelayCastOffsetPercent.Add("Blue_Minion_MechCannon", 0.045f);
            AttackDelayCastOffsetPercent.Add("Odin_SoG_Chaos", 0f);
            AttackDelayCastOffsetPercent.Add("OrderTurretShrine", 0.1f);
            AttackDelayCastOffsetPercent.Add("OriannaBall", -0.124561404f);
            AttackDelayCastOffsetPercent.Add("ChaosTurretShrine", 0.1f);
            AttackDelayCastOffsetPercent.Add("LuluCupcake", 0.072f);
            AttackDelayCastOffsetPercent.Add("HA_AP_ChaosTurretShrine", 0.1f);
            AttackDelayCastOffsetPercent.Add("TT_Chains_Bot_Lane", 0f);
            AttackDelayCastOffsetPercent.Add("TT_Tree_A", 0f);
            AttackDelayCastOffsetPercent.Add("Odin_Drill", 0f);
            AttackDelayCastOffsetPercent.Add("Odin_Minecart", 0f);
            AttackDelayCastOffsetPercent.Add("TT_Brazier", 0f);
            AttackDelayCastOffsetPercent.Add("odin_lifts_buckets", 0f);
            AttackDelayCastOffsetPercent.Add("OdinRockSaw", 0f);
            AttackDelayCastOffsetPercent.Add("SyndraSphere", 0f);
            AttackDelayCastOffsetPercent.Add("TT_Nexus_Gears", 0f);
            AttackDelayCastOffsetPercent.Add("Red_Minion_MechMelee", 0.0889f);
            AttackDelayCastOffsetPercent.Add("crystal_platform", 0f);
            AttackDelayCastOffsetPercent.Add("MaokaiSproutling", 0.01219f);
            AttackDelayCastOffsetPercent.Add("Urf", 0.3118f);
            AttackDelayCastOffsetPercent.Add("MalzaharVoidling", 0f);
            AttackDelayCastOffsetPercent.Add("MonkeyKingFlying", -0.060536398f);
            AttackDelayCastOffsetPercent.Add("LuluPig", 0.072f);
            AttackDelayCastOffsetPercent.Add("yonkey", 0f);
            AttackDelayCastOffsetPercent.Add("Odin_SoG_Order", 0f);
            AttackDelayCastOffsetPercent.Add("LuluDragon", 0.072f);
            AttackDelayCastOffsetPercent.Add("OdinCrane", 0f);
            AttackDelayCastOffsetPercent.Add("TT_Tree1", 0f);
            AttackDelayCastOffsetPercent.Add("TT_Chains_Order_Base", 0f);
            AttackDelayCastOffsetPercent.Add("Odin_Windmill_Gears", 0f);
            AttackDelayCastOffsetPercent.Add("TT_OrderTurret4", 0.1f);
            AttackDelayCastOffsetPercent.Add("Odin_SOG_Chaos_Crystal", 0f);
            AttackDelayCastOffsetPercent.Add("TT_SpiderLayer_Web", 0f);
            AttackDelayCastOffsetPercent.Add("JarvanIVStandard", 0f);
            AttackDelayCastOffsetPercent.Add("OdinClaw", 0f);
            AttackDelayCastOffsetPercent.Add("EliseSpiderling", 0f);
            AttackDelayCastOffsetPercent.Add("ShacoBox", -0.01198f);
            AttackDelayCastOffsetPercent.Add("AnnieTibbers", 0.0958f);
            AttackDelayCastOffsetPercent.Add("HA_AP_OrderShrineTurret", 0.1f);
            AttackDelayCastOffsetPercent.Add("HA_AP_OrderTurretRubble", 0f);
            AttackDelayCastOffsetPercent.Add("HA_AP_Chains_Long", 0f);
            AttackDelayCastOffsetPercent.Add("HA_AP_OrderCloth", 0f);
            AttackDelayCastOffsetPercent.Add("HA_AP_PeriphBridge", 0f);
            AttackDelayCastOffsetPercent.Add("HA_AP_BridgeLaneStatue", 0f);
            AttackDelayCastOffsetPercent.Add("HA_AP_ChaosTurretRubble", -.161f);
            AttackDelayCastOffsetPercent.Add("HA_AP_BannerMidBridge", 0f);
            AttackDelayCastOffsetPercent.Add("HA_AP_PoroSpawner", 0.072f);
            AttackDelayCastOffsetPercent.Add("HA_AP_Cutaway", 0f);
            AttackDelayCastOffsetPercent.Add("HA_AP_Chains", 0f);
            AttackDelayCastOffsetPercent.Add("HA_AP_ShpSouth", 0f);
            AttackDelayCastOffsetPercent.Add("HA_AP_HeroTower", 0f);
            AttackDelayCastOffsetPercent.Add("HA_AP_ShpNorth", 0f);
            AttackDelayCastOffsetPercent.Add("ZacRebirthBloblet", -0.0917f);
            AttackDelayCastOffsetPercent.Add("Nidalee_Spear", 0f);
            AttackDelayCastOffsetPercent.Add("TT_Buffplat_Chain", -0.0917f);
            AttackDelayCastOffsetPercent.Add("WriggleLantern", 0f);
            AttackDelayCastOffsetPercent.Add("TwistedLizardElder", -0.09167f);
            AttackDelayCastOffsetPercent.Add("RabidWolf", 0.08417f);
            AttackDelayCastOffsetPercent.Add("HeimerTGreen", -0.01198f);
            AttackDelayCastOffsetPercent.Add("HeimerTRed", -0.01198f);
            AttackDelayCastOffsetPercent.Add("ViktorFF", 0.3118f);
            AttackDelayCastOffsetPercent.Add("TwistedGolem", 0.11f);
            AttackDelayCastOffsetPercent.Add("TwistedSmallWolf", 0.072f);
            AttackDelayCastOffsetPercent.Add("TwistedGiantWolf", 0.08417f);
            AttackDelayCastOffsetPercent.Add("TwistedTinyWraith", 0.06065f);
            AttackDelayCastOffsetPercent.Add("TwistedBlueWraith", -0.12f);
            AttackDelayCastOffsetPercent.Add("TwistedYoungLizard", 0.10865f);
            AttackDelayCastOffsetPercent.Add("Red_Minion_Melee", 0.13167f);
            AttackDelayCastOffsetPercent.Add("Blue_Minion_Melee", 0.1167f);
            AttackDelayCastOffsetPercent.Add("Blue_Minion_Healer", 0.0087f);
            AttackDelayCastOffsetPercent.Add("Ghast", -0.12f);
            AttackDelayCastOffsetPercent.Add("blueDragon", 0.3118f);
            AttackDelayCastOffsetPercent.Add("Red_Minion_MechRange", -0.01198f);
            AttackDelayCastOffsetPercent.Add("Test_CubeSphere", 0f);


            Attacks.Add("frostarrow"); //Ashes Q

            NoAttacks.Add("shyvanadoubleattackdragon");
            NoAttacks.Add("shyvanadoubleattack");
            NoAttacks.Add("monkeykingdoubleattack");

            AttackResets.Add("vaynetumble");
            AttackResets.Add("dariusnoxiantacticsonh ");
            AttackResets.Add("fioraflurry ");
            AttackResets.Add("parley");
            AttackResets.Add("jaxempowertwo");
            AttackResets.Add("leonashieldofdaybreak");
            AttackResets.Add("mordekaisermaceofspades");
            AttackResets.Add("nasusq");
            AttackResets.Add("nautiluspiercinggaze");
            AttackResets.Add("javelintoss");
            AttackResets.Add("poppydevastatingblow");
            AttackResets.Add("renektonpreexecute");
            AttackResets.Add("rengarq");
            AttackResets.Add("shyvanadoubleattack");
            AttackResets.Add("sivirw");
            AttackResets.Add("talonnoxiandiplomacy");
            AttackResets.Add("trundletrollsmash ");
            AttackResets.Add("vie");
            AttackResets.Add("volibearq");
            AttackResets.Add("monkeykingdoubleattack");
            AttackResets.Add("garenq");
            AttackResets.Add("khazixq");
            AttackResets.Add("cassiopeiatwinfang");
            AttackResets.Add("xenzhaocombotarget");
        }

        private class AttackPassive
        {
            public readonly string BuffName;
            public float APScaling;
            public float BaseDamageMultiplicator = 0f;
            public string Champion;

            public int DamageType; //0 True, 1 Magic, 2 Physical

            public float LevelBaseDamage = 0;
            public float[] LevelDamageArray;
            public float LevelDamagePerLevel = 0;

            public float SpellBaseDamage;
            public float SpellDamagePerLevel;

            public float TotalDamageMultiplicator;
            public SpellSlot slot;

            public AttackPassive(string Champion, string BuffName)
            {
                this.Champion = Champion;
                this.BuffName = BuffName;
            }

            public float CalcExtraDamage(Obj_AI_Minion minion)
            {
                var Damage = 0f;
                // TODO asd
                if (LevelBaseDamage != 0)
                    Damage += LevelBaseDamage;

                if (LevelDamagePerLevel != 0)
                    Damage += ObjectManager.Player.Level * LevelDamagePerLevel;

                if (LevelDamageArray != null)
                    Damage += LevelDamageArray[ObjectManager.Player.Level - 1];

                if (SpellBaseDamage != 0)
                {
                    Damage += SpellBaseDamage;
                }

                if (SpellDamagePerLevel != 0)
                {
                    Damage += ObjectManager.Player.Spellbook.GetSpell(slot).Level * SpellDamagePerLevel;
                }
                Damage += BaseDamageMultiplicator * (ObjectManager.Player.BaseAttackDamage);
                Damage += TotalDamageMultiplicator *
                          (ObjectManager.Player.BaseAttackDamage + ObjectManager.Player.FlatPhysicalDamageMod);

                Damage += APScaling * ObjectManager.Player.FlatMagicDamageMod * ObjectManager.Player.PercentMagicDamageMod;

                if (DamageType == 0)
                {
                    return Damage;
                }

                if (DamageType == 1)
                {
                    return (float)DamageLib.CalcMagicMinionDmg(Damage, minion, false);
                }
                if (DamageType == 2)
                {
                    return (float)DamageLib.CalcPhysicalMinionDmg(Damage, minion, false);
                }
                return 0f;
            }
        }
    }
}