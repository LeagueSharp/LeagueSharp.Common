// <copyright file="PredictionOutput.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Collections.Generic;

    using SharpDX;

    /// <summary>
    ///     Contains the output information of a prediction calculation.
    /// </summary>
    public class PredictionOutput
    {
        #region Fields

        private int aoeTargetsHitCountBackingField;

        private Vector3 castPositionBackingField;

        private Vector3 unitPositionBackingField;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the area of effect targets hit collection, if acceptable.
        /// </summary>
        public List<Obj_AI_Hero> AoeTargetsHit { get; set; } = new List<Obj_AI_Hero>();

        /// <summary>
        ///     Gets or sets the area of effect targets hit count.
        /// </summary>
        public int AoeTargetsHitCount
        {
            get
            {
                return Math.Max(this.aoeTargetsHitCountBackingField, this.AoeTargetsHit.Count);
            }

            set
            {
                this.aoeTargetsHitCountBackingField = value;
            }
        }

        /// <summary>
        ///     Gets or sets the cast position.
        /// </summary>
        public Vector3 CastPosition
        {
            get
            {
                return this.castPositionBackingField.To2D().IsValid()
                           ? this.castPositionBackingField.SetZ()
                           : this.Input.Unit.ServerPosition;
            }

            set
            {
                this.castPositionBackingField = value;
            }
        }

        /// <summary>
        ///     Gets or sets the collision objects the skillshot would collide with.
        /// </summary>
        public List<Obj_AI_Base> CollisionObjects { get; set; } = new List<Obj_AI_Base>();

        /// <summary>
        ///     Gets or sets the hitchance.
        /// </summary>
        public HitChance Hitchance { get; set; } = HitChance.Immobile;

        /// <summary>
        ///     Gets or sets the unit position.
        /// </summary>
        public Vector3 UnitPosition
        {
            get
            {
                return this.unitPositionBackingField.To2D().IsValid()
                           ? this.unitPositionBackingField.SetZ()
                           : this.Input.Unit.ServerPosition;
            }

            set
            {
                this.unitPositionBackingField = value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the input.
        /// </summary>
        internal PredictionInput Input { get; set; }

        #endregion
    }
}