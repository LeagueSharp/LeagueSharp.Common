// <copyright file="DamageGragasW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Gragas W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Gragas", SpellSlot.W)]
    public class DamageGragasW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageGragasW" /> class.
        /// </summary>
        public DamageGragasW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 20, 50, 80, 110, 140 }[level] + (8 / 100f * target.MaxHealth) + (0.3 * source.TotalMagicalDamage);
        }

        #endregion
    }
}