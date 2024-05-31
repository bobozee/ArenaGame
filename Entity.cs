using System.ComponentModel;

namespace ArenaGame {
    
    public abstract class Entity(string name, string description, LifeStatus life) {
        
        protected readonly string _name = name;
        protected readonly string _description = description;
        protected readonly LifeStatus _life = life;

        public string getName() { return _name; }
        public string getDescription() { return _description; }
        public bool isDead() { return _life.isDead(); }
    }

    public interface IKillable {
        public void kill();
    }

    public interface IRevivable {
        public void revive();
    }

    public abstract class LifeStatus : IKillable, IRevivable {
        public abstract bool isDead();
        public abstract void kill();
        public abstract void revive();
        public abstract string getLifeInfo();
    }

    public class HPLife(float initialHp) : LifeStatus {
        private float _initialHp = initialHp;
        private float _hp = initialHp;
        public void changeHp(float val) { _hp += val;}
        public override void kill() { _hp = 0; }
        public override void revive() { _hp = _initialHp; }
        public override bool isDead() { return _hp <= 0; }
        public override string getLifeInfo() { return "HP: " + _hp; }
    }

}