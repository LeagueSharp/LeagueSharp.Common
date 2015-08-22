#region LICENSE

/*
 Copyright 2014 - 2015 LeagueSharp
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

#endregion

#region

using System;
using System.Collections.Generic;
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
        public delegate void AfterAttackEvenH(AttackableUnit unit, AttackableUnit target);

        public delegate void BeforeAttackEvenH(BeforeAttackEventArgs args);

        public delegate void OnAttackEvenH(AttackableUnit unit, AttackableUnit target);

        public delegate void OnNonKillableMinionH(AttackableUnit minion);

        public delegate void OnTargetChangeH(AttackableUnit oldTarget, AttackableUnit newTarget);

        public enum OrbwalkingMode
        {
            LastHit,
            Mixed,
            LaneClear,
            Combo,
            None
        }

        //Spells that reset the attack timer.
        private static readonly string[] AttackResets =
        {
            "dariusnoxiantacticsonh", "fioraflurry", "garenq",
            "hecarimrapidslash", "jaxempowertwo", "jaycehypercharge", "leonashieldofdaybreak", "luciane", "lucianq",
            "monkeykingdoubleattack", "mordekaisermaceofspades", "nasusq", "nautiluspiercinggaze", "netherblade",
            "parley", "poppydevastatingblow", "powerfist", "renektonpreexecute", "rengarq", "shyvanadoubleattack",
            "sivirw", "takedown", "talonnoxiandiplomacy", "trundletrollsmash", "vaynetumble", "vie", "volibearq",
            "xenzhaocombotarget", "yorickspectral", "reksaiq"
        };

        //Spells that are not attacks even if they have the "attack" word in their name.
        private static readonly string[] NoAttacks =
        {
            "jarvanivcataclysmattack", "monkeykingdoubleattack",
            "shyvanadoubleattack", "shyvanadoubleattackdragon", "zyragraspingplantattack", "zyragraspingplantattack2",
            "zyragraspingplantattackfire", "zyragraspingplantattack2fire", "viktorpowertransfer", "sivirwattackbounce"
        };

        //Spells that are attacks even if they dont have the "attack" word in their name.
        private static readonly string[] Attacks =
        {
            "caitlynheadshotmissile", "frostarrow", "garenslash2",
            "kennenmegaproc", "lucianpassiveattack", "masteryidoublestrike", "quinnwenhanced", "renektonexecute",
            "renektonsuperexecute", "rengarnewpassivebuffdash", "trundleq", "xenzhaothrust", "xenzhaothrust2",
            "xenzhaothrust3", "viktorqbuff"
        };

        // Champs whose auto attacks can't be cancelled
        private static readonly string[] NoCancelChamps = { "Kalista" };
        public static int LastAaTick;
        public static bool Attack = true;
        public static bool DisableNextAttack;
        public static bool Move = true;
        public static int LastMoveCommandT;
        public static Vector3 LastMoveCommandPosition = Vector3.Zero;
        private static AttackableUnit lastTarget;
        private static readonly Obj_AI_Hero Player;
        private static int delay;
        private static float minDistance = 400;
        private static bool missileLaunched;
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        static Orbwalking()
        {
            Player = ObjectManager.Player;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
            GameObject.OnCreate += MissileClient_OnCreate;
            Spellbook.OnStopCast += SpellbookOnStopCast;
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

        /// <summary>
        ///     Gets called on target changes
        /// </summary>
        public static event OnTargetChangeH OnTargetChange;

        //  <summary>
        //      Gets called if you can't kill a minion with auto attacks
        //  </summary>
        public static event OnNonKillableMinionH OnNonKillableMinion;

        private static void FireBeforeAttack(AttackableUnit target)
        {
            if (BeforeAttack != null)
            {
                BeforeAttack(new BeforeAttackEventArgs { Target = target });
            }
            else
            {
                DisableNextAttack = false;
            }
        }

        private static void FireOnAttack(AttackableUnit unit, AttackableUnit target)
        {
            OnAttack?.Invoke(unit, target);
        }

        private static void FireAfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (AfterAttack != null && target.IsValidTarget())
            {
                AfterAttack(unit, target);
            }
        }

        private static void FireOnTargetSwitch(AttackableUnit newTarget)
        {
            if (OnTargetChange != null && (!lastTarget.IsValidTarget() || lastTarget != newTarget))
            {
                OnTargetChange(lastTarget, newTarget);
            }
        }

        private static void FireOnNonKillableMinion(AttackableUnit minion)
        {
            OnNonKillableMinion?.Invoke(minion);
        }

        /// <summary>
        ///     Returns true if the spellname resets the attack timer.
        /// </summary>
        public static bool IsAutoAttackReset(string name)
        {
            return AttackResets.Contains(name.ToLower());
        }

        /// <summary>
        ///     Returns true if the unit is melee
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
        public static float GetRealAutoAttackRange(AttackableUnit target)
        {
            var result = Player.AttackRange + Player.BoundingRadius;
            if (target.IsValidTarget())
            {
                return result + target.BoundingRadius;
            }
            return result;
        }

        /// <summary>
        ///     Returns true if the target is in auto-attack range.
        /// </summary>
        public static bool InAutoAttackRange(AttackableUnit target)
        {
            if (!target.IsValidTarget())
            {
                return false;
            }
            var myRange = GetRealAutoAttackRange(target);
            return
                Vector2.DistanceSquared(
                    (target as Obj_AI_Base)?.ServerPosition.To2D() ?? target.Position.To2D(),
                    Player.ServerPosition.To2D()) <= myRange * myRange;
        }

        /// <summary>
        ///     Returns player auto-attack missile speed.
        /// </summary>
        public static float GetMyProjectileSpeed()
        {
            return IsMelee(Player) || Player.ChampionName == "Azir" ? float.MaxValue : Player.BasicAttack.MissileSpeed;
        }

        /// <summary>
        ///     Returns if the player's auto-attack is ready.
        /// </summary>
        public static bool CanAttack()
        {
            return Utils.GameTimeTickCount + Game.Ping / 2 + 25 >= LastAaTick + Player.AttackDelay * 1000 && Attack;
        }

        /// <summary>
        ///     Returns true if moving won't cancel the auto-attack.
        /// </summary>
        public static bool CanMove(float extraWindup)
        {
            if (!Move)
            {
                return false;
            }

            if (missileLaunched && Orbwalker.MissileCheck)
            {
                return true;
            }

            return NoCancelChamps.Contains(Player.ChampionName) || (Utils.GameTimeTickCount + Game.Ping / 2 >= LastAaTick + Player.AttackCastDelay * 1000 + extraWindup);
        }

        public static void SetMovementDelay(int sDelay)
        {
            Orbwalking.delay = sDelay;
        }

        public static void SetMinimumOrbwalkDistance(float d)
        {
            minDistance = d;
        }

        public static float GetLastMoveTime()
        {
            return LastMoveCommandT;
        }

        public static Vector3 GetLastMovePosition()
        {
            return LastMoveCommandPosition;
        }

        public static void MoveTo(Vector3 position,
            float holdAreaRadius = 0,
            bool overrideTimer = false,
            bool useFixedDistance = true,
            bool randomizeMinDistance = true)
        {
            if (Utils.GameTimeTickCount - LastMoveCommandT < delay && !overrideTimer)
            {
                return;
            }

            LastMoveCommandT = Utils.GameTimeTickCount;

            var playerPosition = Player.ServerPosition;

            if (playerPosition.Distance(position, true) < holdAreaRadius * holdAreaRadius)
            {
                if (Player.Path.Length > 0)
                {
                    Player.IssueOrder(GameObjectOrder.Stop, playerPosition);
                    LastMoveCommandPosition = playerPosition;
                }
                return;
            }

            var point = position;
            if (useFixedDistance)
            {
                point = playerPosition.Extend(
                    position, (randomizeMinDistance ? (Random.NextFloat(0.6f, 1) + 0.2f) * minDistance : minDistance));
            }
            else
            {
                if (randomizeMinDistance)
                {
                    point = playerPosition.Extend(position, (Random.NextFloat(0.6f, 1) + 0.2f) * minDistance);
                }
                else if (playerPosition.Distance(position) > minDistance)
                {
                    point = playerPosition.Extend(position, minDistance);
                }
            }

            Player.IssueOrder(GameObjectOrder.MoveTo, point);
            LastMoveCommandPosition = point;
        }

        /// <summary>
        ///     Orbwalk a target while moving to Position.
        /// </summary>
        public static void Orbwalk(AttackableUnit target,
            Vector3 position,
            float extraWindup = 90,
            float holdAreaRadius = 0,
            bool useFixedDistance = true,
            bool randomizeMinDistance = true)
        {
            try
            {
                if (target.IsValidTarget() && CanAttack())
                {
                    DisableNextAttack = false;
                    FireBeforeAttack(target);

                    if (!DisableNextAttack)
                    {
                        if (!NoCancelChamps.Contains(Player.ChampionName))
                        {
                            LastAaTick = Utils.GameTimeTickCount + Game.Ping + 100 - (int)(ObjectManager.Player.AttackCastDelay * 1000f);
                            missileLaunched = false;
                        }

                        if (!Player.IssueOrder(GameObjectOrder.AttackUnit, target))
                        {
                            ResetAutoAttackTimer();
                        }

                        lastTarget = target;
                        return;
                    }
                }

                if (CanMove(extraWindup))
                {
                    MoveTo(position, holdAreaRadius, false, useFixedDistance, randomizeMinDistance);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        ///     Resets the Auto-Attack timer.
        /// </summary>
        public static void ResetAutoAttackTimer()
        {
            LastAaTick = 0;
        }

        private static void SpellbookOnStopCast(Spellbook spellbook, SpellbookStopCastEventArgs args)
        {
            if (spellbook.Owner.IsValid && spellbook.Owner.IsMe && args.DestroyMissile && args.StopAnimation)
            {
                ResetAutoAttackTimer();
            }
        }

        private static void MissileClient_OnCreate(GameObject sender, EventArgs args)
        {
            var missile = sender as MissileClient;
            if (missile != null && missile.SpellCaster.IsMe && IsAutoAttack(missile.SData.Name))
            {
                missileLaunched = true;
            }
        }

        private static void OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs Spell)
        {
            try
            {
                var spellName = Spell.SData.Name;

                if (IsAutoAttackReset(spellName) && unit.IsMe)
                {
                    Utility.DelayAction.Add(250, ResetAutoAttackTimer);
                }

                if (!IsAutoAttack(spellName))
                {
                    return;
                }

                if (unit.IsMe &&
                    (Spell.Target is Obj_AI_Base || Spell.Target is Obj_BarracksDampener || Spell.Target is Obj_HQ))
                {
                    LastAaTick = Utils.GameTimeTickCount - Game.Ping / 2;
                    missileLaunched = false;

                    var @base = Spell.Target as Obj_AI_Base;
                    if (@base != null)
                    {
                        var target = @base;
                        if (target.IsValid)
                        {
                            FireOnTargetSwitch(target);
                            lastTarget = target;
                        }

                        //Trigger it for ranged until the missiles catch normal attacks again!
                        Utility.DelayAction.Add(
                            (int)(unit.AttackCastDelay * 1000 + 40), () => FireAfterAttack(unit, lastTarget));
                    }
                }

                FireOnAttack(unit, lastTarget);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public class BeforeAttackEventArgs
        {
            private bool process = true;
            public AttackableUnit Target;
            public Obj_AI_Base Unit = ObjectManager.Player;

            public bool Process
            {
                get { return this.process; }
                set
                {
                    DisableNextAttack = !value;
                    this.process = value;
                }
            }
        }

        /// <summary>
        ///     This class allows you to add an instance of "Orbwalker" to your assembly in order to control the orbwalking in an
        ///     easy way.
        /// </summary>
        public class Orbwalker
        {
            private const float LaneClearWaitTimeMod = 2f;
            private static Menu config;
            private readonly Obj_AI_Hero player;
            private Obj_AI_Base forcedTarget;
            private OrbwalkingMode mode = OrbwalkingMode.None;
            private Vector3 orbwalkingPoint;
            private Obj_AI_Minion prevMinion;
            public static List<Orbwalker> Instances = new List<Orbwalker>();

            public Orbwalker(Menu attachToMenu)
            {
                config = attachToMenu;
                /* Drawings submenu */
                var drawings = new Menu("Drawings", "drawings");
                drawings.AddItem(
                    new MenuItem("AACircle", "AACircle").SetShared()
                        .SetValue(new Circle(true, Color.FromArgb(255, 255, 0, 255))));
                drawings.AddItem(
                    new MenuItem("AACircle2", "Enemy AA circle").SetShared()
                        .SetValue(new Circle(false, Color.FromArgb(255, 255, 0, 255))));
                drawings.AddItem(
                    new MenuItem("HoldZone", "HoldZone").SetShared()
                        .SetValue(new Circle(false, Color.FromArgb(255, 255, 0, 255))));
                config.AddSubMenu(drawings);

                /* Misc options */
                var misc = new Menu("Misc", "Misc");
                misc.AddItem(
                    new MenuItem("HoldPosRadius", "Hold Position Radius").SetShared().SetValue(new Slider(0, 0, 250)));
                misc.AddItem(new MenuItem("PriorizeFarm", "Priorize farm over harass").SetShared().SetValue(true));

                config.AddSubMenu(misc);

                /* Missile check */
                config.AddItem(new MenuItem("MissileCheck", "Use Missile Check").SetShared().SetValue(true));

                /* Delay sliders */
                config.AddItem(
                    new MenuItem("ExtraWindup", "Extra windup time").SetShared().SetValue(new Slider(80, 0, 200)));
                config.AddItem(new MenuItem("FarmDelay", "Farm sDelay").SetShared().SetValue(new Slider(0, 0, 200)));
                config.AddItem(
                    new MenuItem("MovementDelay", "Movement sDelay").SetShared().SetValue(new Slider(30, 0, 250)))
                    .ValueChanged += (sender, args) => SetMovementDelay(args.GetNewValue<Slider>().Value);


                /*Load the menu*/
                config.AddItem(
                    new MenuItem("LastHit", "Last hit").SetShared().SetValue(new KeyBind('X', KeyBindType.Press)));

                config.AddItem(new MenuItem("Farm", "Mixed").SetShared().SetValue(new KeyBind('C', KeyBindType.Press)));

                config.AddItem(
                    new MenuItem("LaneClear", "LaneClear").SetShared().SetValue(new KeyBind('V', KeyBindType.Press)));

                config.AddItem(
                    new MenuItem("Orbwalk", "Combo").SetShared().SetValue(new KeyBind(32, KeyBindType.Press)));

                delay = config.Item("MovementDelay").GetValue<Slider>().Value;


                this.player = ObjectManager.Player;
                Game.OnUpdate += this.GameOnOnGameUpdate;
                Drawing.OnDraw += this.DrawingOnOnDraw;
                Instances.Add(this);
            }

            public virtual bool InAutoAttackRange(AttackableUnit target)
            {
                return Orbwalking.InAutoAttackRange(target);
            }

            private static int FarmDelay => config.Item("FarmDelay").GetValue<Slider>().Value;

            public static bool MissileCheck => config.Item("MissileCheck").GetValue<bool>();

            public OrbwalkingMode ActiveMode
            {
                get
                {
                    if (this.mode != OrbwalkingMode.None)
                    {
                        return this.mode;
                    }

                    if (config.Item("Orbwalk").GetValue<KeyBind>().Active)
                    {
                        return OrbwalkingMode.Combo;
                    }

                    if (config.Item("LaneClear").GetValue<KeyBind>().Active)
                    {
                        return OrbwalkingMode.LaneClear;
                    }

                    if (config.Item("Farm").GetValue<KeyBind>().Active)
                    {
                        return OrbwalkingMode.Mixed;
                    }

                    if (config.Item("LastHit").GetValue<KeyBind>().Active)
                    {
                        return OrbwalkingMode.LastHit;
                    }

                    return OrbwalkingMode.None;
                }
                set { this.mode = value; }
            }

            /// <summary>
            ///     Enables or disables the auto-attacks.
            /// </summary>
            public void SetAttack(bool b)
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
                this.forcedTarget = target;
            }

            /// <summary>
            ///     Forces the orbwalker to move to that point while orbwalking (Game.CursorPos by default).
            /// </summary>
            public void SetOrbwalkingPoint(Vector3 point)
            {
                this.orbwalkingPoint = point;
            }

            private bool ShouldWait()
            {
                return
                    ObjectManager.Get<Obj_AI_Minion>()
                        .Any(
                            minion =>
                                minion.IsValidTarget() && minion.Team != GameObjectTeam.Neutral &&
                                Orbwalking.InAutoAttackRange(minion) &&
                                HealthPrediction.LaneClearHealthPrediction(
                                    minion, (int)((this.player.AttackDelay * 1000) * LaneClearWaitTimeMod), FarmDelay) <=
                                this.player.GetAutoAttackDamage(minion));
            }

            public virtual AttackableUnit GetTarget()
            {
                AttackableUnit result = null;

                if ((this.ActiveMode == OrbwalkingMode.Mixed || this.ActiveMode == OrbwalkingMode.LaneClear) &&
                    !config.Item("PriorizeFarm").GetValue<bool>())
                {
                    var target = TargetSelector.GetTarget(-1, TargetSelector.DamageType.Physical);
                    if (target != null)
                    {
                        return target;
                    }
                }

                /*Killable Minion*/
                if (this.ActiveMode == OrbwalkingMode.LaneClear || this.ActiveMode == OrbwalkingMode.Mixed || this.ActiveMode == OrbwalkingMode.LastHit)
                {
                    var minionList =
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Where(
                                minion =>
                                minion.IsValidTarget() && this.InAutoAttackRange(minion)
                                && minion.Health
                                < 2
                                * (ObjectManager.Player.BaseAttackDamage + ObjectManager.Player.FlatPhysicalDamageMod))
                            .OrderBy(cannonMinion => cannonMinion.CharData.BaseSkinName.Contains("Siege"))
                            .ThenByDescending(minionHealth => minionHealth.Health);

                    foreach (var minion in minionList)
                    {
                        var t = (int)(this.player.AttackCastDelay * 1000) - 100 + Game.Ping / 2 +
                                1000 * (int)this.player.Distance(minion) / (int)GetMyProjectileSpeed();
                        var predHealth = HealthPrediction.GetHealthPrediction(minion, t, FarmDelay);

                        if (minion.Team == GameObjectTeam.Neutral || !MinionManager.IsMinion(minion, true))
                        {
                            continue;
                        }
                        if (predHealth <= 0)
                        {
                            FireOnNonKillableMinion(minion);
                        }

                        if (predHealth > 0 && predHealth <= this.player.GetAutoAttackDamage(minion, true))
                        {
                            return minion;
                        }
                    }
                }

                //Forced target
                if (this.forcedTarget.IsValidTarget() && Orbwalking.InAutoAttackRange(this.forcedTarget))
                {
                    return this.forcedTarget;
                }

                /* turrets / inhibitors / nexus */
                if (this.ActiveMode == OrbwalkingMode.LaneClear)
                {
                    /* turrets */
                    foreach (var turret in
                        ObjectManager.Get<Obj_AI_Turret>().Where(t => t.IsValidTarget() && Orbwalking.InAutoAttackRange(t)))
                    {
                        return turret;
                    }

                    /* inhibitor */
                    foreach (var turret in
                        ObjectManager.Get<Obj_BarracksDampener>().Where(t => t.IsValidTarget() && Orbwalking.InAutoAttackRange(t)))
                    {
                        return turret;
                    }

                    /* nexus */
                    foreach (var nexus in
                        ObjectManager.Get<Obj_HQ>().Where(t => t.IsValidTarget() && Orbwalking.InAutoAttackRange(t)))
                    {
                        return nexus;
                    }
                }

                /*Champions*/
                if (this.ActiveMode != OrbwalkingMode.LastHit)
                {
                    var target = TargetSelector.GetTarget(-1, TargetSelector.DamageType.Physical);
                    if (target.IsValidTarget())
                    {
                        return target;
                    }
                }

                /*Jungle minions*/
                if (this.ActiveMode == OrbwalkingMode.LaneClear || this.ActiveMode == OrbwalkingMode.Mixed)
                {
                    result =
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Where(
                                mob =>
                                    mob.IsValidTarget() && mob.Team == GameObjectTeam.Neutral && this.InAutoAttackRange(mob) && mob.CharData.BaseSkinName != "gangplankbarrel")
                            .MaxOrDefault(mob => mob.MaxHealth);
                    if (result != null)
                    {
                        return result;
                    }
                }

                /*Lane Clear minions*/
                if (this.ActiveMode != OrbwalkingMode.LaneClear)
                {
                    return null;
                }
                if (this.ShouldWait())
                {
                    return null;
                }
                if (this.prevMinion.IsValidTarget() && Orbwalking.InAutoAttackRange(this.prevMinion))
                {
                    var predHealth = HealthPrediction.LaneClearHealthPrediction(
                        this.prevMinion, (int)((this.player.AttackDelay * 1000) * LaneClearWaitTimeMod), FarmDelay);
                    if (predHealth >= 2 * this.player.GetAutoAttackDamage(this.prevMinion) ||
                        Math.Abs(predHealth - this.prevMinion.Health) < float.Epsilon)
                    {
                        return this.prevMinion;
                    }
                }

                result = (from minion in
                              ObjectManager.Get<Obj_AI_Minion>()
                              .Where(minion => minion.IsValidTarget() && Orbwalking.InAutoAttackRange(minion) && minion.CharData.BaseSkinName != "gangplankbarrel")
                          let predHealth =
                              HealthPrediction.LaneClearHealthPrediction(
                                  minion, (int)((this.player.AttackDelay * 1000) * LaneClearWaitTimeMod), FarmDelay)
                          where
                              predHealth >= 2 * this.player.GetAutoAttackDamage(minion) ||
                              Math.Abs(predHealth - minion.Health) < float.Epsilon
                          select minion).MaxOrDefault(m => m.Health);

                if (result != null)
                {
                    this.prevMinion = (Obj_AI_Minion)result;
                }

                return result;
            }

            private void GameOnOnGameUpdate(EventArgs args)
            {
                try
                {
                    if (this.ActiveMode == OrbwalkingMode.None)
                    {
                        return;
                    }

                    //Prevent canceling important spells
                    if (this.player.IsCastingInterruptableSpell(true))
                    {
                        return;
                    }

                    var target = this.GetTarget();
                    Orbwalk(
                        target, (this.orbwalkingPoint.To2D().IsValid()) ? this.orbwalkingPoint : Game.CursorPos,
                        config.Item("ExtraWindup").GetValue<Slider>().Value,
                        config.Item("HoldPosRadius").GetValue<Slider>().Value);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            private void DrawingOnOnDraw(EventArgs args)
            {
                if (config.Item("AACircle").GetValue<Circle>().Active)
                {
                    Render.Circle.DrawCircle(
                        this.player.Position, GetRealAutoAttackRange(null) + 65,
                        config.Item("AACircle").GetValue<Circle>().Color);
                }

                if (config.Item("AACircle2").GetValue<Circle>().Active)
                {
                    foreach (var target in
                        HeroManager.Enemies.FindAll(target => target.IsValidTarget(1175)))
                    {
                        Render.Circle.DrawCircle(
                            target.Position, GetRealAutoAttackRange(target) + 65,
                            config.Item("AACircle2").GetValue<Circle>().Color);
                    }
                }

                if (config.Item("HoldZone").GetValue<Circle>().Active)
                {
                    Render.Circle.DrawCircle(
                        this.player.Position, config.Item("HoldPosRadius").GetValue<Slider>().Value,
                        config.Item("HoldZone").GetValue<Circle>().Color, 5, true);
                }

            }
        }
    }
}