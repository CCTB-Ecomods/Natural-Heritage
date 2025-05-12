using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResidentGroup : IResidentGroup
{
	//XXX check if a modifier of 0.5 gives acceptable approval values in the actual game
	// (normal ranges between 20% and 80% would be good)
	private float _requiredLandscapeValue = 0.9F;
    private float _approvalRating;
    private string _name;
	private List<Bonus> _approvalBonuses;

	private List<float> _pastApprovalRatings = new List<float>();

    public ResidentGroup(string name)
    {
        _name = name;
		_approvalBonuses = new List<Bonus>();
    }

    public float GetApprovalRating()
    {
        return _approvalRating;
    }

    public float GetLastTermApprovalRating()
    {
	    return _pastApprovalRatings[(_pastApprovalRatings.Count - 2)];
    }

    public void AddTermApprovalRating(float rating)
    {
	    _pastApprovalRatings.Add(rating);
    }

    public string GetName()
    {
        return _name;
    }

	/**
	 * Every point of productivity/diversity/tourist value in the landscape contributes to the 
	 * happiness of the voter groups, scaled by the importance the group attaches to that land
	 * use category. To get a percentage approval, divide this absolute happiness by the
	 * maximum value the landscape could in theory produce.
	 */
    public void UpdateApprovalRating(WorldData worldData)
    {
		LandUseValue voterValues = GetIdealLandscapeValues();
		LandUseValue landscapeValues = worldData.landscapeValues;
		LandUseValue mlv = worldData.maxLandscapeValues;
		float natureRating = (voterValues.NatureValue / 100) * landscapeValues.NatureValue;
		float touristRating = (voterValues.TouristValue / 100) * landscapeValues.TouristValue;
		float industryRating = (voterValues.IndustryValue / 100) * landscapeValues.IndustryValue;
		// We don't actually know *what* the maximum possible landscape value is, the only value
		// we can estimate is significantly too high. (Nature value and industry value will never
		// reach their respective maximums together.) Therefore, to get balanced approval ratings,
		// we need to scale this summed maxValue by a factor we glean from experience
		float maxAvgValue = (((mlv.NatureValue * voterValues.NatureValue / 100) +
							  (mlv.TouristValue * voterValues.TouristValue / 100) +
							  (mlv.IndustryValue * voterValues.IndustryValue / 100)) *
							 _requiredLandscapeValue) / worldData.tiles.Length;
		float avgTileRating = (natureRating+touristRating+industryRating) / (worldData.tiles.Length);
		// Determine approval along a sigmoid curve (https://en.wikipedia.org/wiki/Logistic_function)
		// with L = 1, k = 0.2, and x0 = 22.5 (50/2*_requiredLandscapeValue)
		// This means that approval ratings respond quicker in the intermediate value range
		float approvalRating = 1 / (1 + (float) Math.Exp(-0.2*(avgTileRating-(maxAvgValue/2))));
		approvalRating += ApprovalBonusSum();
		//approval ratings decay over time
		approvalRating -= (float) Math.Floor((double) worldData.year/2)/100;
		if (approvalRating > 1) {
			approvalRating = 1;
			Debug.Log("Approval rating has reached 100%");
		}
		_approvalRating = approvalRating;
    }

    protected abstract LandUseValue GetIdealLandscapeValues();

	/**
	 * Add an approval bonus to this resident group.
	 */
	public void AddBonus(Bonus b)
	{
		_approvalBonuses.Add(b);
	}
	
	/**
	 * Calculate the sum of all bonusses (and remove any that are inactive).
	 */
	private float ApprovalBonusSum()
    {
		float sum = 0;
		for (int i = 0; i < _approvalBonuses.Count; i++) {
			Bonus b = _approvalBonuses[i];
			if (b.IsValid()) sum += b.GetValue();
			else {
				_approvalBonuses.Remove(b);
				i--;
			}
		}
		return sum/100;
	}

}
