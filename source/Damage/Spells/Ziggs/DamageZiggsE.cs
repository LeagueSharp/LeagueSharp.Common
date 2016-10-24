// <copyright file="DamageZiggsE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Ziggs E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Ziggs", SpellSlot.E)]
    public class DamageZiggsE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageZiggsE" /> class.
        /// </summary>
        public DamageZiggsE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 40, 65, 90, 115, 140 }[level] + (0.3 * source.TotalMagicalDamage);
        }

        #endregion
    }
}