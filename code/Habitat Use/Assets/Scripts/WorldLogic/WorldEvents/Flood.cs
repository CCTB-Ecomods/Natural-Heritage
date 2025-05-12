using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flood : WorldEvent
{
	private const string _name = "flood";
	private const string _description = "Your rivers flood! Tiles adjacent to a water tile suffer a productivity penalty relative to the usage intensity of that water tile.";

    public Flood() : base (_name, _description) 
    {
        //nothing to do here after the base constructor was called
    }

    public override bool EventTakesPlace(WorldData world)
    {
		bool success = false;
		foreach (TileData td in world.tiles) {
			int penalty = CalculateFloodDamage(td);
			if (penalty > 0) {
				td.productivityBonus.Add(new Bonus("flood", penalty*-1));
				success = true;
			}
		}
		return success;
    }

	/**
	 * Is this tile next to a river? Return 0 if not, or the highest river use intensity
	 * in the neighbourhood.
	 */
	private int CalculateFloodDamage(TileData tile)
	{
		if (tile.type.Type == TileType.RIVER)
			return 0;
		
		int adjacency = 0;
		foreach (TileData n in tile.adjacentTiles.Where(n => !(n == null))) {
			if (n.type.Type == TileType.RIVER && (int) n.intensity > adjacency)
				adjacency = (int) n.intensity;
		}
		if (adjacency > 0 && Parliament.IsEnacted("Flood Protection Act")) adjacency -= 1;
		return adjacency*3;
	}
}
