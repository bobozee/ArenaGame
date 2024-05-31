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
            throw new NotImplementedException();
        }

        protected override void attackPhase(List<Side> sides)
        {
            foreach (Side side in sides) {

            }
        }

        protected override bool turnPhase(List<Side> sides)
        {
            throw new NotImplementedException();
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
