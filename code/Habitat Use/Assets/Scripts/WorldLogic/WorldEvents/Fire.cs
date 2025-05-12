using System.Collections.Generic;
using System.Linq;

public class Fire : WorldEvent
{
    private const string _name = "fire";
    private const string _description =
        "One of your forests has caught fire! Its tiles will have a two-year productivity and biodiversity penalty.";

    public Fire() : base(_name, _description)
    {
        //nothing to do here after the base constructor was called
    }

    public override bool EventTakesPlace(WorldData world)
    {
		//choose a random forest area
		List<IArea> forests = world.areas.Where(a => a.GetTileType() == TileType.FOREST).ToList();
		if (forests.Count == 0) return false;
		IArea forest = forests[GameLogic.Random(forests.Count)];
		//then give it productivity and diversity penalties
		int damage = -6;
		if (Parliament.IsEnacted("Fire Brigade Act")) damage = -3;
		foreach (TileData td in forest.GetTileList()) {
			td.diversityBonus.Add(new Bonus("fire", damage, 2));
			td.productivityBonus.Add(new Bonus("fire", damage*2, 2));
		}
		return true;
    }
}
