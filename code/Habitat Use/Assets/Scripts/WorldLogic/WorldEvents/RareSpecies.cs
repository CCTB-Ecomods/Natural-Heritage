public class RareSpecies : WorldEvent
{
    private const string _name = "rare species";

    private const string _description =
        "A population of a rare species has established itself in one of your wilderness areas. Its habitat will get a biodiversity bonus as long as it remains unused.";

    public RareSpecies() : base(_name, _description)
    {
        //nothing to do here after the base constructor was called
    }

    public override bool EventTakesPlace(WorldData world)
    {
	    foreach (IArea habitat in world.areas)
	    {
		    if (habitat.GetIntensity() == Intensity.NONE &&
		        habitat.GetAverageBiodiversity() > 35 &&
		        GameLogic.Random(100) > 0)
		    {
			    foreach (TileData td in habitat.GetTileList())
			    {
				    Bonus b = new Bonus("rare species", 3, () => td.intensity == Intensity.NONE);
				    td.diversityBonus.Add(b);
				    td.productivityBonus.Add(b);
			    }
			    return true;
		    }
	    }
	    return false;
    }
}
