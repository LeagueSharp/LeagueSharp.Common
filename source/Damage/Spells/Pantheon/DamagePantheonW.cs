// <copyright file="DamagePantheonW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Pantheon W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Pantheon", SpellSlot.W)]
    public class DamagePantheonW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamagePantheonW" /> class.
        /// </summary>
        public DamagePantheonW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 75, 100, 125, 150 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}