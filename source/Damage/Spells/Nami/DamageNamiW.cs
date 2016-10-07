// <copyright file="DamageNamiW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Nami W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Nami", SpellSlot.W)]
    public class DamageNamiW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageNamiW" /> class.
        /// </summary>
        public DamageNamiW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 70, 110, 150, 190, 230 }[level] + (0.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}