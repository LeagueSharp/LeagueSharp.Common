using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace LeagueSharp.Common.DWIntegration
{
    public class TargetSpell
    {
        public int blockBelow = 0;

        public Obj_AI_Hero Caster { get; set; }
        public Obj_AI_Base Target { get; set; }
        public TargetSpellData Spell { get; set; }
        public int StartTick { get; set; }
        public Vector2 StartPosition { get; set; }

        public bool hadPart = false;
        public GameObject particle { get; set; }
        public GameObjectProcessSpellCastEventArgs spellArgs;

        public String particleName;

        public int created = 0;

        public int EndTick
        {
            get { return (Spell == null) ? 0 : (int)(StartTick + Spell.Delay + 1000 * (StartPosition.Distance(EndPosition) / Spell.Speed)); }
        }

        public Vector2 EndPosition
        {
            get { return Target.ServerPosition.To2D(); }
        }

        public Vector2 Direction
        {
            get { return (EndPosition - StartPosition).Normalized(); }
        }

        public double Damage
        {
            get { return Caster.GetSpellDamage(Target, Spell.Name); }
        }

        public bool IsKillable
        {
            get { return Damage >= Target.Health; }
        }

        public CcType CrowdControl
        {
            get { return Spell.CcType; }
        }

        public SpellType Type
        {
            get { return Spell.Type; }
        }

        public bool IsActive
        {
            get { return Environment.TickCount <= EndTick || StartTick + 4000 < Environment.TickCount; }
        }

        public bool HasMissile
        {
            get { return Spell.Speed == float.MaxValue || Spell.Speed == 0; }
        }
    }

    public class TargetSpellData
    {
        public string ChampionName { get; set; }
        public SpellSlot Spellslot { get; set; }
        public SpellType Type { get; set; }
        public CcType CcType { get; set; }
        public string Name { get; set; }
        public float Range { get; set; }
        public double Delay { get; set; }
        public double Speed { get; set; }

        public TargetSpellData(string champion, string name, SpellSlot slot, SpellType type, CcType cc, float range,
            float delay, float speed)
        {
            ChampionName = champion;
            Name = name;
            Spellslot = slot;
            Type = type;
            CcType = cc;
            Range = range;
            Speed = speed;
            Delay = delay;
        }
    }

    public enum SpellType
    {
        Skillshot,
        Targeted,
        Self,
        AutoAttack
    }


    public enum CcType
    {
        No,
        Stun,
        Silence,
        Taunt,
        Polymorph,
        Slow,
        Snare,
        Fear,
        Charm,
        Suppression,
        Blind,
        Flee,
        Knockup,
        Knockback,
        Pull
    }
}
