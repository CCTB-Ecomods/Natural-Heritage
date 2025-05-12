using System;

public abstract class Law : ILaw
{
    private readonly string _description;
    private bool _isEnacted;
    private readonly string _name;
    private readonly string _requirement;
    private readonly string _effect;
    private readonly string _penalty;
    private readonly LawType _type;

    public Law(string name, string description, LawType type)
    {
        _isEnacted = false;
        _name = name;
        _description = description;
        _type = type;
    }

    public Law(string name, string description, LawType type,
			   string requirement, string effect, string penalty)
    {
        _isEnacted = false;
        _name = name;
        _description = description;
        _type = type;
        _requirement = requirement;
        _effect = effect;
        _penalty = penalty;
    }

    public string GetName()
    {
        return _name;
    }

    public string GetDescriptionForPlayer()
    {
        return _description;
    }

    public string GetRequirement()
    {
        return _requirement;
    }

    public string GetEffect()
    {
        return _effect;
    }

    public string GetPenalty()
    {
        return _penalty;
    }


    public LawType GetLawType()
    {
        return _type;
    }

    public bool Enacted()
    {
        return _isEnacted;
    }

    public void EnactLaw(WorldData worldData)
    {
        _isEnacted = true; //XXX Exempt petitions from this?
        EnactThisLaw(worldData);
    }

    public void RepealLaw(WorldData worldData)
    {
        _isEnacted = false;
        RepealThisLaw(worldData);
    }

    public void RejectLaw(WorldData worldData)
    {
        // currently, this template method isn't really needed, but I'm
        // including it anyway for uniformity and future extensibility
        RejectThisLaw(worldData);
    }

    // Template method pattern - so not every inheriting class
    // has to remember to set _isEnacted
    protected abstract void EnactThisLaw(WorldData worldData);
    protected abstract void RepealThisLaw(WorldData worldData);
    protected abstract void RejectThisLaw(WorldData worldData);
}
