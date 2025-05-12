using System;
using System.Linq;

public class NationalParks : Law
{
    private const string _name = "Petition: National Parks";

    private const string _description =
        "Conservationists would like you to do more for the protection of the region's biodiversity by setting up national parks. Specifically, they want you to ensure that 10% of all tiles are not used economically. If you accept and reach the target, you will have a popularity bonus with conservationists and residents. If you reject or fail to meet the target, you will suffer a popularity penalty with the conservationists.";
	private const string _requirement = "10% of all tiles in the landscape have usage intensity 'none'";
	private const string _effect = "+5% popularity with conservationists and residents";
	private const string _penalty = "-5% popularity with conservationists and residents";
    private const LawType _type = LawType.PETITION;

    public NationalParks() : base(_name, _description, _type, _requirement, _effect, _penalty)
    {
        //nothing to do here after the base constructor was called
    }

    protected override void EnactThisLaw(WorldData world)
    {
        //Add the actual law method to the runtime hooks
        //XXX Not sure if this works as intended - we'll have to see...
        Action<WorldData> lawFunc = NationalParksPopularityBonus;
        GameLogic.hooks["national parks"] = lawFunc;
    }

    public static void NationalParksPopularityBonus(WorldData world)
    {
        // check if the law is still in force
        if (!Parliament.IsEnacted("Petition: National Parks"))
        {
            GameLogic.hooks.Remove("national parks");
            return;
        }

        // calculate the percentage of unused tiles
        var unusedTiles = 0;
        foreach (var td in world.tiles)
            if (td.intensity == Intensity.NONE)
                unusedTiles++;
        var percentageUnused = (float) unusedTiles / world.tiles.Count();
        // apply the bonuses
        var bp = new Bonus("national parks succeeded", 5);
        var bn = new Bonus("national parks failed", -5);
        if (percentageUnused >= 0.1)
        {
            world.residentGroups[0].AddBonus(bp);
            world.residentGroups[2].AddBonus(bp);
        }
        else
        {
            world.residentGroups[0].AddBonus(bn);
        }
    }

    protected override void RepealThisLaw(WorldData world)
    {
        //nothing happens here - all bonuses disappear automatically
    }

    protected override void RejectThisLaw(WorldData world)
    {
        var b = new Bonus("national parks rejected", -5, 6);
        world.residentGroups[0].AddBonus(b);
    }
}
