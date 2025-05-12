public class GoodWeather : WorldEvent
{
    private const string _name = "good weather";

    private const string _description =
        "The weather has been kind to you this year. Your fields are more productive and have a bumper crop.";

    public GoodWeather() : base(_name, _description)
    {
        //nothing to do here after the base constructor was called
    }

    public override bool EventTakesPlace(WorldData world)
    {
        var success = false;
        foreach (var td in world.tiles)
            if (td.type.Type == TileType.FIELD && td.intensity > 0)
            {
                td.productivityBonus.Add(new Bonus("good weather", 5));
                success = true;
            }

        return success;
    }
}