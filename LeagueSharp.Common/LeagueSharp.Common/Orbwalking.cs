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

#endregion

#region

using System;
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

        public delegate void OnTargetChangeH(Obj_AI_Base oldTarget, Obj_AI_Base newTarget);

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
            "dariusnoxiantacticsonh", "fioraflurry", "garenq",
            "hecarimrapidslash", "jaxempowertwo", "jaycehypercharge", "leonashieldofdaybreak", "luciane", "lucianq",
            "monkeykingdoubleattack", "mordekaisermaceofspades", "nasusq", "nautiluspiercinggaze", "netherblade",
            "parley", "poppydevastatingblow", "powerfist", "renektonpreexecute", "rengarq", "shyvanadoubleattack",
            "sivirw", "takedown", "talonnoxiandiplomacy", "trundletrollsmash", "vaynetumble", "vie", "volibearq",
            "xenzhaocombotarget", "yorickspectral"
        };

        //Spells that are not attacks even if they have the "attack" word in their name.
        private static readonly string[] NoAttacks =
        {
            "jarvanivcataclysmattack", "monkeykingdoubleattack",
            "shyvanadoubleattack", "shyvanadoubleattackdragon", "zyragraspingplantattack", "zyragraspingplantattack2",
            "zyragraspingplantattackfire", "zyragraspingplantattack2fire"
        };

        //Spells that are attacks even if they dont have the "attack" word in their name.
        private static readonly string[] Attacks =
        {
            "caitlynheadshotmissile", "frostarrow", "garenslash2",
            "kennenmegaproc", "lucianpassiveattack", "masteryidoublestrike", "quinnwenhanced", "renektonexecute",
            "renektonsuperexecute", "rengarnewpassivebuffdash", "trundleq", "xenzhaothrust", "xenzhaothrust2",
            "xenzhaothrust3"
        };

        public static int LastAATick;

        public static bool Attack = true;
        public static bool DisableNextAttack = false;
        public static bool Move = true;
        public static int LastMoveCommandT = 0;
        private static Obj_AI_Base _lastTarget;
        private static readonly Obj_AI_Hero Player;

        static Orbwalking()
        {
            Player = ObjectManager.Player;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
            GameObject.OnCreate += Obj_SpellMissile_OnCreate;
            Game.OnGameProcessPacket += OnProcessPacket;
        }

        private static void Obj_SpellMissile_OnCreate(GameObject sender, EventArgs args)
        {
            if (sender is Obj_SpellMissile && sender.IsValid)
            {
                var missile = (Obj_SpellMissile) sender;
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

        /// <summary>
        ///     Gets called on target changes
        /// </summary>
        public static event OnTargetChangeH OnTargetChange;

        private static void FireBeforeAttack(Obj_AI_Base target)
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

        private static void FireOnAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if (OnAttack != null)
            {
                OnAttack(unit, target);
            }
        }

        private static void FireAfterAttack(Obj_AI_Base unit, Obj_AI_Base target)
        {
            if (AfterAttack != null)
            {
                AfterAttack(unit, target);
            }
        }

        private static void FireOnTargetSwitch(Obj_AI_Base newTarget)
        {
            if (OnTargetChange != null && (_lastTarget == null || _lastTarget.NetworkId != newTarget.NetworkId))
            {
                OnTargetChange(_lastTarget, newTarget);
            }
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
            if (target != null)
            {
                return result + target.BoundingRadius - (target is Obj_AI_Hero ? 50 : 0);
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
                return Vector2.DistanceSquared(target.ServerPosition.To2D(), Player.ServerPosition.To2D()) <=
                       myRange * myRange;
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
                return Environment.TickCount + Game.Ping / 2 + 25 >= LastAATick + Player.AttackDelay * 1000 && Attack;
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
                return (Environment.TickCount + Game.Ping / 2 >=
                        LastAATick + Player.AttackCastDelay * 1000 + extraWindup) && Move;
            }

            return false;
        }

        private static void MoveTo(Vector3 position, float holdAreaRadius = 0, bool overrideTimer = false)
        {
            if (Environment.TickCount - LastMoveCommandT < 80 && !overrideTimer)
            {
                return;
            }

            LastMoveCommandT = Environment.TickCount;

            if (Player.ServerPosition.Distance(position) < holdAreaRadius)
            {
                if (Player.Path.Count() > 1)
                {
                    Player.IssueOrder(GameObjectOrder.HoldPosition, Player.ServerPosition);
                }
                return;
            }

            var point = Player.ServerPosition +
                        400 * (position.To2D() - Player.ServerPosition.To2D()).Normalized().To3D();
            Player.IssueOrder(GameObjectOrder.MoveTo, point);
        }

        /// <summary>
        ///     Orbwalk a target while moving to Position.
        /// </summary>
        public static void Orbwalk(Obj_AI_Base target,
            Vector3 position,
            float extraWindup = 90,
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
                    {
                        LastAATick = Environment.TickCount + Game.Ping / 2;
                    }
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
            if (args.PacketData[0] == 0x34 && args.PacketData[9] == 17 &&
                new GamePacket(args.PacketData).ReadInteger(1) == ObjectManager.Player.NetworkId)
            {
                ResetAutoAttackTimer();
            }
        }

        private static void OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs Spell)
        {
            if (IsAutoAttackReset(Spell.SData.Name) && unit.IsMe)
            {
                Utility.DelayAction.Add(250, ResetAutoAttackTimer);
            }

            if (IsAutoAttack(Spell.SData.Name))
            {
                if (unit.IsMe)
                {
                    LastAATick = Environment.TickCount - Game.Ping / 2;
                    if (Spell.Target is Obj_AI_Base)
                    {
                        FireOnTargetSwitch((Obj_AI_Base) Spell.Target);
                        _lastTarget = (Obj_AI_Base) Spell.Target;
                    }

                    if (unit.IsMelee())
                    {
                        Utility.DelayAction.Add(
                            (int) (unit.AttackCastDelay * 1000 + 40), () => FireAfterAttack(unit, _lastTarget));
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
            private static Menu _config;
            private readonly Obj_AI_Hero Player;

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
                    new MenuItem("AACircle2", "Enemy AA circle").SetShared()
                        .SetValue(new Circle(false, Color.FromArgb(255, 255, 0, 255))));
                drawings.AddItem(
                    new MenuItem("HoldZone", "HoldZone").SetShared()
                        .SetValue(new Circle(false, Color.FromArgb(255, 255, 0, 255))));
                _config.AddSubMenu(drawings);

                /* Misc options */
                var misc = new Menu("Misc", "Misc");
                misc.AddItem(
                    new MenuItem("HoldPosRadius", "Hold Position Radius").SetShared().SetValue(new Slider(0, 150, 0)));
                misc.AddItem(new MenuItem("PriorizeFarm", "Priorize farm over harass").SetShared().SetValue(true));
                _config.AddSubMenu(misc);


                /* Delay sliders */
                _config.AddItem(
                    new MenuItem("ExtraWindup", "Extra windup time").SetShared().SetValue(new Slider(80, 200, 0)));
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
                        .SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press, false)));

                _config.AddItem(
                    new MenuItem("Orbwalk", "Combo").SetShared().SetValue(new KeyBind(32, KeyBindType.Press, false)));


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
                    {
                        return OrbwalkingMode.Combo;
                    }

                    if (_config.Item("LaneClear").GetValue<KeyBind>().Active)
                    {
                        return OrbwalkingMode.LaneClear;
                    }

                    if (_config.Item("Farm").GetValue<KeyBind>().Active)
                    {
                        return OrbwalkingMode.Mixed;
                    }

                    if (_config.Item("LastHit").GetValue<KeyBind>().Active)
                    {
                        return OrbwalkingMode.LastHit;
                    }


                    return OrbwalkingMode.None;
                }
            }

            /// <summary>
            ///     Enables or disables the auto-attacks.
            /// </summary>
            [Obsolete("Use SetAttack")]
            public void SetAttacks(bool b)
            {
                Attack = b;
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
                                HealthPrediction.LaneClearHealthPrediction(
                                    minion, (int) ((Player.AttackDelay * 1000) * LaneClearWaitTimeMod), FarmDelay) <=
                                Player.GetAutoAttackDamage(minion));
            }

            public Obj_AI_Base GetTarget()
            {
                Obj_AI_Base result = null;
                var r = float.MaxValue;

                if ((ActiveMode == OrbwalkingMode.Mixed || ActiveMode == OrbwalkingMode.LaneClear) &&
                    !_config.Item("PriorizeFarm").GetValue<bool>())
                {
                    var target = SimpleTs.GetTarget(-1, SimpleTs.DamageType.Physical);
                    if (target != null)
                    {
                        return target;
                    }
                }

                /*Killable Minion*/
                if (ActiveMode == OrbwalkingMode.LaneClear || ActiveMode == OrbwalkingMode.Mixed ||
                    ActiveMode == OrbwalkingMode.LastHit)
                {
                    foreach (var minion in
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Where(minion => minion.IsValidTarget() && InAutoAttackRange(minion)))
                    {
                        var t = (int) (Player.AttackCastDelay * 1000) - 100 + Game.Ping / 2 +
                                1000 * (int) Player.Distance(minion) / (int) GetMyProjectileSpeed();
                        var predHealth = HealthPrediction.GetHealthPrediction(minion, t, FarmDelay);

                        if (minion.Team != GameObjectTeam.Neutral && predHealth > 0 &&
                            predHealth <= Player.GetAutoAttackDamage(minion, true))
                        {
                            //Game.PrintChat("Current Health: " + minion.Health + " Predicted Health:" + (DamageLib.CalcPhysicalMinionDmg(Player.BaseAttackDamage + Player.FlatPhysicalDamageMod, minion, true) - 1 + Orbwalking.GetPassiveDamage(minion)));
                            return minion;
                        }
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
                        return target;
                    }
                }

                /*Jungle minions*/
                if (ActiveMode == OrbwalkingMode.LaneClear || ActiveMode == OrbwalkingMode.Mixed)
                {
                    foreach (var mob in
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Where(
                                mob =>
                                    mob.IsValidTarget() && InAutoAttackRange(mob) && mob.Team == GameObjectTeam.Neutral)
                            .Where(mob => mob.MaxHealth >= r || Math.Abs(r - float.MaxValue) < float.Epsilon))
                    {
                        result = mob;
                        r = mob.MaxHealth;
                    }
                }

                if (result != null)
                {
                    return result;
                }

                /*Lane Clear minions*/
                r = float.MaxValue;
                if (ActiveMode == OrbwalkingMode.LaneClear)
                {
                    if (!ShouldWait())
                    {
                        if (_prevMinion != null && _prevMinion.IsValidTarget() && InAutoAttackRange(_prevMinion))
                        {
                            var predHealth = HealthPrediction.LaneClearHealthPrediction(
                                _prevMinion, (int) ((Player.AttackDelay * 1000) * LaneClearWaitTimeMod), FarmDelay);
                            if (predHealth >= 2 * Player.GetAutoAttackDamage(_prevMinion, false) ||
                                Math.Abs(predHealth - _prevMinion.Health) < float.Epsilon)
                            {
                                return _prevMinion;
                            }
                        }

                        foreach (var minion in
                            ObjectManager.Get<Obj_AI_Minion>()
                                .Where(minion => minion.IsValidTarget() && InAutoAttackRange(minion)))
                        {
                            var predHealth = HealthPrediction.LaneClearHealthPrediction(
                                minion, (int) ((Player.AttackDelay * 1000) * LaneClearWaitTimeMod), FarmDelay);
                            if (predHealth >= 2 * Player.GetAutoAttackDamage(minion, false) ||
                                Math.Abs(predHealth - minion.Health) < float.Epsilon)
                            {
                                if (minion.Health >= r || Math.Abs(r - float.MaxValue) < float.Epsilon)
                                {
                                    result = minion;
                                    r = minion.Health;
                                    _prevMinion = minion;
                                }
                            }
                        }
                    }
                }

                /*turrets*/
                if (ActiveMode == OrbwalkingMode.LaneClear)
                {
                    foreach (var turret in
                        ObjectManager.Get<Obj_AI_Turret>().Where(t => t.IsValidTarget() && InAutoAttackRange(t)))
                    {
                        return turret;
                    }
                }

                return result;
            }

            private void GameOnOnGameUpdate(EventArgs args)
            {
                if (ActiveMode == OrbwalkingMode.None)
                {
                    return;
                }

                //Prevent canceling important channeled spells like Miss Fortunes R.
                if (Player.IsChannelingImportantSpell())
                {
                    return;
                }

                var target = GetTarget();
                Orbwalk(
                    target, (_orbwalkingPoint.To2D().IsValid()) ? _orbwalkingPoint : Game.CursorPos,
                    _config.Item("ExtraWindup").GetValue<Slider>().Value,
                    _config.Item("HoldPosRadius").GetValue<Slider>().Value);
            }

            private void DrawingOnOnDraw(EventArgs args)
            {
                if (_config.Item("AACircle").GetValue<Circle>().Active)
                {
                    Utility.DrawCircle(
                        Player.Position, GetRealAutoAttackRange(null) + 65,
                        _config.Item("AACircle").GetValue<Circle>().Color);
                }

                if (_config.Item("AACircle2").GetValue<Circle>().Active)
                {
                    foreach (var target in ObjectManager.Get<Obj_AI_Hero>().Where(target => target.IsValidTarget(1175)))
                    {
                        Utility.DrawCircle(
                            target.Position, GetRealAutoAttackRange(target) + 65,
                            _config.Item("AACircle2").GetValue<Circle>().Color);
                    }
                }

                if (_config.Item("HoldZone").GetValue<Circle>().Active)
                {
                    Utility.DrawCircle(
                        Player.Position, _config.Item("HoldPosRadius").GetValue<Slider>().Value,
                        _config.Item("HoldZone").GetValue<Circle>().Color);
                }
            }
        }
    }
}