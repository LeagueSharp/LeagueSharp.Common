// <copyright file="DamageEkkoW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Ekko W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Ekko", SpellSlot.W)]
    public class DamageEkkoW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageEkkoW" /> class.
        /// </summary>
        public DamageEkkoW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 150, 195, 240, 285, 330 }[level] + (0.8 * source.TotalMagicalDamage);
        }

        #endregion
    }
}