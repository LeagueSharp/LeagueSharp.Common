// <copyright file="DamageJarvanIVQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, JarvanIV Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("JarvanIV", SpellSlot.Q)]
    public class DamageJarvanIVQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageJarvanIVQ" /> class.
        /// </summary>
        public DamageJarvanIVQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Physical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 115, 160, 205, 250 }[level] + (1.2 * source.FlatPhysicalDamageMod);
        }

        #endregion
    }
}