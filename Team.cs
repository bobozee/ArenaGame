using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

namespace ArenaGame
{
    public record Team(List<IAttacker> entities, TeamFlag flag) : EntitySide {
        private readonly List<IAttacker> _entities = entities;
        private readonly TeamFlag _flag = flag;

        public override IAttacker[] getEntities() { return _entities.ToArray(); }
        public override IBattleEntity[] getAliveEntities() {
            return _entities
                .Where(x => x is IBattleEntity)
                .Select(x => (IBattleEntity)x)
                .Where(x => !x.isDead()).ToArray();
        }
        public override SidePresentation getPresentation() { return _flag; }

    }

    public record Singular(IBattleEntity entity) : EntitySide {
        private IBattleEntity _entity = entity;
        public override IBattleEntity[] getEntities() { return [entity]; }
        public override IBattleEntity[] getAliveEntities() { return entity.isDead() ? [] : [entity]; }
        public override SidePresentation getPresentation() { return new(_entity.getName()); }
    }

    public abstract record EntitySide : Side {
        public override bool hasLost() { return getAliveEntities().Length == 0; }
        public abstract IBattleEntity[] getAliveEntities();
    }

    public abstract record Side {
        public abstract bool hasLost();
        public abstract IAttacker[] getEntities();
        public abstract SidePresentation getPresentation();

    }

    public record SidePresentation(string name) {
        private string _name = name;
        public string getName() { return _name; }
    }

    public record TeamFlag(string teamName, string teamMotto, Color teamColor) : SidePresentation(teamName)
    { 
        private readonly string _teamMotto = teamMotto;
        private readonly Color _teamColor = teamColor;

        public string getTeamMotto() { return _teamMotto; }
        public Color getTeamColor() { return _teamColor; }
    }
}
