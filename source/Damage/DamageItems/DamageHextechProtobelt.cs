// <copyright file="DamageHextechProtobelt.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Hextech Protobelt-01 item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.HextechProtobelt)]
    public class DamageHextechProtobelt : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageHextechProtobelt" /> class.
        /// </summary>
        public DamageHextechProtobelt()
        {
            this.ItemId = 3152;
            this.DamageType = Damage.DamageType.Magical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            return
                new[] { 75, 79, 83, 88, 92, 97, 101, 106, 110, 115, 119, 124, 128, 132, 137, 141, 146, 150 }[
                    source.Level - 1] + (.35 * source.TotalMagicalDamage);
        }

        #endregion
    }
}