public class SubsidiseTourism : Law
{
    private const string _name = "Subsidies: Tourism";
    private const string _description =
        "In the coming legislation period, invest $1000 a year to increase your region's tourism value and the productivity of tiles with low usage. Popular with conservationists, tourists, and residents; unpopular with farmers.";
	private const string _requirement = "$1000 per year for 6 years";
	private const string _penalty = "No penalty";
    private const string _effect = "Increase your region's tourism value and the productivity of tiles with low usage.";
    private const LawType _type = LawType.SUBSIDY;

    public SubsidiseTourism() : base(_name, _description, _type, _requirement, _effect, _penalty)
    {
        //nothing to do here after the base constructor was called
    }

    protected override void EnactThisLaw(WorldData world)
    {
		//Pay the cost
		world.revenue.Add(new Bonus("tourism subsidy", -1000, 6));
		//Adjust voter approval
		Bonus bp = new Bonus("subsidising tourism", 5, 6);
		Bonus bt = new Bonus("subsidising tourism", 10, 6);
		Bonus bn = new Bonus("subsidising tourism", -5, 6);
		world.residentGroups[0].AddBonus(bp);
		world.residentGroups[1].AddBonus(bt);
		world.residentGroups[2].AddBonus(bp);
		world.residentGroups[3].AddBonus(bn);		
		//Increase tile productivity
		Bonus bprod = new Bonus("subsidising tourism", 5, 6);
		foreach (TileData td in world.tiles) {
			if (td.intensity == Intensity.LOW)
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
