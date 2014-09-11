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
        MaxHealth,
    }

    public enum MinionTeam
    {
        Neutral,
        Ally,
        Enemy,
        NotAlly,
        NotAllyForEnemy,
        All,
    }

    public enum MinionTypes
    {
        Ranged,
        Melee,
        All,
        Wards, //TODO
    }

    public static class MinionManager
    {
        /// <summary>
        /// Returns the minions in range from From.
        /// </summary>
        public static List<Obj_AI_Base> GetMinions(Vector3 from,
            float range,
            MinionTypes type = MinionTypes.All,
            MinionTeam team = MinionTeam.Enemy,
            MinionOrderTypes order = MinionOrderTypes.Health)
        {
            var result = new List<Obj_AI_Base>();

            foreach (var minion in ObjectManager.Get<Obj_AI_Minion>())
            {
                if (minion.IsValidTarget(range, false))
                {
                    if (team == MinionTeam.Neutral && minion.Team == GameObjectTeam.Neutral ||
                        team == MinionTeam.Ally &&
                        minion.Team ==
                        (ObjectManager.Player.Team == GameObjectTeam.Chaos ? GameObjectTeam.Chaos : GameObjectTeam.Order) ||
                        team == MinionTeam.Enemy &&
                        minion.Team ==
                        (ObjectManager.Player.Team == GameObjectTeam.Chaos ? GameObjectTeam.Order : GameObjectTeam.Chaos) ||
                        team == MinionTeam.NotAlly && minion.Team != ObjectManager.Player.Team ||
                        team == MinionTeam.NotAllyForEnemy &&
                        (minion.Team == ObjectManager.Player.Team || minion.Team == GameObjectTeam.Neutral) ||
                        team == MinionTeam.All)
                    {
                        if (minion.IsMelee() && type == MinionTypes.Melee ||
                            !minion.IsMelee() && type == MinionTypes.Ranged || type == MinionTypes.All)
                        {
                            result.Add(minion);
                        }
                    }
                }
            }

            if (order == MinionOrderTypes.Health)
            {
                result = result.OrderBy(o => o.Health).ToList();
            }
            else if (order == MinionOrderTypes.MaxHealth)
            {
                result = result.OrderBy(o => o.MaxHealth).Reverse().ToList();
            }

            return result;
        }


        /// <summary>
        /// Returns the point where, when casted, the circular spell with hit the maximum amount of minions.
        /// </summary>
        public static FarmLocation GetBestCircularFarmLocation(List<Vector2> minionPositions,
            float width,
            float range,
            int useMECMax = 9)
        {
            var result = new Vector2();
            var minionCount = 0;

            range = range * range;

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
                    var circle = MEC.GetMec(subGroup);

                    if (circle.Radius <= width &&
                        Vector2.DistanceSquared(circle.Center, ObjectManager.Player.ServerPosition.To2D()) <= range)
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
                    if (Vector2.DistanceSquared(pos, ObjectManager.Player.ServerPosition.To2D()) <= range)
                    {
                        var count = 0;
                        foreach (var pos2 in minionPositions)
                        {
                            if (Vector2.DistanceSquared(pos, pos2) <= width * width)
                            {
                                count++;
                            }
                        }
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
        /// Returns the point where, when casted, the lineal spell with hit the maximum amount of minions.
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
                if (Vector2.DistanceSquared(pos, ObjectManager.Player.ServerPosition.To2D()) <= range * range)
                {
                    var count = 0;
                    var endPos = startPos + range * (pos - startPos).Normalized();

                    foreach (var pos2 in minionPositions)
                    {
                        if (pos2.Distance(startPos, endPos, true, true) <= width * width)
                        {
                            count++;
                        }
                    }

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
            var result = new List<Vector2>();
            from = from.To2D().IsValid() ? from : ObjectManager.Player.ServerPosition;
            foreach (var minion in minions)
            {
                var pos = Prediction.GetPrediction(new PredictionInput
                {
                    Unit = minion,
                    Delay = delay,
                    Radius = width,
                    Speed = speed,
                    From = from,
                    Range = range,
                    Collision = collision,
                    Type = stype,
                    RangeCheckFrom = rangeCheckFrom
                });
                 
                if (pos.Hitchance >= HitChance.High)
                {
                    result.Add(pos.UnitPosition.To2D());
                }
            }

            return result;
        }

        /*
         from: https://stackoverflow.com/questions/10515449/generate-all-combinations-for-a-list-of-strings :^)
         */

        /// <summary>
        /// Returns all the subgroup combinations that can be made from a group
        /// </summary>
        private static List<List<Vector2>> GetCombinations(List<Vector2> allValues)
        {
            var collection = new List<List<Vector2>>();
            for (var counter = 0; counter < (1 << allValues.Count); ++counter)
            {
                var combination = new List<Vector2>();
                for (var i = 0; i < allValues.Count; ++i)
                {
                    if ((counter & (1 << i)) == 0)
                    {
                        combination.Add(allValues[i]);
                    }
                }

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