namespace ArenaGame {

    public class MovingBattleEntity (
        string name,
        string description,
        HPLife hp,
        ComplexBattleStats stats,
        Pipeline<float, MovedAttack> attackPipeline,
        Pipeline<float, MovedAttack> defensePipeline,
        Pipeline<(ComplexBattleStats, HPLife)> turnPipeline)
    : BattleEntity (
        name,
        description,
        hp,
        new MovingAttacker(stats, new RandomTargeter(), attackPipeline),
        new MovingDefender(stats, defensePipeline, hp),
        new MovingTurner(stats, turnPipeline, hp)
    ) {}

    public class ComplexBattleStats(float attack, float defense, float armor, float piercing) : BattleStats {
        float _attack = attack;
        float _defense = defense;
        float _armor = armor;
        float _piercing = piercing;

        public float getAttack() { return _attack; }
        public void addAttack(float val) { _attack += val;}
        public float getDefense() { return _defense; }
        public void addDefense(float val) { _defense += val;}
        public float getArmor() { return _armor; }
        public void addArmor(float val) { _armor += val;}
        public float getPiercing() { return _piercing; }
        public void addPiercing(float val) { _piercing += val;}

        public override float calculateDamageAgainst(BattleStats defender) {
            return getAttack();
        }

        public float calculateDamageAgainst(ComplexBattleStats defender) {
            return Math.Max(0, getAttack() - defender.getDefense()) * Math.Min(1, getPiercing() / defender.getArmor());
        }

        public override string getSummary() {
            return "Attack: " + getAttack() + "\n" +
                   "Defense: " + getDefense() + "\n" +
                   "Armor: " + getArmor() + "\n" +
                   "Piercing: " + getPiercing();
        }
    }

    public class MovingAttacker(ComplexBattleStats stats, Targeter targeter, Pipeline<float, MovedAttack> pipeline) : Attacker(targeter) {
        
        Pipeline<float, MovedAttack> _pipeline = pipeline;
        ComplexBattleStats _stats = stats;

        public void selectMove(Func<float, MovedAttack, MovedAttack> move) {
            if (_pipeline.hasPipe(move)) { return; }
           _pipeline.addPipe(move);
        }
        public void deselectMove(Func<float, MovedAttack, MovedAttack> move) {
            _pipeline.removePipe(move);
        }
        
        public override Attack[] createAttacks(IAttackable[] targets, BattleEnvironment surroundings) {
            List<Attack> acc = new List<Attack>();
            foreach(IAttackable target in targets) {
                acc.Add(_pipeline.execute(new(new(), _stats, [target]), _stats.getAttack()));
            }
            return acc.ToArray();
        }

        public override string getStatsSummary() {
            return _stats.getSummary();
        }
    }

    public class MovingDefender(ComplexBattleStats stats, Pipeline<float, MovedAttack> pipeline, HPLife hp) : HPDefender(hp) {
        private HPLife _hp = hp;
        private readonly Pipeline<float, MovedAttack> _pipeline = pipeline;
        ComplexBattleStats _stats = stats;

        public void selectMove(Func<float, MovedAttack, MovedAttack> move) {
            if (_pipeline.hasPipe(move)) { return;}
           _pipeline.addPipe(move);
        }
        public void deselectMove(Func<float, MovedAttack, MovedAttack> move) {
            _pipeline.removePipe(move);
        }

        public void receiveAttack(MovedAttack attack) {
            MovedAttack final = _pipeline.execute(attack, _stats.getAttack());
            _hp.changeHp(final.getAttackStats().calculateDamageAgainst(_stats));
        }
    }

    public class MovingTurner(ComplexBattleStats stats, Pipeline<(ComplexBattleStats, HPLife)> pipeline, HPLife hp) : Turner {

        private HPLife _hp = hp;
        private readonly Pipeline<(ComplexBattleStats, HPLife)> _pipeline = pipeline;
        ComplexBattleStats _stats = stats;

        public void selectMove(Func<(ComplexBattleStats, HPLife), (ComplexBattleStats, HPLife)> move) {
            if (_pipeline.hasPipe(move)) { return; }
           _pipeline.addPipe(move);
        }
        public void deselectMove(Func<(ComplexBattleStats, HPLife), (ComplexBattleStats, HPLife)> move) {
            _pipeline.removePipe(move);
        }

        public override void turn(){
            _pipeline.execute((_stats, _hp));
        }

    }



    public record MovedAttack(MoveValueCollection moveValues, ComplexBattleStats attackStats, IAttackable[] targets) : Attack(targets) {

        public MovedAttack(MovedAttack copy, MoveValueCollection moveValues) : this(moveValues, copy.getAttackStats(), copy.getTargets()) {}

        MoveValueCollection _moveValues = moveValues;
        ComplexBattleStats _attackStats = attackStats;
        public MoveValueCollection getMoveValues() { return _moveValues; }
        public ComplexBattleStats getAttackStats() {
            return _attackStats;
        }
        public override float getAttack() {
            return _moveValues.getSum();
        }

    }

    public readonly struct MoveValue(MoveType type, float value) {
        private readonly float _value = value;
        private readonly MoveType _type = type;
        public readonly float getValue() { return _value; }
        public readonly MoveType getType() { return _type; }
    }

    public readonly struct MoveValueCollection {
        private readonly float[] _values;
        private readonly int valueExistance;

        public MoveValueCollection(MoveValue[] input) {
            _values = new float[Enum.GetNames(typeof(MoveType)).Length];
            foreach(MoveValue val in input) {
                _values[(int)val.getType()] = val.getValue();
                valueExistance += 1 << (int)val.getType();
            }
        }

        public MoveValueCollection(MoveValueCollection existing, MoveValue[] input) {
            
            _values = new float[Enum.GetNames(typeof(MoveType)).Length];

            foreach (MoveType type in Enum.GetValues(typeof(MoveType))) {
                input[(int)type] = new(type, input[(int)type].getValue() + existing.getValue(type));
            }
            foreach(MoveValue val in input) {
                _values[(int)val.getType()] = val.getValue();
                valueExistance += 1 << (int)val.getType();
            }

        }

        public float getValue(MoveType type) { return _values[(int)type]; }
        public float getSum() { return _values.Sum(); }
        public bool hasValue(MoveType type) { return (valueExistance & (1 << (int)type)) > 0; }
    }

}