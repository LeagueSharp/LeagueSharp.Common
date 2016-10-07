// <copyright file="DamageLordVanDammsPillager.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Lord Van Damm's Pillager item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.LordVanDammsPillager)]
    public class DamageLordVanDammsPillager : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageLordVanDammsPillager" /> class.
        /// </summary>
        public DamageLordVanDammsPillager()
        {
            this.ItemId = 3104;
            this.DamageType = Damage.DamageType.True;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return 0d;
        }

        #endregion
    }
}