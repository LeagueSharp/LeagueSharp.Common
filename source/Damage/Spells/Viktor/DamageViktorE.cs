// <copyright file="DamageViktorE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Viktor E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Viktor", SpellSlot.E)]
    public class DamageViktorE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageViktorE" /> class.
        /// </summary>
        public DamageViktorE()
        {
            this.Slot = SpellSlot.E;
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