#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 TargetSelector.cs is part of LeagueSharp.Common.
 
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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SharpDX;

#endregion

namespace LeagueSharp.Common {
    public class TargetSelector {
        #region Vars

        public static TargetingMode Mode => TargetingMode.AutoPriority;

        #endregion

        #region Enum

        public enum DamageType {
            Magical,
            Physical,
            True
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum TargetingMode {
            AutoPriority,
            LowHP,
            MostAD,
            MostAP,
            Closest,
            NearMouse,
            LessAttack,
            LessCast,
            MostStack
        }

        #endregion

        #region Functions

        public static Obj_AI_Hero SelectedTarget {
            get { return (SFXTargetSelector.TargetSelector.Selected.Target); }
            set { SFXTargetSelector.TargetSelector.Selected.Target = value; }
        }

        /// <summary>
        ///     Sets the priority of the hero
        /// </summary>
        public static void SetPriority(Obj_AI_Hero hero, int newPriority) {
            SFXTargetSelector.TargetSelector.Priorities.SetPriority(hero, newPriority);
        }

        /// <summary>
        ///     Returns the priority of the hero
        /// </summary>
        public static float GetPriority(Obj_AI_Hero hero) {
            return SFXTargetSelector.TargetSelector.Priorities.GetPriority(hero);
        }

        internal static void Initialize() {
            CustomEvents.Game.OnGameLoad += args => {
                Menu config = new Menu("Target Selector", "TargetSelector");
                SFXTargetSelector.TargetSelector.AddToMenu(config);
                CommonMenu.Instance.AddSubMenu(config);
            };
        }

        public static void AddToMenu(Menu config) {
            config.AddItem(new MenuItem("Alert", "----Use TS in Common Menu----"));
        }

        public static bool IsInvulnerable(Obj_AI_Base target, DamageType damageType, bool ignoreShields = true) {
#pragma warning disable 618
            return SFXTargetSelector.TargetSelector.IsInvulnerable(target, damageType, ignoreShields);
#pragma warning restore 618
        }


        public static void SetTarget(Obj_AI_Hero hero) {
            SFXTargetSelector.TargetSelector.Selected.Target = hero;
        }

        public static Obj_AI_Hero GetSelectedTarget() {
            return SFXTargetSelector.TargetSelector.Selected.Target;
        }

        public static Obj_AI_Hero GetTarget(float range,
                                            DamageType damageType,
                                            bool ignoreShield = true,
                                            IEnumerable<Obj_AI_Hero> ignoredChamps = null,
                                            Vector3? rangeCheckFrom = null) {
#pragma warning disable 618
            return SFXTargetSelector.TargetSelector.GetTarget(range, damageType, ignoreShield, ignoredChamps,
#pragma warning restore 618
                rangeCheckFrom);
        }

        public static Obj_AI_Hero GetTargetNoCollision(Spell spell,
                                                       bool ignoreShield = true,
                                                       IEnumerable<Obj_AI_Hero> ignoredChamps = null,
                                                       Vector3? rangeCheckFrom = null) {
#pragma warning disable 618
            return SFXTargetSelector.TargetSelector.GetTargetNoCollision(spell, ignoreShield, ignoredChamps,
#pragma warning restore 618
                rangeCheckFrom);
        }

        public static Obj_AI_Hero GetTarget(Obj_AI_Base champion,
                                            float range,
                                            DamageType type,
                                            bool ignoreShieldSpells = true,
                                            IEnumerable<Obj_AI_Hero> ignoredChamps = null,
                                            Vector3? rangeCheckFrom = null) {
#pragma warning disable 618
            return SFXTargetSelector.TargetSelector.GetTarget(champion, range, type, ignoreShieldSpells, ignoredChamps,
#pragma warning restore 618
                rangeCheckFrom);
        }

        #endregion
    }

    /// <summary>
    ///     This TS attempts to always lock the same target, useful for people getting targets for each spell, or for champions
    ///     that have to burst 1 target.
    /// </summary>
    public class LockedTargetSelector {
        private static Obj_AI_Hero _lastTarget;
        private static TargetSelector.DamageType _lastDamageType;

        public static Obj_AI_Hero GetTarget(float range,
                                            TargetSelector.DamageType damageType,
                                            bool ignoreShield = true,
                                            IEnumerable<Obj_AI_Hero> ignoredChamps = null,
                                            Vector3? rangeCheckFrom = null) {
            if ((_lastTarget == null) || !_lastTarget.IsValidTarget() || (_lastDamageType != damageType)) {
                Obj_AI_Hero newTarget = TargetSelector.GetTarget(range, damageType, ignoreShield, ignoredChamps,
                    rangeCheckFrom);

                _lastTarget = newTarget;
                _lastDamageType = damageType;

                return newTarget;
            }

            if (_lastTarget.IsValidTarget(range) && (damageType == _lastDamageType)) {
                return _lastTarget;
            }

            Obj_AI_Hero newTarget2 = TargetSelector.GetTarget(range, damageType, ignoreShield, ignoredChamps,
                rangeCheckFrom);

            _lastTarget = newTarget2;
            _lastDamageType = damageType;

            return newTarget2;
        }

        /// <summary>
        ///     Unlocks the currently locked target.
        /// </summary>
        public static void UnlockTarget() {
            _lastTarget = null;
        }

        public static void AddToMenu(Menu menu) {
            TargetSelector.AddToMenu(menu);
        }
    }
}