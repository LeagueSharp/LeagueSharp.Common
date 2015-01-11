#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 MinionManager.cs is part of LeagueSharp.Common.
 
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
using System.Linq;
using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    public enum MinionOrderTypes
    {
        None,
        Health,
        MaxHealth
    }

    public enum MinionTeam
    {
        Neutral,
        Ally,
        Enemy,
        NotAlly,
        NotAllyForEnemy,
        All
    }

    public enum MinionTypes
    {
        Ranged,
        Melee,
        All,
        Wards //TODO
    }

    public static class MinionManager
    {
        /// <summary>
        ///     Returns the minions in range from From.
        /// </summary>
        public static List<Obj_AI_Base> GetMinions(Vector3 from,
            float range,
            MinionTypes type = MinionTypes.All,
            MinionTeam team = MinionTeam.Enemy,
            MinionOrderTypes order = MinionOrderTypes.Health)
        {
            var result = (from minion in ObjectManager.Get<Obj_AI_Minion>()
                where minion.IsValidTarget(range, false, @from)
                where
                    minion.IsValidNeutral(team) || minion.IsValidAlly(team) || minion.IsValidEnemy(team) ||
                    minion.IsValidAllyOrNeutral(team)
                where type.IsAll() || (minion.IsMelee() && type.IsMelee()) || (!minion.IsMelee() && type.IsRanged())
                where IsMinion(minion) || minion.Team.IsNeutral()
                select minion).Cast<Obj_AI_Base>().ToList();

            switch (order)
            {
                case MinionOrderTypes.Health:
                    result = result.OrderBy(o => o.Health).ToList();
                    break;
                case MinionOrderTypes.MaxHealth:
                    result = result.OrderBy(o => o.MaxHealth).Reverse().ToList();
                    break;
            }

            return result;
        }

        public static List<Obj_AI_Base> GetMinions(float range,
            MinionTypes type = MinionTypes.All,
            MinionTeam team = MinionTeam.Enemy,
            MinionOrderTypes order = MinionOrderTypes.Health)
        {
            return GetMinions(ObjectManager.Player.ServerPosition, range, type, team, order);
        }

        public static bool IsMinion(Obj_AI_Minion minion, bool includeWards = false)
        {
            var name = minion.BaseSkinName.ToLower();
            return name.Contains("minion") || (includeWards && (name.Contains("ward") || name.Contains("trinket")));
        }

        /// <summary>
        ///     Returns the point where, when casted, the circular spell with hit the maximum amount of minions.
        /// </summary>
        public static FarmLocation GetBestCircularFarmLocation(List<Vector2> minionPositions,
            float width,
            float range,
            int useMECMax = 9)
        {
            var result = new Vector2();
            var minionCount = 0;

            range *= range;

            if (minionPositions.Count == 0)
            {
                return new FarmLocation(result, minionCount);
            }

            /* Use MEC to get the best positions only when there are less than 9 positions because it causes lag with more. */
            if (minionPositions.Count <= useMECMax)
            {
                var subGroups = GetCombinations(minionPositions);
                foreach (var subGroup in subGroups)
                {
                    if (subGroup.Count <= 0)
                    {
                        continue;
                    }

                    var circle = MEC.GetMec(subGroup);

                    if (circle.Radius <= width &&
                        circle.Center.Distance(ObjectManager.Player.ServerPosition.To2D(), true) <= range)
                    {
                        minionCount = subGroup.Count;
                        return new FarmLocation(circle.Center, minionCount);
                    }
                }
            }
            else
            {
                foreach (var pos in minionPositions)
                {
                    if (pos.Distance(ObjectManager.Player.ServerPosition.To2D(), true) > range)
                    {
                        continue;
                    }

                    var count = minionPositions.Count(pos2 => pos.Distance(pos2, true) <= width * width);

                    if (count < minionCount)
                    {
                        continue;
                    }

                    result = pos;
                    minionCount = count;
                }
            }

            return new FarmLocation(result, minionCount);
        }

        /// <summary>
        ///     Returns the point where, when casted, the lineal spell with hit the maximum amount of minions.
        /// </summary>
        public static FarmLocation GetBestLineFarmLocation(List<Vector2> minionPositions, float width, float range)
        {
            var result = new Vector2();
            var minionCount = 0;
            var startPos = ObjectManager.Player.ServerPosition.To2D();

            var max = minionPositions.Count;
            for (var i = 0; i < max; i++)
            {
                for (var j = 0; j < max; j++)
                {
                    if (minionPositions[j] != minionPositions[i])
                    {
                        minionPositions.Add((minionPositions[j] + minionPositions[i]) / 2);
                    }
                }
            }

            foreach (var pos in minionPositions)
            {
                if (pos.Distance(ObjectManager.Player.ServerPosition.To2D(), true) > range * range)
                {
                    continue;
                }

                var endPos = startPos + range * (pos - startPos).Normalized();

                var count = minionPositions.Count(pos2 => pos2.Distance(startPos, endPos, true, true) <= width * width);

                if (count < minionCount)
                {
                    continue;
                }

                result = endPos;
                minionCount = count;
            }

            return new FarmLocation(result, minionCount);
        }

        public static List<Vector2> GetMinionsPredictedPositions(List<Obj_AI_Base> minions,
            float delay,
            float width,
            float speed,
            Vector3 from,
            float range,
            bool collision,
            SkillshotType stype,
            Vector3 rangeCheckFrom = new Vector3())
        {
            from = from.To2D().IsValid() ? from : ObjectManager.Player.ServerPosition;

            return (from minion in minions
                select
                    Prediction.GetPrediction(
                        new PredictionInput
                        {
                            Unit = minion,
                            Delay = delay,
                            Radius = width,
                            Speed = speed,
                            From = @from,
                            Range = range,
                            Collision = collision,
                            Type = stype,
                            RangeCheckFrom = rangeCheckFrom
                        })
                into pos
                where pos.Hitchance >= HitChance.High
                select pos.UnitPosition.To2D()).ToList();
        }

        /*
         from: https://stackoverflow.com/questions/10515449/generate-all-combinations-for-a-list-of-strings :^)
         */

        /// <summary>
        ///     Returns all the subgroup combinations that can be made from a group
        /// </summary>
        private static IEnumerable<List<Vector2>> GetCombinations(IReadOnlyCollection<Vector2> allValues)
        {
            var collection = new List<List<Vector2>>();
            for (var counter = 0; counter < (1 << allValues.Count); ++counter)
            {
                var combination = allValues.Where((t, i) => (counter & (1 << i)) == 0).ToList();

                collection.Add(combination);
            }
            return collection;
        }

        public struct FarmLocation
        {
            public int MinionsHit;
            public Vector2 Position;

            public FarmLocation(Vector2 position, int minionsHit)
            {
                Position = position;
                MinionsHit = minionsHit;
            }
        }
    }

    internal static class Utilities
    {
        public static readonly GameObjectTeam Team = ObjectManager.Player.Team;

        public static bool IsAlly(this MinionTeam team)
        {
            return team.Equals(MinionTeam.Ally);
        }

        public static bool IsNeutral(this MinionTeam team)
        {
            return team.Equals(MinionTeam.Neutral);
        }

        public static bool IsEnemy(this MinionTeam team)
        {
            return team.Equals(MinionTeam.Enemy);
        }

        public static bool IsNeutral(this GameObjectTeam team)
        {
            return team.Equals(GameObjectTeam.Neutral);
        }

        public static bool IsAlly(this GameObjectTeam team)
        {
            return Team.Equals(GameObjectTeam.Chaos)
                ? team.Equals(GameObjectTeam.Chaos)
                : team.Equals(GameObjectTeam.Order);
        }

        public static bool IsEnemy(this GameObjectTeam team)
        {
            return Team.Equals(GameObjectTeam.Chaos)
                ? team.Equals(GameObjectTeam.Order)
                : team.Equals(GameObjectTeam.Chaos);
        }

        public static bool IsNotAllyForEnemy(this MinionTeam team)
        {
            return team.Equals(MinionTeam.NotAllyForEnemy);
        }

        public static bool IsAll(this MinionTeam team)
        {
            return team.Equals(MinionTeam.All);
        }

        public static bool IsValidNeutral(this Obj_AI_Minion unit, MinionTeam team)
        {
            return team.IsNeutral() && unit.Team.IsNeutral();
        }

        public static bool IsValidAlly(this Obj_AI_Minion unit, MinionTeam team)
        {
            return team.IsAlly() && unit.Team.IsAlly();
        }

        public static bool IsValidEnemy(this Obj_AI_Minion unit, MinionTeam team)
        {
            return team.IsEnemy() && unit.Team.IsEnemy();
        }

        public static bool IsValidAllyOrNeutral(this Obj_AI_Minion unit, MinionTeam team)
        {
            return team.IsAll() || (team.IsNotAllyForEnemy() && (unit.Team.IsAlly() || unit.Team.IsNeutral()));
        }

        public static bool IsMelee(this MinionTypes type)
        {
            return type.Equals(MinionTypes.Melee);
        }

        public static bool IsRanged(this MinionTypes type)
        {
            return type.Equals(MinionTypes.Ranged);
        }

        public static bool IsAll(this MinionTypes type)
        {
            return type.Equals(MinionTypes.All);
        }
    }
}