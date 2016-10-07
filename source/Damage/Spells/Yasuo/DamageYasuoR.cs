// <copyright file="DamageYasuoR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Yasuo R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Yasuo", SpellSlot.R)]
    public class DamageYasuoR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageYasuoR" /> class.
        /// </summary>
        public DamageYasuoR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 200, 300, 400 }[level] + (1.5 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}