// <copyright file="DamageGragasE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Gragas E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Gragas", SpellSlot.E)]
    public class DamageGragasE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageGragasE" /> class.
        /// </summary>
        public DamageGragasE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 130, 180, 230, 280 }[level] + (0.6 * source.TotalMagicalDamage);
        }

        #endregion
    }
}