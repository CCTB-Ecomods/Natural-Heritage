using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldData : MonoBehaviour
{
	//The current game year
	public int year;
	
	//The actual array holding all tiles
	public TileData[] tiles;

	//A list of areas with connected habitat
	public List<IArea> areas;

	//Land use intensity averaged across the landscape
	public float averageIntensity;

	//actual and (theoretical) max landscape values - Nature/Tourism/Industry
	public LandUseValue landscapeValues = new LandUseValue(0, 0, 0);
	public LandUseValue maxLandscapeValues;
	
    //Voter approval - Conservationist/Tourist/Resident/Farmer/Industrialist/Overall
	public List<IResidentGroup> residentGroups = new List<IResidentGroup> {
			new Conservationist("Conservationists", new LandUseValue(80, 20, 0)),
			new Tourist("Tourists", new LandUseValue(30, 60, 10)),
			new Resident("Residents", new LandUseValue(20, 40, 40)),
			new Farmer("Farmers", new LandUseValue(30, 0, 70)),
			new Industrialist("Industrialists", new LandUseValue(0, 20, 80))
	};

	//Size of the population - depends on the number of city tiles
	public int populationSize;

	//Added income or expenditures (e.g. from subsidies)
	public List<Bonus> revenue;
	
    //Current bank balance
	public float finances = 0;

	public List<float> TermFinances = new List<float>();
	public List<float> sumTermApprovalValues = new List<float>();

	//DEBUG INFORMATION
	public float maxDiv = 0;
	public float maxProd = 0;
	public float meanDiv = 0;
	public float meanProd = 0;
	public float minDiv = 50;
	public float minProd = 50;

	public void init()
	{
		TermFinances.Add(finances);
		
		//approvalPercentage
		float sumApprovalValues = 0;
		float amountGroups = 0;
		foreach (var group in residentGroups)
		{
			group.AddTermApprovalRating(group.GetApprovalRating());
			sumApprovalValues += group.GetApprovalRating();
			amountGroups++;
		}
		float approvalPercentage = 0;
		if (amountGroups > 0)
			approvalPercentage = sumApprovalValues / amountGroups;
		sumTermApprovalValues.Add(approvalPercentage);

	}
}
