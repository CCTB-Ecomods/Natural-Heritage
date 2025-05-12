public class SubsidiseOrganicFarming : Law
{
    private const string _name = "Subsidies: Organic Farming";
    private const string _description =
        "In the coming legislation period, invest $1000 a year to increase the productivity of tiles with low or medium usage. Popular with conservationists and farmers; unpopular with industrialists.";
	private const string _requirement = "$1000 per year for 6 years";
	private const string _penalty = "No penalty";
    private const string _effect = "Temporarily increase the productivity of tiles with low or medium usage.";
    private const LawType _type = LawType.SUBSIDY;

    public SubsidiseOrganicFarming() : base(_name, _description, _type, _requirement, _effect, _penalty)
    {
        //nothing to do here after the base constructor was called
    }

    protected override void EnactThisLaw(WorldData world)
    {
		//Pay the cost
		world.revenue.Add(new Bonus("organic farming subsidy", -1000, 6));
		//Adjust voter approval
		Bonus bp = new Bonus("subsidising organic farming", 5, 6);
		Bonus bn = new Bonus("subsidising organic farming", -5, 6);
		world.residentGroups[0].AddBonus(bp);
		world.residentGroups[3].AddBonus(bp);
		world.residentGroups[4].AddBonus(bn);
		//Increase tile productivity
		Bonus bprod = new Bonus("subsidising organic farming", 8, 6);
		foreach (TileData td in world.tiles) {
			if (td.intensity == Intensity.MEDIUM || td.intensity == Intensity.LOW)
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
