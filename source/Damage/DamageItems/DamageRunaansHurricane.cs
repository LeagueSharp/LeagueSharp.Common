// <copyright file="DamageRunaansHurricane.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Runaan's Hurricane item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.RunaansHurricane)]
    public class DamageRunaansHurricane : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRunaansHurricane" /> class.
        /// </summary>
        public DamageRunaansHurricane()
        {
            this.ItemId = 3085;
            this.DamageType = Damage.DamageType.Physical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return source.TotalAttackDamage * .25;
        }

        #endregion
    }
}