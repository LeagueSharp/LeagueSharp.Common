// <copyright file="DamageOlafE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Olaf E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Olaf", SpellSlot.E)]
    public class DamageOlafE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageOlafE" /> class.
        /// </summary>
        public DamageOlafE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.True;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 115, 160, 205, 250 }[level] + (0.4 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod));
        }

        #endregion
    }
}