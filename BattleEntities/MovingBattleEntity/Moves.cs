using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;

namespace ArenaGame {

    public static class AttackMoves {
        public static List<MovedAttack> Beat(MoveContext cxt, List<MovedAttack> input) {
            input.addOnto(
                cxt.getPossibleTargets()[0],
                [
                    new MoveValue(MoveType.BLUNT, cxt.att()),
                ]
            );
            return input;
        }

        public static List<MovedAttack> Slash(MoveContext cxt, List<MovedAttack> input) {
            input.addOnto(
                cxt.getPossibleTargets()[0],
                [
                    new MoveValue(MoveType.SLASHING, cxt.att()*0.8f),
                ]
            );
            return input;
        }

        public static List<MovedAttack> Pierce(MoveContext cxt, List<MovedAttack> input) {
            input.addOnto(
                cxt.getPossibleTargets()[0],
                [
                    new MoveValue(MoveType.PIERCING, cxt.att()*0.8f),
                ]
            );
            return input;
        }
    }

    public static class DefenceMoves {
        public static MovedAttack Evade(MoveContext cxt, MovedAttack input) {
            if (MoveCommands.dice(Math.Clamp(0.4f-input.getMoveValues().getValue(MoveType.QUICK), 0, 1))) {
                return new(new([]), new ComplexBattleStats(0,0,0,0), null);
            }
            return input;
        }

        public static MovedAttack Block(MoveContext cxt, MovedAttack input) {
            return new(input, [
                new MoveValue(MoveType.BLUNT, cxt.def()*-0.6f),
                new MoveValue(MoveType.PIERCING, cxt.def()*-1.33f),
                new MoveValue(MoveType.BLAST, cxt.def()*-0.4f),
                new MoveValue(MoveType.RANGED, cxt.def()*-1.33f),
            ]);
        }
 
        public static MovedAttack Parry(MoveContext cxt, MovedAttack input) {
            return new(input, [
                new MoveValue(MoveType.PIERCING, cxt.def()*-1f),
            ]);
        }

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
        MAGIC,
        BLAST,
        RANGED,
        POISONOUS,
        QUICK,
    }

}