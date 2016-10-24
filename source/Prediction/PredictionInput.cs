// <copyright file="PredictionInput.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using SharpDX;

    /// <summary>
    ///     Prediction input, contains essential information to calculate a prediction.
    /// </summary>
    public class PredictionInput
    {
        #region Fields

        private Vector3 fromBackingField;

        private Vector3 rangeCheckFromBackingField;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether the prediction should consider with area of effect calculations.
        /// </summary>
        public bool Aoe { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the prediction should consider collisions.
        /// </summary>
        public bool Collision { get; set; }

        /// <summary>
        ///     Gets or sets the collision objects that the prediction should consider.
        /// </summary>
        public CollisionableObjects[] CollisionObjects { get; set; } =
        {
            CollisionableObjects.Minions,
            CollisionableObjects.YasuoWall
        };

        /// <summary>
        ///     Gets or sets the delay.
        /// </summary>
        public float Delay { get; set; }

        /// <summary>
        ///     Gets or sets the from position.
        /// </summary>
        public Vector3 From
        {
            get
            {
                return this.fromBackingField.To2D().IsZero ? ObjectManager.Player.ServerPosition : this.fromBackingField;
            }

            set
            {
                this.fromBackingField = value;
            }
        }

        /// <summary>
        ///     Gets or sets the radius.
        /// </summary>
        public float Radius { get; set; } = 1f;

        /// <summary>
        ///     Gets or sets the range.
        /// </summary>
        public float Range { get; set; } = float.MaxValue;

        /// <summary>
        ///     Gets or sets the range check from.
        /// </summary>
        public Vector3 RangeCheckFrom
        {
            get
            {
                return this.rangeCheckFromBackingField.To2D().IsZero ? this.From : this.rangeCheckFromBackingField;
            }

            set
            {
                this.rangeCheckFromBackingField = value;
            }
        }

        /// <summary>
        ///     Gets or sets the speed.
        /// </summary>
        public float Speed { get; set; } = float.MaxValue;

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        public SkillshotType Type { get; set; } = SkillshotType.SkillshotLine;

        /// <summary>
        ///     Gets or sets the unit.
        /// </summary>
        public Obj_AI_Base Unit { get; set; } = ObjectManager.Player;

        /// <summary>
        ///     Gets or sets a value indicating whether the bounding radius should be used.
        /// </summary>
        public bool UseBoundingRadius { get; set; } = true;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the real radius.
        /// </summary>
        internal float RealRadius
            => this.UseBoundingRadius ? this.Radius + (this.Unit?.BoundingRadius ?? 0f) : this.Radius;

        #endregion
    }
}