// <copyright file="DamageIcebornGauntlet.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Iceborn Gauntlet item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.IcebornGauntlet)]
    public class DamageIcebornGauntlet : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageIcebornGauntlet" /> class.
        /// </summary>
        public DamageIcebornGauntlet()
        {
            this.ItemId = 3025;
            this.DamageType = Damage.DamageType.Physical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("ItemFrozenFist") ? source.BaseAttackDamage : 0;
        }

        #endregion
    }
}