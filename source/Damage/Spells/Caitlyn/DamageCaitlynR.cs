// <copyright file="DamageCaitlynR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Caitlyn R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Caitlyn", SpellSlot.R)]
    public class DamageCaitlynR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageCaitlynR" /> class.
        /// </summary>
        public DamageCaitlynR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 250, 475, 700 }[level] + (2 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}