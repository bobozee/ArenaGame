namespace ArenaGame {

    public class BattleEnvironment(List<IAttacker> allies) {
        
        List<IAttacker> _allies = allies;
        public List<IAttacker> getAllies() { return _allies; }

    }

    public record Battle(IAttacker[] attackers, IAttackable[] defenders, BattleEnvironment surroundings) {

        private readonly IAttacker[] _attackers = attackers;
        private readonly IAttackable[] _defenders = defenders;
        private readonly BattleEnvironment _surroundings = surroundings;

        public void resolveBattle() {
            foreach (IAttacker attacker in _attackers) {
                Attack[] attacks = attacker.createAttacks(_defenders, _surroundings);
                foreach (Attack attack in attacks) {
                    foreach(IAttackable target in attack.getTargets()) {
                       target.receiveAttack(attack); 
                    }
                }
            }
        }
    }

}