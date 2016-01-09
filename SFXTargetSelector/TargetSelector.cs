#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 TargetSelector.cs is part of SFXTargetSelector.

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
using System.Reflection;
using LeagueSharp;
using LeagueSharp.Common;
using SFXTargetSelector.Others;
using SharpDX;
using Version = System.Version;

#endregion

/*
 * Don't copy paste this without asking & giving credits fuckers :^) 
 */

namespace SFXTargetSelector
{
    public static partial class TargetSelector
    {
        static TargetSelector()
        {
            LeagueSharp.Common.TargetSelector.CustomTS = true;
            GameObjects.Initialize();
            CustomEvents.Game.OnGameLoad += delegate
            {
                Drawings.Initialize();
                Notifications.AddNotification(string.Format("{0} loaded.", Name), 7500);
                Game.PrintChat(string.Format("<font color='#259FF8'>{0} v{1} loaded.</font>", Name, Version));
            };
        }

        public static Menu Menu { get; private set; }

        public static string Name
        {
            get { return "SFXTargetSelector"; }
        }

        public static Version Version
        {
            get { return Assembly.GetEntryAssembly().GetName().Version; }
        }

        public static Obj_AI_Hero GetTargetNoCollision(Spell spell,
            bool ignoreShields = true,
            Vector3 from = default(Vector3),
            IEnumerable<Obj_AI_Hero> ignoreChampions = null)
        {
            return
                GetTargets(spell.Range, Utils.ConvertDamageType(spell.DamageType), ignoreShields, from, ignoreChampions)
                    .FirstOrDefault(t => spell.GetPrediction(t).Hitchance != HitChance.Collision);
        }

        public static Obj_AI_Hero GetTarget(Spell spell,
            bool ignoreShields = true,
            Vector3 from = default(Vector3),
            IEnumerable<Obj_AI_Hero> ignoreChampions = null)
        {
            return
                GetTarget(
                    spell.Range + spell.Width +
                    Targets.Items.Select(e => e.Hero.BoundingRadius).DefaultIfEmpty(50).Max(),
                    Utils.ConvertDamageType(spell.DamageType), ignoreShields, from, ignoreChampions);
        }

        public static Obj_AI_Hero GetTarget(float range,
            DamageType damageType = DamageType.True,
            bool ignoreShields = true,
            Vector3 from = default(Vector3),
            IEnumerable<Obj_AI_Hero> ignoreChampions = null)
        {
            var targets = GetTargets(range, damageType, ignoreShields, from, ignoreChampions);
            return targets != null ? targets.FirstOrDefault() : null;
        }

        public static IEnumerable<Obj_AI_Hero> GetTargets(float range,
            DamageType damageType = DamageType.True,
            bool ignoreShields = true,
            Vector3 from = default(Vector3),
            IEnumerable<Obj_AI_Hero> ignoreChampions = null)
        {
            Weights.Range = Math.Max(range, Weights.Range);

            var selectedTarget = Selected.GetTarget(range, damageType, ignoreShields, from);
            if (selectedTarget != null)
            {
                return new List<Obj_AI_Hero> { selectedTarget };
            }

            range = Modes.Current.Mode == Mode.Weights && Selected.Focus.Enabled && Selected.Focus.Force
                ? Weights.Range
                : range;

            var targets =
                Humanizer.FilterTargets(Targets.Items)
                    .Where(h => ignoreChampions == null || ignoreChampions.All(i => i.NetworkId != h.Hero.NetworkId))
                    .Where(h => Utils.IsValidTarget(h.Hero, range, damageType, ignoreShields, from))
                    .ToList();

            if (targets.Count > 0)
            {
                var t = Modes.GetOrderedChampions(targets).ToList();
                if (t.Count > 0)
                {
                    if (Selected.Target != null && Selected.Focus.Enabled && t.Count > 1)
                    {
                        t = t.OrderByDescending(x => x.Hero.NetworkId.Equals(Selected.Target.NetworkId)).ToList();
                    }
                    return t.Select(h => h.Hero).ToList();
                }
            }
            return new List<Obj_AI_Hero>();
        }

        public static void AddToMenu(Menu menu)
        {
            menu.Name = "sfx.ts";
            Menu = menu;

            Drawings.AddToMainMenu();
            Weights.AddToMainMenu();
            Priorities.AddToMainMenu();
            Selected.Focus.AddToMainMenu();
            Humanizer.AddToMainMenu();
            Modes.AddToMainMenu();
        }
    }
}