public class ClimateProtection : Law
{
    private const string _name = "Climate Protection Act";
    private const string _description =
        "An annual sum of $300 shall be payed to mitigate the impact of human-caused climate change; this will reduce the likelihood of extreme weather events. It is unpopular with industrialists and popular with everybody else.";
	private const string _requirement = "$300 per year";
	private const string _effect = "Extreme weather events are less likely";
	private const string _penalty = "No penalty";
    private const LawType _type = LawType.BILL;

    public ClimateProtection() : base(_name, _description, _type, _requirement, _effect, _penalty)
    {
        //nothing to do here after the base constructor was called
    }

    protected override void EnactThisLaw(WorldData world)
    {
		//Pay the cost
		world.revenue.Add(new Bonus("climate protection", -1000, () => Parliament.IsEnacted(_name)));
		//Increase voter approval
		foreach (ResidentGroup rg in world.residentGroups) {
			int value = 5;
			if (rg.GetName() == "Industrialist") value = -5;
			Bonus b = new Bonus("climate protection", value, () => Parliament.IsEnacted(_name));
			rg.AddBonus(b);
		}
		//Lower the climate change rate
		Climate.ModifyClimateChangeRate(-0.05F);
    }

    protected override void RepealThisLaw(WorldData world)
    {
        Climate.ModifyClimateChangeRate(0.5F);
    }

    protected override void RejectThisLaw(WorldData world)
    {
        //nothing happens here
    }
}
