// <copyright file="DamageChoGathR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, ChoGath R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("ChoGath", SpellSlot.R)]
    public class DamageChoGathR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageChoGathR" /> class.
        /// </summary>
        public DamageChoGathR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.True;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 300, 475, 650 }[level] + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}