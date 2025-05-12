public class ResearchAgroecology : Law
{
    private const string _name = "Research: Agroecology";
    private const string _description =
        "In the coming legislation period, invest $1000 a year into research on sustainable methods of agriculture. Will permanently raise the productivity and biodiversity on fields with low or medium usage intensity. Popular with conservationists, residents, and farmers.";
	private const string _requirement = "$1000 per year for 6 years";
	private const string _penalty = "No penalty";
	private const string _effect = "Permanently raise the productivity and biodiversity on fields with low or medium usage intensity.";
    private const LawType _type = LawType.RESEARCH;

    public ResearchAgroecology() : base(_name, _description, _type, _requirement, _effect, _penalty)
    {
        //nothing to do here after the base constructor was called
    }

    protected override void EnactThisLaw(WorldData world)
    {
		//Pay the cost
		world.revenue.Add(new Bonus("research agroecology", -1000, 6));
		//Adjust voter approval
		Bonus bp = new Bonus("research agroecology", 3, 6);
		world.residentGroups[0].AddBonus(bp);
		world.residentGroups[2].AddBonus(bp);
		world.residentGroups[3].AddBonus(bp);		
		//Increase tile productivity and diversity
		//FIXME This isn't quite correct, as it doesn't affect tiles changed to
		// low/medium intensity in the future
		// -> should be done via hooks, but I don't quite know how
		Bonus b = new Bonus("research agroecology", 2, () => true);
		foreach (TileData td in world.tiles) {
			if (td.type.Type == TileType.FIELD &&
				(td.intensity == Intensity.MEDIUM || td.intensity == Intensity.LOW)) {
				td.productivityBonus.Add(b);
				td.diversityBonus.Add(b);
			}
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
