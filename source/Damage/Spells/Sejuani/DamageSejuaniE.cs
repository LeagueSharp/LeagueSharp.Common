// <copyright file="DamageSejuaniE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Sejuani E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Sejuani", SpellSlot.E)]
    public class DamageSejuaniE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageSejuaniE" /> class.
        /// </summary>
        public DamageSejuaniE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 90, 120, 150, 180 }[level] + (0.5 * source.TotalMagicalDamage);
        }

        #endregion
    }
}