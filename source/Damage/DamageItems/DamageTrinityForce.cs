// <copyright file="DamageTrinityForce.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Trinity Force item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.TrinityForce)]
    public class DamageTrinityForce : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTrinityForce" /> class.
        /// </summary>
        public DamageTrinityForce()
        {
            this.ItemId = 3078;
            this.DamageType = Damage.DamageType.Physical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("sheen") ? source.BaseAttackDamage * 2 : 0;
        }

        #endregion
    }
}