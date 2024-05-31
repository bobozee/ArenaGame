namespace ArenaGame {
    
    public class Sentinel(string name, string description) : MovingBattleEntity(
        name,
        description,
        new(initialHp: 70),
        new(attack: 20, defense: 5, armor: 0, piercing: 5),
        new([

        ]),
        new([
            
        ]),
        new([
            
        ])
    ) {}

    public class Footman(string name, string description) : MovingBattleEntity(
        name,
        description,
        new(initialHp: 100),
        new(attack: 30, defense: 20, armor: 5, piercing: 5),
        new([

        ]),
        new([
            
        ]),
        new([
            
        ])
    ) {}

}