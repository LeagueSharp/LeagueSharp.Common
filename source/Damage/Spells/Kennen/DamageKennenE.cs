// <copyright file="DamageKennenE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Kennen E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Kennen", SpellSlot.E)]
    public class DamageKennenE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKennenE" /> class.
        /// </summary>
        public DamageKennenE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 85, 125, 165, 205, 245 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}