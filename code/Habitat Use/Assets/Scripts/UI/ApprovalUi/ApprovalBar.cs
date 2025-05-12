using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ApprovalBar
{    
    private GameObject _barUiPrefab;
    
    private IResidentGroup _group;
    //private Color _color;
    private ResidentApprovalUi _parentUi;

    private TextMeshProUGUI _changeText;
    private GameObject _associatedBar;
    private TextMeshProUGUI _text;
    private GameObject _foreground;

    float oldPercentage = -1;

    public ApprovalBar(IResidentGroup group, Color color, ResidentApprovalUi parentUi, GameObject bar)
    {
        _group = group;
        //_color = color;
        _parentUi = parentUi;
        _barUiPrefab = bar;

        _associatedBar = GameObject.Instantiate(_barUiPrefab);
        _associatedBar.transform.SetParent(_parentUi.gameObject.transform);

        _changeText = _associatedBar.transform.Find("Icon").Find("Change").gameObject.GetComponent<TextMeshProUGUI>();
        _text = _associatedBar.transform.Find("Body").Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
        _text.text = _group.GetName();
        _foreground = _associatedBar.transform.Find("Body").Find("Bar").Find("Foreground").gameObject;
        //_foreground.GetComponent<Image>().color = _color;
    }

    public void Update()
    {
        float percentage = _group.GetApprovalRating();

        if (oldPercentage != -1)
            _changeText.text = percentage > oldPercentage ? "+" : "-";


        _foreground.transform.localScale = new Vector2(percentage, 1);

        oldPercentage = percentage;
    }
}
