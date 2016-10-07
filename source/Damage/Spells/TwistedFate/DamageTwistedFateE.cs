// <copyright file="DamageTwistedFateE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, TwistedFate E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("TwistedFate", SpellSlot.E)]
    public class DamageTwistedFateE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTwistedFateE" /> class.
        /// </summary>
        public DamageTwistedFateE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 55, 80, 105, 130, 155 }[level] + (0.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}