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

        private static readonly List<AttackPassive> AttackPassives = new List<AttackPassive>();

        private static int LastAATick;

        public static bool Attack = true;
        public static bool Move = true;
        private static Obj_AI_Base _lastTarget;
        private static readonly Obj_AI_Hero Player;

        static Orbwalking()
        {
            Player = ObjectManager.Player;
            LoadTheData();
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
            GameObject.OnCreate += Obj_SpellMissile_OnCreate;
            Game.OnGameProcessPacket += OnProcessPacket;
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
        private static float GetAutoAttackPassiveDamage(Obj_AI_Minion minion)
        {
            var totaldamage = 0f;

            foreach (var passive in AttackPassives)
            {
                if (Player.HasBuff(passive.BuffName))
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
                return result + target.BoundingRadius - ((target.Path.Length > 0) ? 20 : 10);
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
        public static void Orbwalk(Obj_AI_Base target, Vector3 Position, float ExtraWindup = 90,
            float holdAreaRadius = 0)
        {
            if (target != null && CanAttack())
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                if (!(target is Obj_AI_Hero))
                    LastAATick = Environment.TickCount + Game.Ping / 2;
                return;
            }

            if (CanMove(ExtraWindup))
            {
                MoveTo(Position, holdAreaRadius);
            }
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
                    {
                        LastAATick = 0;
                    }
                }
            }
        }

        private static void OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs Spell)
        {
            if (IsAutoAttackReset(Spell.SData.Name) && unit.IsMe)
            {
                Utility.DelayAction.Add(250, delegate { LastAATick = 0; });
            }

            if (IsAutoAttack(Spell.SData.Name))
            {
                if (unit.IsMe)
                {
                    LastAATick = Environment.TickCount - Game.Ping / 2;
                    if(Spell.Target is Obj_AI_Base)
                        _lastTarget = (Obj_AI_Base) Spell.Target;

                    if (unit.IsMe && unit.IsMelee())
                    {
                        Utility.DelayAction.Add((int)(unit.AttackCastDelay * 1000 + 40),
                            delegate { FireAfterAttack(unit, _lastTarget); });
                    }
                }

                FireOnAttack(unit, _lastTarget);
            }
        }

        private static void LoadTheData()
        {
            /*Passive list*/

            #region Caitlyn

            var PassiveToAdd = new AttackPassive
            {
                Champion = "Caitlyn",
                BuffName = "CaitlynHeadshotReady",
                TotalDamageMultiplicator = 1.5f,
                DamageType = 2,
            };
            AttackPassives.Add(PassiveToAdd);

            #endregion

            #region Draven

            PassiveToAdd = new AttackPassive
            {
                Champion = "Draven",
                BuffName = "dravenspinning",
                TotalDamageMultiplicator = 0.45f,
                DamageType = 2,
            };
            AttackPassives.Add(PassiveToAdd);

            #endregion

            #region Vayne

            PassiveToAdd = new AttackPassive
            {
                Champion = "Vayne",
                BuffName = "VayneTumble",
                TotalDamageMultiplicator = 0.3f,
                DamageType = 2,
            };
            AttackPassives.Add(PassiveToAdd);

            #endregion

            #region Corki

            PassiveToAdd = new AttackPassive
            {
                Champion = "Corki",
                BuffName = "RapidReload",
                TotalDamageMultiplicator = 0.1f,
                DamageType = 0
            };
            AttackPassives.Add(PassiveToAdd);

            #endregion

            #region Teemo

            PassiveToAdd = new AttackPassive
            {
                Champion = "Teemo",
                BuffName = "Toxic Attack",
                APScaling = 0.3f,
                slot = SpellSlot.E,
                SpellBaseDamage = 0,
                SpellDamagePerLevel = 10,
                DamageType = 1
            };
            AttackPassives.Add(PassiveToAdd);

            #endregion

            #region Varus

            PassiveToAdd = new AttackPassive
            {
                Champion = "Varus",
                BuffName = "VarusW",
                APScaling = 0.25f,
                slot = SpellSlot.W,
                SpellBaseDamage = 6,
                SpellDamagePerLevel = 4,
                DamageType = 1
            };
            AttackPassives.Add(PassiveToAdd);

            #endregion

            #region MissFortune

            PassiveToAdd = new AttackPassive
            {
                Champion = "MissFortune",
                BuffName = "MissFortunePassive",
                TotalDamageMultiplicator = 0.06f,
                DamageType = 1
            };
            AttackPassives.Add(PassiveToAdd);

            #endregion

            #region Twisted Fate

            PassiveToAdd = new AttackPassive
            {
                Champion = "TwistedFate",
                BuffName = "Pick A Card Blue",
                APScaling = 0.5f,
                slot = SpellSlot.E,
                SpellBaseDamage = 20,
                SpellDamagePerLevel = 20,
                DamageType = 1
            };
            AttackPassives.Add(PassiveToAdd);

            PassiveToAdd = new AttackPassive
            {
                Champion = "TwistedFate",
                BuffName = "CardMasterStackParticle",
                APScaling = 0.5f,
                slot = SpellSlot.E,
                SpellBaseDamage = 30,
                SpellDamagePerLevel = 25,
                DamageType = 1,
            };
            AttackPassives.Add(PassiveToAdd);

            #endregion

            #region Orianna

            PassiveToAdd = new AttackPassive
            {
                Champion = "Orianna",
                BuffName = "OrianaSpellSword",
                APScaling = 0.15f,
                LevelDamageArray =
                    new float[] { 10, 10, 10, 18, 18, 18, 26, 26, 26, 34, 34, 34, 42, 42, 42, 50, 50, 50 },
                DamageType = 1,
            };
            AttackPassives.Add(PassiveToAdd);

            #endregion

            #region Ziggs

            PassiveToAdd = new AttackPassive
            {
                Champion = "Ziggs",
                BuffName = "ziggsShortFuse",
                APScaling = 0.25f,
                LevelDamageArray =
                    new float[] { 20, 24, 28, 32, 36, 40, 48, 56, 64, 72, 80, 88, 100, 112, 124, 136, 148, 160 },
                DamageType = 1,
            };
            AttackPassives.Add(PassiveToAdd);

            #endregion
        }

        internal class AttackPassive
        {
            public float APScaling;
            public float BaseDamageMultiplicator = 0f;
            public string BuffName;
            public string Champion;

            public int DamageType; //0 True, 1 Magic, 2 Physical

            public float LevelBaseDamage = 0;
            public float[] LevelDamageArray;
            public float LevelDamagePerLevel = 0;

            public float SpellBaseDamage;
            public float SpellDamagePerLevel;

            public float TotalDamageMultiplicator;
            public SpellSlot slot;

            public AttackPassive()
            {
            }

            public AttackPassive(string Champion, string BuffName)
            {
                this.Champion = Champion;
                this.BuffName = BuffName;
            }

            public float CalcExtraDamage(Obj_AI_Minion minion)
            {
                var Damage = 0f;

                if (LevelBaseDamage != 0)
                    Damage += LevelBaseDamage;

                if (LevelDamagePerLevel != 0)
                    Damage += Player.Level * LevelDamagePerLevel;

                if (LevelDamageArray != null)
                    Damage += LevelDamageArray[Player.Level - 1];

                if (SpellBaseDamage != 0)
                {
                    Damage += SpellBaseDamage;
                }

                if (SpellDamagePerLevel != 0)
                {
                    Damage += Player.Spellbook.GetSpell(slot).Level * SpellDamagePerLevel;
                }
                Damage += BaseDamageMultiplicator * (Player.BaseAttackDamage);
                Damage += TotalDamageMultiplicator *
                          (Player.BaseAttackDamage + Player.FlatPhysicalDamageMod);

                Damage += APScaling * Player.FlatMagicDamageMod *
                          Player.PercentMagicDamageMod;

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

        /// <summary>
        ///     This class allows you to add an instance of "Orbwalker" to your assembly in order to control the orbwalking in an easy way.
        /// </summary>
        public class Orbwalker
        {
            private const float LaneClearWaitTimeMod = 2f;
            private readonly Menu Config;

            private Obj_AI_Base ForcedTarget;
            private Vector3 OrbwalkingPoint;

            private Obj_AI_Hero Player;
            private Obj_AI_Minion prevMinion;

            public Orbwalker(Menu attachToMenu)
            {
                Config = attachToMenu;
                /* Drawings submenu */
                var drawings = new Menu("Drawings", "drawings");
                drawings.AddItem(
                    new MenuItem("AACircle", "AACircle").SetShared()
                        .SetValue(new Circle(true, Color.FromArgb(255, 255, 0, 255))));
                Config.AddSubMenu(drawings);

                /* Misc options */
                var misc = new Menu("Misc", "Misc");
                misc.AddItem(
                    new MenuItem("HoldPosRadius", "Hold Position Radius").SetShared()
                        .SetValue(new Slider(0, 150, 0)));
                Config.AddSubMenu(misc);

                /* Delay sliders */
                Config.AddItem(
                    new MenuItem("ExtraWindup", "Extra windup time").SetShared().SetValue(new Slider(50, 200, 0)));
                Config.AddItem(new MenuItem("FarmDelay", "Farm delay").SetShared().SetValue(new Slider(0, 200, 0)));

                /*Load the menu*/
                Config.AddItem(
                    new MenuItem("LastHit", "Last hit").SetShared()
                        .SetValue(new KeyBind("X".ToCharArray()[0], KeyBindType.Press, false)));

                Config.AddItem(
                    new MenuItem("Farm", "Mixed").SetShared()
                        .SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press, false)));

                Config.AddItem(
                    new MenuItem("LaneClear", "LaneClear").SetShared()
                        .SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press,
                            false)));

                Config.AddItem(
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
                get { return Config.Item("FarmDelay").GetValue<Slider>().Value; }
            }

            public OrbwalkingMode ActiveMode
            {
                get
                {
                    if (Config.Item("Orbwalk").GetValue<KeyBind>().Active)
                        return OrbwalkingMode.Combo;

                    if (Config.Item("LaneClear").GetValue<KeyBind>().Active)
                        return OrbwalkingMode.LaneClear;

                    if (Config.Item("Farm").GetValue<KeyBind>().Active)
                        return OrbwalkingMode.Mixed;

                    if (Config.Item("LastHit").GetValue<KeyBind>().Active)
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
                ForcedTarget = target;
            }


            /// <summary>
            ///     Forces the orbwalker to move to that point while orbwalking (Game.CursorPos by default).
            /// </summary>
            public void SetOrbwalkingPoint(Vector3 point)
            {
                OrbwalkingPoint = point;
            }

            private bool ShouldWait()
            {
                foreach (var minion in ObjectManager.Get<Obj_AI_Minion>())
                {
                    if (minion.IsValidTarget() && minion.Team != GameObjectTeam.Neutral &&
                        InAutoAttackRange(minion) &&
                        HealthPrediction.LaneClearHealthPrediction(minion,
                            (int)((Player.AttackDelay * 1000) * LaneClearWaitTimeMod),
                            FarmDelay) <=
                        DamageLib.CalcPhysicalMinionDmg(
                            Player.BaseAttackDamage + Player.FlatPhysicalDamageMod, minion,
                            true) -
                        1 + Math.Max(0, GetAutoAttackPassiveDamage(minion) - 10))
                    {
                        return true;
                    }
                }
                return false;
            }

            public Obj_AI_Base GetTarget()
            {
                Obj_AI_Base result = null;
                var r = float.MaxValue;


                /*Killable Minion*/
                if (ActiveMode == OrbwalkingMode.LaneClear || ActiveMode == OrbwalkingMode.Mixed ||
                    ActiveMode == OrbwalkingMode.LastHit)
                    foreach (var minion in ObjectManager.Get<Obj_AI_Minion>())
                    {
                        if (minion.IsValidTarget() && InAutoAttackRange(minion))
                        {
                            var predHealth = HealthPrediction.GetHealthPrediction(minion,
                                (int)(Player.AttackCastDelay * 1000) - 100 + Game.Ping / 2 +
                                1000 *
                                (int)Player.Distance(minion) / (int)GetMyProjectileSpeed(),
                                FarmDelay);

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
                    }

                //Forced target
                if (ForcedTarget != null && ForcedTarget.IsValidTarget() && InAutoAttackRange(ForcedTarget))
                {
                    return ForcedTarget;
                }

                /*Champions*/
                if (ActiveMode != OrbwalkingMode.LastHit)
                {
                    var target = SimpleTs.GetTarget(-1, SimpleTs.DamageType.Physical);
                    if (target != null)
                        return target;
                }

                /*Jungle minions*/
                if (ActiveMode == OrbwalkingMode.LaneClear || ActiveMode == OrbwalkingMode.Mixed)
                    foreach (var mob in ObjectManager.Get<Obj_AI_Minion>())
                    {
                        if (mob.IsValidTarget() && Orbwalking.InAutoAttackRange(mob) &&
                            mob.Team == GameObjectTeam.Neutral)
                        {
                            if (mob.MaxHealth >= r || r == float.MaxValue)
                            {
                                result = mob;
                                r = mob.MaxHealth;
                            }
                        }
                    }

                if (result != null)
                    return result;

                /*Lane Clear minions*/
                r = float.MaxValue;
                if (ActiveMode == OrbwalkingMode.LaneClear)
                {
                    if (!ShouldWait())
                    {
                        if (prevMinion != null && prevMinion.IsValidTarget() && InAutoAttackRange(prevMinion))
                        {
                            var predHealth = HealthPrediction.LaneClearHealthPrediction(prevMinion,
                                (int)((Player.AttackDelay * 1000) * LaneClearWaitTimeMod),
                                FarmDelay);
                            if (predHealth >=
                                2 * DamageLib.CalcPhysicalMinionDmg(
                                    Player.BaseAttackDamage + Player.FlatPhysicalDamageMod,
                                    prevMinion, true) - 1 +
                                Math.Max(0, Orbwalking.GetAutoAttackPassiveDamage(prevMinion) - 10) ||
                                predHealth == prevMinion.Health)
                            {
                                return prevMinion;
                            }
                        }

                        foreach (var minion in ObjectManager.Get<Obj_AI_Minion>())
                        {
                            if (minion.IsValidTarget() && InAutoAttackRange(minion))
                            {
                                var predHealth = HealthPrediction.LaneClearHealthPrediction(minion,
                                    (int)((Player.AttackDelay * 1000) * LaneClearWaitTimeMod),
                                    FarmDelay);
                                if (predHealth >=
                                    2 * DamageLib.CalcPhysicalMinionDmg(
                                        Player.BaseAttackDamage +
                                        Player.FlatPhysicalDamageMod,
                                        minion, true) - 1 +
                                    Math.Max(0, Orbwalking.GetAutoAttackPassiveDamage(minion) - 10) ||
                                    predHealth == minion.Health)
                                {
                                    if (minion.Health >= r || r == float.MaxValue)
                                    {
                                        result = minion;
                                        r = minion.Health;
                                        prevMinion = minion;
                                    }
                                }
                            }
                        }
                    }
                }

                if (result != null)
                    return result;
                return result;
            }

            private void GameOnOnGameUpdate(EventArgs args)
            {
                if (ActiveMode == OrbwalkingMode.None)
                    return;

                //Prevent canceling important channeled spells like Miss Fortunes R.
                if (Player.IsChannelingImportantSpell()) return;

                var target = GetTarget();
                Orbwalk(target, (OrbwalkingPoint.To2D().IsValid()) ? OrbwalkingPoint : Game.CursorPos,
                    Config.Item("ExtraWindup").GetValue<Slider>().Value,
                    Config.Item("HoldPosRadius").GetValue<Slider>().Value);
            }

            private void DrawingOnOnDraw(EventArgs args)
            {
                if (Config.Item("AACircle").GetValue<Circle>().Active)
                    Utility.DrawCircle(Player.Position, GetRealAutoAttackRange(null) + 65,
                        Config.Item("AACircle").GetValue<Circle>().Color);
            }
        }
    }
}