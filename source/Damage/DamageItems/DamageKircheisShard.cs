// <copyright file="DamageKircheisShard.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Kircheis Shard item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.KircheisShard)]
    public class DamageKircheisShard : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKircheisShard" /> class.
        /// </summary>
        public DamageKircheisShard()
        {
            this.ItemId = 2015;
            this.DamageType = Damage.DamageType.Magical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.GetBuff("ItemStatikShankCharge")?.Count == 100 ? 40 : 0;
        }

        #endregion
    }
}