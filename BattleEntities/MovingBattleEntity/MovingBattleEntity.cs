namespace ArenaGame {

    public class MovingBattleEntity (
        string name,
        string description,
        HPLife hp,
        ComplexBattleStats stats,
        Pipeline<MoveContext, List<MovedAttack>> attackPipeline,
        Pipeline<MoveContext, MovedAttack> defensePipeline,
        Pipeline<(ComplexBattleStats, HPLife)> turnPipeline)
    : BattleEntity (
        name,
        description,
        hp,
        new MovingAttacker(stats, attackPipeline),
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

    public struct MoveContext(IAttackable[] possibleTargets, BattleEnvironment surroundings, ComplexBattleStats battleStats) {
        IAttackable[] _possibleTargets = possibleTargets;
        BattleEnvironment _surroundings = surroundings;
        ComplexBattleStats _battleStats = battleStats;

        public IAttackable[] getPossibleTargets() { return _possibleTargets; }
        public BattleEnvironment getBattleEnvironment() { return _surroundings; }
        public ComplexBattleStats getBattleStats() { return _battleStats; }
        public float att() { return _battleStats.getAttack(); }
        public float def() {return _battleStats.getDefense(); }
    }

    public class MovingAttacker(ComplexBattleStats stats, Pipeline<MoveContext, List<MovedAttack>> pipeline) : Attacker() {
        
        Pipeline<MoveContext, List<MovedAttack>> _pipeline = pipeline;
        ComplexBattleStats _stats = stats;

        public void selectMove(Func<MoveContext, List<MovedAttack>, List<MovedAttack>> move) {
            if (_pipeline.hasPipe(move)) { return; }
           _pipeline.addPipe(move);
        }
        public void deselectMove(Func<MoveContext, List<MovedAttack>, List<MovedAttack>> move) {
            _pipeline.removePipe(move);
        }
        
        public override Attack[] createAttacks(IAttackable[] targets, BattleEnvironment surroundings) {
            return _pipeline.execute(new List<MovedAttack>(), new (targets, surroundings, _stats)).ToArray();
        }

        public override string getStatsSummary() {
            return _stats.getSummary();
        }
    }

    public class MovingDefender(ComplexBattleStats stats, Pipeline<MoveContext, MovedAttack> pipeline, HPLife hp) : HPDefender(hp) {
        private HPLife _hp = hp;
        private readonly Pipeline<MoveContext, MovedAttack> _pipeline = pipeline;
        ComplexBattleStats _stats = stats;

        public void selectMove(Func<MoveContext, MovedAttack, MovedAttack> move) {
            if (_pipeline.hasPipe(move)) { return;}
           _pipeline.addPipe(move);
        }
        public void deselectMove(Func<MoveContext, MovedAttack, MovedAttack> move) {
            _pipeline.removePipe(move);
        }

        public void receiveAttack(MovedAttack attack, BattleEnvironment surroundings) {
            MovedAttack final = _pipeline.execute(attack, new (null, surroundings, _stats));
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



    public record MovedAttack(MoveValueCollection moveValues, ComplexBattleStats attackStats, IAttackable target) : Attack(target) {

        public MovedAttack(MovedAttack copy, MoveValue[] moveValues) : this(new(), copy.getAttackStats(), copy.getTarget()) {
            _moveValues = new MoveValueCollection(copy.getMoveValues(), moveValues);
        }

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
                input[(int)type] = new(type, Math.Max(0, input[(int)type].getValue() + existing.getValue(type)));
            }
            foreach(MoveValue val in input) {
                _values[(int)val.getType()] = val.getValue();
                valueExistance += 1 << (int)val.getType();
            }

        }

        public float getValue(MoveType type) { return _values[(int)type]; }
        public float getSum() { return _values.Sum()*1.15f; }
        public bool hasValue(MoveType type) { return (valueExistance & (1 << (int)type)) > 0; }
    }

}