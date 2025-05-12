public abstract class WorldEvent : IWorldEvent
{
    private readonly string _description;
    private readonly string _name;

    public WorldEvent(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public string GetName()
    {
        return _name;
    }

    public string GetDescriptionForPlayer()
    {
        return _description;
    }

    public abstract bool EventTakesPlace(WorldData worldData);
}