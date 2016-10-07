// <copyright file="DamageLiandrysTorment.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Liandry's Torment item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.LiandrysTorment)]
    public class DamageLiandrysTorment : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLiandrysTorment" /> class.
        /// </summary>
        public DamageLiandrysTorment()
        {
            this.ItemId = 3151;
            this.IsDot = true;
            this.DamageType = Damage.DamageType.Magical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return this.GetDotDamage(source, target) * 3;
        }

        /// <inheritdoc />
        public override double GetDotDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return .02 * target.Health;
        }

        #endregion
    }
}