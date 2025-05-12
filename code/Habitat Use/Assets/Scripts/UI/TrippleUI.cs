using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrippleUI : MonoBehaviour, IUIPopup
{
    public GameObject Window1, Window2, Window3, Button;
    private Image _blur;
    
    private UIReferences UI;

    private WorldData _worldData;
    public bool first = true;
    public ILaw Law1, Law2, Law3, Law4, Law5, Law6;
    
    public new void Awake()
    {
        UI = GameObject.Find("GameUI").GetComponent<UIReferences>();
        _blur = gameObject.GetComponent<Image>();
        SetVisibility(false);
    }
    
    public void StartTrippleLaw(List<ILaw> ResearchLaws, List<ILaw> Subsidies, ref WorldData worldData)
    {
        Law1 = ResearchLaws[0];
        Law2 = ResearchLaws[1];
        Law3 = ResearchLaws[2];
        Law4 = Subsidies[0];
        Law5 = Subsidies[1];
        Law6 = Subsidies[2];
        first = true;
        _worldData = worldData;
        UI.enqueuePopup(this);
    }

    public void ManageTrippleUi()
    {
        if (first)
            UpdateAndShowTrippleUI(Law1, Law2, Law3);
        else
            UpdateAndShowTrippleUI(Law4, Law5, Law6);
        SetVisibility(true);
    }

    public void UpdateAndShowTrippleUI(ILaw Law1, ILaw Law2, ILaw Law3)
    {
        UI.trippleLaw1Description.GetComponent<TextMeshProUGUI>().text = Law1.GetDescriptionForPlayer();
        UI.trippleLaw2Description.GetComponent<TextMeshProUGUI>().text = Law2.GetDescriptionForPlayer();
        UI.trippleLaw3Description.GetComponent<TextMeshProUGUI>().text = Law3.GetDescriptionForPlayer();

        UI.trippleLaw1Title.GetComponent<TextMeshProUGUI>().text = Law1.GetName();
        UI.trippleLaw2Title.GetComponent<TextMeshProUGUI>().text = Law2.GetName();
        UI.trippleLaw3Title.GetComponent<TextMeshProUGUI>().text = Law3.GetName();
        
        UI.trippleLaw1Requirement.GetComponent<TextMeshProUGUI>().text = Law1.GetRequirement();
        UI.trippleLaw2Requirement.GetComponent<TextMeshProUGUI>().text = Law2.GetRequirement();
        UI.trippleLaw3Requirement.GetComponent<TextMeshProUGUI>().text = Law3.GetRequirement();
        
        UI.trippleLaw1Reward.GetComponent<TextMeshProUGUI>().text = Law1.GetEffect();
        UI.trippleLaw2Reward.GetComponent<TextMeshProUGUI>().text = Law2.GetEffect();
        UI.trippleLaw3Reward.GetComponent<TextMeshProUGUI>().text = Law3.GetEffect();
    }

    public void dismissAll()
    {
        if (first)
        {
            Law1.RejectLaw(_worldData);
            Law2.RejectLaw(_worldData);
            Law3.RejectLaw(_worldData);
            first = false;
            ManageTrippleUi();
        }
        else
        {
            Law4.RejectLaw(_worldData);
            Law5.RejectLaw(_worldData);
            Law6.RejectLaw(_worldData);
            cleanup();
        }
    }

    public void accept(int law)
    {
        if (first)
        {
            first = false;
            switch (law)
            {
                case 1:
                    AcceptReject(Law1, Law2, Law3);
                    break;
                case 2:
                    AcceptReject(Law2, Law1, Law3);
                    break;
                case 3:
                    AcceptReject(Law3, Law1, Law2);
                    break;
            }
            ManageTrippleUi();
        }
        else
        {
            switch (law)
            {
                case 1:
                    AcceptReject(Law4, Law5, Law6);
                    break;
                case 2:
                    AcceptReject(Law5, Law4, Law6);
                    break;
                case 3:
                    AcceptReject(Law6, Law4, Law5);
                    break;
            }
            cleanup();
        }
        TopBarUiController.UpdateBalanceChange();
    }

    private void AcceptReject(ILaw accept, ILaw reject1, ILaw reject2)
    {
        accept.EnactLaw(_worldData);
        reject1.RejectLaw(_worldData);
        reject2.RejectLaw(_worldData);
        TopBarUiController.UpdateBalanceChange();
    }

    private void cleanup()
    {
        first = true;
        SetVisibility(false);
        UI.nextPopup();
    }

    public void Popup()
    {
        ManageTrippleUi();
    }
    
    public void SetVisibility(bool visible)
    {
        Window1.SetActive(visible);
        Window2.SetActive(visible);
        Window3.SetActive(visible);
        Button.SetActive(visible);
        _blur.enabled = visible;
    }
}