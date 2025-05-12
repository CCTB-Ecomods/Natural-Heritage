using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Nature
{

	/**
	 * Calculate the average land use intensity across the landscape
	 */
	public static float AverageIntensity(TileData[] tilemap)
	{
		float intensities = 0;
		foreach (TileData tile in tilemap) {
			intensities += (float)tile.intensity;
		}

		if (tilemap.Length > 0)
		{
			//Debug.Log("average intensity: " + intensities / tilemap.Length);
			return (intensities / tilemap.Length);
		}
		else
			return 0;

    }

	/**
	 * Sum up the biodiversity values of all tiles in the landscape.
	 */
	public static float NatureValue(TileData[] tilemap)
	{
		float value = 0;
		foreach (TileData tile in tilemap) {
			value += tile.currentBiodiversity;
		}
		return(value);
	}

	/**
	 * Figure out which adjoining tiles share a use type and intensity
	 */
	public static List<IArea> CalculateAreas(TileData[] tilemap)
	{
		//reset all areas
		var areas = new List<IArea>(); //A 2D list of areas and their associated tiles
		
		foreach (TileData tile in tilemap) 
		{
			tile.area = null;
		}

		//loop through the landscape, identifying current areas
		foreach (TileData tile in tilemap) 
		{
			if (tile.area == null)
			{
				IArea currentArea = new Area();
				currentArea.AddTile(tile);

				tile.area = currentArea;

				IdentifyArea(tile);

				areas.Add(currentArea);
			}
		}

		return areas;
    }

	//XXX A possible optimisation here might be to only call IdentifyArea() on tiles that
	// have been changed in the last round?

	/**
	 * Given a single tile, find all connected tiles of equal use type and intensity.
	 * (-> depth-first recursive search)
	 */
	private static void IdentifyArea(TileData tile)
	{				
		foreach (TileData neighbour in tile.adjacentTiles) 
		{
			if (neighbour == null)
				continue;

			if (neighbour.area != null)
				continue;

			else if (neighbour.type.Type == tile.type.Type &&
					 neighbour.intensity == tile.intensity) 
			{
				//label the tile with the area ID (as a side effect), then
				//recurse down until the whole habitat is identified
				neighbour.area = tile.area;
				tile.area.AddTile(neighbour);
				IdentifyArea(neighbour);
			}
		}
	}

}
