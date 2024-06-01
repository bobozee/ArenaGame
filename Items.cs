namespace ArenaGame {

    public class Item(Func<MoveContext, List<MovedAttack>, List<MovedAttack>> move, string name) {
        Func<MoveContext, List<MovedAttack>, List<MovedAttack>> _move = move;
        string _name = name;

        public Func<MoveContext, List<MovedAttack>, List<MovedAttack>> getMove() { return _move; }
        public string getName() { return _name; }
    }

    public class Bow(string name) : Item(AttackMoves.ShootArrow, name) {}

}

