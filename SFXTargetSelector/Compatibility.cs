#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 Compatibility.cs is part of SFXTargetSelector.

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
using LeagueSharp;
using LeagueSharp.Common;
using SFXTargetSelector.Others;
using SharpDX;

#endregion

namespace SFXTargetSelector
{
    public static partial class TargetSelector
    {
        [Obsolete("Use SFXTargetSelector.TargetSelector.Selected.Target instead.")]
        public static Obj_AI_Hero SelectedTarget
        {
            get { return Selected.Focus.Enabled ? Selected.Target : null; }
        }

        [Obsolete("Use SFXTargetSelector.TargetSelector.Priorities.SetPriority(Obj_AI_Hero, int) instead.")]
        public static void SetPriority(Obj_AI_Hero hero, int newPriority)
        {
            Priorities.SetPriority(hero, newPriority);
        }

        [Obsolete("Use SFXTargetSelector.TargetSelector.Priorities.GetPriority instead.")]
        public static float GetPriority(Obj_AI_Hero hero)
        {
            return Priorities.GetPriority(hero);
        }

        [Obsolete("Use SFXTargetSelector.Others.Invulnerable.Check(Obj_AI_Hero, DamageType, bool) instead.")]
        public static bool IsInvulnerable(Obj_AI_Base target, DamageType damageType, bool ignoreShields = true)
        {
            var hero = target as Obj_AI_Hero;
            return hero != null && Invulnerable.Check(hero, damageType, ignoreShields);
        }

        [Obsolete("Use SFXTargetSelector.Others.Invulnerable.Check(Obj_AI_Hero, DamageType, bool) instead.")]
        public static bool IsInvulnerable(Obj_AI_Base target,
            LeagueSharp.Common.TargetSelector.DamageType damageType,
            bool ignoreShields = true)
        {
            return IsInvulnerable(target, Utils.ConvertDamageType(damageType), ignoreShields);
        }

        [Obsolete("Use SFXTargetSelector.TargetSelector.Selected.Target instead.")]
        public static void SetTarget(Obj_AI_Hero hero)
        {
            if (hero.IsValidTarget())
            {
                Selected.Target = hero;
            }
        }

        [Obsolete("Use SFXTargetSelector.TargetSelector.Selected.Target instead.")]
        public static Obj_AI_Hero GetSelectedTarget()
        {
            return Selected.Target;
        }

        [Obsolete(
            "Use SFXTargetSelector.TargetSelector.GetTarget(float, DamageType, bool, Vector3, IEnumerable<Obj_AI_Hero>) instead."
            )]
        public static Obj_AI_Hero GetTarget(float range,
            DamageType damageType,
            bool ignoreShield,
            IEnumerable<Obj_AI_Hero> ignoredChamps = null,
            Vector3? rangeCheckFrom = null)
        {
            return GetTarget(ObjectManager.Player, range, damageType, ignoreShield, ignoredChamps, rangeCheckFrom);
        }

        [Obsolete(
            "Use SFXTargetSelector.TargetSelector.GetTarget(float, DamageType, bool, Vector3, IEnumerable<Obj_AI_Hero>) instead."
            )]
        public static Obj_AI_Hero GetTarget(float range,
            LeagueSharp.Common.TargetSelector.DamageType damageType,
            bool ignoreShield,
            IEnumerable<Obj_AI_Hero> ignoredChamps = null,
            Vector3? rangeCheckFrom = null)
        {
            return GetTarget(range, Utils.ConvertDamageType(damageType), ignoreShield, ignoredChamps, rangeCheckFrom);
        }

        [Obsolete(
            "Use SFXTargetSelector.TargetSelector.GetTargetNoCollision(Spell, bool, Vector3, IEnumerable<Obj_AI_Hero>) instead."
            )]
        public static Obj_AI_Hero GetTargetNoCollision(Spell spell,
            bool ignoreShield,
            IEnumerable<Obj_AI_Hero> ignoredChamps = null,
            Vector3? rangeCheckFrom = null)
        {
            return GetTargetNoCollision(
                spell, ignoreShield,
                rangeCheckFrom == null
                    ? default(Vector3)
                    : new Vector3(rangeCheckFrom.Value.X, rangeCheckFrom.Value.Y, rangeCheckFrom.Value.Z), ignoredChamps);
        }

        [Obsolete(
            "Use SFXTargetSelector.TargetSelector.GetTarget(float, DamageType, bool, Vector3, IEnumerable<Obj_AI_Hero>) instead."
            )]
        public static Obj_AI_Hero GetTarget(Obj_AI_Base champion,
            float range,
            DamageType type,
            bool ignoreShieldSpells,
            IEnumerable<Obj_AI_Hero> ignoredChamps = null,
            Vector3? rangeCheckFrom = null)
        {
            return GetTarget(
                range, type, ignoreShieldSpells,
                rangeCheckFrom == null
                    ? default(Vector3)
                    : new Vector3(rangeCheckFrom.Value.X, rangeCheckFrom.Value.Y, rangeCheckFrom.Value.Z), ignoredChamps);
        }

        [Obsolete(
            "Use SFXTargetSelector.TargetSelector.GetTarget(float, DamageType, bool, Vector3, IEnumerable<Obj_AI_Hero>) instead."
            )]
        public static Obj_AI_Hero GetTarget(Obj_AI_Base champion,
            float range,
            LeagueSharp.Common.TargetSelector.DamageType type,
            bool ignoreShieldSpells,
            IEnumerable<Obj_AI_Hero> ignoredChamps = null,
            Vector3? rangeCheckFrom = null)
        {
            return GetTarget(
                champion, range, Utils.ConvertDamageType(type), ignoreShieldSpells, ignoredChamps, rangeCheckFrom);
        }
    }
}