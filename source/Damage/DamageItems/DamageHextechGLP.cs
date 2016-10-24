// <copyright file="DamageHextechGLP.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Hextech GLP-800 item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.HextechGLP)]
    public class DamageHextechGLP : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageHextechGLP" /> class.
        /// </summary>
        public DamageHextechGLP()
        {
            this.ItemId = 3030;
            this.DamageType = Damage.DamageType.Magical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return
                new[] { 100, 106, 112, 118, 124, 130, 136, 141, 147, 153, 159, 165, 171, 176, 182, 188, 194, 200 }[
                    source.Level - 1] + (.35 * source.TotalMagicalDamage);
        }

        #endregion
    }
}