// <copyright file="DamageVarusW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Varus W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Varus", SpellSlot.W)]
    public class DamageVarusW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageVarusW" /> class.
        /// </summary>
        public DamageVarusW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 10, 14, 18, 22, 26 }[level] + (0.25 * source.TotalMagicalDamage);
        }

        #endregion
    }
}