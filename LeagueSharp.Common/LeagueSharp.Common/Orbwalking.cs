#region LICENSE

// Copyright 2014 - 2014 LeagueSharp
// Orbwalking.cs is part of LeagueSharp.Common.
// 
// LeagueSharp.Common is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// LeagueSharp.Common is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with LeagueSharp.Common. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    ///     This class offers everything related to auto-attacks and orbwalking.
    /// </summary>
    public static class Orbwalking
    {
        public delegate void AfterAttackEvenH(Obj_AI_Base unit, Obj_AI_Base target);

        public delegate void BeforeAttackEvenH(BeforeAttackEventArgs args);

        public delegate void OnAttackEvenH(Obj_AI_Base unit, Obj_AI_Base target);

        public enum OrbwalkingMode
        {
            LastHit,
            Mixed,
            LaneClear,
            Combo,
            None,
        }

        //Spells that reset the attack timer.
        private static readonly string[] AttackResets =
        {
            "blindingdart", "cassiopeiatwinfang", "dariusnoxiantacticsonh", "detonatingshot", "fioraflurry", "garenq",
            "hecarimrapidslash", "jaxempowertwo", "jaycehypercharge", "kogmawqmis", "leonashieldofdaybreak", "luciane",
            "lucianq", "missfortunericochetshot", "monkeykingdoubleattack", "mordekaisermaceofspades", "nasusq",
            "nautiluspiercinggaze", "netherblade", "parley", "poppydevastatingblow", "powerfist", "renektonpreexecute",
            "rengarq", "shyvanadoubleattack", "sivirw", "sonahymnofvalor", "takedown", "talonnoxiandiplomacy",
            "trundletrollsmash", "vaynetumble", "vie", "volibearq", "xenzhaocombotarget", "yorickspectral"
        };

        //Spells that are not attacks even if they have the "attack" word in their name.
        private static readonly string[] NoAttacks =
        {
            "jarvanivcataclysmattack", "monkeykingdoubleattack", "shyvanadoubleattack", "shyvanadoubleattackdragon",
            "zyragraspingplantattack", "zyragraspingplantattack2", "zyragraspingplantattackfire",
            "zyragraspingplantattack2fire"
        };

        //Spells that are attacks even if they dont have the "attack" word in their name.
        private static readonly string[] Attacks =
        {
            "caitlynheadshotmissile", "frostarrow", "garenslash2", "kennenmegaproc", "lucianpassiveattack",
            "masteryidoublestrike", "quinnwenhanced", "renektonexecute", "renektonsuperexecute",
            "rengarnewpassivebuffdash", "trundleq", "xenzhaothrust", "xenzhaothrust2",
            "xenzhaothrust3"
        };

        private static readonly List<PassiveDamage> AttackPassives = new List<PassiveDamage>();

        public static int LastAATick;

        public static bool Attack = true;
        public static bool DisableNextAttack = false;
        public static bool Move = true;
        private static Obj_AI_Base _lastTarget;
        private static readonly Obj_AI_Hero Player;

        static Orbwalking()
        {
            Player = ObjectManager.Player;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
            GameObject.OnCreate += Obj_SpellMissile_OnCreate;
            Game.OnGameProcessPacket += OnProcessPacket;

            //Add the passive damages
            PassiveDamage p;

            #region Caitlyn

            p = new PassiveDamage
            {
                ChampionName = "Caitlyn",
                IsActive = minion => (Player.HasBuff("CaitlynHeadshotReady")),
                GetDamage =
                    minion =>
                        ((float)
                            DamageLib.CalcPhysicalDmg(1.5d * (Player.BaseAttackDamage + Player.FlatPhysicalDamageMod),
                                minion)),
            };
            AttackPassives.Add(p);

            #endregion

            #region Draven

            p = new PassiveDamage
            {
                ChampionName = "Draven",
                IsActive = minion => (Player.HasBuff("dravenspinning")),
                GetDamage =
                    minion =>
                        ((float)
                            DamageLib.CalcPhysicalDmg(0.45d * (Player.BaseAttackDamage + Player.FlatPhysicalDamageMod),
                                minion)),
            };
            AttackPassives.Add(p);

            #endregion

            #region Corki

            p = new PassiveDamage
            {
                ChampionName = "Corki",
                IsActive = minion => (Player.HasBuff("RapidReload")),
                GetDamage = minion => ((float)0.1d * (Player.BaseAttackDamage + Player.FlatPhysicalDamageMod)),
            };
            AttackPassives.Add(p);

            #endregion
            #region Gnar
            p = new PassiveDamage
            {
                ChampionName = "Gnar",
                IsActive =
                    minion =>
                        (from buff in minion.Buffs where buff.DisplayName == "GnarWProc" select buff.Count)
                            .FirstOrDefault() == 2,
                GetDamage = minion => ((float)DamageLib.getDmg(minion, DamageLib.SpellType.W)),
            };
            AttackPassives.Add(p);
#endregion
            #region Jinx

            p = new PassiveDamage
            {
                ChampionName = "Jinx",
                IsActive = minion => (Player.HasBuff("JinxQ")),
                GetDamage =
                    minion =>
                        ((float)
                            DamageLib.CalcPhysicalDmg(0.1d * (Player.BaseAttackDamage + Player.FlatPhysicalDamageMod),
                                minion)),
            };
            AttackPassives.Add(p);

            #endregion

            #region Katarina

            p = new PassiveDamage
            {
                ChampionName = "Katarina",
                IsActive = minion => (minion.HasBuff("KataQMark1")),
                GetDamage =
                    minion => ((float)DamageLib.getDmg(minion, DamageLib.SpellType.Q, DamageLib.StageType.FirstDamage)),
            };
            AttackPassives.Add(p);

            #endregion

            #region KogMaw

            p = new PassiveDamage
            {
                ChampionName = "KogMaw",
                IsActive = minion => (Player.HasBuff("KogMawBioArcaneBarrage")),
                GetDamage =
                    minion =>
                        ((float)
                            DamageLib.getDmg(minion, DamageLib.SpellType.W)),
            };
            AttackPassives.Add(p);

            #endregion

            #region MissFortune

            p = new PassiveDamage
            {
                ChampionName = "MissFortune",
                IsActive = minion => (Player.HasBuff("MissFortunePassive")),
                GetDamage =
                    minion =>
                        (float)
                            DamageLib.CalcMagicDmg(
                                (float)0.06d * (Player.BaseAttackDamage + Player.FlatPhysicalDamageMod), minion),
            };
            AttackPassives.Add(p);

            #endregion

            #region Nasus

            p = new PassiveDamage
            {
                ChampionName = "Nasus",
                IsActive = minion => (Player.HasBuff("SiphoningStrike")),
                GetDamage = minion => ((float)DamageLib.getDmg(minion, DamageLib.SpellType.Q)),
            };
            AttackPassives.Add(p);

            #endregion

            #region Orianna

            p = new PassiveDamage
            {
                ChampionName = "Orianna",
                IsActive = minion => (Player.HasBuff("OrianaSpellSword")),
                GetDamage =
                    minion =>
                        (float)
                            DamageLib.CalcMagicDmg(
                                (float)0.15d * Player.FlatMagicDamageMod +
                                new float[] { 10, 10, 10, 18, 18, 18, 26, 26, 26, 34, 34, 34, 42, 42, 42, 50, 50, 50 }[
                                    Player.Level - 1], minion),
            };
            AttackPassives.Add(p);

            #endregion

            #region Teemo

            p = new PassiveDamage
            {
                ChampionName = "Teemo",
                IsActive = minion => (Player.HasBuff("Toxic Attack")),
                GetDamage =
                    minion =>
                        ((float)
                            DamageLib.CalcMagicDmg(
                                Player.Spellbook.GetSpell(SpellSlot.E).Level * 10 + Player.FlatMagicDamageMod * 0.3d,
                                minion)),
            };
            AttackPassives.Add(p);

            #endregion

            #region TwistedFate

            p = new PassiveDamage
            {
                ChampionName = "TwistedFate",
                IsActive = minion => (Player.HasBuff("Pick A Card Blue")),
                GetDamage =
                    minion =>
                        (float)DamageLib.getDmg(minion, DamageLib.SpellType.W, DamageLib.StageType.FirstDamage) -
                        (float)
                            DamageLib.CalcPhysicalDmg(
                                (ObjectManager.Player.BaseAttackDamage + ObjectManager.Player.FlatPhysicalDamageMod),
                                minion),
            };
            AttackPassives.Add(p);

            p = new PassiveDamage
            {
                ChampionName = "TwistedFate",
                IsActive = minion => (Player.HasBuff("CardMasterStackParticle")),
                GetDamage = minion => (float)DamageLib.getDmg(minion, DamageLib.SpellType.E),
            };
            AttackPassives.Add(p);

            #endregion

            #region Varus

            p = new PassiveDamage
            {
                ChampionName = "Varus",
                IsActive = minion => (Player.HasBuff("VarusW")),
                GetDamage = minion => ((float)DamageLib.getDmg(minion, DamageLib.SpellType.W)),
            };
            AttackPassives.Add(p);

            #endregion

            #region Vayne

            p = new PassiveDamage
            {
                ChampionName = "Vayne",
                IsActive = minion => (Player.HasBuff("VayneTumble")),
                GetDamage = minion => ((float)DamageLib.getDmg(minion, DamageLib.SpellType.Q)),
            };
            AttackPassives.Add(p);

            p = new PassiveDamage
            {
                ChampionName = "Vayne",
                IsActive =
                    minion =>
                        (from buff in minion.Buffs where buff.DisplayName == "VayneSilverDebuff" select buff.Count)
                            .FirstOrDefault() == 2,
                GetDamage = minion => ((float)DamageLib.getDmg(minion, DamageLib.SpellType.W)),
            };
            AttackPassives.Add(p);

            #endregion

            #region Ziggs

            p = new PassiveDamage
            {
                ChampionName = "Ziggs",
                IsActive = minion => (Player.HasBuff("ziggsShortFuse")),
                GetDamage =
                    minion =>
                        (float)
                            DamageLib.CalcMagicDmg(
                                (float)0.25d * Player.FlatMagicDamageMod +
                                new float[]
                                { 20, 24, 28, 32, 36, 40, 48, 56, 64, 72, 80, 88, 100, 112, 124, 136, 148, 160 }[
                                    Player.Level - 1], minion),
            };
            AttackPassives.Add(p);

            #endregion
        }

        private static void Obj_SpellMissile_OnCreate(GameObject sender, EventArgs args)
        {
            if (sender is Obj_SpellMissile && sender.IsValid)
            {
                var missile = (Obj_SpellMissile)sender;
                if (missile.SpellCaster is Obj_AI_Hero && missile.SpellCaster.IsValid &&
                    IsAutoAttack(missile.SData.Name))
                {
                    FireAfterAttack(missile.SpellCaster, _lastTarget);
                }
            }
        }

        /// <summary>
        ///     This event is fired before the player auto attacks.
        /// </summary>
        public static event BeforeAttackEvenH BeforeAttack;

        /// <summary>
        ///     This event is fired when a unit is about to auto-attack another unit.
        /// </summary>
        public static event OnAttackEvenH OnAttack;

        /// <summary>
        ///     This event is fired after a unit finishes auto-attacking another unit (Only works with player for now).
        /// </summary>
        public static event AfterAttackEvenH AfterAttack;

        private static void FireBeforeAttack(Obj_AI_Base target)
        {
            if (BeforeAttack != null)
                BeforeAttack(new BeforeAttackEventArgs { Target = target });
            else
                DisableNextAttack = false;
        }

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
        private static float GetAutoAttackPassiveDamage(Obj_AI_Minion minion)
        {
            return
                AttackPassives.Where(
                    p => (p.ChampionName == "" || p.ChampionName == Player.ChampionName) && p.IsActive(minion))
                    .Sum(passive => passive.GetDamage(minion));
        }

        /// <summary>
        ///     Returns true if the spellname resets the attack timer.
        /// </summary>
        public static bool IsAutoAttackReset(string name)
        {
            return AttackResets.Contains(name.ToLower());
        }

        /// <summary>
        /// Returns true if the unit is melee
        /// </summary>
        public static bool IsMelee(this Obj_AI_Base unit)
        {
            return unit.CombatType == GameObjectCombatType.Melee;
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
            var result = Player.AttackRange + Player.BoundingRadius;
            if (target.IsValidTarget())
            {
                return result + target.BoundingRadius - ((target.Path.Length > 0) ? 35 : 20);
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
                        Player.ServerPosition.To2D()) <= myRange * myRange;
            }
            return false;
        }

        /// <summary>
        ///     Returns player auto-attack missile speed.
        /// </summary>
        public static float GetMyProjectileSpeed()
        {
            return IsMelee(Player) ? float.MaxValue : Player.BasicAttack.MissileSpeed;
        }

        /// <summary>
        ///     Returns if the player's auto-attack is ready.
        /// </summary>
        public static bool CanAttack()
        {
            if (LastAATick <= Environment.TickCount)
            {
                return Environment.TickCount + Game.Ping / 2 + 25 >=
                       LastAATick + Player.AttackDelay * 1000 &&
                       Attack;
            }

            return false;
        }

        /// <summary>
        ///     Returns true if moving won't cancel the auto-attack.
        /// </summary>
        public static bool CanMove(float extraWindup)
        {
            if (LastAATick <= Environment.TickCount)
            {
                return Environment.TickCount + Game.Ping / 2 >=
                       LastAATick + Player.AttackCastDelay * 1000 + extraWindup && Move;
            }

            return false;
        }

        private static void MoveTo(Vector3 position, float holdAreaRadius = 0)
        {
            if (Player.ServerPosition.Distance(position) < holdAreaRadius)
            {
                if (Player.Path.Count() > 0)
                    Player.IssueOrder(GameObjectOrder.HoldPosition, Player.ServerPosition);
                return;
            }

            var point = Player.ServerPosition +
                        400 * (position.To2D() - Player.ServerPosition.To2D()).Normalized().To3D();
            Player.IssueOrder(GameObjectOrder.MoveTo, point);
        }

        /// <summary>
        ///     Orbwalk a target while moving to Position.
        /// </summary>
        public static void Orbwalk(Obj_AI_Base target, Vector3 position, float extraWindup = 90,
            float holdAreaRadius = 0)
        {
            if (target != null && CanAttack())
            {
                DisableNextAttack = false;
                FireBeforeAttack(target);

                if (!DisableNextAttack)
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                    if (!(target is Obj_AI_Hero))
                        LastAATick = Environment.TickCount + Game.Ping / 2;
                    return;
                }
            }

            if (CanMove(extraWindup))
            {
                MoveTo(position, holdAreaRadius);
            }
        }

        /// <summary>
        /// Resets the Auto-Attack timer.
        /// </summary>
        public static void ResetAutoAttackTimer()
        {
            LastAATick = 0;
        }

        private static void OnProcessPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] == 0x34)
            {
                var stream = new MemoryStream(args.PacketData);
                var b = new BinaryReader(stream);
                b.BaseStream.Position = b.BaseStream.Position + 1;
                var unit = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(BitConverter.ToInt32(b.ReadBytes(4), 0));

                if (args.PacketData[9] == 17)
                {
                    if (unit.IsMe)
                        ResetAutoAttackTimer();
                }
            }
        }

        private static void OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs Spell)
        {
            if (IsAutoAttackReset(Spell.SData.Name) && unit.IsMe)
                Utility.DelayAction.Add(250, ResetAutoAttackTimer);

            if (IsAutoAttack(Spell.SData.Name))
            {
                if (unit.IsMe)
                {
                    LastAATick = Environment.TickCount - Game.Ping / 2;
                    if (Spell.Target is Obj_AI_Base)
                        _lastTarget = (Obj_AI_Base)Spell.Target;

                    if (unit.IsMelee())
                    {
                        Utility.DelayAction.Add((int)(unit.AttackCastDelay * 1000 + 40),
                            () => FireAfterAttack(unit, _lastTarget));
                    }
                }

                FireOnAttack(unit, _lastTarget);
            }
        }

        public class BeforeAttackEventArgs
        {
            public Obj_AI_Base Target;
            public Obj_AI_Base Unit = ObjectManager.Player;
            private bool _process = true;

            public bool Process
            {
                get { return _process; }
                set
                {
                    DisableNextAttack = !value;
                    _process = value;
                }
            }
        }

        /// <summary>
        ///     This class allows you to add an instance of "Orbwalker" to your assembly in order to control the orbwalking in an easy way.
        /// </summary>
        public class Orbwalker
        {
            private const float LaneClearWaitTimeMod = 2f;
            private readonly Obj_AI_Hero Player;
            private readonly Menu _config;

            private Obj_AI_Base _forcedTarget;
            private Vector3 _orbwalkingPoint;

            private Obj_AI_Minion _prevMinion;

            public Orbwalker(Menu attachToMenu)
            {
                _config = attachToMenu;
                /* Drawings submenu */
                var drawings = new Menu("Drawings", "drawings");
                drawings.AddItem(
                    new MenuItem("AACircle", "AACircle").SetShared()
                        .SetValue(new Circle(true, Color.FromArgb(255, 255, 0, 255))));
                drawings.AddItem(
                    new MenuItem("HoldZone", "HoldZone").SetShared()
                        .SetValue(new Circle(false, Color.FromArgb(255, 255, 0, 255))));

                _config.AddSubMenu(drawings);

                /* Misc options */
                var misc = new Menu("Misc", "Misc");
                misc.AddItem(
                    new MenuItem("HoldPosRadius", "Hold Position Radius").SetShared()
                        .SetValue(new Slider(0, 150, 0)));
                _config.AddSubMenu(misc);

                /* Delay sliders */
                _config.AddItem(
                    new MenuItem("ExtraWindup", "Extra windup time").SetShared().SetValue(new Slider(50, 200, 0)));
                _config.AddItem(new MenuItem("FarmDelay", "Farm delay").SetShared().SetValue(new Slider(0, 200, 0)));

                /*Load the menu*/
                _config.AddItem(
                    new MenuItem("LastHit", "Last hit").SetShared()
                        .SetValue(new KeyBind("X".ToCharArray()[0], KeyBindType.Press, false)));

                _config.AddItem(
                    new MenuItem("Farm", "Mixed").SetShared()
                        .SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press, false)));

                _config.AddItem(
                    new MenuItem("LaneClear", "LaneClear").SetShared()
                        .SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press,
                            false)));

                _config.AddItem(
                    new MenuItem("Orbwalk", "Combo").SetShared().SetValue(new KeyBind(32, KeyBindType.Press, false)));

                if (Common.isInitialized == false)
                {
                    Common.InitializeCommonLib();
                }

                Player = ObjectManager.Player;
                Game.OnGameUpdate += GameOnOnGameUpdate;
                Drawing.OnDraw += DrawingOnOnDraw;
            }

            private int FarmDelay
            {
                get { return _config.Item("FarmDelay").GetValue<Slider>().Value; }
            }

            public OrbwalkingMode ActiveMode
            {
                get
                {
                    if (_config.Item("Orbwalk").GetValue<KeyBind>().Active)
                        return OrbwalkingMode.Combo;

                    if (_config.Item("LaneClear").GetValue<KeyBind>().Active)
                        return OrbwalkingMode.LaneClear;

                    if (_config.Item("Farm").GetValue<KeyBind>().Active)
                        return OrbwalkingMode.Mixed;

                    if (_config.Item("LastHit").GetValue<KeyBind>().Active)
                        return OrbwalkingMode.LastHit;


                    return OrbwalkingMode.None;
                }
            }

            /// <summary>
            ///     Enables or disables the auto-attacks.
            /// </summary>
            public void SetAttacks(bool b)
            {
                Attack = b;
            }

            /// <summary>
            ///     Enables or disables the movement.
            /// </summary>
            public void SetMovement(bool b)
            {
                Move = b;
            }

            /// <summary>
            ///     Forces the orbwalker to attack the set target if valid and in range.
            /// </summary>
            public void ForceTarget(Obj_AI_Base target)
            {
                _forcedTarget = target;
            }


            /// <summary>
            ///     Forces the orbwalker to move to that point while orbwalking (Game.CursorPos by default).
            /// </summary>
            public void SetOrbwalkingPoint(Vector3 point)
            {
                _orbwalkingPoint = point;
            }

            private bool ShouldWait()
            {
                return
                    ObjectManager.Get<Obj_AI_Minion>()
                        .Any(
                            minion =>
                                minion.IsValidTarget() && minion.Team != GameObjectTeam.Neutral &&
                                InAutoAttackRange(minion) &&
                                HealthPrediction.LaneClearHealthPrediction(minion,
                                    (int)((Player.AttackDelay * 1000) * LaneClearWaitTimeMod), FarmDelay) <=
                                DamageLib.CalcPhysicalMinionDmg(Player.BaseAttackDamage + Player.FlatPhysicalDamageMod,
                                    minion, true) - 1 + Math.Max(0, GetAutoAttackPassiveDamage(minion) - 10));
            }

            public Obj_AI_Base GetTarget()
            {
                Obj_AI_Base result = null;
                float[] r = { float.MaxValue };

                /*Killable Minion*/
                if (ActiveMode == OrbwalkingMode.LaneClear || ActiveMode == OrbwalkingMode.Mixed ||
                    ActiveMode == OrbwalkingMode.LastHit)
                    foreach (
                        var minion in
                            ObjectManager.Get<Obj_AI_Minion>()
                                .Where(minion => minion.IsValidTarget() && InAutoAttackRange(minion)))
                    {
                        var t = (int)(Player.AttackCastDelay * 1000) - 100 + Game.Ping / 2 +
                                1000 *
                                (int)Player.Distance(minion) / (int)GetMyProjectileSpeed();
                        var predHealth = HealthPrediction.GetHealthPrediction(minion, t, FarmDelay);

                        if (minion.Team != GameObjectTeam.Neutral && predHealth > 0 &&
                            predHealth <=
                            DamageLib.CalcPhysicalMinionDmg(
                                Player.BaseAttackDamage + Player.FlatPhysicalDamageMod,
                                minion,
                                true) - 1 + Math.Max(0, GetAutoAttackPassiveDamage(minion) - 10))
                        {
                            //Game.PrintChat("Current Health: " + minion.Health + " Predicted Health:" + (DamageLib.CalcPhysicalMinionDmg(Player.BaseAttackDamage + Player.FlatPhysicalDamageMod, minion, true) - 1 + Orbwalking.GetPassiveDamage(minion)));
                            return minion;
                        }
                    }

                //Forced target
                if (_forcedTarget != null && _forcedTarget.IsValidTarget() && InAutoAttackRange(_forcedTarget))
                {
                    return _forcedTarget;
                }

                /*Champions*/
                if (ActiveMode != OrbwalkingMode.LastHit)
                {
                    var target = SimpleTs.GetTarget(-1, SimpleTs.DamageType.Physical);
                    if (target != null)
                    {
                        if (ActiveMode != OrbwalkingMode.Combo)
                        {
                            if (!Utility.UnderTurret(target) || !Utility.UnderTurret(Player)) //ToDo: Check if ally has tower aggro and shoot on enemy
                            {
                                return target;
                            }
                        }
                        else
                        {
                            return target;
                        }
                    }
                }

                /*Jungle minions*/
                if (ActiveMode == OrbwalkingMode.LaneClear || ActiveMode == OrbwalkingMode.Mixed)
                    foreach (
                        var mob in
                            ObjectManager.Get<Obj_AI_Minion>()
                                .Where(mob => mob.IsValidTarget() && InAutoAttackRange(mob) &&
                                              mob.Team == GameObjectTeam.Neutral)
                                .Where(mob => mob.MaxHealth >= r[0] || Math.Abs(r[0] - float.MaxValue) < float.Epsilon))
                    {
                        result = mob;
                        r[0] = mob.MaxHealth;
                    }

                if (result != null)
                    return result;

                /*Lane Clear minions*/
                r[0] = float.MaxValue;
                if (ActiveMode == OrbwalkingMode.LaneClear)
                {
                    if (!ShouldWait())
                    {
                        if (_prevMinion != null && _prevMinion.IsValidTarget() && InAutoAttackRange(_prevMinion))
                        {
                            var predHealth = HealthPrediction.LaneClearHealthPrediction(_prevMinion,
                                (int)((Player.AttackDelay * 1000) * LaneClearWaitTimeMod),
                                FarmDelay);
                            if (predHealth >=
                                2 * DamageLib.CalcPhysicalMinionDmg(
                                    Player.BaseAttackDamage + Player.FlatPhysicalDamageMod,
                                    _prevMinion, true) - 1 +
                                Math.Max(0, GetAutoAttackPassiveDamage(_prevMinion) - 10) ||
                                Math.Abs(predHealth - _prevMinion.Health) < float.Epsilon)
                            {
                                return _prevMinion;
                            }
                        }

                        foreach (
                            var minion in
                                ObjectManager.Get<Obj_AI_Minion>()
                                    .Where(minion => minion.IsValidTarget() && InAutoAttackRange(minion)))
                        {
                            var predHealth = HealthPrediction.LaneClearHealthPrediction(minion,
                                (int)((Player.AttackDelay * 1000) * LaneClearWaitTimeMod),
                                FarmDelay);
                            if (predHealth >=
                                2 * DamageLib.CalcPhysicalMinionDmg(
                                    Player.BaseAttackDamage +
                                    Player.FlatPhysicalDamageMod,
                                    minion, true) - 1 +
                                Math.Max(0, GetAutoAttackPassiveDamage(minion) - 10) ||
                                Math.Abs(predHealth - minion.Health) < float.Epsilon)
                            {
                                if (minion.Health >= r[0] || Math.Abs(r[0] - float.MaxValue) < float.Epsilon)
                                {
                                    result = minion;
                                    r[0] = minion.Health;
                                    _prevMinion = minion;
                                }
                            }
                        }
                    }
                }

                /*turrets*/
                if (ActiveMode == OrbwalkingMode.LaneClear)
                    foreach (
                        var turret in
                            ObjectManager.Get<Obj_AI_Turret>().Where(t => t.IsValidTarget() && InAutoAttackRange(t)))

                        return turret;

                return result;
            }

            private void GameOnOnGameUpdate(EventArgs args)
            {
                if (ActiveMode == OrbwalkingMode.None)
                    return;

                //Prevent canceling important channeled spells like Miss Fortunes R.
                if (Player.IsChannelingImportantSpell()) return;

                var target = GetTarget();
                Orbwalk(target, (_orbwalkingPoint.To2D().IsValid()) ? _orbwalkingPoint : Game.CursorPos,
                    _config.Item("ExtraWindup").GetValue<Slider>().Value,
                    _config.Item("HoldPosRadius").GetValue<Slider>().Value);
            }

            private void DrawingOnOnDraw(EventArgs args)
            {

                if (_config.Item("AACircle").GetValue<Circle>().Active)
                    Utility.DrawCircle(Player.Position, GetRealAutoAttackRange(null) + 65,
                        _config.Item("AACircle").GetValue<Circle>().Color);

                if (_config.Item("HoldZone").GetValue<Circle>().Active)
                    Utility.DrawCircle(Player.Position, _config.Item("HoldPosRadius").GetValue<Slider>().Value,
                        _config.Item("HoldZone").GetValue<Circle>().Color);
            }
        }

        internal class PassiveDamage
        {
            public delegate float GetDamageD(Obj_AI_Base minion);

            public delegate bool IsActiveD(Obj_AI_Base minion);

            public string ChampionName = "";

            public GetDamageD GetDamage;
            public IsActiveD IsActive;
        }
    }
}
