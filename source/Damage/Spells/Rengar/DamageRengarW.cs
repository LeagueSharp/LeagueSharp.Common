// <copyright file="DamageRengarW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Rengar W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Rengar", SpellSlot.W)]
    public class DamageRengarW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRengarW" /> class.
        /// </summary>
        public DamageRengarW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 50, 80, 110, 140, 170 }[level] + (0.8 * source.TotalMagicalDamage);
        }

        #endregion
    }
}