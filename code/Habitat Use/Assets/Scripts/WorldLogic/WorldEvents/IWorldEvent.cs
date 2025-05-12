public interface IWorldEvent
{
	/// <summary>
	///     Execute the event logic
	/// </summary>
	/// <returns>True if the event did take place, false if it was prevented</returns>
	bool EventTakesPlace(WorldData worldData);

	/// <summary>
	///     Get the name of the event
	/// </summary>
	/// <returns>Name of event as string</returns>
	string GetName();

	/// <summary>
	///     Get a long-form description of this event to be displayed to the player
	/// </summary>
	/// <returns>Description of event as string</returns>
	string GetDescriptionForPlayer();
}