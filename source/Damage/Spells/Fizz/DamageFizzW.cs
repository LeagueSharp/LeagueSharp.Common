// <copyright file="DamageFizzW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Fizz W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Fizz", SpellSlot.W)]
    public class DamageFizzW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageFizzW" /> class.
        /// </summary>
        public DamageFizzW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 10, 15, 20, 25, 30 }[level] + (0.3 * source.TotalMagicalDamage);
        }

        #endregion
    }
}