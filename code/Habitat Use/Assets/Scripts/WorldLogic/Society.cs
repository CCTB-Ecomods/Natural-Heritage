using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Society
{

	/**
	 * Sum up the "tourist" values of all tiles in the landscape.
	 */
	public static int TouristValue(TileData[] tilemap)
	{
		int value = 0;
		foreach (TileData tile in tilemap) {
			int touristiness = 0;
			switch (tile.intensity) {
				case Intensity.NONE: touristiness = 30; break;
				case Intensity.LOW: touristiness = 40; break;
				case Intensity.MEDIUM: touristiness = 20; break;
				case Intensity.HIGH: touristiness = 10; break;
			}
			value += touristiness;
		}
		return(value);
	}

	/**
	 * Calculate political approval of the five voter groups. Approval depends
	 * on how closely the current landscape value distribution mirrors their
	 * ideal distribution.
	 */
	public static void UpdateApproval(WorldData world)
	{
		foreach (var residentGroup in world.residentGroups)
		{
			residentGroup.UpdateApprovalRating(world);
		}
    }

	/// <summary>
	/// Calculates average approval among residentgroups
	/// </summary>
	/// <param name="residentGroups">All resident groups existing in the game world</param>
	/// <returns>Average approval as float</returns>
	public static float CalculateOverallApproval(List<IResidentGroup> residentGroups)
	{
		float overallApproval = 0;

		foreach (var residentGroup in residentGroups)
		{
			overallApproval += residentGroup.GetApprovalRating();
		}

		//so we do not divide by 0
		if (residentGroups.Count > 0)
			return overallApproval / residentGroups.Count;
		else
			return 0;
	}

	/// <summary>
	/// City sprawl. At the start of every legislative period, one city grows by one tile.
	/// </summary>
	public static void CityGrowth(WorldData world)
	{
		//choose a random city
		List<IArea> cities = world.areas.Where(a => a.GetTileType() == TileType.CITY).ToList();
		if (cities.Count == 0) return; //should never happen
		IArea ac = cities[GameLogic.Random(cities.Count)];
		//pick a neighbouring tile and convert it to a city
		foreach (TileData ct in ac.GetTileList()) {
			foreach (TileData neighbour in ct.adjacentTiles) {
				if (neighbour.type.Type != TileType.CITY &&
					neighbour.type.Type != TileType.RIVER &&
					GameLogic.Random(100) > 66) {
					neighbour.type = ct.type;
					world.populationSize += Economy.costPerCityTile;
					return;
				}
			}
		}
	}
}
