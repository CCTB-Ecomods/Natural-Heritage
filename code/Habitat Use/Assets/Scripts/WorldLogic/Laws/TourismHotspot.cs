using System;
using System.Linq;

public class TourismHotspot : Law
{
    private const string _name = "Petition: Tourism Hotspot";

    private const string _description =
        "Your region's residents would like to see more tourist activity. As tourists do not like intensively used landscapes, the residents want you to ensure that 20% of all tiles are set to low usage intensity. If you accept and reach the target, you will have a popularity bonus with tourists and residents. If you reject or fail to meet the target, you will suffer a popularity penalty with the residents.";
	private const string _requirement = "20% of all tiles have low usage intensity";
	private const string _effect = "+5 popularity with tourists and residents";
	private const string _penalty = "-5 popularity with tourists and residents";
    private const LawType _type = LawType.PETITION;

    public TourismHotspot() : base(_name, _description, _type, _requirement, _effect, _penalty)
    {
        //nothing to do here after the base constructor was called
    }

    protected override void EnactThisLaw(WorldData world)
    {
        //Add the actual law method to the runtime hooks
        //XXX Not sure if this works as intended - we'll have to see...
        Action<WorldData> lawFunc = TourismPopularityBonus;
        GameLogic.hooks["tourism hotspot"] = lawFunc;
    }

    public static void TourismPopularityBonus(WorldData world)
    {
        // check if the law is still in force
        if (!Parliament.IsEnacted("Petition: Tourism Hotspot"))
        {
            GameLogic.hooks.Remove("tourism hotspot");
            return;
        }

        // calculate the percentage of low usage tiles
        var lowUsedTiles = 0;
        foreach (var td in world.tiles)
            if (td.intensity == Intensity.LOW)
                lowUsedTiles++;
        var percentageLowUsed = (float) lowUsedTiles / world.tiles.Count();
        // apply the bonuses
        var bp = new Bonus("tourism hotspot succeeded", 5);
        var bn = new Bonus("tourism hotspot failed", -5);
        if (percentageLowUsed >= 0.2)
        {
            world.residentGroups[1].AddBonus(bp);
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
        var b = new Bonus("tourism hotspot rejected", -5, 6);
        world.residentGroups[2].AddBonus(b);
    }
}
