// <copyright file="DamageNidaleeE1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Nidalee E (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Nidalee", SpellSlot.E, 1)]
    public class DamageNidaleeE1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageNidaleeE1" /> class.
        /// </summary>
        public DamageNidaleeE1()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
            this.Stage = 1;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 130, 190, 250 }[source.Spellbook.GetSpell(SpellSlot.R).Level - 1] + (0.45 * source.TotalMagicalDamage);
        }

        #endregion
    }
}