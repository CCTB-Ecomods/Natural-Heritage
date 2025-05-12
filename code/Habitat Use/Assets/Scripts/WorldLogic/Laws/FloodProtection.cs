public class FloodProtection : Law
{
	private const string _name = "Flood Protection Act";
    private const string _description =
        "A total sum of $6000 shall be payed over the next three years for the construction of effective flood barriers. This will reduce the damage caused by floods by half and increases approval with all voter groups except conservationists.";
	private const string _requirement = "$2000 per year for 3 years";
	private const string _effect = "Floods cause less damage";
	private const string _penalty = "No penalty";
    private const LawType _type = LawType.BILL;

    public FloodProtection() : base(_name, _description, _type, _requirement, _effect, _penalty)
    {
        //nothing to do here after the base constructor was called
    }

    protected override void EnactThisLaw(WorldData world)
    {
		//Pay the cost
		world.revenue.Add(new Bonus("flood protection", -3000, 3));
		//Increase voter approval
		foreach (ResidentGroup rg in world.residentGroups) {
			Bonus b = new Bonus("flood protection", 5, () => Parliament.IsEnacted(_name));
			if (rg.GetName() != "Conservationist") rg.AddBonus(b);
		}
		//Reducing flood damage happens in the Flood event class
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
