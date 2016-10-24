// <copyright file="DamageHeimerdingerE1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Heimerdinger E (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Heimerdinger", SpellSlot.E, 1)]
    public class DamageHeimerdingerE1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageHeimerdingerE1" /> class.
        /// </summary>
        public DamageHeimerdingerE1()
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
            return new double[] { 150, 200, 250 }[source.Spellbook.GetSpell(SpellSlot.R).Level - 1] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}