using ArenaGame;

public class Program
{
	public static void Main()
	{
		Program p = new Program();
        p.start();
	}

    public void start() {
        Arena arena = new Arena(new SimpleArenaRules(), [
            new Singular(new Footman("Sir Lancelot", "The master Lancelot")),
            new Singular(new Sentinel("Squire Robert", "The foolish Robert"))
        ]);
        arena.play();
    }
}