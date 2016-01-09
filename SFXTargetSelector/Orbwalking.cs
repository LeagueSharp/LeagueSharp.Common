#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Orbwalking.cs is part of SFXTargetSelector.

 SFXTargetSelector is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 SFXTargetSelector is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with SFXTargetSelector. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion License

#region

using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SFXTargetSelectorEx.Others;
using SharpDX;
using Color = System.Drawing.Color;
using MinionManager = SFXTargetSelectorEx.Others.MinionManager;
using MinionOrderTypes = SFXTargetSelectorEx.Others.MinionOrderTypes;
using MinionTeam = SFXTargetSelectorEx.Others.MinionTeam;
using MinionTypes = SFXTargetSelectorEx.Others.MinionTypes;

#endregion

namespace SFXTargetSelectorEx
{
    /// <summary>
    ///     This class offers everything related to auto-attacks and orbwalking.
    /// </summary>
    public static class Orbwalking
    {
        /// <summary>
        ///     Delegate AfterAttackEvenH
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        /// <param name="target">The target.</param>
        public delegate void AfterAttackEvenH(AttackableUnit unit, AttackableUnit target);

        /// <summary>
        ///     Delegate BeforeAttackEvenH
        /// </summary>
        /// <param name="args">The <see cref="BeforeAttackEventArgs" /> instance containing the event data.</param>
        public delegate void BeforeAttackEvenH(BeforeAttackEventArgs args);

        /// <summary>
        ///     Delegate OnAttackEvenH
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        public delegate void OnAttackEvenH(AttackableUnit unit, AttackableUnit target);

        /// <summary>
        ///     Delegate OnNonKillableMinionH
        /// </summary>
        /// <param name="minion">The minion.</param>
        public delegate void OnNonKillableMinionH(AttackableUnit minion);

        /// <summary>
        ///     Delegate OnTargetChangeH
        /// </summary>
        /// <param name="oldTarget">The old target.</param>
        /// <param name="newTarget">The new target.</param>
        public delegate void OnTargetChangeH(AttackableUnit oldTarget, AttackableUnit newTarget);

        /// <summary>
        ///     The orbwalking mode.
        /// </summary>
        public enum OrbwalkingMode
        {
            /// <summary>
            ///     The orbalker will only last hit minions.
            /// </summary>
            LastHit,

            /// <summary>
            ///     The orbwalker will alternate between last hitting and auto attacking champions.
            /// </summary>
            Mixed,

            /// <summary>
            ///     The orbwalker will clear the lane of minions as fast as possible while attempting to get the last hit.
            /// </summary>
            LaneClear,

            /// <summary>
            ///     The orbwalker will only attack the target.
            /// </summary>
            Combo,

            /// <summary>
            ///     The orbwalker will only move.
            /// </summary>
            Flee,

            /// <summary>
            ///     The orbwalker will only move.
            /// </summary>
            CustomMode,

            /// <summary>
            ///     The orbwalker does nothing.
            /// </summary>
            None
        }

        /// <summary>
        ///     The orbwalking delay.
        /// </summary>
        public enum OrbwalkingRandomize
        {
            Move,
            Attack
        }

        /// <summary>
        ///     Spells that reset the attack timer.
        /// </summary>
        private static readonly string[] AttackResets =
        {
            "dariusnoxiantacticsonh", "fioraflurry", "garenq",
            "gravesmove", "hecarimrapidslash", "jaxempowertwo", "jaycehypercharge", "leonashieldofdaybreak", "luciane",
            "lucianq", "monkeykingdoubleattack", "mordekaisermaceofspades", "nasusq", "nautiluspiercinggaze",
            "netherblade", "gangplankqwrapper", "poppydevastatingblow", "powerfist", "renektonpreexecute", "rengarq",
            "shyvanadoubleattack", "sivirw", "takedown", "talonnoxiandiplomacy", "trundletrollsmash", "vaynetumble",
            "vie", "volibearq", "xenzhaocombotarget", "yorickspectral", "reksaiq", "itemtitanichydracleave", "masochism",
            "illaoiw"
        };

        /// <summary>
        ///     Spells that are not attacks even if they have the "attack" word in their name.
        /// </summary>
        private static readonly string[] NoAttacks =
        {
            "volleyattack", "volleyattackwithsound",
            "jarvanivcataclysmattack", "monkeykingdoubleattack", "shyvanadoubleattack", "shyvanadoubleattackdragon",
            "zyragraspingplantattack", "zyragraspingplantattack2", "zyragraspingplantattackfire",
            "zyragraspingplantattack2fire", "viktorpowertransfer", "sivirwattackbounce", "asheqattacknoonhit",
            "elisespiderlingbasicattack", "heimertyellowbasicattack", "heimertyellowbasicattack2",
            "heimertbluebasicattack", "annietibbersbasicattack", "annietibbersbasicattack2",
            "yorickdecayedghoulbasicattack", "yorickravenousghoulbasicattack", "yorickspectralghoulbasicattack",
            "malzaharvoidlingbasicattack", "malzaharvoidlingbasicattack2", "malzaharvoidlingbasicattack3",
            "kindredwolfbasicattack", "kindredbasicattackoverridelightbombfinal"
        };

        /// <summary>
        ///     Spells that are attacks even if they dont have the "attack" word in their name.
        /// </summary>
        private static readonly string[] Attacks =
        {
            "caitlynheadshotmissile", "frostarrow", "garenslash2",
            "kennenmegaproc", "lucianpassiveattack", "masteryidoublestrike", "quinnwenhanced", "renektonexecute",
            "renektonsuperexecute", "rengarnewpassivebuffdash", "trundleq", "xenzhaothrust", "xenzhaothrust2",
            "xenzhaothrust3", "viktorqbuff"
        };

        /// <summary>
        ///     Champs whose auto attacks can't be cancelled
        /// </summary>
        private static readonly string[] NoCancelChamps = { "Kalista" };

        private static readonly Dictionary<OrbwalkingRandomize, Randomize> Randomizes =
            new Dictionary<OrbwalkingRandomize, Randomize>();

        /// <summary>
        ///     The last auto attack tick
        /// </summary>
        public static int LastAaTick;

        /// <summary>
        ///     <c>true</c> if the orbwalker will attack.
        /// </summary>
        public static bool Attack = true;

        /// <summary>
        ///     <c>true</c> if the orbwalker will skip the next attack.
        /// </summary>
        public static bool DisableNextAttack;

        /// <summary>
        ///     <c>true</c> if the orbwalker will move.
        /// </summary>
        public static bool Move = true;

        /// <summary>
        ///     The tick the most recent attack command was sent.
        /// </summary>
        public static int LastAttackCommandT;

        /// <summary>
        ///     The tick the most recent move command was sent.
        /// </summary>
        public static int LastMoveCommandT;

        /// <summary>
        ///     The last move command position
        /// </summary>
        public static Vector3 LastMoveCommandPosition = Vector3.Zero;

        /// <summary>
        ///     The last target
        /// </summary>
        private static AttackableUnit _lastTarget;

        /// <summary>
        ///     The player
        /// </summary>
        private static readonly Obj_AI_Hero Player;

        /// <summary>
        ///     The minimum distance
        /// </summary>
        private static float _minDistance = 400;

        /// <summary>
        ///     <c>true</c> if the auto attack missile was launched from the player.
        /// </summary>
        private static bool _missileLaunched;

        /// <summary>
        ///     The champion name
        /// </summary>
        private static readonly string ChampionName;

        /// <summary>
        ///     The random
        /// </summary>
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        private static int _autoattackCounter;

        /// <summary>
        ///     Initializes static members of the <see cref="Orbwalking" /> class.
        /// </summary>
        static Orbwalking()
        {
            Player = ObjectManager.Player;
            ChampionName = Player.ChampionName;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
            Obj_AI_Base.OnDoCast += Obj_AI_Base_OnDoCast;
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

        /// <summary>
        ///     Occurs when a minion is not killable by an auto attack.
        /// </summary>
        public static event OnNonKillableMinionH OnNonKillableMinion;

        /// <summary>
        ///     Fires the before attack event.
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
        ///     Fires the on attack event.
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
        ///     Fires the after attack event.
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
        ///     Fires the on target switch event.
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
        ///     Fires the on non killable minion event.
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
        ///     Returns true if the spellname resets the attack timer.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the specified name is an auto attack reset; otherwise, <c>false</c>.</returns>
        public static bool IsAutoAttackReset(string name)
        {
            return AttackResets.Contains(name.ToLower());
        }

        /// <summary>
        ///     Returns true if the unit is melee
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns><c>true</c> if the specified unit is melee; otherwise, <c>false</c>.</returns>
        public static bool IsMelee(this Obj_AI_Base unit)
        {
            return unit.CombatType == GameObjectCombatType.Melee;
        }

        /// <summary>
        ///     Returns true if the spellname is an auto-attack.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the name is an auto attack; otherwise, <c>false</c>.</returns>
        public static bool IsAutoAttack(string name)
        {
            return (name.ToLower().Contains("attack") && !NoAttacks.Contains(name.ToLower())) ||
                   Attacks.Contains(name.ToLower());
        }

        /// <summary>
        ///     Returns the auto-attack range of local player with respect to the target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>System.Single.</returns>
        public static float GetRealAutoAttackRange(AttackableUnit target)
        {
            var result = Player.AttackRange + Player.BoundingRadius;
            if (target.IsValidTarget())
            {
                var aiBase = target as Obj_AI_Base;
                if (aiBase != null && Player.ChampionName == "Caitlyn")
                {
                    if (aiBase.HasBuff("caitlynyordletrapinternal"))
                    {
                        result += 650;
                    }
                }
                return result + target.BoundingRadius;
            }
            return result;
        }

        /// <summary>
        ///     Returns the auto-attack range of the target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>System.Single.</returns>
        public static float GetAttackRange(Obj_AI_Hero target)
        {
            var result = target.AttackRange + target.BoundingRadius;
            return result;
        }

        /// <summary>
        ///     Returns true if the target is in auto-attack range.
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
                    target is Obj_AI_Base ? ((Obj_AI_Base) target).ServerPosition.To2D() : target.Position.To2D(),
                    Player.ServerPosition.To2D()) <= myRange * myRange;
        }

        /// <summary>
        ///     Returns player auto-attack missile speed.
        /// </summary>
        /// <returns>System.Single.</returns>
        public static float GetMyProjectileSpeed()
        {
            return IsMelee(Player) || ChampionName == "Azir" || ChampionName == "Velkoz" ||
                   ChampionName == "Viktor" && Player.HasBuff("ViktorPowerTransferReturn")
                ? float.MaxValue
                : Player.BasicAttack.MissileSpeed;
        }

        /// <summary>
        ///     Returns if the player's auto-attack is ready.
        /// </summary>
        /// <param name="extraDelay">The extra delay.</param>
        /// <returns><c>true</c> if this instance can attack; otherwise, <c>false</c>.</returns>
        public static bool CanAttack(float extraDelay = 0)
        {
            if (Player.ChampionName == "Graves" && Attack)
            {
                var attackDelay = 1.0740296828d * 1000 * Player.AttackDelay - 716.2381256175d;
                if (LeagueSharp.Common.Utils.GameTimeTickCount + Game.Ping / 2 + 25 >= LastAaTick + attackDelay &&
                    Player.HasBuff("GravesBasicAttackAmmo1"))
                {
                    return true;
                }
                return false;
            }
            return LeagueSharp.Common.Utils.GameTimeTickCount + Game.Ping / 2 + 25 >=
                   LastAaTick + Player.AttackDelay * 1000 + extraDelay && Attack;
        }

        /// <summary>
        ///     Returns true if moving won't cancel the auto-attack.
        /// </summary>
        /// <param name="extraWindup">The extra windup.</param>
        /// <param name="disableMissileCheck">Disable missile check.</param>
        /// <returns><c>true</c> if this instance can move the specified extra windup; otherwise, <c>false</c>.</returns>
        public static bool CanMove(float extraWindup, bool disableMissileCheck = false)
        {
            if (!Move)
            {
                return false;
            }

            if (_missileLaunched && Orbwalker.MissileCheck && !disableMissileCheck)
            {
                return true;
            }

            var localExtraWindup = 0;
            if (ChampionName == "Rengar" && (Player.HasBuff("rengarqbase") || Player.HasBuff("rengarqemp")))
            {
                localExtraWindup = 200;
            }

            return NoCancelChamps.Contains(ChampionName) ||
                   (LeagueSharp.Common.Utils.GameTimeTickCount + Game.Ping / 2 >=
                    LastAaTick + Player.AttackCastDelay * 1000 + extraWindup + localExtraWindup);
        }

        public static void SetRandomize(float value, OrbwalkingRandomize randomize)
        {
            Randomize randomizeEntry;
            if (Randomizes.TryGetValue(randomize, out randomizeEntry))
            {
                randomizeEntry.Default = value;
                randomizeEntry.Current = value;
            }
            else
            {
                Randomizes[randomize] = new Randomize { Default = value, Current = value };
            }
        }

        public static void SetRandomizeMin(float value, OrbwalkingRandomize randomize)
        {
            Randomize randomizeEntry;
            if (Randomizes.TryGetValue(randomize, out randomizeEntry))
            {
                randomizeEntry.Min = value;
            }
            else
            {
                Randomizes[randomize] = new Randomize { Min = value };
            }
        }

        public static void SetRandomizeMax(float value, OrbwalkingRandomize randomize)
        {
            Randomize randomizeEntry;
            if (Randomizes.TryGetValue(randomize, out randomizeEntry))
            {
                randomizeEntry.Max = value;
            }
            else
            {
                Randomizes[randomize] = new Randomize { Max = value };
            }
        }

        public static void SetRandomizeProbability(float value, OrbwalkingRandomize randomize)
        {
            Randomize randomizeEntry;
            if (Randomizes.TryGetValue(randomize, out randomizeEntry))
            {
                randomizeEntry.Probability = value;
            }
            else
            {
                Randomizes[randomize] = new Randomize { Probability = value };
            }
        }

        public static void SetRandomizeEnabled(bool value, OrbwalkingRandomize randomize)
        {
            Randomize randomizeEntry;
            if (Randomizes.TryGetValue(randomize, out randomizeEntry))
            {
                randomizeEntry.Enabled = value;
            }
            else
            {
                Randomizes[randomize] = new Randomize { Enabled = value };
            }
        }

        private static void SetRandomizeCurrent(Randomize randomize)
        {
            if (randomize.Enabled && Random.Next(0, 101) >= 100 - randomize.Probability)
            {
                if (randomize.Default > 0)
                {
                    var min = randomize.Default / 100f * randomize.Min;
                    var max = randomize.Default / 100f * randomize.Max;
                    randomize.Current = Random.Next(
                        (int) Math.Floor(Math.Min(min, max)), (int) Math.Ceiling(Math.Max(min, max)) + 1);
                }
                else
                {
                    randomize.Current = 0;
                }
            }
            else
            {
                randomize.Current = randomize.Default > 0
                    ? Random.Next(
                        (int) Math.Floor(randomize.Default * (randomize.Default >= 50 ? 0.95f : 0.9f)),
                        (int) Math.Ceiling(randomize.Default * (randomize.Default >= 50 ? 1.05f : 1.1f)) + 1)
                    : randomize.Default;
            }
        }

        /// <summary>
        ///     Sets the minimum orbwalk distance.
        /// </summary>
        /// <param name="d">The d.</param>
        public static void SetMinimumOrbwalkDistance(float d)
        {
            _minDistance = d;
        }

        /// <summary>
        ///     Gets the last move time.
        /// </summary>
        /// <returns>System.Single.</returns>
        public static float GetLastMoveTime()
        {
            return LastMoveCommandT;
        }

        /// <summary>
        ///     Gets the last move position.
        /// </summary>
        /// <returns>Vector3.</returns>
        public static Vector3 GetLastMovePosition()
        {
            return LastMoveCommandPosition;
        }

        /// <summary>
        ///     Moves to the position.
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
                    LastMoveCommandT = LeagueSharp.Common.Utils.GameTimeTickCount - 70;
                }
                return;
            }

            var point = position;

            if (Player.Distance(point, true) < 150 * 150)
            {
                point = playerPosition.Extend(
                    position, randomizeMinDistance ? (Random.NextFloat(0.6f, 1) + 0.2f) * _minDistance : _minDistance);
            }
            var angle = 0f;
            var currentPath = Player.GetWaypoints();
            if (currentPath.Count > 1 && currentPath.PathLength() > 100)
            {
                var movePath = Player.GetPath(point);

                if (movePath.Length > 1)
                {
                    var v1 = currentPath[1] - currentPath[0];
                    var v2 = movePath[1] - movePath[0];
                    angle = v1.AngleBetween(v2.To2D());
                    var distance = movePath.Last().To2D().Distance(currentPath.Last(), true);

                    if ((angle < 10 && distance < 500 * 500) || distance < 50 * 50)
                    {
                        return;
                    }
                }
            }

            if (LeagueSharp.Common.Utils.GameTimeTickCount - LastMoveCommandT < 70 + Math.Min(60, Game.Ping) &&
                !overrideTimer && angle < 60)
            {
                return;
            }

            if (angle >= 60 && LeagueSharp.Common.Utils.GameTimeTickCount - LastMoveCommandT < 60)
            {
                return;
            }

            var randomize = Randomizes[OrbwalkingRandomize.Move];
            if (LeagueSharp.Common.Utils.GameTimeTickCount - LastMoveCommandT < randomize.Current && !overrideTimer &&
                angle <= 80)
            {
                return;
            }
            SetRandomizeCurrent(randomize);

            Player.IssueOrder(GameObjectOrder.MoveTo, point);
            LastMoveCommandPosition = point;
            LastMoveCommandT = LeagueSharp.Common.Utils.GameTimeTickCount;
        }

        /// <summary>
        ///     Orbwalks a target while moving to Position.
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
            if (LeagueSharp.Common.Utils.GameTimeTickCount - LastAttackCommandT < 70 + Math.Min(60, Game.Ping))
            {
                return;
            }

            try
            {
                var randomize = Randomizes[OrbwalkingRandomize.Attack];
                if (target.IsValidTarget() && CanAttack(randomize.Current))
                {
                    SetRandomizeCurrent(randomize);
                    DisableNextAttack = false;
                    FireBeforeAttack(target);

                    if (!DisableNextAttack)
                    {
                        if (!NoCancelChamps.Contains(ChampionName))
                        {
                            _missileLaunched = false;
                        }

                        if (Player.IssueOrder(GameObjectOrder.AttackUnit, target))
                        {
                            LastAttackCommandT = LeagueSharp.Common.Utils.GameTimeTickCount;
                            _lastTarget = target;
                        }

                        return;
                    }
                }

                if (CanMove(extraWindup))
                {
                    if (Orbwalker.LimitAttackSpeed && (Player.AttackDelay < 1 / 2.6f) && _autoattackCounter % 3 != 0 &&
                        !CanMove(500, true))
                    {
                        return;
                    }
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

        /// <summary>
        ///     Fired when the spellbook stops casting a spell.
        /// </summary>
        /// <param name="spellbook">The spellbook.</param>
        /// <param name="args">The <see cref="SpellbookStopCastEventArgs" /> instance containing the event data.</param>
        private static void SpellbookOnStopCast(Spellbook spellbook, SpellbookStopCastEventArgs args)
        {
            if (spellbook.Owner.IsValid && spellbook.Owner.IsMe && args.DestroyMissile && args.StopAnimation)
            {
                ResetAutoAttackTimer();
            }
        }

        /// <summary>
        ///     Fired when an auto attack is fired.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        private static void Obj_AI_Base_OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                if (Game.Ping <= 30) //First world problems kappa
                {
                    Utility.DelayAction.Add(30, () => Obj_AI_Base_OnDoCast_Delayed(sender, args));
                    return;
                }

                Obj_AI_Base_OnDoCast_Delayed(sender, args);
            }
        }

        /// <summary>
        ///     Fired 30ms after an auto attack is launched.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        private static void Obj_AI_Base_OnDoCast_Delayed(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (IsAutoAttackReset(args.SData.Name))
            {
                ResetAutoAttackTimer();
            }

            if (IsAutoAttack(args.SData.Name))
            {
                FireAfterAttack(sender, args.Target as AttackableUnit);
                _missileLaunched = true;
            }
        }

        /// <summary>
        ///     Handles the <see cref="E:ProcessSpell" /> event.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="spell">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        private static void OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs spell)
        {
            try
            {
                var spellName = spell.SData.Name;

                if (unit.IsMe && IsAutoAttackReset(spellName) && Math.Abs(spell.SData.SpellCastTime) <= 0)
                {
                    ResetAutoAttackTimer();
                }

                if (!IsAutoAttack(spellName))
                {
                    return;
                }

                if (unit.IsMe &&
                    (spell.Target is Obj_AI_Base || spell.Target is Obj_BarracksDampener || spell.Target is Obj_HQ))
                {
                    LastAaTick = LeagueSharp.Common.Utils.GameTimeTickCount - Game.Ping / 2;
                    _missileLaunched = false;
                    LastMoveCommandT = 0;
                    _autoattackCounter++;

                    var target = spell.Target as Obj_AI_Base;
                    if (target != null && target.IsValid)
                    {
                        FireOnTargetSwitch(target);
                        _lastTarget = target;
                    }
                }

                FireOnAttack(unit, _lastTarget);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public class Randomize
        {
            public float Default { get; set; }
            public float Min { get; set; }
            public float Max { get; set; }
            public float Probability { get; set; }
            public bool Enabled { get; set; }
            public float Current { get; set; }
        }

        /// <summary>
        ///     The before attack event arguments.
        /// </summary>
        public class BeforeAttackEventArgs : EventArgs
        {
            /// <summary>
            ///     <c>true</c> if the orbwalker should continue with the attack.
            /// </summary>
            private bool _process = true;

            /// <summary>
            ///     The target
            /// </summary>
            public AttackableUnit Target;

            /// <summary>
            ///     The unit
            /// </summary>
            public Obj_AI_Base Unit = ObjectManager.Player;

            /// <summary>
            ///     Gets or sets a value indicating whether this <see cref="BeforeAttackEventArgs" /> should continue with the attack.
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
        ///     This class allows you to add an instance of "Orbwalker" to your assembly in order to control the orbwalking in an
        ///     easy way.
        /// </summary>
        public class Orbwalker
        {
            /// <summary>
            ///     The lane clear wait time modifier.
            /// </summary>
            private const float LaneClearWaitTimeMod = 2f;

            /// <summary>
            ///     The configuration
            /// </summary>
            private static Menu _config;

            /// <summary>
            ///     The instances of the orbwalker.
            /// </summary>
            public static List<Orbwalker> Instances = new List<Orbwalker>();

            private readonly Dictionary<string, bool> _attackableObjects = new Dictionary<string, bool>();
            private readonly string[] _attackleCloneChamps = { "Shaco", "LeBlanc", "Wukong" };

            private readonly string[] _attackleObjectChamps =
            {
                "Zyra", "Heimerdinger", "Shaco", "Teemo", "Gangplank",
                "Annie", "Yorick", "Mordekaiser", "Malzahar", "Elise"
            };

            /// <summary>
            ///     The player
            /// </summary>
            private readonly Obj_AI_Hero _player;

            /// <summary>
            ///     The name of the CustomMode if it is set.
            /// </summary>
            private string _customModeName;

            /// <summary>
            ///     The forced target
            /// </summary>
            private Obj_AI_Base _forcedTarget;

            /// <summary>
            ///     The orbalker mode
            /// </summary>
            private OrbwalkingMode _mode = OrbwalkingMode.None;

            /// <summary>
            ///     The orbwalking point
            /// </summary>
            private Vector3 _orbwalkingPoint;

            /// <summary>
            ///     The previous minion the orbwalker was targeting.
            /// </summary>
            private Obj_AI_Minion _prevMinion;

            /// <summary>
            ///     Initializes a new instance of the <see cref="Orbwalker" /> class.
            /// </summary>
            /// <param name="attachToMenu">The menu the orbwalker should attach to.</param>
            public Orbwalker(Menu attachToMenu)
            {
                GameObjects.Initialize();
                _config = attachToMenu;

                /* Drawings menu */
                var drawings = new Menu("Drawings", "drawings");
                drawings.AddItem(
                    new MenuItem("CircleThickness", "Circle Thickness").SetShared().SetValue(new Slider(4, 1, 10)));
                drawings.AddItem(
                    new MenuItem("AACircle", "AA Circle").SetShared()
                        .SetValue(new Circle(true, Color.FromArgb(155, 255, 255, 0))));
                drawings.AddItem(
                    new MenuItem("AACircle2", "Enemy AA Circle").SetShared()
                        .SetValue(new Circle(false, Color.FromArgb(155, 255, 255, 0))));
                drawings.AddItem(
                    new MenuItem("HoldZone", "Hold Zone").SetShared()
                        .SetValue(new Circle(false, Color.FromArgb(155, 255, 255, 0))));
                drawings.AddItem(
                    new MenuItem("LastHitHelper", "Last Hit Helper").SetShared()
                        .SetValue(new Circle(false, Color.LimeGreen)));
                _config.AddSubMenu(drawings);

                /* Attackables menu */
                var attackables = new Menu("Attackable Objects", "Attackables");
                attackables.AddItem(new MenuItem("AttackWard", "Ward").SetShared().SetValue(true)).ValueChanged +=
                    (sender, args) => SetAttackableObject("ward", args.GetNewValue<bool>());
                attackables.AddItem(new MenuItem("AttackZyra", "Zyra Plant").SetShared().SetValue(true)).ValueChanged +=
                    (sender, args) => SetAttackableObject("zyra", args.GetNewValue<bool>());
                attackables.AddItem(new MenuItem("AttackHeimerdinger", "Heimer Turret").SetShared().SetValue(true))
                    .ValueChanged += (sender, args) => SetAttackableObject("heimerdinger", args.GetNewValue<bool>());
                attackables.AddItem(new MenuItem("AttackShaco", "Shaco Box").SetShared().SetValue(true)).ValueChanged +=
                    (sender, args) => SetAttackableObject("shaco", args.GetNewValue<bool>());
                attackables.AddItem(new MenuItem("AttackTeemo", "Teemo Shroom").SetShared().SetValue(true)).ValueChanged
                    += (sender, args) => SetAttackableObject("teemo", args.GetNewValue<bool>());
                attackables.AddItem(new MenuItem("AttackGangplank", "Gangplank Barrel").SetShared().SetValue(true))
                    .ValueChanged += (sender, args) => SetAttackableObject("gangplank", args.GetNewValue<bool>());
                attackables.AddItem(new MenuItem("AttackAnnie", "Annie Tibbers").SetShared().SetValue(true))
                    .ValueChanged += (sender, args) => SetAttackableObject("annie", args.GetNewValue<bool>());
                attackables.AddItem(new MenuItem("AttackYorick", "Yorick Ghost").SetShared().SetValue(true))
                    .ValueChanged += (sender, args) => SetAttackableObject("yorick", args.GetNewValue<bool>());
                attackables.AddItem(new MenuItem("AttackMalzahar", "Malzahar Voidling").SetShared().SetValue(true))
                    .ValueChanged += (sender, args) => SetAttackableObject("malzahar", args.GetNewValue<bool>());
                attackables.AddItem(new MenuItem("AttackElise", "Elise Spiderling").SetShared().SetValue(true))
                    .ValueChanged += (sender, args) => SetAttackableObject("elise", args.GetNewValue<bool>());
                attackables.AddItem(new MenuItem("AttackMordekaiser", "Mordekaiser Ghost").SetShared().SetValue(true))
                    .ValueChanged += (sender, args) => SetAttackableObject("mordekaiser", args.GetNewValue<bool>());
                attackables.AddItem(new MenuItem("AttackClone", "Clones").SetShared().SetValue(true)).ValueChanged +=
                    (sender, args) => SetAttackableObject("clone", args.GetNewValue<bool>());
                _config.AddSubMenu(attackables);

                /* Delays menu */
                var delays = new Menu("Delays", "Delays");
                delays.AddItem(new MenuItem("ExtraWindup", "Windup").SetShared().SetValue(new Slider(70, 0, 200)));
                delays.AddItem(new MenuItem("MovementDelay", "Movement").SetShared().SetValue(new Slider(70, 0, 250)))
                    .ValueChanged +=
                    (sender, args) => SetRandomize(args.GetNewValue<Slider>().Value, OrbwalkingRandomize.Move);
                delays.AddItem(new MenuItem("AttackDelay", "Attack").SetShared().SetValue(new Slider(0, 0, 250)))
                    .ValueChanged +=
                    (sender, args) => SetRandomize(args.GetNewValue<Slider>().Value, OrbwalkingRandomize.Attack);
                delays.AddItem(new MenuItem("FarmDelay", "Farm").SetShared().SetValue(new Slider(25, 0, 200)));
                _config.AddSubMenu(delays);

                /* Randomize: Movement menu */
                var randomizeMovement = new Menu("Movement Humanizer", "Movement");
                randomizeMovement.AddItem(
                    new MenuItem("MovementMin", "Min. Multi %").SetShared().SetValue(new Slider(170, 100, 300)))
                    .ValueChanged +=
                    (sender, args) => SetRandomizeMin(args.GetNewValue<Slider>().Value, OrbwalkingRandomize.Move);
                randomizeMovement.AddItem(
                    new MenuItem("MovementMax", "Max. Multi %").SetShared().SetValue(new Slider(220, 100, 300)))
                    .ValueChanged +=
                    (sender, args) => SetRandomizeMax(args.GetNewValue<Slider>().Value, OrbwalkingRandomize.Move);
                randomizeMovement.AddItem(
                    new MenuItem("MovementProbability", "Probability %").SetShared().SetValue(new Slider(30)))
                    .ValueChanged +=
                    (sender, args) =>
                        SetRandomizeProbability(args.GetNewValue<Slider>().Value, OrbwalkingRandomize.Move);
                randomizeMovement.AddItem(new MenuItem("MovementEnabled", "Enabled").SetShared().SetValue(false))
                    .ValueChanged +=
                    (sender, args) => SetRandomizeEnabled(args.GetNewValue<bool>(), OrbwalkingRandomize.Move);
                _config.AddSubMenu(randomizeMovement);

                /* Randomize: Attacks menu */
                var randomizeAttack = new Menu("Attacks Humanizer", "Attack");
                randomizeAttack.AddItem(
                    new MenuItem("AttackMin", "Min. Multi %").SetShared().SetValue(new Slider(170, 100, 300)))
                    .ValueChanged +=
                    (sender, args) => SetRandomizeMin(args.GetNewValue<Slider>().Value, OrbwalkingRandomize.Attack);
                randomizeAttack.AddItem(
                    new MenuItem("AttackMax", "Max. Multi %").SetShared().SetValue(new Slider(220, 100, 300)))
                    .ValueChanged +=
                    (sender, args) => SetRandomizeMax(args.GetNewValue<Slider>().Value, OrbwalkingRandomize.Attack);
                randomizeAttack.AddItem(
                    new MenuItem("AttackProbability", "Probability %").SetShared().SetValue(new Slider(30)))
                    .ValueChanged +=
                    (sender, args) =>
                        SetRandomizeProbability(args.GetNewValue<Slider>().Value, OrbwalkingRandomize.Attack);
                randomizeAttack.AddItem(new MenuItem("AttackEnabled", "Enabled").SetShared().SetValue(false))
                    .ValueChanged +=
                    (sender, args) => SetRandomizeEnabled(args.GetNewValue<bool>(), OrbwalkingRandomize.Attack);
                _config.AddSubMenu(randomizeAttack);

                /* Misc options */
                var misc = new Menu("Miscellaneous", "Misc");
                misc.AddItem(
                    new MenuItem("HoldPosRadius", "Hold Position Radius").SetShared().SetValue(new Slider(0, 0, 250)));
                misc.AddItem(new MenuItem("PriorizeFarm", "Prioritize Farm Over Harass").SetShared().SetValue(true));
                misc.AddItem(new MenuItem("Smallminionsprio", "Focus Small Jungle First").SetShared().SetValue(false));
                misc.AddItem(
                    new MenuItem("LimitAttackSpeed", "Don't Kite if Attack Speed > 2.5").SetShared().SetValue(false));
                misc.AddItem(
                    new MenuItem("FocusMinionsOverTurrets", "Focus Minions Over Objectives").SetShared()
                        .SetValue(new KeyBind('M', KeyBindType.Toggle)));
                misc.AddItem(new MenuItem("MissileCheck", "Use Missile Check").SetShared().SetValue(true));

                _config.AddSubMenu(misc);

                /*Load the menu*/
                _config.AddItem(
                    new MenuItem("Orbwalk", "Combo").SetShared().SetValue(new KeyBind(32, KeyBindType.Press)));
                _config.AddItem(
                    new MenuItem("Orbwalk2", "Combo Alternate").SetShared().SetValue(new KeyBind(32, KeyBindType.Press)));
                _config.AddItem(
                    new MenuItem("StillCombo", "Combo Without Moving").SetShared()
                        .SetValue(new KeyBind('N', KeyBindType.Press))).ValueChanged +=
                    (sender, args) => Move = !args.GetNewValue<KeyBind>().Active;
                _config.AddItem(new MenuItem("Farm", "Mixed").SetShared().SetValue(new KeyBind('C', KeyBindType.Press)));
                _config.AddItem(
                    new MenuItem("LaneClear", "Lane Clear").SetShared().SetValue(new KeyBind('V', KeyBindType.Press)));
                _config.AddItem(
                    new MenuItem("LastHit", "Last Hit").SetShared().SetValue(new KeyBind('X', KeyBindType.Press)));
                _config.AddItem(new MenuItem("Flee", "Flee").SetShared().SetValue(new KeyBind('Z', KeyBindType.Press)));

                _player = ObjectManager.Player;

                SetRandomize(_config.Item("MovementDelay").GetValue<Slider>().Value, OrbwalkingRandomize.Move);
                SetRandomizeMin(_config.Item("MovementMin").GetValue<Slider>().Value, OrbwalkingRandomize.Move);
                SetRandomizeMax(_config.Item("MovementMax").GetValue<Slider>().Value, OrbwalkingRandomize.Move);
                SetRandomizeProbability(
                    _config.Item("MovementProbability").GetValue<Slider>().Value, OrbwalkingRandomize.Move);
                SetRandomizeEnabled(_config.Item("MovementEnabled").GetValue<bool>(), OrbwalkingRandomize.Move);

                SetRandomize(_config.Item("AttackDelay").GetValue<Slider>().Value, OrbwalkingRandomize.Attack);
                SetRandomizeMin(_config.Item("AttackMin").GetValue<Slider>().Value, OrbwalkingRandomize.Attack);
                SetRandomizeMax(_config.Item("AttackMax").GetValue<Slider>().Value, OrbwalkingRandomize.Attack);
                SetRandomizeProbability(
                    _config.Item("AttackProbability").GetValue<Slider>().Value, OrbwalkingRandomize.Attack);
                SetRandomizeEnabled(_config.Item("AttackEnabled").GetValue<bool>(), OrbwalkingRandomize.Attack);

                CustomEvents.Game.OnGameLoad += GameOnOnGameLoad;
                Game.OnUpdate += GameOnOnGameUpdate;
                Drawing.OnDraw += DrawingOnOnDraw;

                Instances.Add(this);
            }

            /// <summary>
            ///     Gets the farm delay.
            /// </summary>
            /// <value>The farm delay.</value>
            private int FarmDelay
            {
                get { return _config.Item("FarmDelay").GetValue<Slider>().Value; }
            }

            /// <summary>
            ///     Gets a value indicating whether the orbwalker is orbwalking by checking the missiles.
            /// </summary>
            /// <value><c>true</c> if the orbwalker is orbwalking by checking the missiles; otherwise, <c>false</c>.</value>
            public static bool MissileCheck
            {
                get { return _config.Item("MissileCheck").GetValue<bool>(); }
            }

            public static bool LimitAttackSpeed
            {
                get { return _config.Item("LimitAttackSpeed").GetValue<bool>(); }
            }


            public int HoldAreaRadius
            {
                get { return _config.Item("HoldPosRadius").GetValue<Slider>().Value; }
            }

            /// <summary>
            ///     Gets or sets the active mode.
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

                    if (_config.Item("Orbwalk").GetValue<KeyBind>().Active ||
                        _config.Item("Orbwalk2").GetValue<KeyBind>().Active)
                    {
                        return OrbwalkingMode.Combo;
                    }

                    if (_config.Item("StillCombo").GetValue<KeyBind>().Active)
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

                    if (_config.Item("Flee").GetValue<KeyBind>().Active)
                    {
                        return OrbwalkingMode.Flee;
                    }

                    if (_config.Item(_customModeName) != null &&
                        _config.Item(_customModeName).GetValue<KeyBind>().Active)
                    {
                        return OrbwalkingMode.CustomMode;
                    }

                    return OrbwalkingMode.None;
                }
                set { _mode = value; }
            }

            private void GameOnOnGameLoad(EventArgs args)
            {
                var clone = false;
                foreach (var enemy in GameObjects.EnemyHeroes)
                {
                    if (_attackleObjectChamps.Any(v => v.Equals(enemy.ChampionName, StringComparison.OrdinalIgnoreCase)))
                    {
                        _attackableObjects.Add(
                            enemy.ChampionName.ToLower(), _config.Item("Attack" + enemy.ChampionName).GetValue<bool>());
                    }
                    if (!clone &&
                        _attackleCloneChamps.Any(v => v.Equals(enemy.ChampionName, StringComparison.OrdinalIgnoreCase)))
                    {
                        clone = true;
                    }
                }
                if (clone)
                {
                    _attackableObjects.Add("clone", _config.Item("AttackClone").GetValue<bool>());
                }
                _attackableObjects.Add("ward", _config.Item("AttackWard").GetValue<bool>());
            }

            /// <summary>
            ///     Determines if a target is in auto attack range.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <returns><c>true</c> if a target is in auto attack range, <c>false</c> otherwise.</returns>
            // ReSharper disable once MemberHidesStaticFromOuterClass
            public virtual bool InAutoAttackRange(AttackableUnit target)
            {
                return Orbwalking.InAutoAttackRange(target);
            }

            /// <summary>
            ///     Registers the Custom Mode of the Orbwalker. Useful for adding a flee mode and such.
            /// </summary>
            /// <param name="name">The name of the mode Ex. "Myassembly.FleeMode" </param>
            /// <param name="displayname">The name of the mode in the menu. Ex. Flee</param>
            /// <param name="key">The default key for this mode.</param>
            public virtual void RegisterCustomMode(string name, string displayname, uint key)
            {
                _customModeName = name;
                if (_config.Item(name) == null)
                {
                    _config.AddItem(
                        new MenuItem(name, displayname).SetShared().SetValue(new KeyBind(key, KeyBindType.Press)));
                }
            }

            /// <summary>
            ///     Enables or disables the auto-attacks.
            /// </summary>
            /// <param name="b">if set to <c>true</c> the orbwalker will attack units.</param>
            public void SetAttack(bool b)
            {
                Attack = b;
            }

            /// <summary>
            ///     Enables or disables the movement.
            /// </summary>
            /// <param name="b">if set to <c>true</c> the orbwalker will move.</param>
            public void SetMovement(bool b)
            {
                Move = b;
            }

            /// <summary>
            ///     Forces the orbwalker to attack the set target if valid and in range.
            /// </summary>
            /// <param name="target">The target.</param>
            public void ForceTarget(Obj_AI_Base target)
            {
                _forcedTarget = target;
            }

            /// <summary>
            ///     Returns the currently forced target.
            /// </summary>
            public Obj_AI_Base ForcedTarget()
            {
                return _forcedTarget;
            }

            /// <summary>
            ///     Forces the orbwalker to move to that point while orbwalking (Game.CursorPos by default).
            /// </summary>
            /// <param name="point">The point.</param>
            public void SetOrbwalkingPoint(Vector3 point)
            {
                _orbwalkingPoint = point;
            }

            private void SetAttackableObject(string name, bool value)
            {
                if (_attackableObjects.ContainsKey(name.ToLower()))
                {
                    _attackableObjects[name.ToLower()] = value;
                }
            }

            private bool IsAttackableObject(string name)
            {
                return _attackableObjects.ContainsKey(name.ToLower()) && _attackableObjects[name.ToLower()];
            }

            /// <summary>
            ///     Determines if the orbwalker should wait before attacking a minion.
            /// </summary>
            /// <returns><c>true</c> if the orbwalker should wait before attacking a minion, <c>false</c> otherwise.</returns>
            public bool ShouldWait()
            {
                return
                    LeagueSharp.Common.MinionManager.GetMinions(
                        Player.Position, float.MaxValue, LeagueSharp.Common.MinionTypes.All, LeagueSharp.Common.MinionTeam.Enemy, LeagueSharp.Common.MinionOrderTypes.None)
                        .Any(
                            minion =>
                                InAutoAttackRange(minion) &&
                                HealthPrediction.LaneClearHealthPrediction(
                                    minion, (int) (Player.AttackDelay * 1000 * LaneClearWaitTimeMod), FarmDelay) <=
                                Player.GetAutoAttackDamage(minion));
            }

            private bool ShouldWaitUnderTurret(Obj_AI_Minion noneKillableMinion)
            {
                return
                    LeagueSharp.Common.MinionManager.GetMinions(
                        Player.Position, float.MaxValue, LeagueSharp.Common.MinionTypes.All, LeagueSharp.Common.MinionTeam.Enemy, LeagueSharp.Common.MinionOrderTypes.None)
                        .Any(
                            minion =>
                                (noneKillableMinion == null || noneKillableMinion.NetworkId != minion.NetworkId) &&
                                minion.IsValidTarget() && minion.Team != GameObjectTeam.Neutral &&
                                InAutoAttackRange(minion) && LeagueSharp.Common.MinionManager.IsMinion(minion) &&
                                HealthPrediction.LaneClearHealthPrediction(
                                    minion,
                                    (int)
                                        (Player.AttackDelay * 1000 +
                                         (Player.IsMelee
                                             ? Player.AttackCastDelay * 1000
                                             : Player.AttackCastDelay * 1000 +
                                               1000 * (Player.AttackRange + 2 * Player.BoundingRadius) /
                                               Player.BasicAttack.MissileSpeed)), FarmDelay) <=
                                Player.GetAutoAttackDamage(minion));
            }

            /// <summary>
            ///     Gets the target.
            /// </summary>
            /// <returns>AttackableUnit.</returns>
            public virtual AttackableUnit GetTarget()
            {
                AttackableUnit result = null;

                if ((ActiveMode == OrbwalkingMode.Mixed || ActiveMode == OrbwalkingMode.LaneClear) &&
                    !_config.Item("PriorizeFarm").GetValue<bool>())
                {
                    var target = TargetSelector.GetTarget(-1, DamageType.Physical);
                    if (target != null && InAutoAttackRange(target))
                    {
                        return target;
                    }
                }

                var minions = new List<Obj_AI_Minion>();
                if (ActiveMode != OrbwalkingMode.None && ActiveMode != OrbwalkingMode.Flee)
                {
                    minions = GetAttackableObjects(ActiveMode != OrbwalkingMode.Combo);
                }

                /*Killable Minion*/
                if (ActiveMode == OrbwalkingMode.LaneClear || ActiveMode == OrbwalkingMode.Mixed ||
                    ActiveMode == OrbwalkingMode.LastHit)
                {
                    var minionList =
                        minions.Where(minion => minion.IsValidTarget() && InAutoAttackRange(minion))
                            .OrderByDescending(minion => minion.CharData.BaseSkinName.Contains("Siege"))
                            .ThenBy(minion => minion.CharData.BaseSkinName.Contains("Super"))
                            .ThenBy(minion => minion.Health)
                            .ThenByDescending(minion => minion.MaxHealth);

                    foreach (var minion in minionList)
                    {
                        var t = (int) (_player.AttackCastDelay * 1000) - 100 + Game.Ping / 2 +
                                1000 * (int) Math.Max(0, _player.Distance(minion) - _player.BoundingRadius) /
                                (int) GetMyProjectileSpeed();
                        if (minion.MaxHealth <= 10)
                        {
                            if (minion.Health <= 1)
                            {
                                return minion;
                            }
                        }
                        else
                        {
                            var predHealth = HealthPrediction.GetHealthPrediction(minion, t, FarmDelay);
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
                if (ActiveMode == OrbwalkingMode.LaneClear &&
                    (!_config.Item("FocusMinionsOverTurrets").GetValue<KeyBind>().Active || !minions.Any()))
                {
                    /* turrets */
                    foreach (var turret in
                        GameObjects.EnemyTurrets.Where(t => t.IsValidTarget() && InAutoAttackRange(t)))
                    {
                        return turret;
                    }

                    /* inhibitor */
                    foreach (var inhib in
                        GameObjects.EnemyInhibitors.Where(i => i.IsValidTarget() && InAutoAttackRange(i)))
                    {
                        return inhib;
                    }

                    /* nexus */
                    if (GameObjects.EnemyNexus != null && GameObjects.EnemyNexus.IsValidTarget() &&
                        InAutoAttackRange(GameObjects.EnemyNexus))
                    {
                        return GameObjects.EnemyNexus;
                    }
                }

                /*Champions*/
                if (ActiveMode != OrbwalkingMode.LastHit)
                {
                    var target = TargetSelector.GetTarget(-1, DamageType.Physical);
                    if (target.IsValidTarget() && InAutoAttackRange(target))
                    {
                        return target;
                    }
                }

                /*Jungle minions*/
                if (ActiveMode == OrbwalkingMode.LaneClear || ActiveMode == OrbwalkingMode.Mixed)
                {
                    var jminions = minions.Where(m => m.Team == GameObjectTeam.Neutral);
                    result = _config.Item("Smallminionsprio").GetValue<bool>()
                        ? jminions.MinOrDefault(mob => mob.MaxHealth)
                        : jminions.MaxOrDefault(mob => mob.MaxHealth);
                    if (result != null)
                    {
                        return result;
                    }
                }

                /* UnderTurret Farming */
                if (ActiveMode == OrbwalkingMode.LaneClear || ActiveMode == OrbwalkingMode.Mixed ||
                    ActiveMode == OrbwalkingMode.LastHit)
                {
                    Obj_AI_Minion farmUnderTurretMinion = null;
                    Obj_AI_Minion noneKillableMinion = null;
                    // return all the minions underturret in auto attack range
                    var turretMinions =
                        minions.Where(
                            minion => minion.MaxHealth > 10 && InAutoAttackRange(minion) && minion.UnderAllyTurret())
                            .OrderByDescending(minion => minion.CharData.BaseSkinName.Contains("Siege"))
                            .ThenBy(minion => minion.CharData.BaseSkinName.Contains("Super"))
                            .ThenByDescending(minion => minion.MaxHealth);
                    if (turretMinions.Any())
                    {
                        // get the turret aggro minion
                        var turretMinion =
                            turretMinions.FirstOrDefault(
                                minion => (minion != null) && HealthPrediction.HasTurretAggro(minion));

                        if (turretMinion != null)
                        {
                            var hpLeftBeforeDie = 0;
                            var hpLeft = 0;
                            var turretAttackCount = 0;
                            var turret = HealthPrediction.GetAggroTurret(turretMinion);
                            if (turret != null)
                            {
                                var turretStarTick = HealthPrediction.TurretAggroStartTick(turretMinion);
                                // from healthprediction (don't blame me :S)
                                var turretLandTick = turretStarTick + (int) (turret.AttackCastDelay * 1000) +
                                                     1000 *
                                                     Math.Max(
                                                         0,
                                                         (int) (turretMinion.Distance(turret) - turret.BoundingRadius)) /
                                                     (int) (turret.BasicAttack.MissileSpeed + 70);
                                // calculate the HP before try to balance it
                                for (float i = turretLandTick + 50;
                                    i < turretLandTick + 3 * turret.AttackDelay * 1000 + 50;
                                    i = i + turret.AttackDelay * 1000)
                                {
                                    var time = (int) i - LeagueSharp.Common.Utils.GameTimeTickCount + Game.Ping / 2;
                                    var predHp =
                                        (int)
                                            HealthPrediction.LaneClearHealthPrediction(
                                                turretMinion, time > 0 ? time : 0);
                                    if (predHp > 0)
                                    {
                                        hpLeft = predHp;
                                        turretAttackCount += 1;
                                        continue;
                                    }
                                    hpLeftBeforeDie = hpLeft;
                                    hpLeft = 0;
                                    break;
                                }
                                // calculate the hits is needed and possibilty to balance
                                if (hpLeft == 0 && turretAttackCount != 0 && hpLeftBeforeDie != 0)
                                {
                                    var damage = (int) Player.GetAutoAttackDamage(turretMinion, true);
                                    var hits = hpLeftBeforeDie / damage;
                                    var timeBeforeDie = turretLandTick +
                                                        (turretAttackCount + 1) * (int) (turret.AttackDelay * 1000) -
                                                        LeagueSharp.Common.Utils.GameTimeTickCount;
                                    var timeUntilAttackReady = LastAaTick + (int) (Player.AttackDelay * 1000) >
                                                               LeagueSharp.Common.Utils.GameTimeTickCount +
                                                               Game.Ping / 2 + 25
                                        ? LastAaTick + (int) (Player.AttackDelay * 1000) -
                                          (LeagueSharp.Common.Utils.GameTimeTickCount + Game.Ping / 2 + 25)
                                        : 0;
                                    var timeToLandAttack = Player.IsMelee
                                        ? Player.AttackCastDelay * 1000
                                        : Player.AttackCastDelay * 1000 +
                                          1000 * Math.Max(0, turretMinion.Distance(Player) - Player.BoundingRadius) /
                                          Player.BasicAttack.MissileSpeed;
                                    if (hits >= 1 &&
                                        hits * Player.AttackDelay * 1000 + timeUntilAttackReady + timeToLandAttack <
                                        timeBeforeDie)
                                    {
                                        farmUnderTurretMinion = turretMinion;
                                    }
                                    else if (hits >= 1 &&
                                             hits * Player.AttackDelay * 1000 + timeUntilAttackReady + timeToLandAttack >
                                             timeBeforeDie)
                                    {
                                        noneKillableMinion = turretMinion;
                                    }
                                }
                                else if (hpLeft == 0 && turretAttackCount == 0 && hpLeftBeforeDie == 0)
                                {
                                    noneKillableMinion = turretMinion;
                                }
                                if (ShouldWaitUnderTurret(noneKillableMinion))
                                {
                                    return null;
                                }
                                if (farmUnderTurretMinion != null)
                                {
                                    return farmUnderTurretMinion;
                                }
                                // balance other minions
                                return
                                    (from minion in
                                        turretMinions.Where(
                                            x =>
                                                x.NetworkId != turretMinion.NetworkId &&
                                                !HealthPrediction.HasMinionAggro(x))
                                        where
                                            (int) minion.Health % (int) turret.GetAutoAttackDamage(minion, true) >
                                            (int) Player.GetAutoAttackDamage(minion)
                                        select minion).FirstOrDefault();
                            }
                        }
                        else
                        {
                            if (ShouldWaitUnderTurret(null))
                            {
                                return null;
                            }
                            // balance other minions
                            return
                                (from minion in
                                    turretMinions.Where(x => (x != null) && !HealthPrediction.HasMinionAggro(x))
                                    let turret =
                                        GameObjects.AllyTurrets.FirstOrDefault(
                                            x => x.IsValidTarget(950, false, minion.Position))
                                    where
                                        turret != null &&
                                        (int) minion.Health % (int) turret.GetAutoAttackDamage(minion, true) >
                                        (int) Player.GetAutoAttackDamage(minion)
                                    select minion).FirstOrDefault();
                        }
                        return null;
                    }
                }

                /*Lane Clear minions*/
                if (ActiveMode == OrbwalkingMode.LaneClear)
                {
                    if (!ShouldWait())
                    {
                        if (_prevMinion.IsValidTarget() && InAutoAttackRange(_prevMinion))
                        {
                            if (_prevMinion.MaxHealth <= 10)
                            {
                                return _prevMinion;
                            }
                            var predHealth = HealthPrediction.LaneClearHealthPrediction(
                                _prevMinion, (int) (Player.AttackDelay * 1000 * LaneClearWaitTimeMod), FarmDelay);
                            if (predHealth >= 2 * _player.GetAutoAttackDamage(_prevMinion) ||
                                Math.Abs(predHealth - _prevMinion.Health) < float.Epsilon)
                            {
                                return _prevMinion;
                            }
                        }
                        foreach (var minion in minions.Where(m => m.Team != GameObjectTeam.Neutral))
                        {
                            if (minion.MaxHealth <= 10)
                            {
                                result = minion;
                            }
                            else
                            {
                                var predHealth = HealthPrediction.LaneClearHealthPrediction(
                                    minion, (int) (Player.AttackDelay * 1000 * LaneClearWaitTimeMod), FarmDelay);
                                if (predHealth >= 2 * Player.GetAutoAttackDamage(minion) ||
                                    Math.Abs(predHealth - minion.Health) < float.Epsilon)
                                {
                                    if (result == null || minion.Health > result.Health && result.MaxHealth > 10)
                                    {
                                        result = minion;
                                    }
                                }
                            }
                        }
                        if (result != null)
                        {
                            _prevMinion = (Obj_AI_Minion) result;
                        }
                    }
                }

                if (result == null && ActiveMode == OrbwalkingMode.Combo)
                {
                    if (
                        !GameObjects.EnemyHeroes.Any(
                            e =>
                                e.IsValid && !e.IsDead && e.IsVisible &&
                                e.Distance(Player) <= GetRealAutoAttackRange(e) * 2f))
                    {
                        return minions.FirstOrDefault();
                    }
                }

                return result;
            }

            private List<Obj_AI_Minion> GetAttackableObjects(bool minion)
            {
                var targets = new List<Obj_AI_Minion>();
                var minions = new List<Obj_AI_Minion>();
                var clones = new List<Obj_AI_Minion>();

                var units = IsAttackableObject("ward")
                    ? GameObjects.EnemyMinions.Concat(GameObjects.EnemyWards)
                    : GameObjects.EnemyMinions;
                foreach (var unit in units.Where(u => u.IsValidTarget() && InAutoAttackRange(u)))
                {
                    var baseName = unit.CharData.BaseSkinName.ToLower();
                    if (minion) //minions
                    {
                        if (baseName.Contains("minion") || baseName.Contains("bilge") || baseName.Contains("bw_"))
                        {
                            minions.Add(unit);
                            continue;
                        }
                    }
                    if (IsAttackableObject("ward")) //wards
                    {
                        if (baseName.Contains("ward") || baseName.Contains("trinket"))
                        {
                            targets.Add(unit);
                            continue;
                        }
                    }
                    if (IsAttackableObject("zyra")) //zyra plant
                    {
                        if (baseName.Contains("zyra") && baseName.Contains("plant"))
                        {
                            targets.Add(unit);
                            continue;
                        }
                    }
                    if (IsAttackableObject("heimerdinger")) //heimer turret
                    {
                        if (baseName.Contains("heimert"))
                        {
                            targets.Add(unit);
                            continue;
                        }
                    }
                    if (IsAttackableObject("annie")) //annie tibber
                    {
                        if (baseName.Contains("annietibbers"))
                        {
                            targets.Add(unit);
                            continue;
                        }
                    }
                    if (IsAttackableObject("teemo")) //teemo shroom
                    {
                        if (baseName.Contains("teemomushroom"))
                        {
                            targets.Add(unit);
                            continue;
                        }
                    }
                    if (IsAttackableObject("shaco")) //shaco box
                    {
                        if (baseName.Contains("shacobox"))
                        {
                            targets.Add(unit);
                            continue;
                        }
                    }
                    if (IsAttackableObject("gangplank")) //gangplank barrel
                    {
                        if (baseName.Contains("gangplankbarrel") && unit.IsHPBarRendered)
                        {
                            targets.Add(unit);
                            continue;
                        }
                    }
                    if (IsAttackableObject("yorick")) //yorick ghouls
                    {
                        if (baseName.Contains("yorick") && baseName.Contains("ghoul"))
                        {
                            targets.Add(unit);
                            continue;
                        }
                    }
                    if (IsAttackableObject("malzahar")) //malzahar voidlings
                    {
                        if (baseName.Contains("malzaharvoidling"))
                        {
                            targets.Add(unit);
                            continue;
                        }
                    }
                    if (IsAttackableObject("elise")) //elise spiderling
                    {
                        if (baseName.Contains("elisespiderling"))
                        {
                            targets.Add(unit);
                            continue;
                        }
                    }
                    if (IsAttackableObject("clone")) //clones
                    {
                        if (baseName.Contains("shaco") || baseName.Contains("leblanc") ||
                            baseName.Contains("monkeyking"))
                        {
                            clones.Add(unit);
                            continue;
                        }
                    }
                    if (IsAttackableObject("mordekaiser")) //Mordekaiser Ghost
                    {
                        if (GameObjects.AllyHeroes.Any(e => e.CharData.BaseSkinName.ToLower().Equals(baseName)))
                        {
                            targets.Add(unit);
                        }
                    }
                }
                var finalTargets = targets;
                if (minion)
                {
                    finalTargets =
                        finalTargets.Concat(minions)
                            .Concat(GameObjects.Jungle.Where(u => u.IsValidTarget() && InAutoAttackRange(u)))
                            .ToList();
                }
                return finalTargets.Concat(clones).ToList();
            }

            /// <summary>
            ///     Fired when the game is updated.
            /// </summary>
            /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
            private void GameOnOnGameUpdate(EventArgs args)
            {
                try
                {
                    if (ActiveMode == OrbwalkingMode.None || ActiveMode == OrbwalkingMode.Flee)
                    {
                        return;
                    }

                    //Prevent canceling important spells
                    if (_player.IsCastingInterruptableSpell(true))
                    {
                        return;
                    }

                    var target = GetTarget();
                    Orbwalk(
                        target, _orbwalkingPoint.To2D().IsValid() ? _orbwalkingPoint : Game.CursorPos,
                        _config.Item("ExtraWindup").GetValue<Slider>().Value,
                        Math.Max(_config.Item("HoldPosRadius").GetValue<Slider>().Value, 30));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            /// <summary>
            ///     Fired when the game is drawn.
            /// </summary>
            /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
            private void DrawingOnOnDraw(EventArgs args)
            {
                var circleThickness = _config.Item("CircleThickness").GetValue<Slider>().Value;
                var aaCircle = _config.Item("AACircle").GetValue<Circle>();
                if (aaCircle.Active)
                {
                    Render.Circle.DrawCircle(
                        _player.Position, GetRealAutoAttackRange(null) + 65, aaCircle.Color, circleThickness);
                }

                var aaCircle2 = _config.Item("AACircle2").GetValue<Circle>();
                if (aaCircle2.Active)
                {
                    foreach (var target in
                        HeroManager.Enemies.FindAll(target => target.IsValidTarget(1175)))
                    {
                        Render.Circle.DrawCircle(
                            target.Position, GetAttackRange(target), aaCircle2.Color, circleThickness);
                    }
                }

                var holdCircle = _config.Item("HoldZone").GetValue<Circle>();
                if (holdCircle.Active)
                {
                    Render.Circle.DrawCircle(
                        _player.Position, _config.Item("HoldPosRadius").GetValue<Slider>().Value, holdCircle.Color,
                        circleThickness, true);
                }

                _config.Item("FocusMinionsOverTurrets")
                    .Permashow(_config.Item("FocusMinionsOverTurrets").GetValue<KeyBind>().Active);

                var helperCircle = _config.Item("LastHitHelper").GetValue<Circle>();
                if (helperCircle.Active)
                {
                    foreach (var minion in
                        GameObjects.EnemyMinions.Where(x => x.IsHPBarRendered && x.IsValidTarget(1000)))
                    {
                        if (minion.Health < ObjectManager.Player.GetAutoAttackDamage(minion, true))
                        {
                            Render.Circle.DrawCircle(minion.Position, 50, helperCircle.Color);
                        }
                    }
                }
            }
        }
    }
}