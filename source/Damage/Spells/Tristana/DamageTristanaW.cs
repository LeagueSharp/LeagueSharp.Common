// <copyright file="DamageTristanaW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Tristana W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Tristana", SpellSlot.W)]
    public class DamageTristanaW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTristanaW" /> class.
        /// </summary>
        public DamageTristanaW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 110, 160, 210, 260 }[level] + (0.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}