public class FireBrigade : Law
{
	private const string _name = "Fire Brigade Act";
    private const string _description =
        "An annual sum of $500 shall be payed for the upkeep of an effective fire brigade. This will reduce the damage caused by forest fires by half and increases approval with all voter groups.";
	private const string _requirement = "$500 per year";
	private const string _effect = "Fires cause less damage";
	private const string _penalty = "No penalty";
    private const LawType _type = LawType.BILL;

    public FireBrigade() : base(_name, _description, _type, _requirement, _effect, _penalty)
    {
        //nothing to do here after the base constructor was called
    }

    protected override void EnactThisLaw(WorldData world)
    {
		//Pay the cost
		world.revenue.Add(new Bonus("fire brigade", -1500, () => Parliament.IsEnacted(_name)));
		//Increase voter approval
		foreach (ResidentGroup rg in world.residentGroups) {
			Bonus b = new Bonus("fire brigade", 5, () => Parliament.IsEnacted(_name));
			rg.AddBonus(b);
		}
		//Reducing fire damage happens in the Fire event class
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
