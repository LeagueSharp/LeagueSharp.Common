// <copyright file="DamageSorakaE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Soraka E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Soraka", SpellSlot.E)]
    public class DamageSorakaE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSorakaE" /> class.
        /// </summary>
        public DamageSorakaE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 110, 150, 190, 230 }[level] + (0.4 * source.TotalMagicalDamage);
        }

        #endregion
    }
}