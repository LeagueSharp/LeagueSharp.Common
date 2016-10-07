// <copyright file="DamageSunfireCape.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Sunfire Cape item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.SunfireCape)]
    public class DamageSunfireCape : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSunfireCape" /> class.
        /// </summary>
        public DamageSunfireCape()
        {
            this.ItemId = 3068;
            this.DamageType = Damage.DamageType.Magical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            var value = 25 + (1 * source.Level);
            return target is Obj_AI_Minion ? value * (37.5 + (1.5 * source.Level)) : value;
        }

        #endregion
    }
}