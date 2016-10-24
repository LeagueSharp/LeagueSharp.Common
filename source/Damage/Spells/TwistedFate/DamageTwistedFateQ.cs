// <copyright file="DamageTwistedFateQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, TwistedFate Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("TwistedFate", SpellSlot.Q)]
    public class DamageTwistedFateQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTwistedFateQ" /> class.
        /// </summary>
        public DamageTwistedFateQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 105, 150, 195, 240 }[level] + (0.65 * source.TotalMagicalDamage);
        }

        #endregion
    }
}