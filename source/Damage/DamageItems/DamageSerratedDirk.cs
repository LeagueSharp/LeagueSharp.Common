// <copyright file="DamageSerratedDirk.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Serrated Dirk item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.SerratedDirk)]
    public class DamageSerratedDirk : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSerratedDirk" /> class.
        /// </summary>
        public DamageSerratedDirk()
        {
            this.ItemId = 3134;
            this.DamageType = Damage.DamageType.Physical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return 15;
        }

        #endregion
    }
}