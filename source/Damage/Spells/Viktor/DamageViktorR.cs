// <copyright file="DamageViktorR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Viktor R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Viktor", SpellSlot.R)]
    public class DamageViktorR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageViktorR" /> class.
        /// </summary>
        public DamageViktorR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 100, 175, 250 }[level] + (0.50 * source.TotalMagicalDamage);
        }

        #endregion
    }
}