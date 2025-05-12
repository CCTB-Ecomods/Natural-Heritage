using System;

public class SubsidiseConventionalFarming : Law
{
    private const string _name = "Subsidies: Conventional Farming";
    private const string _description =
        "In the coming legislation period, invest $1000 a year to increase the productivity of tiles with medium or high usage. Popular with farmers and industrialists; unpopular with conservationists.";
	private const string _requirement = "$1000 per year for 6 years";
	private const string _penalty = "No penalty";
    private const string _effect = "Temporarily increase the productivity of tiles with medium or high usage.";
    private const LawType _type = LawType.SUBSIDY;

    public SubsidiseConventionalFarming() : base(_name, _description, _type, _requirement, _effect, _penalty)
    {
        //nothing to do here after the base constructor was called
    }

    protected override void EnactThisLaw(WorldData world)
    {
		//Pay the cost
		world.revenue.Add(new Bonus("conventional farming subsidy", -1000, 6));
		//Adjust voter approval
		Bonus bp = new Bonus("subsidising conventional farming", 5, 6);
		Bonus bn = new Bonus("subsidising conventional farming", -5, 6);
		world.residentGroups[0].AddBonus(bn);
		world.residentGroups[3].AddBonus(bp);
		world.residentGroups[4].AddBonus(bp);		
		//Increase tile productivity
		Bonus bprod = new Bonus("subsidising conventional farming", 8, 6);
		foreach (TileData td in world.tiles) {
			if (td.intensity == Intensity.MEDIUM || td.intensity == Intensity.HIGH)
				td.productivityBonus.Add(bprod);
		}

    }

    protected override void RepealThisLaw(WorldData world)
    {
        //nothing happens here - all bonuses disappear automatically
    }

    protected override void RejectThisLaw(WorldData world)
    {
        //nothing happens here
    }
}
