// <copyright file="DamageCaitlynE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Caitlyn E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Caitlyn", SpellSlot.E)]
    public class DamageCaitlynE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageCaitlynE" /> class.
        /// </summary>
        public DamageCaitlynE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 110, 150, 190, 230 }[level] + (0.8 * source.TotalMagicalDamage);
        }

        #endregion
    }
}