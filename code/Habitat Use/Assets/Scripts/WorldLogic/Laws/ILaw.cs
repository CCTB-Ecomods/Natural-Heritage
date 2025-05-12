public interface ILaw
{
	/// <summary>
	///     What happens when this bill is passed into law
	/// </summary>
	void EnactLaw(WorldData worldData);

	/// <summary>
	///     What happens when this law is repealed after being enacted
	/// </summary>
	void RepealLaw(WorldData worldData);

	/// <summary>
	///     What happens when this bill is rejected in parliament
	/// </summary>
	void RejectLaw(WorldData worldData);

	/// <summary>
	///     Get the name of this law
	/// </summary>
	/// <returns>Name of law as string</returns>
	string GetName();

	/// <summary>
	///     Get a long-form description of this law to be displayed to the player
	/// </summary>
	/// <returns>Description of law as string</returns>
	string GetDescriptionForPlayer();
	
	/// <summary>
	///     Get the effect of this law
	/// </summary>
	/// <returns>Effect of of law as string</returns>
	string GetEffect();
	
	/// <summary>
	///     Get the penalty of this law
	/// </summary>
	/// <returns>Penalty of of law as string</returns>
	string GetPenalty();

	/// <summary>
	///     Get the requirements of this law
	/// </summary>
	/// <returns>Requirement of the law as string</returns>
	string GetRequirement();

	/// <summary>
	///     What type of law is this? (budget, bill, or petition)
	/// </summary>
	/// <returns>Law type</returns>
	LawType GetLawType();

	/// <summary>
	///     Is this law currently enacted?
	/// </summary>
	bool Enacted();
}
