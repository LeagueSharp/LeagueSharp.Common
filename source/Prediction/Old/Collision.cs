namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using SharpDX;

    /// <summary>
    ///     Class that helps in calculating collision.
    /// </summary>
    public static class Collision
    {
        #region Static Fields

        /// <summary>
        ///     The tick yasuo casted wind wall.
        /// </summary>
        private static int _wallCastT;

        /// <summary>
        ///     The yasuo wind wall casted position.
        /// </summary>
        private static Vector2 _yasuoWallCastedPos;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="Collision" /> class.
        /// </summary>
        static Collision()
        {
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns the list of the units that the skillshot will hit before reaching the set positions.
        /// </summary>
        /// <param name="positions">The positions.</param>
        /// <param name="input">The input.</param>
        /// <returns>List&lt;Obj_AI_Base&gt;.</returns>
        public static List<Obj_AI_Base> GetCollision(List<Vector3> positions, PredictionInput input)
        {
            var result = new List<Obj_AI_Base>();

            foreach (var position in positions)
            {
                foreach (var objectType in input.CollisionObjects)
                {
                    switch (objectType)
                    {
                        case CollisionableObjects.Minions:
                            foreach (var minion in
                                ObjectManager.Get<Obj_AI_Minion>()
                                    .Where(
                                        minion =>
                                            minion.IsValidTarget(
                                                Math.Min(input.Range + input.Radius + 100, 2000),
                                                true,
                                                input.RangeCheckFrom)))
                            {
                                input.Unit = minion;
                                var minionPrediction = Prediction.GetPrediction(input, false, false);
                                if (minionPrediction.UnitPosition.To2D()
                                        .Distance(input.From.To2D(), position.To2D(), true, true)
                                    <= Math.Pow(input.Radius + 15 + minion.BoundingRadius, 2))
                                {
                                    result.Add(minion);
                                }
                            }
                            break;
                        case CollisionableObjects.Heroes:
                            foreach (var hero in
                                HeroManager.Enemies.FindAll(
                                    hero =>
                                        hero.IsValidTarget(
                                            Math.Min(input.Range + input.Radius + 100, 2000),
                                            true,
                                            input.RangeCheckFrom)))
                            {
                                input.Unit = hero;
                                var prediction = Prediction.GetPrediction(input, false, false);
                                if (prediction.UnitPosition.To2D()
                                        .Distance(input.From.To2D(), position.To2D(), true, true)
                                    <= Math.Pow(input.Radius + 50 + hero.BoundingRadius, 2))
                                {
                                    result.Add(hero);
                                }
                            }
                            break;

                        case CollisionableObjects.Allies:
                            foreach (var hero in
                                HeroManager.Allies.FindAll(
                                    hero =>
                                        Vector3.Distance(ObjectManager.Player.ServerPosition, hero.ServerPosition)
                                        <= Math.Min(input.Range + input.Radius + 100, 2000)))
                            {
                                input.Unit = hero;
                                var prediction = Prediction.GetPrediction(input, false, false);
                                if (prediction.UnitPosition.To2D()
                                        .Distance(input.From.To2D(), position.To2D(), true, true)
                                    <= Math.Pow(input.Radius + 50 + hero.BoundingRadius, 2))
                                {
                                    result.Add(hero);
                                }
                            }
                            break;

                        case CollisionableObjects.Walls:
                            var step = position.Distance(input.From) / 20;
                            for (var i = 0; i < 20; i++)
                            {
                                var p = input.From.To2D().Extend(position.To2D(), step * i);
                                if (NavMesh.GetCollisionFlags(p.X, p.Y).HasFlag(CollisionFlags.Wall))
                                {
                                    result.Add(ObjectManager.Player);
                                }
                            }
                            break;

                        case CollisionableObjects.YasuoWall:

                            if (Utils.TickCount - _wallCastT > 4000)
                            {
                                break;
                            }

                            GameObject wall = null;
                            foreach (var gameObject in
                                ObjectManager.Get<GameObject>()
                                    .Where(
                                        gameObject =>
                                            gameObject.IsValid
                                            && Regex.IsMatch(
                                                gameObject.Name,
                                                "_w_windwall_enemy_0.\\.troy",
                                                RegexOptions.IgnoreCase)))
                            {
                                wall = gameObject;
                            }
                            if (wall == null)
                            {
                                break;
                            }
                            var level = wall.Name.Substring(wall.Name.Length - 6, 1);
                            var wallWidth = 300 + (50 * Convert.ToInt32(level));

                            var wallDirection =
                                (wall.Position.To2D() - _yasuoWallCastedPos).Normalized().Perpendicular();
                            var wallStart = wall.Position.To2D() + (wallWidth / 2f * wallDirection);
                            var wallEnd = wallStart - (wallWidth * wallDirection);

                            if (wallStart.Intersection(wallEnd, position.To2D(), input.From.To2D()).Intersects)
                            {
                                var t = Utils.TickCount
                                        + (((wallStart.Intersection(wallEnd, position.To2D(), input.From.To2D())
                                               .Point.Distance(input.From) / input.Speed) + input.Delay) * 1000);
                                if (t < _wallCastT + 4000)
                                {
                                    result.Add(ObjectManager.Player);
                                }
                            }

                            break;
                    }
                }
            }

            return result.Distinct().ToList();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Fired when the game processes a spell cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsValid && sender.Team != ObjectManager.Player.Team && args.SData.Name == "YasuoWMovingWall")
            {
                _wallCastT = Utils.TickCount;
                _yasuoWallCastedPos = sender.ServerPosition.To2D();
            }
        }

        #endregion
    }
}