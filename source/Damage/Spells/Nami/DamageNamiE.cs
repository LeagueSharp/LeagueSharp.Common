// <copyright file="DamageNamiE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Nami E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Nami", SpellSlot.E)]
    public class DamageNamiE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageNamiE" /> class.
        /// </summary>
        public DamageNamiE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 25, 40, 55, 70, 85 }[level] + (0.2 * source.TotalMagicalDamage);
        }

        #endregion
    }
}