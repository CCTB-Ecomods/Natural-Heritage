public class Erosion : WorldEvent
{
    private const string _name = "erosion";
    private const string _description =
        "One of your fields has been so intensely used that some of its topsoil has been washed away. It will suffer a productivity penalty for the next two turns.";

    public Erosion() : base(_name, _description)
    {
        //nothing to do here after the base constructor was called
    }

    public override bool EventTakesPlace(WorldData world)
    {
        foreach (IArea habitat in world.areas)
        {
            if (habitat.GetTileType() == TileType.FIELD &&
                habitat.GetAverageBiodiversity() < 10 &&
                GameLogic.Random(100) > 90)
            {
                foreach (TileData td in habitat.GetTileList())
                {
                    Bonus bp = new Bonus("erosion", -10, 2);
                    td.productivityBonus.Add(bp);
                }

                return true;
            }
        }
        return false;
    }
}
