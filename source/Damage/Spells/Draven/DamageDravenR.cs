// <copyright file="DamageDravenR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Draven R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Draven", SpellSlot.R)]
    public class DamageDravenR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageDravenR" /> class.
        /// </summary>
        public DamageDravenR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 175, 275, 375 }[level] + (1.1 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}