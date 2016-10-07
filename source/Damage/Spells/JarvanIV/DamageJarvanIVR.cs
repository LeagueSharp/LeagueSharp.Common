// <copyright file="DamageJarvanIVR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, JarvanIV R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("JarvanIV", SpellSlot.R)]
    public class DamageJarvanIVR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageJarvanIVR" /> class.
        /// </summary>
        public DamageJarvanIVR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 200, 325, 450 }[level] + (1.5 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}