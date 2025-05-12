using System;

public class OrganicFarming : Law
{
    private const string _name = "Petition: Organic Farming";

    private const string _description =
        "A coalition of conservationists and residents is calling for more sustainable agriculture. They want you to ensure that in future, 60% of field tiles will have a low or medium usage intensity. If you accept and implement this petition, you will gain a popularity bonus with conservationists and residents, although farmers and industrialists will temporarily be unhappy with you. If you reject or fail to implement the petition, you will suffer a popularity penalty with the conservationists and residents.";
	private const string _requirement = "60% of field tiles have usage intensity 'low' or 'medium'";
	private const string _effect = "+5% popularity with conservationists and residents";
	private const string _penalty = "-5% popularity with conservationists and residents";
    private const LawType _type = LawType.PETITION;

    public OrganicFarming() : base(_name, _description, _type, _requirement, _effect, _penalty)
    {
        //nothing to do here after the base constructor was called
    }

    protected override void EnactThisLaw(WorldData world)
    {
        // Farmers and industrialists are unhappy
        var bnf = new Bonus("organic farming accepted", -5, 6);
        world.residentGroups[3].AddBonus(bnf);
        world.residentGroups[4].AddBonus(bnf);
        // Add the actual law method to the runtime hooks
        //XXX Not sure if this works as intended - we'll have to see...
        Action<WorldData> lawFunc = OrganicFarmingPopularityBonus;
        GameLogic.hooks["organic farming"] = lawFunc;
    }

    public static void OrganicFarmingPopularityBonus(WorldData world)
    {
        // check if the law is still in force
        if (!Parliament.IsEnacted("Petition: Organic Farming"))
        {
            GameLogic.hooks.Remove("organic farming");
            return;
        }

        // calculate the percentage of low or medium usage field tiles
        var lowUsedTiles = 0;
        var fieldTiles = 0;
        foreach (var td in world.tiles)
            if (td.type.Type == TileType.FIELD)
            {
                fieldTiles++;
                if (td.intensity == Intensity.LOW || td.intensity == Intensity.MEDIUM)
                    lowUsedTiles++;
            }

        var percentageLowUsed = (float) lowUsedTiles / fieldTiles;
        // apply the bonuses
        var bp = new Bonus("organic farming succeeded", 5);
        var bn = new Bonus("organic farming failed", -5);
        if (percentageLowUsed >= 0.6)
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
        var b = new Bonus("organic farming rejected", -5, 6);
        world.residentGroups[0].AddBonus(b);
        world.residentGroups[2].AddBonus(b);
    }
}
