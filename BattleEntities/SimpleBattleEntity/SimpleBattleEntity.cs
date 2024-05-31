namespace ArenaGame{

    public class SimpleBattleEntity(string name, string description, HPLife hp, SimpleBattleStats stats) : BattleEntity(
        name,
        description,
        hp,
        new SimpleAttacker(stats, new RandomTargeter()),
        new HPDefender(hp),
        new SimpleTurner()
    ) {}

    public class SimpleBattleStats(float attack) : BattleStats {
        float _attack = attack;
        public float getAttack() { return _attack; }
        public float changeAttack() { return _attack; }

        public override float calculateDamageAgainst(BattleStats defender) {
            return _attack;
        }

        public override string getSummary() {
            return "Attack: " + getAttack();
        }
    }

    public record SimpleAttack(float attack, IAttackable[] targets) : Attack(targets) {
        float _attack = attack;
        public override float getAttack() { return _attack; }
    }

    public class SimpleAttacker(SimpleBattleStats stats, Targeter targeter) : Attacker(targeter) {
        SimpleBattleStats _stats = stats;
        public override Attack[] createAttacks(IAttackable[] targets, BattleEnvironment surroundings) {
            return [
                new SimpleAttack(
                    _stats.getAttack(),
                    [
                        _targeter.target(targets, surroundings)
                    ]
                )
            ];
        }

        public override string getStatsSummary() {
            return _stats.getSummary();
        }

    }

    public class HPDefender(HPLife hPLife) : Defender {
        private HPLife _hPLife = hPLife;
        public override void receiveAttack(Attack attack) {
            _hPLife.changeHp(attack.getAttack()*-1f);
        }

        public override string getStatsSummary() { return ""; }
    }

    public class SimpleTurner : Turner {
        public override void turn() {}
    }

    public class RandomTargeter : Targeter {
        public override IAttackable target(IAttackable[] potentialTargets, BattleEnvironment surroundings) {
            return potentialTargets.RandomElement();
        }
    }
    
    public abstract class BattleStats {
        public abstract float calculateDamageAgainst(BattleStats defender);
        public abstract string getSummary();

    }

}