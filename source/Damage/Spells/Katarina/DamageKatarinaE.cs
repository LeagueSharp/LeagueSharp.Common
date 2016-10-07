// <copyright file="DamageKatarinaE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Katarina E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Katarina", SpellSlot.E)]
    public class DamageKatarinaE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKatarinaE" /> class.
        /// </summary>
        public DamageKatarinaE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 40, 70, 100, 130, 160 }[level] + (0.25 * source.TotalMagicalDamage);
        }

        #endregion
    }
}