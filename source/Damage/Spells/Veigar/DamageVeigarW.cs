// <copyright file="DamageVeigarW.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.Spells
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Spell Damage, Veigar W.
    /// </summary>
    [Export(typeof(IDamageSpell))]
    [ExportDamageMetadata("Veigar", SpellSlot.W)]
    public class DamageVeigarW : DamageSpell
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageVeigarW" /> class.
        /// </summary>
        public DamageVeigarW()
        {
            this.Slot = SpellSlot.W;
            this.DamageType = Common.Damage.DamageType.Magical;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override double GetDamage(Obj_AI_Base source, Obj_AI_Base target, int level)
        {
            return new double[] { 120, 170, 220, 270, 320 }[level] + (1 * source.TotalMagicalDamage);
        }

        #endregion
    }
}