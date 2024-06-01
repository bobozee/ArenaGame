using System.Diagnostics;

namespace ArenaGame {

    public abstract record Attack(IAttackable target) {
        private IAttackable _target = target;
        public IAttackable getTarget() { return _target; }
        public abstract float getAttack();
    }

    public interface IAttackable {
        public void receiveAttack(Attack attack);
    }

    public interface IAttacker {
        public Attack[] createAttacks(IAttackable[] targets, BattleEnvironment surroundings);
    }

    public interface IBattleEntity : IAttacker, IAttackable {
        public string getName();
        public new Attack[] createAttacks(IAttackable[] targets, BattleEnvironment surroundings);
        public new void receiveAttack(Attack att);
        public string getDescription();
        public void turn();
        public bool isDead();
        public string getLifeInfo();
        public string getStatsSummary();
    }

    public abstract class BattleEntity(string name, string description, LifeStatus life, Attacker attacker, Defender defender, Turner turner) : Entity(name, description, life), IBattleEntity {
        protected readonly Attacker _attacker = attacker;
        protected readonly Defender _defender = defender;
        protected readonly Turner _turner = turner;

        public Attack[] createAttacks(IAttackable[] targets, BattleEnvironment surroundings) {
            return _attacker.createAttacks(targets, surroundings);
        }
        public void receiveAttack(Attack att) {
            _defender.receiveAttack(att);
        }
        public void turn() {
            _turner.turn();
        }

        public string getStatsSummary() {
            return _attacker.getStatsSummary()+"\n"+_defender.getStatsSummary();
        }

        public string getLifeInfo() { 
            return _life.getLifeInfo();
        }

    }

    public abstract class Attacker : IAttacker {
        public abstract Attack[] createAttacks(IAttackable[] targets, BattleEnvironment surroundings);
        public abstract string getStatsSummary();
    }
    public abstract class Defender : IAttackable {
        public abstract void receiveAttack(Attack attack);
        public abstract string getStatsSummary();
    }
    public abstract class Turner {
        public abstract void turn();
    }

}