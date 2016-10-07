// <copyright file="DamageBotRK.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System;
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Blade of the Ruined King item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.Botrk)]
    public class DamageBotRK : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageBotRK" /> class.
        /// </summary>
        public DamageBotRK()
        {
            this.ItemId = 3153;
            this.DamageType = Damage.DamageType.Physical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return Math.Max(100, .1 * target.MaxHealth);
        }

        /// <inheritdoc />
        public override double GetPassiveDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            var value = Math.Max(10, .06 * target.Health);
            if (target is Obj_AI_Minion)
            {
                return Math.Min(60, value);
            }

            return value;
        }

        #endregion
    }
}