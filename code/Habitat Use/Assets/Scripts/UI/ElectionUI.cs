using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElectionUI : MonoBehaviour
{
    private static GameObject _electionWonPanel;
    private static GameObject _electionLostPanel;
    
    private UIReferences UI;

    private static ListEntry[] _wonListEntries;
    private static ListEntry[] _lostListEntries;
    

    private static Image _bgBlur;
    

    void Start()
    {
        _electionWonPanel = gameObject.transform.Find("ElectionWonPanel").gameObject;
        _electionLostPanel = gameObject.transform.Find("ElectionLostPanel").gameObject;
        _wonListEntries = _electionWonPanel.transform.GetComponentsInChildren<ListEntry>(true);
        _lostListEntries = _electionLostPanel.transform.GetComponentsInChildren<ListEntry>(true);
        UI = GameObject.Find("GameUI").GetComponent<UIReferences>();
        _bgBlur = GetComponent<UnityEngine.UI.Image>();
    }

    public static void SetActiveElectionWonUI(bool state, string approvalPerc, WorldData world)
    {
        _bgBlur.enabled = state;
        _electionWonPanel.SetActive(state);
        _electionWonPanel.transform.Find("MiddlePanel").Find("ApprovalPercTxt").
            GetComponent<TextMeshProUGUI>().text = approvalPerc;
        for (int i = 0; i < _wonListEntries.Length; i++)
        {
            ListEntry entry = _wonListEntries[i];
            if (i == 0)
            {
                entry.SetValues(world.TermFinances[world.TermFinances.Count - 2], world.finances, "Money:","$");
            }
            else if (i == 1)
            {
                entry.SetValues(world.sumTermApprovalValues[world.sumTermApprovalValues.Count - 2] * 100,
                    world.sumTermApprovalValues[world.sumTermApprovalValues.Count - 1] * 100, "Average approval:", "%");
            }
            else
            {
                entry.SetValues(world.residentGroups[i-2].GetLastTermApprovalRating()*100, world.residentGroups[i-2].GetApprovalRating()*100, world.residentGroups[i-2].GetName() +":","%");
            }
        }
    }

    public void DisableWonUI()
    {
        _electionWonPanel.SetActive(false);
        _bgBlur.enabled = false;
    }

    public static void SetActiveElectionLostUI(bool state, string msg, string highlightedReason, WorldData world)
    {
        _bgBlur.enabled = state;
        _electionLostPanel.SetActive(state);
        _electionLostPanel.transform.Find("MiddlePanel").Find("GameOverMsgTxt").
            GetComponent<TextMeshProUGUI>().text = msg;
        _electionLostPanel.transform.Find("MiddlePanel").Find("HighlightedReasonTxt").
            GetComponent<TextMeshProUGUI>().text = highlightedReason;
        for (int i = 0; i < _lostListEntries.Length; i++)
        {
            ListEntry entry = _lostListEntries[i];
            if(i == 0)
                entry.SetValues(world.TermFinances[world.TermFinances.Count - 2], world.finances, "Money:", "$");
            else if (i == 1)
                entry.SetValues(world.sumTermApprovalValues[world.sumTermApprovalValues.Count - 2]*100, world.sumTermApprovalValues[world.sumTermApprovalValues.Count - 1]*100, "Average approval:", "%");
            else
            {
                entry.SetValues(world.residentGroups[i-2].GetLastTermApprovalRating()*100, world.residentGroups[i-2].GetApprovalRating()*100, world.residentGroups[i-2].GetName() +":", "%");
            }
        }
    }

    public void DisableLostUI()
    {
        _electionLostPanel.SetActive(false);
        _bgBlur.enabled = false;
    }
}
