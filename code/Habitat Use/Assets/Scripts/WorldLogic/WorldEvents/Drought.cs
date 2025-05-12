public class Drought : WorldEvent
{
    private const string _name = "drought";
    private const string _description =
        "Your region has been hit by a drought! All tiles with a use intensity of at least one will have a productivity penalty this year.";

    public Drought() : base(_name, _description)
    {
        //nothing to do here after the base constructor was called
    }

    public override bool EventTakesPlace(WorldData world)
    {
        foreach (var td in world.tiles)
            if (td.intensity > 0)
                td.productivityBonus.Add(new Bonus("drought", -3));
        return true;
    }
}