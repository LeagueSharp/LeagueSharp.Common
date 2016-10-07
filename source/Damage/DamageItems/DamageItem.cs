// <copyright file="DamageItem.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    /// <summary>
    ///     The damage item base class.
    /// </summary>
    public class DamageItem : IDamageItem
    {
        #region Public Properties

        /// <inheritdoc />
        public Damage.DamageType DamageType { get; protected set; }

        /// <inheritdoc />
        public bool IsDot { get; protected set; }

        /// <inheritdoc />
        public int ItemId { get; protected set; }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public virtual double GetDamage(Obj_AI_Hero source, Obj_AI_Base target) => 0d;

        /// <inheritdoc />
        public virtual double GetDotDamage(Obj_AI_Hero source, Obj_AI_Base target) => 0d;

        /// <inheritdoc />
        public virtual double GetPassiveDamage(Obj_AI_Hero source, Obj_AI_Base target) => this.GetDamage(source, target);

        #endregion
    }
}