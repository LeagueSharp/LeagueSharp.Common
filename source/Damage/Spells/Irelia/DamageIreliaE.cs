// <copyright file="DamageIreliaE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Irelia E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Irelia", SpellSlot.E)]
    public class DamageIreliaE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageIreliaE" /> class.
        /// </summary>
        public DamageIreliaE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 120, 160, 200, 240 }[level] + (0.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}