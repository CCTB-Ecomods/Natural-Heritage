using System;

public class GMO : Law
{
    private const string _name = "Genetically Modified Organisms Act";

    private const string _description =
        "The agricultural use of genetically modified organisms (GMOs) shall be permitted with immediate effect. This will raise the productivity on all field tiles at medium and maximum use intensity. Increases approval rates of farmers and industrialists, decreases those of residents.";
	private const string _requirement = "No requirements";
	private const string _effect = "Raise productivity of medium and high intensity field tiles";
	private const string _penalty = "No penalty";
    private const LawType _type = LawType.BILL;

    public GMO() : base(_name, _description, _type, _requirement, _effect, _penalty)
    {
        //nothing to do here after the base constructor was called
    }

    protected override void EnactThisLaw(WorldData world)
    {
        //Change voter approval
        var b1 = new Bonus("allowing GMOs", 3, () => Parliament.IsEnacted(_name));
        var b2 = new Bonus("allowing GMOs", -10, () => Parliament.IsEnacted(_name));
        world.residentGroups[3].AddBonus(b1); //farmers
        world.residentGroups[4].AddBonus(b1); //industrialists
        world.residentGroups[2].AddBonus(b2); //residents
        //Add the actual law method to the runtime hooks
        //XXX Not sure if this works as intended - we'll have to see...
        Action<WorldData> lawFunc = ApplyBonusToFields;
        GameLogic.hooks["gmo"] = lawFunc;
    }

    public static void ApplyBonusToFields(WorldData world)
    {
        // check if the law is still in force
        if (!Parliament.IsEnacted("Genetically Modified Organisms Act"))
        {
            GameLogic.hooks.Remove("gmo");
            return;
        }

        var bp = new Bonus("allowing GMOs", 3);
        //Cycle through the landscape, applying the bonus to all high-intensity fields
        //XXX This may be somewhat calculation intensive
        foreach (var td in world.tiles)
            if (td.type.Type == TileType.FIELD &&
                (td.intensity == Intensity.HIGH || td.intensity == Intensity.MEDIUM))
                td.productivityBonus.Add(bp);
    }

    protected override void RepealThisLaw(WorldData world)
    {
        var b1 = new Bonus("forbidding GMOs", 3, 6);
        var b2 = new Bonus("forbidding GMOs", -3, 6);
        world.residentGroups[2].AddBonus(b1); //residents
        world.residentGroups[4].AddBonus(b2); //industrialists		
    }

    protected override void RejectThisLaw(WorldData world)
    {
        //nothing happens here
    }
}
