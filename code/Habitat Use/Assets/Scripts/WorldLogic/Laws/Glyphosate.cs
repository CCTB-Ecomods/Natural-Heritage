using System;

public class Glyphosate : Law
{
    private const string _name = "Glyphosate Prohibition Act";

    private const string _description =
        "The application of the herbicide Glyphosate shall be prohibited with immediate effect. This will raise the biodiversity on all field tiles at maximum use intensity, but lower their productivity. Increases approval rates of conservationists and residents, decreases those of farmers and industrialists.";
	private const string _requirement = "No requirements";
	private const string _effect = "Raises the biodiversity and lowers productivity of high intensity field tiles";
	private const string _penalty = "No penalty";
    private const LawType _type = LawType.BILL;

    public Glyphosate() : base(_name, _description, _type, _requirement, _effect, _penalty)
    {
        //nothing to do here after the base constructor was called
    }

    protected override void EnactThisLaw(WorldData world)
    {
        //Change voter approval
        var b1 = new Bonus("banning glyphosate", 3, () => Parliament.IsEnacted(_name));
        var b2 = new Bonus("banning glyphosate", -3, () => Parliament.IsEnacted(_name));
        world.residentGroups[0].AddBonus(b1); //conservationists
        world.residentGroups[2].AddBonus(b1); //residents
        world.residentGroups[3].AddBonus(b2); //farmers
        world.residentGroups[4].AddBonus(b2); //industrialists
        //Add the actual law method to the runtime hooks
        //XXX Not sure if this works as intended - we'll have to see...
        Action<WorldData> lawFunc = ApplyBonusToFields;
        GameLogic.hooks["glyphosate"] = lawFunc;
    }

    public static void ApplyBonusToFields(WorldData world)
    {
        // check if the law is still in force
        if (!Parliament.IsEnacted("Glyphosate Prohibition Act"))
        {
            GameLogic.hooks.Remove("glyphosate");
            return;
        }

        var bp = new Bonus("banning glyphosate", -3);
        var bd = new Bonus("banning glyphosate", 3);
        //Cycle through the landscape, applying the bonus to all high-intensity fields
        //XXX This may be somewhat calculation intensive
        foreach (var td in world.tiles)
            if (td.type.Type == TileType.FIELD && td.intensity == Intensity.HIGH)
            {
                td.diversityBonus.Add(bd);
                td.productivityBonus.Add(bp);
            }
    }

    protected override void RepealThisLaw(WorldData world)
    {
        var b = new Bonus("allowing glyphosate", 3, 6);
        world.residentGroups[3].AddBonus(b); //farmers		
    }

    protected override void RejectThisLaw(WorldData world)
    {
        //nothing happens here
    }
}
