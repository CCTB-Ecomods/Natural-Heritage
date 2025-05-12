using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class ElectionSystem 
{
    public UnityEvent election = new UnityEvent();
    public bool StillInOffice { get; private set; }

    public int _currentRound { get; private set; }

    private readonly WorldData _world;

    private string lostMsg;
    private string highlightedReason;
    
    
    
    public ElectionSystem(WorldData world)
    {
        _world = world;

        StillInOffice = true;
    } 

    public void NextRound()
    {
        if (++_currentRound % GameLogic.termDuration == 0)
        {
            StillInOffice = EvaluateCurrentState();
            election.Invoke();

            if (StillInOffice) ElectionUI.SetActiveElectionWonUI(true, highlightedReason, _world);
            else ElectionUI.SetActiveElectionLostUI(true, lostMsg, highlightedReason, _world);
        }
    }

    public bool EvaluateCurrentState()
    {
        //add current bank balance to the list of previous balances 
        _world.TermFinances.Add(_world.finances);
        //check for bank balance


        // calculate approval
        float sumApprovalValues = 0;
        float amountGroups = 0;
        foreach (var group in _world.residentGroups)
        {
            group.AddTermApprovalRating(group.GetApprovalRating()); //add current groups approval rating to the list of previous ratings 
            sumApprovalValues += group.GetApprovalRating();
            amountGroups++;
        }
        float approvalPercentage = 0;
        if (amountGroups > 0)
            approvalPercentage = sumApprovalValues / amountGroups;
        _world.sumTermApprovalValues.Add(approvalPercentage); //add current summApprovalPrecentage to the list of previous rating percentages 
        Debug.Log("Approval: "+approvalPercentage+"/"+GameLogic.minApprovalPerc);
        if (approvalPercentage < GameLogic.minApprovalPerc)
        {
            lostMsg = "You did not get reelected because your approval rate was lower than " +
				GameLogic.minApprovalPerc*100 + "%. \nYour approval rate: ";
            highlightedReason = (Math.Round(approvalPercentage * 100, 2)).ToString() + "%";
            return false;
        }
        else highlightedReason = (Math.Round(approvalPercentage * 100, 2)).ToString() + "%";

        if (_world.finances < GameLogic.minBankBalance)
        {
            lostMsg = "You did not get reelected because you bank balance dropped below $" +
                GameLogic.minBankBalance.ToString() + ". \nYour current bank balance: ";
            highlightedReason = "$ " + _world.finances.ToString();
            return false;
        }
        return true;
    }

    public override string ToString()
    {
        return "Year: " + _currentRound + " | " + "Election: " + (GameLogic.termDuration - (_currentRound % GameLogic.termDuration) + " y");
    }
}
