// <copyright file="DamageZiggsR.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Ziggs R.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Ziggs", SpellSlot.R)]
    public class DamageZiggsR : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageZiggsR" /> class.
        /// </summary>
        public DamageZiggsR()
        {
            this.Slot = SpellSlot.R;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 300, 450, 600 }[level] + (1.1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}