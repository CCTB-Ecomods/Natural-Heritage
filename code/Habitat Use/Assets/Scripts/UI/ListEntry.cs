using TMPro;
using UnityEngine;

public class ListEntry : MonoBehaviour
{
    private TextMeshProUGUI _oldValue, _newValue, _title;
    private UIReferences UI;


    private void Scan()
    {
        UI = GameObject.Find("GameUI").GetComponent<UIReferences>();
        _oldValue = gameObject.transform.Find("OldValue").gameObject.GetComponent<TextMeshProUGUI>();
        _newValue = gameObject.transform.Find("NewValue").gameObject.GetComponent<TextMeshProUGUI>();
        _title = gameObject.transform.Find("Title").gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void SetValues(float fOldValue, float fNewValue, string title, string unit)
    {
        int oldValue = (int) fOldValue;
        int newValue = (int) fNewValue;
        if (UI == null || _oldValue == null || _newValue == null || _title == null)
        {
            Scan();
        }
        _oldValue.text = oldValue + unit;
        _newValue.text = newValue + unit;
        if (newValue < oldValue)
            _newValue.fontMaterial = UI.red;
        else if (newValue > oldValue)
            _newValue.fontMaterial = UI.green;
        else
            _newValue.fontMaterial = UI.brown;
        _title.text = title;
    }
}