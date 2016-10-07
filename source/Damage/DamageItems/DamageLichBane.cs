// <copyright file="DamageLichBane.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Lich Bane item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.LichBane)]
    public class DamageLichBane : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLichBane" /> class.
        /// </summary>
        public DamageLichBane()
        {
            this.ItemId = 3100;
            this.DamageType = Damage.DamageType.Magical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.HasBuff("LichBane") ? (.75 * source.BaseAttackDamage) + (.5 * source.TotalMagicalDamage) : 0;
        }

        #endregion
    }
}