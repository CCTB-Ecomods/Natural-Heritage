using System.Collections.Generic;
using System.Linq;

public class Rewilding : Law
{
    private const string _name = "Petition: Rewilding a Forest";
	private const string _description =
        "Invest a one-time sum of $4000 to reintroduce a rare animal species to an unused forest habitat. This will raise the diversity in this habitat and is popular with conservationists and tourists. Rejecting this petition will give a temporary popularity penalty with the conservationist group that introduced it.";
	private const string _requirement = "$4000";
	private const string _effect = "Raises the biodiversity of an unused forest habitat. +3% popularity with conservationists and tourists.";
	private const string _penalty = "-5% popularity with conservationists for 6 turns";
    private const LawType _type = LawType.PETITION;

    public Rewilding() : base(_name, _description, _type, _requirement, _effect, _penalty)
    {
        //nothing to do here after the base constructor was called
    }

    protected override void EnactThisLaw(WorldData world)
    {
		//Pay the cost
		world.revenue.Add(new Bonus("rewilding", -4000));
		//Increase voter approval
		Bonus bp = new Bonus("accepted rewilding", 3, () => true);
		world.residentGroups[0].AddBonus(bp);
		world.residentGroups[1].AddBonus(bp);
		//choose a random wilderness area
		List<IArea> wilderness = world.areas.Where(a => a.GetIntensity() == Intensity.NONE).ToList();
		if (wilderness.Count == 0) return; //XXX Not very good, but what else should we do?
		IArea aw = wilderness[GameLogic.Random(wilderness.Count)];
		//then give it a diversity bonus that disappears when the intensity changes
		foreach (TileData td in aw.GetTileList()) {
			Bonus bd = new Bonus("rare species", 3, () => td.intensity == Intensity.NONE);
			td.diversityBonus.Add(bd);
		}
    }

    protected override void RepealThisLaw(WorldData world)
    {
        //nothing happens here - all bonuses disappear automatically
    }

    protected override void RejectThisLaw(WorldData world)
    {
        var b = new Bonus("rejected rewilding", -5, 6);
        world.residentGroups[0].AddBonus(b);
    }
}
