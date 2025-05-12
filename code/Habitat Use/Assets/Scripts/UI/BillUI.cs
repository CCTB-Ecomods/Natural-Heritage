using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BillUI : MonoBehaviour, IUIPopup
{
    public GameObject Window;
    private Image _blur;
    
    private UIReferences UI;

    private WorldData _worldData;
    public ILaw Law;
 
    public new void Awake()
    {
        UI = GameObject.Find("GameUI").GetComponent<UIReferences>();
        _blur = gameObject.GetComponent<Image>();
        SetVisibility(false);
    }
    
    public void StartBill(ILaw Law, ref WorldData worldData)
    {
        this.Law = Law;
        _worldData = worldData;
        UI.enqueuePopup(this);
    }

    private new void UpdateAndShow()
    {
        String description = "";
        if (Law.Enacted())
            description = "You already approved of this law, do you want to keep it active or repeal it? If you decline the bill now the bill will be repealed. If you accept the bill again the bill will stay active." +
                          Environment.NewLine;
        UI.lawDescription.GetComponent<TextMeshProUGUI>().text = description + Law.GetDescriptionForPlayer();
        UI.lawTitle.GetComponent<TextMeshProUGUI>().text = Law.GetName();
        UI.lawRequirement.GetComponent<TextMeshProUGUI>().text = Law.GetRequirement();
        UI.lawReward.GetComponent<TextMeshProUGUI>().text = Law.GetEffect();
        UI.lawRequirementNotMet.GetComponent<TextMeshProUGUI>().text = Law.GetPenalty();
        SetVisibility(true);
    }

    public void dismiss()
    {
        if(Law.Enacted())
            Law.RepealLaw(_worldData);
        else
            Law.RejectLaw(_worldData);
        cleanup();
    }

    public void accept()
    {
        Law.EnactLaw(_worldData);
        cleanup();
    }

    private void cleanup()
    {
        SetVisibility(false);
        UI.nextPopup();
    }

    public void Popup()
    {
        UpdateAndShow();
    }

    public void SetVisibility(bool visible)
    {
        Window.SetActive(visible);
        _blur.enabled = visible;
    }
}