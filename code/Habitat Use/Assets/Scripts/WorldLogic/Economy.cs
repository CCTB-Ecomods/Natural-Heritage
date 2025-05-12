using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class Economy
{
	//CONSTANTS

	//How many inhabitants does each city tile have? (i.e. running costs in $/year)
	public static readonly int costPerCityTile = 1000;
	//The cost for changing a tile's intensity/use type
	public static readonly int intensityChangeCost = 1000;
	public static readonly int usetypeChangeCost = 1500;
	//How much money does the player have at the beginning?
	public static readonly int initBalance = 20000; // $20,000.00

	//FUNCTIONS
	
	/**
	 * Calculate the size of the region's population (depends on the number of city tiles
	 * and the configured city size).
	 */
	public static int GetTotalPopulation(TileData[] tilemap)
	{
		int population = 0;
		foreach (TileData tile in tilemap) {
			//We still need to test `tileType` here, as `type.Type` is not yet initialised
			if (tile.tileType == TileType.CITY) {
				population += costPerCityTile;
			}
		}
		return population;
	}
	
	/**
	 * Sum up the productivity values of all tiles in the landscape.
	 */
	public static float IndustryValue(TileData[] tilemap)
	{
		float value = 0;
		foreach (TileData tile in tilemap) {
			value += tile.currentProductivity;
		}
		return value;
	}

	/**
	 * Register that a tile has been changed (requires labour costs).
	 * @param intensityChange Was it an intensity change (true), or
	 *                        a type change (false)?
	 */
	public static void PayLandUseChange(WorldData world, bool intensityChange)
	{
		int cost = intensityChangeCost;
		if (!intensityChange) cost = usetypeChangeCost;
		world.finances -= cost;
	}

	/**
	 * Calculate this year's net financial balance. The annual balance depends on
	 * population size, total productivity, and accumulated labour cost of
	 * tiles changed in this round. 
	 */
	public static float CalculateBalance(WorldData world)
	{
		float income = NetRevenue(world);
		float balance = world.landscapeValues.IndustryValue - world.populationSize + income;
		return balance;
	}
	
	/**
	 * Calculate the sum of all incomes (and remove any that are inactive).
	 */
	private static float NetRevenue(WorldData world)
	{
		float sum = 0;
		for (int i = 0; i < world.revenue.Count; i++) {
			Bonus b = world.revenue[i];
			if (b.IsValid()) sum += b.GetValue();
			else {
				world.revenue.Remove(b);
				i--;
			}
		}
		return sum;

	}
}
