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
        /// <summary>
        /// Delegate AfterAttackEvenH
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        public delegate void AfterAttackEvenH(AttackableUnit unit, AttackableUnit target);

        /// <summary>
        /// Delegate BeforeAttackEvenH
        /// </summary>
        /// <param name="args">The <see cref="BeforeAttackEventArgs"/> instance containing the event data.</param>
        public delegate void BeforeAttackEvenH(BeforeAttackEventArgs args);

        /// <summary>
        /// Delegate OnAttackEvenH
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        public delegate void OnAttackEvenH(AttackableUnit unit, AttackableUnit target);

        /// <summary>
        /// Delegate OnNonKillableMinionH
        /// </summary>
        /// <param name="minion">The minion.</param>
        public delegate void OnNonKillableMinionH(AttackableUnit minion);

        /// <summary>
        /// Delegate OnTargetChangeH
        /// </summary>
        /// <param name="oldTarget">The old target.</param>
        /// <param name="newTarget">The new target.</param>
        public delegate void OnTargetChangeH(AttackableUnit oldTarget, AttackableUnit newTarget);

        /// <summary>
        /// The orbwalking mode.
        /// </summary>
        public enum OrbwalkingMode
        {
            /// <summary>
            /// The orbalker will only last hit minions.
            /// </summary>
            LastHit,

            /// <summary>
            /// The orbwalker will alternate between last hitting and auto attacking champions.
            /// </summary>
            Mixed,

            /// <summary>
            /// The orbwalker will clear the lane of minions as fast as possible while attempting to get the last hit.
            /// </summary>
            LaneClear,

            /// <summary>
            /// The orbwalker will only attack the target.
            /// </summary>
            Combo,

            /// <summary>
            /// The orbwalker will only move.
            /// </summary>
            CustomMode,

            /// <summary>
            /// The orbwalker does nothing.
            /// </summary>
            None
        }

        /// <summary>
        /// Spells that reset the attack timer.
        /// </summary>
        private static readonly string[] AttackResets =
        {
            "dariusnoxiantacticsonh", "fioraflurry", "garenq",
            "hecarimrapidslash", "jaxempowertwo", "jaycehypercharge", "leonashieldofdaybreak", "luciane", "lucianq",
            "monkeykingdoubleattack", "mordekaisermaceofspades", "nasusq", "nautiluspiercinggaze", "netherblade",
            "gangplankqwrapper", "poppydevastatingblow", "powerfist", "renektonpreexecute", "rengarq", "shyvanadoubleattack",
            "sivirw", "takedown", "talonnoxiandiplomacy", "trundletrollsmash", "vaynetumble", "vie", "volibearq",
            "xenzhaocombotarget", "yorickspectral", "reksaiq", "itemtitanichydracleave"
        };


        /// <summary>
        /// Spells that are not attacks even if they have the "attack" word in their name.
        /// </summary>
        private static readonly string[] NoAttacks =
        {
            "volleyattack", "volleyattackwithsound", "jarvanivcataclysmattack",
            "monkeykingdoubleattack", "shyvanadoubleattack",
            "shyvanadoubleattackdragon", "zyragraspingplantattack",
            "zyragraspingplantattack2", "zyragraspingplantattackfire",
            "zyragraspingplantattack2fire", "viktorpowertransfer",
            "sivirwattackbounce", "asheqattacknoonhit",
            "elisespiderlingbasicattack", "heimertyellowbasicattack",
            "heimertyellowbasicattack2", "heimertbluebasicattack",
            "annietibbersbasicattack", "annietibbersbasicattack2",
            "yorickdecayedghoulbasicattack", "yorickravenousghoulbasicattack",
            "yorickspectralghoulbasicattack", "malzaharvoidlingbasicattack",
            "malzaharvoidlingbasicattack2", "malzaharvoidlingbasicattack3",
            "kindredwolfbasicattack", "kindredbasicattackoverridelightbombfinal"
        };


        /// <summary>
        /// Spells that are attacks even if they dont have the "attack" word in their name.
        /// </summary>
        private static readonly string[] Attacks =
        {
            "caitlynheadshotmissile", "frostarrow", "garenslash2",
            "kennenmegaproc", "lucianpassiveattack", "masteryidoublestrike", "quinnwenhanced", "renektonexecute",
            "renektonsuperexecute", "rengarnewpassivebuffdash", "trundleq", "xenzhaothrust", "xenzhaothrust2",
            "xenzhaothrust3", "viktorqbuff"
        };

        /// <summary>
        /// Champs whose auto attacks can't be cancelled
        /// </summary>
        private static readonly string[] NoCancelChamps = { "Kalista" };

        /// <summary>
        /// The last auto attack tick
        /// </summary>
        public static int LastAATick;

        /// <summary>
        /// <c>true</c> if the orbwalker will attack.
        /// </summary>
        public static bool Attack = true;

        /// <summary>
        /// <c>true</c> if the orbwalker will skip the next attack.
        /// </summary>
        public static bool DisableNextAttack;

        /// <summary>
        /// <c>true</c> if the orbwalker will move.
        /// </summary>
        public static bool Move = true;

        /// <summary>
        /// The tick the most recent attack command was sent.
        /// </summary>
        public static int LastAttackCommandT;

        /// <summary>
        /// The tick the most recent move command was sent.
        /// </summary>
        public static int LastMoveCommandT;

        /// <summary>
        /// The last move command position
        /// </summary>
        public static Vector3 LastMoveCommandPosition = Vector3.Zero;

        /// <summary>
        /// The last target
        /// </summary>
        private static AttackableUnit _lastTarget;

        /// <summary>
        /// The player
        /// </summary>
        private static readonly Obj_AI_Hero Player;

        /// <summary>
        /// The delay
        /// </summary>
        private static int _delay;

        /// <summary>
        /// The minimum distance
        /// </summary>
        private static float _minDistance = 400;

        /// <summary>
        /// <c>true</c> if the auto attack missile was launched from the player.
        /// </summary>
        private static bool _missileLaunched;

        /// <summary>
        /// The champion name
        /// </summary>
        private static string _championName;

        /// <summary>
        /// The random
        /// </summary>
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Initializes static members of the <see cref="Orbwalking"/> class.
        /// </summary>
        static Orbwalking()
        {
            Player = ObjectManager.Player;
            _championName = Player.ChampionName;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
            Obj_AI_Base.OnDoCast += Obj_AI_Base_OnDoCast;
            Spellbook.OnStopCast += SpellbookOnStopCast;
        }

        /// <summary>
        /// This event is fired before the player auto attacks.
        /// </summary>
        public static event BeforeAttackEvenH BeforeAttack;

        /// <summary>
        /// This event is fired when a unit is about to auto-attack another unit.
        /// </summary>
        public static event OnAttackEvenH OnAttack;

        /// <summary>
        /// This event is fired after a unit finishes auto-attacking another unit (Only works with player for now).
        /// </summary>
        public static event AfterAttackEvenH AfterAttack;

        /// <summary>
        /// Gets called on target changes
        /// </summary>
        public static event OnTargetChangeH OnTargetChange;

        ///<summary>
        /// Occurs when a minion is not killable by an auto attack.
        /// </summary>
        public static event OnNonKillableMinionH OnNonKillableMinion;

        /// <summary>
        /// Fires the before attack event.
        /// </summary>
        /// <param name="target">The target.</param>
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

        /// <summary>
        /// Fires the on attack event.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        private static void FireOnAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (OnAttack != null)
            {
                OnAttack(unit, target);
            }
        }

        /// <summary>
        /// Fires the after attack event.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        private static void FireAfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (AfterAttack != null && target.IsValidTarget())
            {
                AfterAttack(unit, target);
            }
        }

        /// <summary>
        /// Fires the on target switch event.
        /// </summary>
        /// <param name="newTarget">The new target.</param>
        private static void FireOnTargetSwitch(AttackableUnit newTarget)
        {
            if (OnTargetChange != null && (!_lastTarget.IsValidTarget() || _lastTarget != newTarget))
            {
                OnTargetChange(_lastTarget, newTarget);
            }
        }

        /// <summary>
        /// Fires the on non killable minion event.
        /// </summary>
        /// <param name="minion">The minion.</param>
        private static void FireOnNonKillableMinion(AttackableUnit minion)
        {
            if (OnNonKillableMinion != null)
            {
                OnNonKillableMinion(minion);
            }
        }

        /// <summary>
        /// Returns true if the spellname resets the attack timer.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the specified name is an auto attack reset; otherwise, <c>false</c>.</returns>
        public static bool IsAutoAttackReset(string name)
        {
            return AttackResets.Contains(name.ToLower());
        }

        /// <summary>
        /// Returns true if the unit is melee
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns><c>true</c> if the specified unit is melee; otherwise, <c>false</c>.</returns>
        public static bool IsMelee(this Obj_AI_Base unit)
        {
            return unit.CombatType == GameObjectCombatType.Melee;
        }

        /// <summary>
        /// Returns true if the spellname is an auto-attack.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the name is an auto attack; otherwise, <c>false</c>.</returns>
        public static bool IsAutoAttack(string name)
        {
            return (name.ToLower().Contains("attack") && !NoAttacks.Contains(name.ToLower())) ||
                   Attacks.Contains(name.ToLower());
        }

        /// <summary>
        /// Returns the auto-attack range of local player with respect to the target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>System.Single.</returns>
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
        /// Returns the auto-attack range of the target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>System.Single.</returns>
        public static float GetAttackRange(Obj_AI_Hero target)
        {
            var result = target.AttackRange + target.BoundingRadius;
            return result;
        }

        /// <summary>
        /// Returns true if the target is in auto-attack range.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool InAutoAttackRange(AttackableUnit target)
        {
            if (!target.IsValidTarget())
            {
                return false;
            }
            var myRange = GetRealAutoAttackRange(target);
            return
                Vector2.DistanceSquared(
                    (target is Obj_AI_Base) ? ((Obj_AI_Base)target).ServerPosition.To2D() : target.Position.To2D(),
                    Player.ServerPosition.To2D()) <= myRange * myRange;
        }

        /// <summary>
        /// Returns player auto-attack missile speed.
        /// </summary>
        /// <returns>System.Single.</returns>
        public static float GetMyProjectileSpeed()
        {
            return IsMelee(Player) || _championName == "Azir" || _championName == "Velkoz" || _championName == "Viktor" && Player.HasBuff("ViktorPowerTransferReturn") ? float.MaxValue : Player.BasicAttack.MissileSpeed;
        }

        /// <summary>
        /// Returns if the player's auto-attack is ready.
        /// </summary>
        /// <returns><c>true</c> if this instance can attack; otherwise, <c>false</c>.</returns>
        public static bool CanAttack()
        {
            return Utils.GameTimeTickCount + Game.Ping / 2 + 25 >= LastAATick + Player.AttackDelay * 1000 && Attack;
        }

        /// <summary>
        /// Returns true if moving won't cancel the auto-attack.
        /// </summary>
        /// <param name="extraWindup">The extra windup.</param>
        /// <returns><c>true</c> if this instance can move the specified extra windup; otherwise, <c>false</c>.</returns>
        public static bool CanMove(float extraWindup)
        {
            if (!Move)
            {
                return false;
            }

            if (_missileLaunched && Orbwalker.MissileCheck)
            {
                return true;
            }

            var localExtraWindup = 0;
            if(_championName == "Rengar" && (Player.HasBuff("rengarqbase") || Player.HasBuff("rengarqemp")))
            {
                localExtraWindup = 200;
            }

            return NoCancelChamps.Contains(_championName) || (Utils.GameTimeTickCount + Game.Ping / 2 >= LastAATick + Player.AttackCastDelay * 1000 + extraWindup + localExtraWindup);
        }

        /// <summary>
        /// Sets the movement delay.
        /// </summary>
        /// <param name="delay">The delay.</param>
        public static void SetMovementDelay(int delay)
        {
            _delay = delay;
        }

        /// <summary>
        /// Sets the minimum orbwalk distance.
        /// </summary>
        /// <param name="d">The d.</param>
        public static void SetMinimumOrbwalkDistance(float d)
        {
            _minDistance = d;
        }

        /// <summary>
        /// Gets the last move time.
        /// </summary>
        /// <returns>System.Single.</returns>
        public static float GetLastMoveTime()
        {
            return LastMoveCommandT;
        }

        /// <summary>
        /// Gets the last move position.
        /// </summary>
        /// <returns>Vector3.</returns>
        public static Vector3 GetLastMovePosition()
        {
            return LastMoveCommandPosition;
        }

        /// <summary>
        /// Moves to the position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="holdAreaRadius">The hold area radius.</param>
        /// <param name="overrideTimer">if set to <c>true</c> [override timer].</param>
        /// <param name="useFixedDistance">if set to <c>true</c> [use fixed distance].</param>
        /// <param name="randomizeMinDistance">if set to <c>true</c> [randomize minimum distance].</param>
        public static void MoveTo(Vector3 position,
            float holdAreaRadius = 0,
            bool overrideTimer = false,
            bool useFixedDistance = true,
            bool randomizeMinDistance = true)
        {
            var playerPosition = Player.ServerPosition;

            if (playerPosition.Distance(position, true) < holdAreaRadius * holdAreaRadius)
            {
                if (Player.Path.Length > 0)
                {
                    Player.IssueOrder(GameObjectOrder.Stop, playerPosition);
                    LastMoveCommandPosition = playerPosition;
                    LastMoveCommandT = Utils.GameTimeTickCount - 70;
                }
                return;
            }

            var point = position;

            if(Player.Distance(point, true) < 150 * 150)
            {
                point = playerPosition.Extend(position, (randomizeMinDistance ? (_random.NextFloat(0.6f, 1) + 0.2f) * _minDistance : _minDistance));
            }
            var angle = 0f;
            var currentPath = Player.GetWaypoints();
            if (currentPath.Count > 1 && currentPath.PathLength() > 100)
            {
                var movePath = Player.GetPath(point);

                if(movePath.Length > 1)
                {
                    var v1 = currentPath[1] - currentPath[0];
                    var v2 = movePath[1] - movePath[0];
                    angle = v1.AngleBetween(v2.To2D());
                    var distance = movePath.Last().To2D().Distance(currentPath.Last(), true);

                    if ((angle < 10 && distance < 500*500) || distance < 50*50)
                    {
                        return;
                    }
                }
            }
            
            if (Utils.GameTimeTickCount - LastMoveCommandT < (70 + Math.Min(60, Game.Ping)) && !overrideTimer && angle < 60)
            {
                return;
            }

            if(angle >= 60 && Utils.GameTimeTickCount - LastMoveCommandT < 60)
            {
                return;
            }

            Player.IssueOrder(GameObjectOrder.MoveTo, point);
            LastMoveCommandPosition = point;
            LastMoveCommandT = Utils.GameTimeTickCount;
        }

        /// <summary>
        /// Orbwalks a target while moving to Position.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="position">The position.</param>
        /// <param name="extraWindup">The extra windup.</param>
        /// <param name="holdAreaRadius">The hold area radius.</param>
        /// <param name="useFixedDistance">if set to <c>true</c> [use fixed distance].</param>
        /// <param name="randomizeMinDistance">if set to <c>true</c> [randomize minimum distance].</param>
        public static void Orbwalk(AttackableUnit target,
            Vector3 position,
            float extraWindup = 90,
            float holdAreaRadius = 0,
            bool useFixedDistance = true,
            bool randomizeMinDistance = true)
        {
            if (Utils.GameTimeTickCount - LastAttackCommandT < (70 + Math.Min(60, Game.Ping)))
            {
                return;
            }

            try
            {
                if (target.IsValidTarget() && CanAttack())
                {
                    DisableNextAttack = false;
                    FireBeforeAttack(target);

                    if (!DisableNextAttack)
                    {
                        if (!NoCancelChamps.Contains(_championName))
                        {
                            _missileLaunched = false;
                        }

                        if(Player.IssueOrder(GameObjectOrder.AttackUnit, target))
                        {
                            LastAttackCommandT = Utils.GameTimeTickCount;
                            _lastTarget = target;
                        }
                        
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
        /// Resets the Auto-Attack timer.
        /// </summary>
        public static void ResetAutoAttackTimer()
        {
            LastAATick = 0;
        }

        /// <summary>
        /// Fired when the spellbook stops casting a spell.
        /// </summary>
        /// <param name="spellbook">The spellbook.</param>
        /// <param name="args">The <see cref="SpellbookStopCastEventArgs"/> instance containing the event data.</param>
        private static void SpellbookOnStopCast(Spellbook spellbook, SpellbookStopCastEventArgs args)
        {
            if (spellbook.Owner.IsValid && spellbook.Owner.IsMe && args.DestroyMissile && args.StopAnimation)
            {
                ResetAutoAttackTimer();
            }
        }

        /// <summary>
        /// Fired when an auto attack is fired.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private static void Obj_AI_Base_OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if(sender.IsMe)
            {
                if(Game.Ping <= 30) //First world problems kappa
                {
                    Utility.DelayAction.Add(30, () => Obj_AI_Base_OnDoCast_Delayed(sender, args));
                    return;
                }

                Obj_AI_Base_OnDoCast_Delayed(sender, args);
            }
        }

        /// <summary>
        /// Fired 30ms after an auto attack is launched.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private static void Obj_AI_Base_OnDoCast_Delayed(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (IsAutoAttackReset(args.SData.Name))
            {
                ResetAutoAttackTimer();
            }

            if(IsAutoAttack(args.SData.Name))
            {
                FireAfterAttack(sender, args.Target as AttackableUnit);
                _missileLaunched = true;
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ProcessSpell" /> event.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="Spell">The <see cref="GameObjectProcessSpellCastEventArgs"/> instance containing the event data.</param>
        private static void OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs Spell)
        {
            try
            {
                var spellName = Spell.SData.Name;

                if (unit.IsMe && IsAutoAttackReset(spellName) && Spell.SData.SpellCastTime == 0)
                {
                    ResetAutoAttackTimer();
                }

                if (!IsAutoAttack(spellName))
                {
                    return;
                }

                if (unit.IsMe &&
                    (Spell.Target is Obj_AI_Base || Spell.Target is Obj_BarracksDampener || Spell.Target is Obj_HQ))
                {
                    LastAATick = Utils.GameTimeTickCount - Game.Ping / 2;
                    _missileLaunched = false;
                    LastMoveCommandT = 0;

                    if (Spell.Target is Obj_AI_Base)
                    {
                        var target = (Obj_AI_Base)Spell.Target;
                        if (target.IsValid)
                        {
                            FireOnTargetSwitch(target);
                            _lastTarget = target;
                        }
                    }
                }

                FireOnAttack(unit, _lastTarget);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// The before attack event arguments.
        /// </summary>
        public class BeforeAttackEventArgs : EventArgs
        {
            /// <summary>
            /// <c>true</c> if the orbwalker should continue with the attack.
            /// </summary>
            private bool _process = true;

            /// <summary>
            /// The target
            /// </summary>
            public AttackableUnit Target;

            /// <summary>
            /// The unit
            /// </summary>
            public Obj_AI_Base Unit = ObjectManager.Player;

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="BeforeAttackEventArgs"/> should continue with the attack.
            /// </summary>
            /// <value><c>true</c> if the orbwalker should continue with the attack; otherwise, <c>false</c>.</value>
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
        /// This class allows you to add an instance of "Orbwalker" to your assembly in order to control the orbwalking in an
        /// easy way.
        /// </summary>
        public class Orbwalker
        {
            /// <summary>
            /// The lane clear wait time modifier.
            /// </summary>
            private const float LaneClearWaitTimeMod = 2f;

            /// <summary>
            /// The configuration
            /// </summary>
            private static Menu _config;

            /// <summary>
            /// The player
            /// </summary>
            private readonly Obj_AI_Hero Player;

            /// <summary>
            /// The forced target
            /// </summary>
            private Obj_AI_Base _forcedTarget;

            /// <summary>
            /// The orbalker mode
            /// </summary>
            private OrbwalkingMode _mode = OrbwalkingMode.None;

            /// <summary>
            /// The orbwalking point
            /// </summary>
            private Vector3 _orbwalkingPoint;

            /// <summary>
            /// The previous minion the orbwalker was targeting.
            /// </summary>
            private Obj_AI_Minion _prevMinion;

            /// <summary>
            /// The instances of the orbwalker.
            /// </summary>
            public static List<Orbwalker> Instances = new List<Orbwalker>();

            /// <summary>
            /// Initializes a new instance of the <see cref="Orbwalker"/> class.
            /// </summary>
            /// <param name="attachToMenu">The menu the orbwalker should attach to.</param>
            public Orbwalker(Menu attachToMenu)
            {
                _config = attachToMenu;
                /* Drawings submenu */
                var drawings = new Menu("Drawings", "drawings");
                drawings.AddItem(
                    new MenuItem("AACircle", "AACircle").SetShared()
                        .SetValue(new Circle(true, Color.FromArgb(155, 255, 255, 0))));
                drawings.AddItem(
                    new MenuItem("AACircle2", "Enemy AA circle").SetShared()
                        .SetValue(new Circle(false, Color.FromArgb(155, 255, 255, 0))));
                drawings.AddItem(
                    new MenuItem("HoldZone", "HoldZone").SetShared()
                        .SetValue(new Circle(false, Color.FromArgb(155, 255, 255, 0))));
                drawings.AddItem(
                    new MenuItem("AALineWidth", "Line Width")).SetShared()
                        .SetValue(new Slider(2, 1, 6));
                _config.AddSubMenu(drawings);

                /* Misc options */
                var misc = new Menu("Misc", "Misc");
                misc.AddItem(
                    new MenuItem("HoldPosRadius", "Hold Position Radius").SetShared().SetValue(new Slider(0, 0, 250)));
                misc.AddItem(new MenuItem("PriorizeFarm", "Priorize farm over harass").SetShared().SetValue(true));
                misc.AddItem(new MenuItem("AttackWards", "Auto attack wards").SetShared().SetValue(false));
                misc.AddItem(new MenuItem("AttackPetsnTraps", "Auto attack pets & traps").SetShared().SetValue(true));
                misc.AddItem(new MenuItem("Smallminionsprio", "Jungle clear small first").SetShared().SetValue(false));

                _config.AddSubMenu(misc);

                /* Missile check */
                _config.AddItem(new MenuItem("MissileCheck", "Use Missile Check").SetShared().SetValue(true));

                /* Delay sliders */
                _config.AddItem(
                    new MenuItem("ExtraWindup", "Extra windup time").SetShared().SetValue(new Slider(80, 0, 200)));
                _config.AddItem(new MenuItem("FarmDelay", "Farm delay").SetShared().SetValue(new Slider(30, 0, 200)));

                /*Load the menu*/
                _config.AddItem(
                    new MenuItem("LastHit", "Last hit").SetShared().SetValue(new KeyBind('X', KeyBindType.Press)));

                _config.AddItem(new MenuItem("Farm", "Mixed").SetShared().SetValue(new KeyBind('C', KeyBindType.Press)));

                _config.AddItem(
                    new MenuItem("LaneClear", "LaneClear").SetShared().SetValue(new KeyBind('V', KeyBindType.Press)));

                _config.AddItem(
                    new MenuItem("Orbwalk", "Combo").SetShared().SetValue(new KeyBind(32, KeyBindType.Press)));

                Player = ObjectManager.Player;
                Game.OnUpdate += GameOnOnGameUpdate;
                Drawing.OnDraw += DrawingOnOnDraw;
                Instances.Add(this);
            }

            /// <summary>
            /// Determines if a target is in auto attack range.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <returns><c>true</c> if a target is in auto attack range, <c>false</c> otherwise.</returns>
            public virtual bool InAutoAttackRange(AttackableUnit target)
            {
                return Orbwalking.InAutoAttackRange(target);
            }

            /// <summary>
            /// Gets the farm delay.
            /// </summary>
            /// <value>The farm delay.</value>
            private int FarmDelay
            {
                get { return _config.Item("FarmDelay").GetValue<Slider>().Value; }
            }

            /// <summary>
            /// Gets a value indicating whether the orbwalker is orbwalking by checking the missiles.
            /// </summary>
            /// <value><c>true</c> if the orbwalker is orbwalking by checking the missiles; otherwise, <c>false</c>.</value>
            public static bool MissileCheck
            {
                get { return _config.Item("MissileCheck").GetValue<bool>(); }
            }

            /// <summary>
            /// Registers the Custom Mode of the Orbwalker. Useful for adding a flee mode and such.
            /// </summary>
            /// <param name="name">The name of the mode in the menu. Ex. Flee</param>
            /// <param name="key">The default key for this mode.</param>
            public virtual void RegisterCustomMode(string name, uint key)
            {
                if (_config.Item("CustomMode") == null)
                {
                    _config.AddItem(
                        new MenuItem("CustomMode", name).SetShared().SetValue(new KeyBind(key, KeyBindType.Press)));
                }
            }

            /// <summary>
            /// Gets or sets the active mode.
            /// </summary>
            /// <value>The active mode.</value>
            public OrbwalkingMode ActiveMode
            {
                get
                {
                    if (_mode != OrbwalkingMode.None)
                    {
                        return _mode;
                    }

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

                    if (_config.Item("CustomMode") != null &&_config.Item("CustomMode").GetValue<KeyBind>().Active)
                    {
                        return OrbwalkingMode.CustomMode;
                    }

                    return OrbwalkingMode.None;
                }
                set { _mode = value; }
            }

            /// <summary>
            /// Enables or disables the auto-attacks.
            /// </summary>
            /// <param name="b">if set to <c>true</c> the orbwalker will attack units.</param>
            public void SetAttack(bool b)
            {
                Attack = b;
            }

            /// <summary>
            /// Enables or disables the movement.
            /// </summary>
            /// <param name="b">if set to <c>true</c> the orbwalker will move.</param>
            public void SetMovement(bool b)
            {
                Move = b;
            }

            /// <summary>
            /// Forces the orbwalker to attack the set target if valid and in range.
            /// </summary>
            /// <param name="target">The target.</param>
            public void ForceTarget(Obj_AI_Base target)
            {
                _forcedTarget = target;
            }

            /// <summary>
            /// Forces the orbwalker to move to that point while orbwalking (Game.CursorPos by default).
            /// </summary>
            /// <param name="point">The point.</param>
            public void SetOrbwalkingPoint(Vector3 point)
            {
                _orbwalkingPoint = point;
            }

            /// <summary>
            /// Determines if the orbwalker should wait before attacking a minion.
            /// </summary>
            /// <returns><c>true</c> if the orbwalker should wait before attacking a minion, <c>false</c> otherwise.</returns>
            private bool ShouldWait()
            {
                return
                    ObjectManager.Get<Obj_AI_Minion>()
                        .Any(
                            minion =>
                                minion.IsValidTarget() && minion.Team != GameObjectTeam.Neutral &&
                                InAutoAttackRange(minion) && MinionManager.IsMinion(minion, false) &&
                                HealthPrediction.LaneClearHealthPrediction(
                                    minion, (int)((Player.AttackDelay * 1000) * LaneClearWaitTimeMod), FarmDelay) <=
                                Player.GetAutoAttackDamage(minion));
            }

            /// <summary>
            /// Gets the target.
            /// </summary>
            /// <returns>AttackableUnit.</returns>
            public virtual AttackableUnit GetTarget()
            {
                AttackableUnit result = null;

                if ((ActiveMode == OrbwalkingMode.Mixed || ActiveMode == OrbwalkingMode.LaneClear) &&
                    !_config.Item("PriorizeFarm").GetValue<bool>())
                {
                    var target = TargetSelector.GetTarget(-1, TargetSelector.DamageType.Physical);
                    if (target != null && InAutoAttackRange(target))
                    {
                        return target;
                    }
                }

                /*Killable Minion*/
                if (ActiveMode == OrbwalkingMode.LaneClear || ActiveMode == OrbwalkingMode.Mixed ||
                    ActiveMode == OrbwalkingMode.LastHit)
                {
                    var MinionList =
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Where(
                                minion =>
                                    minion.IsValidTarget() && InAutoAttackRange(minion))
                                    .OrderByDescending(minion => minion.CharData.BaseSkinName.Contains("Siege"))
                                    .ThenBy(minion => minion.CharData.BaseSkinName.Contains("Super"))
                                    .ThenBy(minion => minion.Health)
                                    .ThenByDescending(minion => minion.MaxHealth);

                    foreach (var minion in MinionList)
                    {
                        var t = (int)(Player.AttackCastDelay * 1000) - 100 + Game.Ping / 2 +
                                1000 * (int) Math.Max(0, Player.Distance(minion) - Player.BoundingRadius) / (int)GetMyProjectileSpeed();
                        var predHealth = HealthPrediction.GetHealthPrediction(minion, t, FarmDelay);

                        if (minion.Team != GameObjectTeam.Neutral && (_config.Item("AttackPetsnTraps").GetValue<bool>() && minion.BaseSkinName != "jarvanivstandard" || MinionManager.IsMinion(minion, _config.Item("AttackWards").GetValue<bool>()) ))
                        {
                            if (predHealth <= 0)
                            {
                                FireOnNonKillableMinion(minion);
                            }
                            
                            if (predHealth > 0 && predHealth <= Player.GetAutoAttackDamage(minion, true))
                            {
                                return minion;
                            }
                        }
                    }
                }

                //Forced target
                if (_forcedTarget.IsValidTarget() && InAutoAttackRange(_forcedTarget))
                {
                    return _forcedTarget;
                }

                /* turrets / inhibitors / nexus */
                if (ActiveMode == OrbwalkingMode.LaneClear)
                {
                    /* turrets */
                    foreach (var turret in
                        ObjectManager.Get<Obj_AI_Turret>().Where(t => t.IsValidTarget() && InAutoAttackRange(t)))
                    {
                        return turret;
                    }

                    /* inhibitor */
                    foreach (var turret in
                        ObjectManager.Get<Obj_BarracksDampener>().Where(t => t.IsValidTarget() && InAutoAttackRange(t)))
                    {
                        return turret;
                    }

                    /* nexus */
                    foreach (var nexus in
                        ObjectManager.Get<Obj_HQ>().Where(t => t.IsValidTarget() && InAutoAttackRange(t)))
                    {
                        return nexus;
                    }
                }

                /*Champions*/
                if (ActiveMode != OrbwalkingMode.LastHit)
                {
                    var target = TargetSelector.GetTarget(-1, TargetSelector.DamageType.Physical);
                    if (target.IsValidTarget() && InAutoAttackRange(target))
                    {
                        return target;
                    }
                }

                /*Jungle minions*/
                if (ActiveMode == OrbwalkingMode.LaneClear || ActiveMode == OrbwalkingMode.Mixed)
                {
                    var jminions =
                        ObjectManager.Get<Obj_AI_Minion>()
                            .Where(
                                mob =>
                                mob.IsValidTarget() && mob.Team == GameObjectTeam.Neutral && this.InAutoAttackRange(mob)
                                && mob.CharData.BaseSkinName != "gangplankbarrel");

                    result = _config.Item("Smallminionsprio").GetValue<bool>()
                                 ? jminions.MinOrDefault(mob => mob.MaxHealth)
                                 : jminions.MaxOrDefault(mob => mob.MaxHealth);

                    if (result != null)
                    {
                        return result;
                    }
                }

                /*Lane Clear minions*/
                if (ActiveMode == OrbwalkingMode.LaneClear)
                {
                    if (!ShouldWait())
                    {
                        if (_prevMinion.IsValidTarget() && InAutoAttackRange(_prevMinion))
                        {
                            var predHealth = HealthPrediction.LaneClearHealthPrediction(
                                _prevMinion, (int)((Player.AttackDelay * 1000) * LaneClearWaitTimeMod), FarmDelay);
                            if (predHealth >= 2 * Player.GetAutoAttackDamage(_prevMinion) ||
                                Math.Abs(predHealth - _prevMinion.Health) < float.Epsilon)
                            {
                                return _prevMinion;
                            }
                        }

                        result = (from minion in
                                      ObjectManager.Get<Obj_AI_Minion>()
                                          .Where(minion => minion.IsValidTarget() && InAutoAttackRange(minion) &&
                                          (_config.Item("AttackWards").GetValue<bool>() || !MinionManager.IsWard(minion.CharData.BaseSkinName.ToLower())) &&
                                          (_config.Item("AttackPetsnTraps").GetValue<bool>() && minion.CharData.BaseSkinName != "jarvanivstandard" || MinionManager.IsMinion(minion, _config.Item("AttackWards").GetValue<bool>())) &&
                                          minion.CharData.BaseSkinName != "gangplankbarrel")
                                  let predHealth =
                                      HealthPrediction.LaneClearHealthPrediction(
                                          minion, (int)((Player.AttackDelay * 1000) * LaneClearWaitTimeMod), FarmDelay)
                                  where
                                      predHealth >= 2 * Player.GetAutoAttackDamage(minion) ||
                                      Math.Abs(predHealth - minion.Health) < float.Epsilon
                                  select minion).MaxOrDefault(m => !MinionManager.IsMinion(m, true) ? float.MaxValue : m.Health);
                        
                        if (result != null)
                        {
                            _prevMinion = (Obj_AI_Minion)result;
                        }
                    }
                }

                return result;
            }

            /// <summary>
            /// Fired when the game is updated.
            /// </summary>
            /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
            private void GameOnOnGameUpdate(EventArgs args)
            {
                try
                {
                    if (ActiveMode == OrbwalkingMode.None)
                    {
                        return;
                    }

                    //Prevent canceling important spells
                    if (Player.IsCastingInterruptableSpell(true))
                    {
                        return;
                    }

                    var target = GetTarget();
                    Orbwalk(
                        target, (_orbwalkingPoint.To2D().IsValid()) ? _orbwalkingPoint : Game.CursorPos,
                        _config.Item("ExtraWindup").GetValue<Slider>().Value,
                        Math.Max(_config.Item("HoldPosRadius").GetValue<Slider>().Value, 30));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            /// <summary>
            /// Fired when the game is drawn.
            /// </summary>
            /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
            private void DrawingOnOnDraw(EventArgs args)
            {
                if (_config.Item("AACircle").GetValue<Circle>().Active)
                {
                    Render.Circle.DrawCircle(
                        Player.Position, GetRealAutoAttackRange(null) + 65,
                        _config.Item("AACircle").GetValue<Circle>().Color,
                        _config.Item("AALineWidth").GetValue<Slider>().Value);
                }

                if (_config.Item("AACircle2").GetValue<Circle>().Active)
                {
                    foreach (var target in
                        HeroManager.Enemies.FindAll(target => target.IsValidTarget(1175)))
                    {
                        Render.Circle.DrawCircle(
                            target.Position, GetAttackRange(target),
                            _config.Item("AACircle2").GetValue<Circle>().Color, 
                            _config.Item("AALineWidth").GetValue<Slider>().Value);
                    }
                }

                if (_config.Item("HoldZone").GetValue<Circle>().Active)
                {
                    Render.Circle.DrawCircle(
                        Player.Position, _config.Item("HoldPosRadius").GetValue<Slider>().Value,
                        _config.Item("HoldZone").GetValue<Circle>().Color,
                        _config.Item("AALineWidth").GetValue<Slider>().Value, true);
                }

            }
        }
    }
}
