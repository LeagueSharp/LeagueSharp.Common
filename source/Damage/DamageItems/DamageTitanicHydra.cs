// <copyright file="DamageTitanicHydra.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Titanic Hydra item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.TitanicHydra)]
    public class DamageTitanicHydra : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTitanicHydra" /> class.
        /// </summary>
        public DamageTitanicHydra()
        {
            this.ItemId = 3748;
            this.DamageType = Damage.DamageType.Physical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            var value = 5 + (.01 * source.MaxHealth);

            if (source.HasBuff("itemtitanichydracleave"))
            {
                value = 40 + (.1 * source.MaxHealth);
            }

            return value;
        }

        #endregion
    }
}