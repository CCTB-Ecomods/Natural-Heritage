public class ResearchAgronomy : Law
{
    private const string _name = "Research: Agronomy";
    private const string _description =
        "In the coming legislation period, invest $1000 a year into research on conventional methods of agriculture. Will permanently raise the productivity (but lower the biodiversity) on fields with medium or high usage intensity. Popular with farmers and industrialists, unpopular with conservationists.";
	private const string _requirement = "$1000 per year for 6 years";
	private const string _penalty = "No penalty";
	private const string _effect = "Permanently raise the productivity (but lower the biodiversity) on fields with medium or high usage intensity.";
    private const LawType _type = LawType.RESEARCH;

    public ResearchAgronomy() : base(_name, _description, _type, _requirement, _effect, _penalty)
    {
        //nothing to do here after the base constructor was called
    }

    protected override void EnactThisLaw(WorldData world)
    {
		//Pay the cost
		world.revenue.Add(new Bonus("research agronomy", -1000, 6));
		//Adjust voter approval
		Bonus bap = new Bonus("research agronomy", 3, 6);
		Bonus ban = new Bonus("research agronomy", -3, 6);
		world.residentGroups[0].AddBonus(ban);
		world.residentGroups[3].AddBonus(bap);
		world.residentGroups[4].AddBonus(bap);		
		//Increase tile productivity and diversity
		//FIXME This isn't quite correct, as it doesn't affect tiles changed to
		// low/medium intensity in the future
		// -> should be done via hooks, but I don't quite know how
		Bonus bp = new Bonus("research agronomy", 2, () => true);
		Bonus bd = new Bonus("research agronomy", -2, () => true);
		foreach (TileData td in world.tiles) {
			if (td.type.Type == TileType.FIELD &&
				(td.intensity == Intensity.MEDIUM || td.intensity == Intensity.HIGH)) {
				td.productivityBonus.Add(bp);
				td.diversityBonus.Add(bd);
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
