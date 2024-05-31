namespace ArenaGame {

    public static class AttackMoves {
        public static MovedAttack Hit(float val, MovedAttack input) {
            return new(input, new(input.getMoveValues(), [

                new(MoveType.BLUNT, val)

            ]));
        }

        public static MovedAttack Slash(float val, MovedAttack input) {
            return new(input, new(input.getMoveValues(), [

                new(MoveType.SLASHING, val*0.85f)

            ]));
        }
    }

    public static class DefenceMoves {

    }

    public static class TurnMoves {

    }


    public static class MoveCommands {

        public static bool dice(float chance) { return Random.Shared.NextDouble() <= chance; }

    }

    //! max 32 types
    public enum MoveType {
        PIERCING,
        SLASHING,
        BLUNT,
        MAGICAL,
        EXPLOSIVE,
        RANGED,
        PHYSICAL,
        POISONOUS,
        QUICK,
        TEMPORAL,
    }

}