// <copyright file="DamageNidaleeQ1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Nidalee Q (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Nidalee", SpellSlot.Q, 1)]
    public class DamageNidaleeQ1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageNidaleeQ1" /> class.
        /// </summary>
        public DamageNidaleeQ1()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Magical;
            this.Stage = 1;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            var dmg = (new double[] { 4, 20, 50, 90 }[source.Spellbook.GetSpell(SpellSlot.R).Level - 1]
                       + (0.36 * source.TotalMagicalDamage)
                       + (0.75 * (source.BaseAttackDamage + source.FlatPhysicalDamageMod)))
                      * (((target.MaxHealth - target.Health) / target.MaxHealth * 1.5) + 1);
            dmg *= target.HasBuff("nidaleepassivehunted") ? 1.33 : 1.0;
            return dmg;
        }

        #endregion
    }
}