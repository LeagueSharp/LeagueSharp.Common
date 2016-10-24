// <copyright file="DamageTahmKenchQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, TahmKench Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("TahmKench", SpellSlot.Q)]
    public class DamageTahmKenchQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageTahmKenchQ" /> class.
        /// </summary>
        public DamageTahmKenchQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 130, 180, 230, 280 }[level] + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}