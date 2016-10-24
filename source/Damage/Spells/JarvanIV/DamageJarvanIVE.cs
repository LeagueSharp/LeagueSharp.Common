// <copyright file="DamageJarvanIVE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, JarvanIV E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("JarvanIV", SpellSlot.E)]
    public class DamageJarvanIVE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageJarvanIVE" /> class.
        /// </summary>
        public DamageJarvanIVE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 105, 150, 195, 240 }[level] + (0.8 * source.TotalMagicalDamage);
        }

        #endregion
    }
}