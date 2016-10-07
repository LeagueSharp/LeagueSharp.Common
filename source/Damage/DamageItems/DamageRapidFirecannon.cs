// <copyright file="DamageRapidFirecannon.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common.DamageItems
{
    using System.ComponentModel.Composition;

    /// <summary>
    ///     Rapid Firecannon item damage.
    /// </summary>
    [Export(typeof(IDamageItem))]
    [ExportMetadata("Item", Damage.DamageItems.RapidFirecannon)]
    public class DamageRapidFirecannon : DamageItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageRapidFirecannon" /> class.
        /// </summary>
        public DamageRapidFirecannon()
        {
            this.ItemId = 3094;
            this.DamageType = Damage.DamageType.Magical;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override double GetDamage(Obj_AI_Hero source, Obj_AI_Base target)
        {
            if (source.GetBuff("ItemStatikShankCharge")?.Count == 100)
            {
                return
                    new[] { 50, 50, 50, 50, 50, 58, 66, 75, 83, 92, 100, 109, 117, 126, 134, 143, 151, 160 }[
                        source.Level - 1];
            }

            return 0;
        }

        #endregion
    }
}