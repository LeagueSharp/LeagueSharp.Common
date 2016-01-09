#region License

/*
 Copyright 2014 - 2015 Nikita Bernthaler
 MinionManager.cs is part of SFXTargetSelector.

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

using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace SFXTargetSelector.Others
{
    internal enum MinionOrderTypes
    {
        None,
        Health,
        MaxHealth
    }

    internal enum MinionTeam
    {
        Neutral,
        Ally,
        Enemy,
        NotAlly,
        NotAllyForEnemy,
        All
    }

    internal enum MinionTypes
    {
        Ranged,
        Melee,
        All
    }

    internal static class MinionManager
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
            var result = (from minion in GameObjects.Minions.Concat(GameObjects.Jungle)
                where minion.IsValidTarget(range, false, @from)
                let minionTeam = minion.Team
                where
                    team == MinionTeam.Neutral && minionTeam == GameObjectTeam.Neutral ||
                    team == MinionTeam.Ally &&
                    minionTeam ==
                    (ObjectManager.Player.Team == GameObjectTeam.Chaos ? GameObjectTeam.Chaos : GameObjectTeam.Order) ||
                    team == MinionTeam.Enemy &&
                    minionTeam ==
                    (ObjectManager.Player.Team == GameObjectTeam.Chaos ? GameObjectTeam.Order : GameObjectTeam.Chaos) ||
                    team == MinionTeam.NotAlly && minionTeam != ObjectManager.Player.Team ||
                    team == MinionTeam.NotAllyForEnemy &&
                    (minionTeam == ObjectManager.Player.Team || minionTeam == GameObjectTeam.Neutral) ||
                    team == MinionTeam.All
                where
                    minion.IsMelee() && type == MinionTypes.Melee || !minion.IsMelee() && type == MinionTypes.Ranged ||
                    type == MinionTypes.All
                where IsMinion(minion) || minionTeam == GameObjectTeam.Neutral
                select minion).Select(m => m as Obj_AI_Base).ToList();

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
            return IsMinion(minion as Obj_AI_Base, includeWards);
        }

        public static bool IsMinion(Obj_AI_Base minion, bool includeWards = false)
        {
            var name = minion.CharData.BaseSkinName.ToLower();
            return name.Contains("minion") || name.Contains("bilge") || name.Contains("bw_") ||
                   (includeWards && (name.Contains("ward") || name.Contains("trinket")));
        }

        /// <summary>
        ///     Returns the point where, when casted, the circular spell with hit the maximum amount of minions.
        /// </summary>
        public static FarmLocation GetBestCircularFarmLocation(List<Vector2> minionPositions,
            float width,
            float range,
            int useMecMax = 9)
        {
            var result = new Vector2();
            var minionCount = 0;
            var startPos = ObjectManager.Player.ServerPosition.To2D();

            range = range * range;

            if (minionPositions.Count == 0)
            {
                return new FarmLocation(result, minionCount);
            }

            /* Use MEC to get the best positions only when there are less than 9 positions because it causes lag with more. */
            if (minionPositions.Count <= useMecMax)
            {
                var subGroups = GetCombinations(minionPositions);
                foreach (var subGroup in subGroups)
                {
                    if (subGroup.Count > 0)
                    {
                        var circle = MEC.GetMec(subGroup);

                        if (circle.Radius <= width && circle.Center.Distance(startPos, true) <= range)
                        {
                            minionCount = subGroup.Count;
                            return new FarmLocation(circle.Center, minionCount);
                        }
                    }
                }
            }
            else
            {
                foreach (var pos in minionPositions)
                {
                    if (pos.Distance(startPos, true) <= range)
                    {
                        var count = minionPositions.Count(pos2 => pos.Distance(pos2, true) <= width * width);

                        if (count >= minionCount)
                        {
                            result = pos;
                            minionCount = count;
                        }
                    }
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

            var posiblePositions = new List<Vector2>();
            posiblePositions.AddRange(minionPositions);

            var max = minionPositions.Count;
            for (var i = 0; i < max; i++)
            {
                for (var j = 0; j < max; j++)
                {
                    if (minionPositions[j] != minionPositions[i])
                    {
                        posiblePositions.Add((minionPositions[j] + minionPositions[i]) / 2);
                    }
                }
            }

            foreach (var pos in posiblePositions)
            {
                if (pos.Distance(startPos, true) <= range * range)
                {
                    var endPos = startPos + range * (pos - startPos).Normalized();

                    var count =
                        minionPositions.Count(pos2 => pos2.Distance(startPos, endPos, true, true) <= width * width);

                    if (count >= minionCount)
                    {
                        result = endPos;
                        minionCount = count;
                    }
                }
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

        /// <summary>
        ///     Returns all the subgroup combinations that can be made from a group
        /// </summary>
        private static List<List<Vector2>> GetCombinations(List<Vector2> allValues)
        {
            var collection = new List<List<Vector2>>();
            for (var counter = 0; counter < 1 << allValues.Count; ++counter)
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
}