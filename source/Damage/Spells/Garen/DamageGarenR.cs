// <copyright file="DamageGarenR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Garen R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Garen", SpellSlot.R)]
    public class DamageGarenR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageGarenR" /> class.
        /// </summary>
        public DamageGarenR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 175, 350, 525 }[level] + (new[] { 28.57, 33.33, 40 }[level] / 100 * (target.MaxHealth - target.Health));
        }

        #endregion
    }
}