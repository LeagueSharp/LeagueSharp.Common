// <copyright file="DamageKogMawE.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, KogMaw E.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("KogMaw", SpellSlot.E)]
    public class DamageKogMawE : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageKogMawE" /> class.
        /// </summary>
        public DamageKogMawE()
        {
            this.Slot = SpellSlot.E;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 60, 110, 160, 210, 260 }[level] + (0.7 * source.TotalMagicalDamage);
        }

        #endregion
    }
}