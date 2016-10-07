// <copyright file="DamageBamisCinder.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Bami's Cinder item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.BamisCinder)]
    public class DamageBamisCinder : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageBamisCinder" /> class.
        /// </summary>
        public DamageBamisCinder()
        {
            this.ItemId = 3751;
            this.DamageType = Damage.DamageType.Magical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            var value = 5 + (1 * source.Level);
            return target is Obj_AI_Minion ? value * 1.5 : value;
        }

        #endregion
    }
}