// <copyright file="DamageBilgewaterCutlass.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Bilgewater Cutlass item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.Bilgewater)]
    public class DamageBilgewaterCutlass : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageBilgewaterCutlass" /> class.
        /// </summary>
        public DamageBilgewaterCutlass()
        {
            this.ItemId = 3144;
            this.DamageType = Damage.DamageType.Magical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return 100;
        }

        #endregion
    }
}