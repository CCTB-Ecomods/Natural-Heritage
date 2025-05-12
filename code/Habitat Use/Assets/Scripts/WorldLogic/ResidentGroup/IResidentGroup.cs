using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResidentGroup 
{

    /// <summary>
    /// Recalculates resident group's approval rate of mayor using landscape data 
    /// </summary>
    void UpdateApprovalRating(WorldData worldData);

    /// <summary>
    /// Get the name of resident group
    /// </summary>
    /// <returns>Name of resident group as string</returns>
    string GetName();

	/// <summary>
	/// Add an approval bonus to this resident group.
	/// </summary>
	void AddBonus(Bonus b);
	
    /// <summary>
    /// Get resident group's approval rate of mayor 
    /// </summary>
    /// <returns>Percentage of approval as float</returns>
    float GetApprovalRating();

    /// <summary>
    /// Adds the given rating value to the list of past Term Ratings
    /// </summary>
    void AddTermApprovalRating(float rating);

    /// <summary>
    /// Returns the secound to last Approval rating value
    /// </summary>
    /// <returns>Percentage of last TermApproval as Float</returns>>
    float GetLastTermApprovalRating();
}
