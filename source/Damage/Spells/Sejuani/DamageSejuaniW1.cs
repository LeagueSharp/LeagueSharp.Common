// <copyright file="DamageSejuaniW1.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Sejuani W (Stage 1).
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Sejuani", SpellSlot.W, 1)]
    public class DamageSejuaniW1 : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSejuaniW1" /> class.
        /// </summary>
        public DamageSejuaniW1()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
            this.Stage = 1;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new[] { 10, 17.5, 25, 32.5, 40 }[level] + ((new double[] { 4, 6, 8, 10, 12 }[level] / 100) * source.MaxHealth) + (0.15 * source.TotalMagicalDamage);
        }

        #endregion
    }
}