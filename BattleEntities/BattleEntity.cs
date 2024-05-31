namespace ArenaGame {

    public abstract record Attack(IAttackable[] targets) {
        private IAttackable[] _targets = targets;
        public IAttackable[] getTargets() { return _targets; }
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
        private readonly Attacker _attacker = attacker;
        private readonly Defender _defender = defender;
        private readonly Turner _turner = turner;

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

    public abstract class Targeter {
        public abstract IAttackable target(IAttackable[] possibleTargets, BattleEnvironment surroundings);
    }

    public abstract class Attacker(Targeter targeter) : IAttacker {
        protected Targeter _targeter = targeter;
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