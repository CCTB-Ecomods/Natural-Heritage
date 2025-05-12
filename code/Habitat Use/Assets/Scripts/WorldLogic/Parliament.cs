using System.Collections.Generic;
using System.Linq;

/**
 * This class administrates the law subsystem and keeps track of enacted laws.
 */
public static class Parliament
{
    //A dictionary of all laws by name
    private static Dictionary<string, ILaw> _laws;

    public static void InitLaws()
    {
        _laws = new Dictionary<string, ILaw>();
        //Commented laws are not yet implemented
        var newLaws = new List<ILaw>
        {
            //Research
            new ResearchAgronomy(),
            new ResearchAgroecology(),
            new ResearchConservation(),
            //Subsidies
            new SubsidiseConventionalFarming(),
            new SubsidiseOrganicFarming(),
            new SubsidiseTourism(),
            //Bills
            new ClimateProtection(),
            new FireBrigade(),
            new FloodProtection(),
            new Glyphosate(),
            new GMO(),
            //Petitions
            new Rewilding(),
            new NationalParks(),
            new OrganicFarming(),
            new TourismHotspot()
        };
        foreach (Law l in newLaws) _laws.Add(l.GetName(), l);
    }

    //TODO get (un-)enacted laws (only needed for petitions?)

    /**
	 * Get all laws of the specified type
	 */
    private static List<ILaw> GetLawsOfType(LawType lawType)
    {
        return _laws.Values.Where(l => l.GetLawType() == lawType).ToList();
    }

    // wrapper functions
    public static List<ILaw> GetAllSubsidies()
    {
        return GetLawsOfType(LawType.SUBSIDY);
    }

    public static List<ILaw> GetAllResearchBudgets()
    {
        return GetLawsOfType(LawType.RESEARCH);
    }

    public static List<ILaw> GetAllBills()
    {
        return GetLawsOfType(LawType.BILL);
    }

    public static List<ILaw> GetAllPetitions()
    {
        return GetLawsOfType(LawType.PETITION);
    }

    /**
	 * Get a random law of the specified type.
	 */
    private static ILaw GetRandomLaw(LawType lawType)
    {
        var subset = GetLawsOfType(lawType);
        return subset[GameLogic.Random(subset.Count)];
    }

    // wrapper functions
    public static ILaw GetRandomBill()
    {
        return GetRandomLaw(LawType.BILL);
    }

    public static ILaw GetRandomPetition()
    {
        return GetRandomLaw(LawType.PETITION);
    }

    /**
	 * Is the law by this name enacted?
	 */
    public static bool IsEnacted(string lawName)
    {
        return _laws[lawName].Enacted();
    }
}