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
using SharpDX;

#endregion

namespace LeagueSharp.Common {
    /// <summary>
    ///     This class offers everything related to auto-attacks and orbwalking.
    /// </summary>
    public static class Orbwalking {
        /// <summary>
        ///     Delegate AfterAttackEvenH
        /// </summary>
        /// <param name="unit">The unit.</param>
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
        public enum OrbwalkingMode {
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

            Freeze,

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
        ///     The last auto attack tick
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static int LastAATick {
            get { return SFXTargetSelector.Orbwalking.LastAaTick; }
            set { SFXTargetSelector.Orbwalking.LastAaTick = value; }
        }

        /// <summary>
        ///     <c>true</c> if the orbwalker will attack.
        /// </summary>
        public static bool Attack {
            get { return SFXTargetSelector.Orbwalking.Attack; }
            set { SFXTargetSelector.Orbwalking.Attack = value; }
        }

        /// <summary>
        ///     <c>true</c> if the orbwalker will skip the next attack.
        /// </summary>
        public static bool DisableNextAttack {
            get { return SFXTargetSelector.Orbwalking.DisableNextAttack; }
            set { SFXTargetSelector.Orbwalking.DisableNextAttack = value; }
        }

        /// <summary>
        ///     <c>true</c> if the orbwalker will move.
        /// </summary>
        public static bool Move {
            get { return SFXTargetSelector.Orbwalking.Move; }
            set { SFXTargetSelector.Orbwalking.Move = value; }
        }

        /// <summary>
        ///     The tick the most recent attack command was sent.
        /// </summary>
        public static int LastAttackCommandT {
            get { return SFXTargetSelector.Orbwalking.LastAttackCommandT; }
            set { SFXTargetSelector.Orbwalking.LastAttackCommandT = value; }
        }

        /// <summary>
        ///     The tick the most recent move command was sent.
        /// </summary>
        public static int LastMoveCommandT {
            get { return SFXTargetSelector.Orbwalking.LastMoveCommandT; }
            set { SFXTargetSelector.Orbwalking.LastMoveCommandT = value; }
        }

        /// <summary>
        ///     The last move command position
        /// </summary>
        public static Vector3 LastMoveCommandPosition {
            get { return SFXTargetSelector.Orbwalking.LastMoveCommandPosition; }
            set { SFXTargetSelector.Orbwalking.LastMoveCommandPosition = value; }
        }

        /// <summary>
        ///     Initializes static members of the <see cref="Orbwalking" /> class.
        /// </summary>
        static Orbwalking() {
            SFXTargetSelector.Orbwalking.AfterAttack += FireAfterAttack;
            SFXTargetSelector.Orbwalking.BeforeAttack += FireBeforeAttack;
            SFXTargetSelector.Orbwalking.OnAttack += FireOnAttack;
            SFXTargetSelector.Orbwalking.OnNonKillableMinion += FireOnNonKillableMinion;
            SFXTargetSelector.Orbwalking.OnTargetChange +=
                FireOnTargetSwitch;
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
        /// <param name="args">The args from SFXOrbwalker.</param>
        private static void FireBeforeAttack(SFXTargetSelector.Orbwalking.BeforeAttackEventArgs args) {
            if (BeforeAttack != null) {
                BeforeAttack(new BeforeAttackEventArgs {Target = args.Target});
            }
            else {
                DisableNextAttack = false;
            }
        }

        /// <summary>
        ///     Fires the on attack event.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        private static void FireOnAttack(AttackableUnit unit, AttackableUnit target) {
            OnAttack?.Invoke(unit, target);
        }

        /// <summary>
        ///     Fires the after attack event.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        private static void FireAfterAttack(AttackableUnit unit, AttackableUnit target) {
            if (target.IsValidTarget()) {
                AfterAttack?.Invoke(unit, target);
            }
        }

        /// <summary>
        ///     Fires the on target switch event.
        /// </summary>
        /// <param name="target">The old target.</param>
        /// <param name="newTarget">The new target.</param>
        private static void FireOnTargetSwitch(AttackableUnit target, AttackableUnit newTarget) {
            OnTargetChange?.Invoke(target, newTarget);
        }

        /// <summary>
        ///     Fires the on non killable minion event.
        /// </summary>
        /// <param name="minion">The minion.</param>
        private static void FireOnNonKillableMinion(AttackableUnit minion) {
            OnNonKillableMinion?.Invoke(minion);
        }

        /// <summary>
        ///     Returns true if the spellname resets the attack timer.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the specified name is an auto attack reset; otherwise, <c>false</c>.</returns>
        public static bool IsAutoAttackReset(string name) {
            return SFXTargetSelector.Orbwalking.IsAutoAttackReset(name);
        }

        /// <summary>
        ///     Returns true if the unit is melee
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns><c>true</c> if the specified unit is melee; otherwise, <c>false</c>.</returns>
        public static bool IsMelee(this Obj_AI_Base unit) {
            return SFXTargetSelector.Orbwalking.IsMelee(unit);
        }

        /// <summary>
        ///     Returns true if the spellname is an auto-attack.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the name is an auto attack; otherwise, <c>false</c>.</returns>
        public static bool IsAutoAttack(string name) {
            return SFXTargetSelector.Orbwalking.IsAutoAttack(name);
        }

        /// <summary>
        ///     Returns the auto-attack range of local player with respect to the target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>System.Single.</returns>
        public static float GetRealAutoAttackRange(AttackableUnit target) {
            return SFXTargetSelector.Orbwalking.GetRealAutoAttackRange(target);
        }

        /// <summary>
        ///     Returns the auto-attack range of the target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>System.Single.</returns>
        public static float GetAttackRange(Obj_AI_Hero target) {
            return SFXTargetSelector.Orbwalking.GetAttackRange(target);
        }

        /// <summary>
        ///     Returns true if the target is in auto-attack range.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool InAutoAttackRange(AttackableUnit target) {
            return SFXTargetSelector.Orbwalking.InAutoAttackRange(target);
        }

        /// <summary>
        ///     Returns player auto-attack missile speed.
        /// </summary>
        /// <returns>System.Single.</returns>
        public static float GetMyProjectileSpeed() {
            return SFXTargetSelector.Orbwalking.GetMyProjectileSpeed();
        }

        /// <summary>
        ///     Returns if the player's auto-attack is ready.
        /// </summary>
        /// <returns><c>true</c> if this instance can attack; otherwise, <c>false</c>.</returns>
        public static bool CanAttack() {
            return SFXTargetSelector.Orbwalking.CanAttack();
        }

        /// <summary>
        ///     Returns true if moving won't cancel the auto-attack.
        /// </summary>
        /// <param name="extraWindup">The extra windup.</param>
        /// <param name="disableMissileCheck">Whether to disable missle check or not</param>
        /// <returns><c>true</c> if this instance can move the specified extra windup; otherwise, <c>false</c>.</returns>
        public static bool CanMove(float extraWindup, bool disableMissileCheck = false) {
            return SFXTargetSelector.Orbwalking.CanMove(extraWindup, disableMissileCheck);
        }

        /// <summary>
        ///     Sets the movement delay.
        /// </summary>
        /// <param name="delay">The delay.</param>
        public static void SetMovementDelay(int delay) {
        }

        /// <summary>
        ///     Sets the minimum orbwalk distance.
        /// </summary>
        /// <param name="d">The d.</param>
        public static void SetMinimumOrbwalkDistance(float d) {
            SFXTargetSelector.Orbwalking.SetMinimumOrbwalkDistance(d);
        }

        /// <summary>
        ///     Gets the last move time.
        /// </summary>
        /// <returns>System.Single.</returns>
        public static float GetLastMoveTime() {
            return SFXTargetSelector.Orbwalking.GetLastMoveTime();
        }

        /// <summary>
        ///     Gets the last move position.
        /// </summary>
        /// <returns>Vector3.</returns>
        public static Vector3 GetLastMovePosition() {
            return SFXTargetSelector.Orbwalking.GetLastMovePosition();
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
                                  bool randomizeMinDistance = true) {
            SFXTargetSelector.Orbwalking.MoveTo(position, holdAreaRadius, overrideTimer, useFixedDistance,
                randomizeMinDistance);
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
                                   bool randomizeMinDistance = true) {
            SFXTargetSelector.Orbwalking.Orbwalk(target, position, extraWindup, holdAreaRadius, useFixedDistance,
                randomizeMinDistance);
        }

        /// <summary>
        ///     Resets the Auto-Attack timer.
        /// </summary>
        public static void ResetAutoAttackTimer() {
            SFXTargetSelector.Orbwalking.ResetAutoAttackTimer();
        }

        /// <summary>
        ///     The before attack event arguments.
        /// </summary>
        public class BeforeAttackEventArgs : EventArgs {
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
            public bool Process {
                get { return this._process; }
                set {
                    DisableNextAttack = !value;
                    this._process = value;
                }
            }
        }

        /// <summary>
        ///     This class allows you to add an instance of "Orbwalker" to your assembly in order to control the orbwalking in an
        ///     easy way.
        /// </summary>
        public class Orbwalker : SFXTargetSelector.Orbwalking.Orbwalker {
            public new OrbwalkingMode ActiveMode {
                get {
                    SFXTargetSelector.Orbwalking.OrbwalkingMode mode = base.ActiveMode;
                    switch (mode) {
                        case SFXTargetSelector.Orbwalking.OrbwalkingMode.LastHit:
                            return OrbwalkingMode.LastHit;
                        case SFXTargetSelector.Orbwalking.OrbwalkingMode.Mixed:
                            return OrbwalkingMode.Mixed;
                        case SFXTargetSelector.Orbwalking.OrbwalkingMode.LaneClear:
                            return OrbwalkingMode.LaneClear;
                        case SFXTargetSelector.Orbwalking.OrbwalkingMode.Combo:
                            return OrbwalkingMode.Combo;
                        case SFXTargetSelector.Orbwalking.OrbwalkingMode.Flee:
                            return OrbwalkingMode.CustomMode;
                        case SFXTargetSelector.Orbwalking.OrbwalkingMode.CustomMode:
                            return OrbwalkingMode.CustomMode;
                    }
                    return OrbwalkingMode.None;
                }
                set {
                    switch (value) {
                        case OrbwalkingMode.LastHit:
                            base.ActiveMode = SFXTargetSelector.Orbwalking.OrbwalkingMode.LastHit;
                            break;
                        case OrbwalkingMode.Mixed:
                            base.ActiveMode = SFXTargetSelector.Orbwalking.OrbwalkingMode.Mixed;
                            break;
                        case OrbwalkingMode.LaneClear:
                            base.ActiveMode = SFXTargetSelector.Orbwalking.OrbwalkingMode.LaneClear;
                            break;
                        case OrbwalkingMode.Combo:
                            base.ActiveMode = SFXTargetSelector.Orbwalking.OrbwalkingMode.Combo;
                            break;
                        case OrbwalkingMode.Freeze:
                            base.ActiveMode = SFXTargetSelector.Orbwalking.OrbwalkingMode.LastHit;
                            break;
                        case OrbwalkingMode.CustomMode:
                            base.ActiveMode = SFXTargetSelector.Orbwalking.OrbwalkingMode.CustomMode;
                            break;
                    }
                    base.ActiveMode = SFXTargetSelector.Orbwalking.OrbwalkingMode.None;
                }
            }

            public Orbwalker(Menu attachToMenu) : base(attachToMenu) {
            }
        }
    }
}