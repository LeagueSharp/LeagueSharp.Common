// <copyright file="DamageDianaW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Diana W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Diana", SpellSlot.W)]
    public class DamageDianaW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageDianaW" /> class.
        /// </summary>
        public DamageDianaW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 22, 34, 46, 58, 70 }[level] + (0.2 * source.TotalMagicalDamage);
        }

        #endregion
    }
}