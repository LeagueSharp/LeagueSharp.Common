// <copyright file="DamageTiamat.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Tiamat item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.Tiamat)]
    public class DamageTiamat : DamageItem
    {
        #region Constants

        private const float Range = 400;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTiamat" /> class.
        /// </summary>
        public DamageTiamat()
        {
            this.ItemId = 3077;
            this.DamageType = Damage.DamageType.Physical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return (target.InRange((Range - 50) / 4) ? .2 : .6) * source.TotalAttackDamage;
        }

        /// <inheritdoc />
        public override double GetPassiveDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return (target.InRange(Range / 4) ? .6 : 1) * source.TotalAttackDamage;
        }

        #endregion
    }
}