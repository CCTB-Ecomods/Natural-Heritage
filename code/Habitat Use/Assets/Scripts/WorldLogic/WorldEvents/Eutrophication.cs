public class Eutrophication : WorldEvent
{
    private const string _name = "eutrophication";
    private const string _description =
        "One of your water bodies has been strongly degraded. It has eutrophied and will suffer a productivity penalty for the next two turns.";

    public Eutrophication() : base(_name, _description)
    {
        //nothing to do here after the base constructor was called
    }

    public override bool EventTakesPlace(WorldData world)
    {
		foreach (IArea habitat in world.areas) {
            if (habitat.GetTileType() == TileType.RIVER &&
                habitat.GetAverageBiodiversity() < 15 &&
                GameLogic.Random(100) > 90)
            {
                foreach (TileData td in habitat.GetTileList())
                {
                    Bonus bp = new Bonus("eutrophication", -10, 2);
                    td.productivityBonus.Add(bp);
                }
                return true;
            }
        }
        return false;
    }
}
