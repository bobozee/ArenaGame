using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

namespace ArenaGame
{

    public abstract class ArenaRules {
        public bool turn(List<Side> sides) {
            controlPhase(sides);
            attackPhase(sides);
            return turnPhase(sides);
        }

        protected abstract void controlPhase(List<Side> sides);

        protected abstract void attackPhase(List<Side> sides);

        protected abstract bool turnPhase(List<Side> sides);
    }

    public class SimpleArenaRules : ArenaRules
    {
        protected override void controlPhase(List<Side> sides)
        {
            
        }

        protected override void attackPhase(List<Side> sides)
        {
            foreach (Side side in sides) {
                foreach (Side otherSide in sides) {
                    if (otherSide != side) {

                        foreach(IAttacker actor in side.getEntities()) {
                            BattleEnvironment surroundings = new BattleEnvironment(side.getEntities().ToList());
                            Battle battle = new Battle([actor], otherSide.getEntities().Where(x => x is IAttackable).Select(x => (IAttackable)x).ToArray(), surroundings);
                            battle.resolveBattle();
                        }

                    }
                }
            }
        }

        protected override bool turnPhase(List<Side> sides)
        {
            foreach(Side side in sides) {
                foreach(IBattleEntity entity in side.getEntities()) {
                    entity.turn();
                }
            }
            return !(sides.FindAll(x => !x.hasLost()).Count > 1);
        }

    }

    public class Arena(ArenaRules rules, List<Side> sides) {
        List<Side> _sides = sides;
        ArenaRules _rules = rules;

        public Side[] play() {
            while (true) {
                if (_rules.turn(_sides)) { break; };
            }
            return _sides.FindAll(s => !s.hasLost()).ToArray();
        }

    }
}
