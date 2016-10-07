// <copyright file="DamageVeigarR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Veigar R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Veigar", SpellSlot.R)]
    public class DamageVeigarR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageVeigarR" /> class.
        /// </summary>
        public DamageVeigarR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 175, 250, 325 }[level] + (0.8 * target.TotalMagicalDamage)
                   + (0.75 * source.TotalMagicalDamage);
        }

        #endregion
    }
}