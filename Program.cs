using ArenaGame;

public class Program
{
	public static void Main()
	{
		Program p = new Program();
        p.start();
	}

    public void start() {
        Footman lancelot = new Footman("Sir Lancelot", "The master Lancelot");
        lancelot.equipItem(new Bow("Trusty ol' bow"));

        Arena arena = new Arena(new SimpleArenaRules(), [
            new Singular(lancelot),
            new Singular(new Sentinel("Squire Robert", "The foolish Robert"))
        ]);
        
        arena.play();
    }
}