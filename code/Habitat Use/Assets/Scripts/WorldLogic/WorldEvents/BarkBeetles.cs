public class BarkBeetles : WorldEvent
{
    private const string _name = "bark beetles";

    private const string _description =
        "One of your forests had such a low biodiversity that a bark beetle infestation has developed. It will suffer a productivity penalty for the next three turns.";

    public BarkBeetles() : base(_name, _description)
    {
        //nothing to do here after the base constructor was called
    }

    public override bool EventTakesPlace(WorldData world)
    {
        foreach (IArea habitat in world.areas)
        {
            if (habitat.GetTileType() == TileType.FOREST &&
                habitat.GetAverageBiodiversity() < 10 &&
                GameLogic.Random(100) > 90)
            {
                foreach (TileData td in habitat.GetTileList())
                {
                    Bonus bp = new Bonus("bark beetles", -10, 3);
                    td.productivityBonus.Add(bp);
                }
                return true;
                
            }
        }
        return false;
    }
}
