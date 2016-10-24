// <copyright file="DamageKassadinE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Kassadin E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Kassadin", SpellSlot.E)]
    public class DamageKassadinE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKassadinE" /> class.
        /// </summary>
        public DamageKassadinE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 105, 130, 155, 180 }[level] + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}