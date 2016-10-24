// <copyright file="DamageFizzR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Fizz R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Fizz", SpellSlot.R)]
    public class DamageFizzR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageFizzR" /> class.
        /// </summary>
        public DamageFizzR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 200, 325, 450 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}