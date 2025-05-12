public class ResearchConservation : Law
{
    private const string _name = "Research: Conservation";
    private const string _description =
        "In the coming legislation period, invest $1000 a year into research on conservation techniques. Will permanently raise biodiversity on tiles with no or low usage intensity. Popular with conservationists.";
	private const string _requirement = "$1000 per year for 6 years";
	private const string _penalty = "No penalty";
    private const string _effect = "Permanently raise biodiversity on tiles with no or low usage intensity.";
    private const LawType _type = LawType.RESEARCH;

    public ResearchConservation() : base(_name, _description, _type, _requirement, _effect, _penalty)
    {
        //nothing to do here after the base constructor was called
    }

    protected override void EnactThisLaw(WorldData world)
    {
		//Pay the cost
		world.revenue.Add(new Bonus("research conservation", -1000, 6));
		//Adjust voter approval
		Bonus bap = new Bonus("research conservation", 5, 6);
		world.residentGroups[0].AddBonus(bap);		
		//Increase tile productivity and diversity
		//FIXME This isn't quite correct, as it doesn't affect tiles changed to
		// low/medium intensity in the future
		// -> should be done via hooks, but I don't quite know how
		Bonus b = new Bonus("research conservation", 4, () => true);
		foreach (TileData td in world.tiles) {
			if (td.intensity == Intensity.NONE || td.intensity == Intensity.LOW) {
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
