using System;
using System.Collections;
using System.Collections.Generic;

/**
 * See the game concept file for equation definitions and explanations.
 * (documentation/spiel_konzept.pdf)
 */
public static class Climate
{

	//TODO add biodiversity events
	
	private static float _climateModifier = 0.1F;

	/// <summary>
    /// Calculate the probability of an event taking place this turn. Events
	/// increase in frequency as the game progresses, due to climate change.
    /// </summary>
    /// <returns>Percentage likelihood as float</returns>
	public static float GetWeatherEventProbability(int update)
	{
		float prob = _climateModifier * (float) Math.Log((update/3)-1);
		return prob;
	}

	/// <summary>
    /// Get a random weather event out of those implemented in the game.
    /// </summary>
    /// <returns>An instance of an event class</returns>
	public static WorldEvent GetRandomWeatherEvent()
	{
		List<WorldEvent> events = new List<WorldEvent>() {
			new Drought(),
			new Fire(),
			new Flood(),
			new GoodWeather()
		};
		return events[GameLogic.Random(events.Count)];
	}

	/// <summary>
    /// Get a random biodiversity event out of those implemented in the game.
    /// </summary>
    /// <returns>An instance of an event class</returns>
	public static WorldEvent GetRandomBiodiversityEvent()
	{
		List<WorldEvent> events = new List<WorldEvent>() {
			new Erosion(),
			new Eutrophication(),
			new BarkBeetles(),
			new RareSpecies()
		};
		return events[GameLogic.Random(events.Count)];
	}

	/// <summary>
    /// Modify the rate of climate change, affecting the frequency of events
	/// in the rest of the game
    /// </summary>
	public static void ModifyClimateChangeRate(float modifier)
	{
		_climateModifier += modifier;
	}

}
