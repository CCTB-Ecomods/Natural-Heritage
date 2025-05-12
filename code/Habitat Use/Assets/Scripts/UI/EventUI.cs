using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventUI : MonoBehaviour, IUIPopup
{
    public GameObject Window;
    private Image _blur;
    
    private UIReferences UI;
    
    private string Title;
    private string Description;

    public new void Awake()
    {
        UI = GameObject.Find("GameUI").GetComponent<UIReferences>();
        _blur = gameObject.GetComponent<Image>();
        SetVisibility(false);
    }

    public void StartEvent(string title, string description)
    {
        Title = title;
        Description = description;
        UI.enqueuePopup(this);
    }

    private new void UpdateAndShow()
    {
        UI.eventTitle.GetComponent<TextMeshProUGUI>().text = Title;
        UI.eventDescription.GetComponent<TextMeshProUGUI>().text = Description;
        
        SetVisibility(true);
    }

    
    public void accept()
    {
        TopBarUiController.UpdateBalanceChange();
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
