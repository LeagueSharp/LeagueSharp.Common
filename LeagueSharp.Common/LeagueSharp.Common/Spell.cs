#region

using SharpDX;

#endregion

namespace LeagueSharp.Common
{
    /// <summary>
    /// This class allows you to handle the spells easily.
    /// </summary>
    internal class Spell
    {
        public enum CastStates
        {
            SuccessfullyCasted,
            NotReady,
            NotCasted,
            OutOfRange,
            Collision,
            LowHitChance,
        }

        public bool Collision;
        public float Delay;
        public Vector3 From;
        public Prediction.HitChance MinHitChange = Prediction.HitChance.VP_HighHitchance;

        public float Range;
        public Vector3 RangeCheckFrom;

        public bool Skillshot = false;
        public SpellSlot Slot;
        public float Speed;
        public Prediction.SkillshotType Type;
        public float Width;

        public Spell(SpellSlot slot, float range)
        {
            Slot = slot;
            Range = range;
        }

        public void SetSkillshot(float delay, float width, float speed, Vector3 from, bool collision,
            Prediction.SkillshotType type, Vector3 rangeCheckFrom)
        {
            Delay = delay;
            Width = width;
            Speed = speed;
            From = from;
            Collision = collision;
            Type = type;
            RangeCheckFrom = rangeCheckFrom;
            Skillshot = true;
        }

        public CastStates Cast(Obj_AI_Base unit, bool packetCast = false, bool aoe = false)
        {
            //Spell not ready.
            if (ObjectManager.Player.Spellbook.CanUseSpell(Slot) != SpellState.Ready)
                return CastStates.NotReady;

            //Targetted spell.
            if (!Skillshot)
            {
                //Target out of range
                if (ObjectManager.Player.Distance(unit) > Range)
                    return CastStates.OutOfRange;

                if (packetCast)
                {
                    Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(unit.NetworkId, Slot)).Send();
                }
                else
                {
                    //Cant cast the Spell.
                    if (!ObjectManager.Player.Spellbook.CastSpell(Slot, unit))
                        return CastStates.NotCasted;
                }


                return CastStates.SuccessfullyCasted;
            }

            //Get the best position to cast the spell.
            var prediction = aoe
                ? Prediction.GetBestAOEPosition(unit, Delay, Width, Speed, From, Range, Collision, Type,
                    RangeCheckFrom)
                : Prediction.GetBestPosition(unit, Delay, Width, Speed, From, Range, Collision, Type,
                    RangeCheckFrom);

            //Skillshot collides.
            if (prediction.CollisionUnitsList.Count > 0)
                return CastStates.Collision;

            //Target out of range.
            if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) > Range)
                return CastStates.OutOfRange;

            //The hitchance is too low.
            if (prediction.HitChance < MinHitChange)
                return CastStates.LowHitChance;


            if (packetCast)
            {
                Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(0, Slot, -1, prediction.CastPosition.X,
                    prediction.CastPosition.Y, prediction.CastPosition.X, prediction.CastPosition.Y)).Send();
            }
            else
            {
                //Cant cast the spell (actually should not happen).
                if (!ObjectManager.Player.Spellbook.CastSpell(Slot, prediction.CastPosition))
                    return CastStates.NotCasted;
            }

            return CastStates.SuccessfullyCasted;
        }

        public void Cast(Vector3 position)
        {
            ObjectManager.Player.Spellbook.CastSpell(Slot, position);
        }
    }
}