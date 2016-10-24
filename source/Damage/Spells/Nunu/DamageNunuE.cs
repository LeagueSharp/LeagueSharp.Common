// <copyright file="DamageNunuE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Nunu E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Nunu", SpellSlot.E)]
    public class DamageNunuE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageNunuE" /> class.
        /// </summary>
        public DamageNunuE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 85, 130, 175, 225, 275 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}