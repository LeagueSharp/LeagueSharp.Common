// <copyright file="DamageChoGathQ.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, ChoGath Q.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("ChoGath", SpellSlot.Q)]
    public class DamageChoGathQ : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageChoGathQ" /> class.
        /// </summary>
        public DamageChoGathQ()
        {
            this.Slot = SpellSlot.Q;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 80, 135, 190, 245, 305 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}