using Assets.Scripts.Core;
using Assets.Scripts.Models;
using Assets.Scripts.Units;

namespace Assets.Scripts
{
    public class BattleManager : Singleton<BattleManager>
    {
        public Unit SpawnUnit(string unitType, UnitLevel unitLevel, int playerId)
        {
            Unit unit;

            if (unitLevel is MobLevel)
            {
                unit = new Mob(unitType, unitLevel as MobLevel, playerId);
            }
            else
            {
                unit = new Unit(unitType, unitLevel, playerId);
            }

            return unit;
        }

        public void Attack(IAttacker attacker, IDestroyable defender)
        {
            if (attacker.PlayerId == defender.PlayerId) return;
            if (!attacker.CanAttack) return;
            attacker.ProduceAttack();
            ApplyDamage(attacker, attacker.Attack, defender);
        }

        public void ApplyDamage(IAttacker attacker, float damage, IDestroyable defender)
        {
            if (defender.HP <= 0) return;
            float resultDamage = damage * (1 - defender.Armor);
            defender.HP -= resultDamage;
           
            if (defender.HP <= 0)
            {
                attacker.OnKill(defender);

                Mob mob = defender as Mob;
                if (mob != null)
                {
                    PlayerManager.Instance.GetPlayer(attacker.PlayerId).ProcessAward(mob.AwardGold);
                }
            }
        }

        public void CastAbility(Hero caster, Ability ability, CastContext castContext)
        {
            ability.Apply(castContext);
        }
    }
}