namespace LeagueSharp.Common
{
    using System.Collections.Generic;
    using System.Linq;

    using SharpDX;

    /// <summary>
    ///     Provides methods regarding items.
    /// </summary>
    public static class Items
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Returns true if the player has the item and its not on cooldown.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static bool CanUseItem(string name)
        {
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Name == name))
            {
                return ObjectManager.Player.Spellbook.CanUseSpell((SpellSlot)(slot.Slot + (int)SpellSlot.Item1))
                       == SpellState.Ready;
            }

            return false;
        }

        /// <summary>
        ///     Returns true if the player has the item and its not on cooldown.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static bool CanUseItem(int id)
        {
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId)id))
            {
                return ObjectManager.Player.Spellbook.CanUseSpell((SpellSlot)(slot.Slot + (int)SpellSlot.Item1))
                       == SpellState.Ready;
            }

            return false;
        }

        /// <summary>
        ///     Returns the ward slot.
        /// </summary>
        /// <returns></returns>
        public static InventorySlot GetWardSlot()
        {
            var wardIds = new[]
                              {
                                  2045, 2049, 2050, 2301, 2302, 2303, 3340, 3361, 3362, 3711, 1408, 1409, 1410, 1411, 2043, 2055
                              };
            return (from wardId in wardIds
                    where CanUseItem(wardId)
                    select ObjectManager.Player.InventoryItems.FirstOrDefault(slot => slot.Id == (ItemId)wardId))
                .FirstOrDefault();
        }

        /// <summary>
        ///     Returns true if the hero has the item.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="hero">The hero.</param>
        /// <returns></returns>
        public static bool HasItem(string name, Obj_AI_Hero hero = null)
        {
            return (hero ?? ObjectManager.Player).InventoryItems.Any(slot => slot.Name == name);
        }

        /// <summary>
        ///     Returns true if the hero has the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="hero">The hero.</param>
        /// <returns></returns>
        public static bool HasItem(int id, Obj_AI_Hero hero = null)
        {
            return (hero ?? ObjectManager.Player).InventoryItems.Any(slot => slot.Id == (ItemId)id);
        }

        /// <summary>
        ///     Casts the item on the target.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static bool UseItem(string name, Obj_AI_Base target = null)
        {
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Name == name))
            {
                if (target != null)
                {
                    return ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot, target);
                }
                else
                {
                    return ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot);
                }
            }

            return false;
        }

        /// <summary>
        ///     Casts the item on the target.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static bool UseItem(int id, Obj_AI_Base target = null)
        {
            foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId)id))
            {
                if (target != null)
                {
                    return ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot, target);
                }
                else
                {
                    return ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot);
                }
            }

            return false;
        }

        /// <summary>
        ///     Casts the item on a Vector2 position.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public static bool UseItem(int id, Vector2 position)
        {
            return UseItem(id, position.To3D());
        }

        /// <summary>
        ///     Casts the item on a Vector3 position.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public static bool UseItem(int id, Vector3 position)
        {
            if (position != Vector3.Zero)
            {
                foreach (var slot in ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId)id))
                {
                    return ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot, position);
                }
            }

            return false;
        }

        #endregion

        /// <summary>
        ///     Represents an item.
        /// </summary>
        public class Item
        {
            #region Fields

            /// <summary>
            ///     The range
            /// </summary>
            private float _range;

            /// <summary>
            ///     The range squared
            /// </summary>
            private float _rangeSqr;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="Item" /> class.
            /// </summary>
            /// <param name="id">The identifier.</param>
            /// <param name="range">The range.</param>
            public Item(int id, float range = 0)
            {
                this.Id = id;
                this.Range = range;
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the identifier.
            /// </summary>
            /// <value>
            ///     The identifier.
            /// </value>
            public int Id { get; private set; }

            /// <summary>
            ///     Gets or sets the range.
            /// </summary>
            /// <value>
            ///     The range.
            /// </value>
            public float Range
            {
                get
                {
                    return this._range;
                }
                set
                {
                    this._range = value;
                    this._rangeSqr = value * value;
                }
            }

            /// <summary>
            ///     Gets the range squared.
            /// </summary>
            /// <value>
            ///     The range squared.
            /// </value>
            public float RangeSqr
            {
                get
                {
                    return this._rangeSqr;
                }
            }

            /// <summary>
            ///     Gets the slots.
            /// </summary>
            /// <value>
            ///     The slots.
            /// </value>
            public List<SpellSlot> Slots
            {
                get
                {
                    return
                        ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId)this.Id)
                            .Select(slot => slot.SpellSlot)
                            .ToList();
                }
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Buys the item.
            /// </summary>
            public void Buy()
            {
                ObjectManager.Player.BuyItem((ItemId)this.Id);
            }

            /// <summary>
            ///     Casts this instance.
            /// </summary>
            /// <returns></returns>
            public bool Cast()
            {
                return UseItem(this.Id);
            }

            /// <summary>
            ///     Casts on the specified target.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <returns></returns>
            public bool Cast(Obj_AI_Base target)
            {
                return UseItem(this.Id, target);
            }

            /// <summary>
            ///     Casts at the specified position.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <returns></returns>
            public bool Cast(Vector2 position)
            {
                return UseItem(this.Id, position);
            }

            /// <summary>
            ///     Casts at the specified position.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <returns></returns>
            public bool Cast(Vector3 position)
            {
                return UseItem(this.Id, position);
            }

            /// <summary>
            ///     Determines whether the specified target is in range of the item.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <returns></returns>
            public bool IsInRange(Obj_AI_Base target)
            {
                return this.IsInRange(target.ServerPosition);
            }

            /// <summary>
            ///     Determines whether the specified vector is in range of the item.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <returns></returns>
            public bool IsInRange(Vector2 target)
            {
                return this.IsInRange(target.To3D());
            }

            /// <summary>
            ///     Determines whether the specified vector is in range of the item.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <returns></returns>
            public bool IsInRange(Vector3 target)
            {
                return ObjectManager.Player.ServerPosition.Distance(target, true) < this.RangeSqr;
            }

            /// <summary>
            ///     Determines whether the specified target owns this item.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <returns></returns>
            public bool IsOwned(Obj_AI_Hero target = null)
            {
                return HasItem(this.Id, target);
            }

            /// <summary>
            ///     Determines whether this instance is ready.
            /// </summary>
            /// <returns></returns>
            public bool IsReady()
            {
                return CanUseItem(this.Id);
            }

            #endregion
        }
    }
}
