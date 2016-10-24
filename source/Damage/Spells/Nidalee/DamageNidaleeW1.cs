// <copyright file="DamageNidaleeW1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Nidalee W (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Nidalee", SpellSlot.W, 1)]
    public class DamageNidaleeW1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageNidaleeW1" /> class.
        /// </summary>
        public DamageNidaleeW1()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
            this.Stage = 1;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 110, 160, 210 }[source.Spellbook.GetSpell(SpellSlot.R).Level - 1] + (0.3 * source.TotalMagicalDamage);
        }

        #endregion
    }
}