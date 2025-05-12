using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * See the game concept file for equation definitions and explanations.
 * (documentation/spiel_konzept.pdf)
 */
public static class TileLogic 
{

    public static void CalculateBiodiversity(TileData td, WorldData world)
    {
		// update the factor array
		td.biodiversityFactors[0] = IntensityDiversityFactor(td);
		td.biodiversityFactors[1] = AreaDiversityFactor(td);
		td.biodiversityFactors[2] = LandscapeDiversityFactor(td);
		td.biodiversityFactors[3] = BonusSum(ref td.diversityBonus);
		// do the calculation
		td.currentBiodiversity = td.biodiversityFactors.Sum();
		// keep the value in bounds (1 <= x <= 50)
		if (td.currentBiodiversity < 1) {
			td.currentBiodiversity = 1;
			Debug.Log("Diversity was <1: "+td.xCoordinate.ToString()+"/"+
					  td.yCoordinate.ToString());
		}
		else if (td.currentBiodiversity > 50) {
			td.currentBiodiversity = 50;
			Debug.Log("Diversity was >50: "+td.xCoordinate.ToString()+"/"+
					  td.yCoordinate.ToString());
		}
    }

    public static void CalculateProductivity(TileData td, WorldData world)
    {
		// update the factor array
		td.productivityFactors[0] = (float) Math.Pow((int) td.intensity+1, 2) * 2;
		td.productivityFactors[1] = (float) Math.Log(td.area.GetSize()) * 2;
		td.productivityFactors[2] = (float) Math.Pow(world.averageIntensity, 3) * -1;
		td.productivityFactors[3] = EcosystemServicesFactor(td) * 3;
		td.productivityFactors[4] = BonusSum(ref td.productivityBonus);
		// do the calculation
		td.currentProductivity = td.productivityFactors.Sum();
		// keep the value in bounds (1 <= x <= 50)
		if (td.currentProductivity < 1) {
			td.currentProductivity = 1;
			Debug.Log("Productivity was <1: "+td.xCoordinate.ToString()+"/"+
					  td.yCoordinate.ToString());
		}
		else if (td.currentProductivity > 50) {
			td.currentProductivity = 50;
			Debug.Log("Productivity was >50: "+td.xCoordinate.ToString()+"/"+
					  td.yCoordinate.ToString());
		}

    }

	/**
	 * Calculate the local (tile-specific) factors of biodiversity:
	 * diversity decreases with land use intensity.
	 */
	private static float IntensityDiversityFactor(TileData td)
	{
		return 5 * (float)(3-(int)td.intensity);
	}

	/**
	 * Calculate the effect of habitat area on this tile's biodiversity,
	 * as per the Species-Area Relationship.
	 */
	private static float AreaDiversityFactor(TileData td)
	{
		int areaSize = td.area.GetSize();
		return 3 * (float)Math.Pow(areaSize, 0.2);
	}
	
	/**
	 * Calculate the effect of the landscape on local biodiversity.
	 * Depends on the adjacent land use intensity and the number of different habitats
	 * in the vicinity (i.e. habitat heterogeneity).
	 */
    private static float LandscapeDiversityFactor(TileData td)
    {
		float habitatFactor = 0;
		
		foreach (TileData neighbour in td.adjacentTiles) 
		{
			if (neighbour != null)
				habitatFactor += 3 - (float)neighbour.intensity + Heterogeneity(neighbour);
		}
        
		return (habitatFactor / 3);
    }

	/**
	 * Calculate how many different land use types are adjacent to this tile.
	 */
	private static int Heterogeneity(TileData td)
	{
		//XXX This is really not ideal - not easily extensible
		// (relies on knowing that there are only four habitat types).
		// Should cycle through all values of the TileType enum.
		int river = 0;
		int forest = 0;
		int field = 0; 
		int city = 0;

		foreach (TileData neighbour in td.adjacentTiles) {
			if (neighbour != null)
			{
				switch (neighbour.type.Type)
				{
					case TileType.RIVER: river = 1; break;
					case TileType.FOREST: forest = 1; break;
					case TileType.FIELD: field = 1; break;
					case TileType.CITY: city = 1; break;
				}
			}

		}
		return river+forest+field+city;
	}

	/**
	 * Calculate the ecosystem services that the biodiversity of the surrounding
	 * tiles brings to this tile.
	 */
    private static float EcosystemServicesFactor(TileData td)
    {
		float diversities = 0;
		float esf = 0;
		foreach (var neighbour in td.adjacentTiles) 
		{
			if (neighbour != null)
				diversities += neighbour.currentBiodiversity;
		}
		diversities = diversities / 6;
		if (diversities >= 1) {
			esf = (float) Math.Log(diversities);
		}
		else { //shouldn't happen
			Debug.Log("ESF: Diversity was <1: "+td.xCoordinate.ToString()+"/"+
					  td.yCoordinate.ToString());
		}
        return esf;
    }

	/**
	 * Calculate the sum of all bonusses (and remove any that are inactive).
	 */
	private static float BonusSum(ref List<Bonus> bonuses)
	{
		float sum = 0;
		for (int i = 0; i < bonuses.Count; i++) {
			Bonus b = bonuses[i];
			if (b.IsValid()) sum += b.GetValue();
			else {
				bonuses.Remove(b);
				i--;
			}
		}
		return sum;
	}
}
