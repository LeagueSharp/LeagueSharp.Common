using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueSharp.Common.DWIntegration
{
    class HealthDeath
    {

        private static int _lastTick;

        public static readonly Dictionary<int, DamageMaker> activeDamageMakers = new Dictionary<int, DamageMaker>();

        public static Dictionary<int, Damager> damagerSources = new Dictionary<int, Damager>();

        public static readonly Dictionary<int, DamageMaker> activeTowerTargets = new Dictionary<int, DamageMaker>();

        public static List<Obj_AI_Base> minionsAround = new List<Obj_AI_Base>();

        private const int towerDamageDelay = 250;

        public static int now
        {
            get { return (int)DateTime.Now.TimeOfDay.TotalMilliseconds; }
        }

        static HealthDeath()
        {
            //TODO: some implication to remove and add back callbacks when enabling and disabling
            Game.OnUpdate += onUpdate;

            GameObject.OnCreate += onCreate;
            GameObject.OnDelete += onDelete;

            Obj_AI_Base.OnDoCast += onDoCast;

            Obj_AI_Base.OnProcessSpellCast += onMeleeStartAutoAttack;
            Spellbook.OnStopCast += onMeleeStopAutoAttack;
        }

        private static void onDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {

        }

        private static void onMeleeStartAutoAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {


            if (sender is Obj_AI_Turret)
            {
                activeTowerTargets.Remove(sender.NetworkId);
                var dMake = new DamageMaker(sender,
                    (Obj_AI_Base)args.Target,
                    null,
                    args.SData,
                    true);

                activeTowerTargets.Add(sender.NetworkId, dMake);


            }

            if (!args.SData.IsAutoAttack())
                return;
            if (args.Target != null && args.Target is Obj_AI_Base && !sender.IsMe)
            {

                var tar = (Obj_AI_Base)args.Target;
                if (damagerSources.ContainsKey(sender.NetworkId))
                {
                    damagerSources[sender.NetworkId].setTarget(tar);
                }
                else
                {
                    damagerSources.Add(sender.NetworkId, new Damager(sender, tar));
                }
            }


            if (!sender.IsMelee())
                return;

            if (args.Target is Obj_AI_Base)
            {
                activeDamageMakers.Remove(sender.NetworkId);
                var dMake = new DamageMaker(sender,
                    (Obj_AI_Base)args.Target,
                    null,
                    args.SData,
                    true);

                activeDamageMakers.Add(sender.NetworkId, dMake);
            }

            if (sender is Obj_AI_Hero)
            {
               // DeathWalker.lastDmg = now;
            }
        }

        private static void onMeleeStopAutoAttack(Spellbook sender, SpellbookStopCastEventArgs args)
        {
            //if (!sender.Owner.IsMelee())
            //    return;

            if (activeDamageMakers.ContainsKey(sender.Owner.NetworkId))
                activeDamageMakers.Remove(sender.Owner.NetworkId);

            //Ranged aswell
            if (args.DestroyMissile && activeDamageMakers.ContainsKey(args.MissileNetworkId))
                activeDamageMakers.Remove(args.MissileNetworkId);
            if (damagerSources.ContainsKey(sender.Owner.NetworkId))
                damagerSources[sender.Owner.NetworkId].setTarget(null);
        }

        private static void onUpdate(EventArgs args)
        {
            //Some failsafe l8er if needed

            //Hope it wont lag :S
            minionsAround = MinionManager.GetMinions(1700, MinionTypes.All, MinionTeam.NotAlly, MinionOrderTypes.None);

            foreach (var minion in minionsAround)
            {
                if (!damagerSources.ContainsKey(minion.NetworkId))
                    damagerSources.Add(minion.NetworkId, new Damager(minion, null));
            }

            damagerSources.ToList()
                .Where(pair => !pair.Value.isValidDamager())
                .ToList()
                .ForEach(pair => damagerSources.Remove(pair.Key));

            if (now - _lastTick <= 60 * 1000)
            {
                return;
            }

            activeDamageMakers.ToList()
                .Where(pair => pair.Value.createdTick < now - 60000)
                .ToList()
                .ForEach(pair => activeDamageMakers.Remove(pair.Key));




            _lastTick = now;
        }

        private static void onCreate(GameObject sender, EventArgs args)
        {
            //most likely AA
            if (sender is MissileClient)
            {
                var mis = (MissileClient)sender;
                if (mis.Target is Obj_AI_Base)
                {
                    var dMake = new DamageMaker(mis.SpellCaster,
                        (Obj_AI_Base)mis.Target,
                        mis,
                        mis.SData);

                    activeDamageMakers.Add(mis.NetworkId, dMake);
                }
            }
        }

        private static void onDelete(GameObject sender, EventArgs args)
        {
            damagerSources.Remove(sender.NetworkId);

            if (sender is MissileClient || sender is Obj_SpellMissile)
            {
                if (activeDamageMakers.ContainsKey(sender.NetworkId))
                    activeDamageMakers.Remove(sender.NetworkId);
            }

            if (sender is Obj_AI_Base)
            {
                int i = 0;
                foreach (var dmgMk in activeDamageMakers)
                {
                    if (dmgMk.Value.source == null || dmgMk.Value.missle == null)
                        continue;
                    if (dmgMk.Value.source.NetworkId == sender.NetworkId)
                    {
                        activeDamageMakers.Remove(dmgMk.Value.missle.NetworkId);
                        return;
                    }
                    i++;
                }
            }
        }
        //Maybe later change so would return data about missile
        public static DamageMaker attackedByTurret(AttackableUnit unit)
        {
            return activeTowerTargets.Values.Where(v => v.target.NetworkId == unit.NetworkId).FirstOrDefault(attack => attack.source is Obj_AI_Turret);
        }

        //Only active attacks
        public static int getTimeTillDeath(AttackableUnit unit, bool ignoreAlmostDead = true)
        {
            int HP = (int)unit.Health;
            foreach (var attacks in activeDamageMakers.Values.OrderBy(atk => atk.hitOn))
            {
                if (attacks.target == null || attacks.target.NetworkId != unit.NetworkId || (ignoreAlmostDead && almostDead(attacks.source)))
                    continue;
                int hitOn = attacks.hitOn;
                if (hitOn > now)
                {
                    HP -= (int)attacks.dealDamage;
                    if (HP <= 0)
                        return hitOn - now;
                }
            }
            return int.MaxValue;
        }

        public static bool almostDead(AttackableUnit unit)
        {
            if (unit == null)
                return true;
            try
            {

                var hitingUnitDamage = misslesHeadedOnDamage(unit);
                // if (unit.Health < hitingUnitDamage * 0.65)
                //    Console.WriteLine("Ignore cus almost dead!");

                return unit.Health < hitingUnitDamage * 0.65;
            }
            catch (Exception)
            {
                return true;
            }

        }

        public static float GetHealthPrediction(AttackableUnit unit, int msTime, bool ignoreAlmostDead = true)
        {
            var predDmg = 0f;
            var predDmgPlus500ms = 0f;

            foreach (var attacks in activeDamageMakers.Values)
            {
                if (attacks.target == null || attacks.target.NetworkId != unit.NetworkId || (ignoreAlmostDead && almostDead(attacks.source)))
                    continue;
                int hitOn = 0;
                if (attacks.missle == null || attacks.sData.MissileSpeed == 0)
                {
                    hitOn = (int)(attacks.createdTick + attacks.source.AttackCastDelay * 1000);
                }
                else
                {
                    hitOn = now + (int)((attacks.missle.Position.Distance(unit.Position) * 1000) / attacks.sData.MissileSpeed) + 100;
                }

                if (now < hitOn && hitOn < now + msTime)
                {
                    predDmg += attacks.dealDamage;
                }
            }
            return unit.Health - predDmg;
        }

        public static float GetLastHitPredPeriodic(AttackableUnit unit, int msTime, bool ignoreAlmostDead = true)
        {
            var predDmg = 0f;

            msTime = (msTime > 10000) ? 10000 : msTime;

            foreach (var attacks in activeDamageMakers.Values)
            {
                if (attacks.target == null || attacks.target.NetworkId != unit.NetworkId || (ignoreAlmostDead && almostDead(attacks.source)) || attacks.source.IsMe)
                    continue;
                int hitOn = 0;
                if (attacks.missle == null || attacks.sData.MissileSpeed == 0)
                {
                    hitOn = (int)(attacks.createdTick + attacks.source.AttackCastDelay * 1000);
                }
                else
                {
                    hitOn = now + (int)((attacks.missle.Position.Distance(unit.Position) * 1000) / attacks.sData.MissileSpeed);
                }

                int timeTo = now + msTime;

                int hits = (attacks.cycle == 0) ? 0 : (int)((timeTo - hitOn) / attacks.cycle) + 1;

                if (now < hitOn && hitOn <= now + msTime)
                {
                    predDmg += attacks.dealDamage * hits;
                }
            }
            return unit.Health - predDmg;
        }

        public static float GetLaneClearPred(AttackableUnit unit, int msTime, bool ignoreAlmostDead = true)
        {
            float predictedDamage = 0;
            var damageDoneTill = now + msTime;
            foreach (var damager in damagerSources.Values)
            {
                if (!damager.isValidDamager() || !(unit is Obj_AI_Base))
                    continue;
                var target = damager.getTarget();
                if (target == null || target.NetworkId != unit.NetworkId || (ignoreAlmostDead && almostDead(damager.source)))
                    continue;
                if (damager.firstHitAt > damageDoneTill)
                    continue;
                damager.firstHitAt = (damager.firstHitAt < now) ? now + damager.cycle : damager.firstHitAt;
                predictedDamage += damager.damage;
                //Console.WriteLine(damager.damage);
                //Can be optimized??
                var nextAA = damager.firstHitAt + damager.cycle;
                while (damageDoneTill > nextAA)
                {
                    predictedDamage += damager.damage;
                    nextAA += damager.cycle;
                }
            }
            //if (predictedDamage > 0)
            // Console.WriteLine("dmg: " + predictedDamage);
            return unit.Health - predictedDamage;
        }

        public static int misslesHeadedOn(AttackableUnit unit)
        {
            return activeDamageMakers.Count(un => un.Value.target.NetworkId == unit.NetworkId);
        }

        public static float misslesHeadedOnDamage(AttackableUnit unit)
        {
            return activeDamageMakers.Where(un => un.Value.target.NetworkId == unit.NetworkId).Sum(un => un.Value.dealDamage);
        }
        //Used for laneclear
        public class Damager
        {
            public Obj_AI_Base source;

            private Obj_AI_Base target;

            public int createdTick;

            public int cycle;

            public int firstHitAt;

            public int lastAATry;

            public float damage;

            public Damager(Obj_AI_Base s, Obj_AI_Base t)
            {
                source = s;
                target = t;
                createdTick = now;
                cycle = (int)(source.AttackDelay * 1000);
                firstHitAt = hitOn;
                damage = getDamage();
                lastAATry = now;
            }

            public bool isValidDamager()
            {
                return source != null && source.IsValid && !source.IsDead;
            }

            public bool isValidTarget()
            {
                return target != null && target.IsValid && !target.IsDead;
            }

            public void setTarget(Obj_AI_Base tar)
            {
                if (tar == null)
                {
                    target = null;
                    return;
                }
                if (target != null && target.NetworkId == tar.NetworkId)
                    return;
                target = tar;
                createdTick = now;
                firstHitAt = hitOn;
                damage = getDamage();

            }

            public Obj_AI_Base getTarget()
            {
                if (isValidTarget())
                    return target;
                var predTarget = minionsAround.Where(min => !min.IsDead && min.Distance(source, true) < 650 * 650)
                        .OrderBy(min => min.Distance(source.Position, true))
                        .FirstOrDefault();
                setTarget(predTarget);
                return predTarget;
            }

            private float getDamage()
            {
                var tar = getTarget();
                if (tar == null || source == null)
                    return 0;
                // Console.WriteLine("Return damge");
                return (float)source.GetAutoAttackDamage(tar, true);
            }

            private int hitOnTar(Obj_AI_Base tar)
            {
                if (tar == null)
                    return int.MaxValue;
                int addTime = 0;
                if (Orbwalking.InAutoAttackRange(source, tar))//+ check if want to move to killabel minion and range it wants to
                {
                    var realDist = Orbwalking.RealDistanceTillUnit(source, target);
                    var aaRange = Orbwalking.GetRealAutoAttackRange(source, tar);

                    addTime += (int)(((realDist - aaRange) * 1000) / source.MoveSpeed);
                }
                //TODO add all checks for range cahmps with melee like azir!
                if (source.IsMelee)
                {
                    return (int)(createdTick + source.AttackCastDelay * 1000) + addTime;
                }
                else
                {
                    return createdTick + (int)((source.Position.Distance(tar.Position) * 1000) / (source.BasicAttack.MissileSpeed)) + ((source is Obj_AI_Turret) ? towerDamageDelay : 0) + addTime;//lil delay cus dunno l8er could try to values what says delay of dmg dealing
                }
            }

            private int hitOn
            {
                get
                {
                    try
                    {
                        if (source == null || !source.IsValid)
                            return int.MaxValue;
                        var tar = getTarget();
                        return hitOnTar(tar);

                    }
                    catch (Exception)
                    {
                        return int.MaxValue;
                    }
                }
            }

        }

        public class DamageMaker
        {
            public readonly GameObject missle;

            public readonly Obj_AI_Base source;

            public readonly Obj_AI_Base target;

            public readonly SpellData sData;

            public readonly float fullDamage;//Unused for now

            public readonly float dealDamage;

            public readonly bool isAutoAtack;

            public readonly int createdTick;

            public readonly bool melee;

            public readonly int cycle;

            public int hitOn
            {
                get
                {
                    try
                    {
                        if (source == null || !source.IsValid)
                            return int.MaxValue;
                        if (missle == null)
                        {
                            return (int)(createdTick + source.AttackCastDelay * 1000);
                        }
                        else
                        {
                            return now + (int)((missle.Position.Distance(target.Position) * 1000) / (sData.MissileSpeed)) + ((source is Obj_AI_Turret) ? towerDamageDelay : 0);//lil delay cus dunno l8er could try to values what says delay of dmg dealing
                        }

                    }
                    catch (Exception)
                    {
                        return int.MaxValue;
                    }
                }
            }

            public DamageMaker(Obj_AI_Base sourceIn, Obj_AI_Base targetIn, GameObject missleIn, SpellData dataIn, bool meleeIn = false)
            {
                source = sourceIn;
                target = targetIn;
                missle = missleIn;
                sData = dataIn;
                melee = !meleeIn;
                createdTick = now;
                isAutoAtack = sData.IsAutoAttack();

                if (isAutoAtack)
                {

                    dealDamage = (float)source.GetAutoAttackDamage(target, true);
                    if (source.IsMelee)
                        cycle = (int)(source.AttackDelay * 1000);
                    else
                    {
                        //var dist = source.Distance(target);
                        cycle = (int)((source.AttackDelay * 1000)) /*+ (dist*1000)/sData.MissileSpeed)*/;
                        //Console.WriteLine("cycle: " + cycle);
                    }
                    //Console.WriteLine("cycle: " + source.AttackSpeedMod);
                }
                else
                {
                    cycle = 0;
                    if (source is Obj_AI_Hero)
                    {
                        var tSpell = TargetSpellDatabase.GetByName(sData.Name);
                        if (tSpell == null)
                        {
                            //Console.WriteLine("Unknown targeted spell: " + sData.Name);
                            dealDamage = 0;
                        }
                        else
                        {
                            try
                            {

                                dealDamage = (float)((Obj_AI_Hero)source).GetSpellDamage(target, tSpell.Spellslot);
                            }
                            catch (Exception)
                            {
                                dealDamage = 0;
                            }
                        }
                    }
                    else
                    {
                        dealDamage = 0;
                    }
                }


            }

        }

    }
}
