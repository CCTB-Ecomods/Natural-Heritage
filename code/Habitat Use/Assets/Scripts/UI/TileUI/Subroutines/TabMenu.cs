using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ChangeType
{
    None,
    Intensity, 
    Type
}

[RequireComponent(typeof(Button))]
public class TabMenu : MonoBehaviour
{
    Button button;
    [SerializeField] ChangeType settingsState;
    [SerializeField] GameObject icon;
    [SerializeField] Material activeIconMaterial;
    [SerializeField] List<TabMenu> disableButtons;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject panel;
    private readonly Vector3 _scale = new Vector3(1.3f, 1.3f, 1.3f);
    private bool active = false;

    Image _iconImage;
    private Material _oldMaterial;
    private Vector3 _oldScale;
    private Vector3 _oldIconScale;

    private void Awake()
    {
        button = GetComponent<Button>();
        _iconImage = icon.GetComponent<Image>();
        button.onClick.AddListener(onButtonClick);
        _oldScale = transform.localScale;
        _oldIconScale = icon.transform.localScale;
        _oldMaterial = _iconImage.material;
    }
    private void Start()
    {
        if (settingsState == ChangeType.Intensity) //prefer intensity
            enableMenu();
    }

    public void onButtonClick()
    {
        if (!active) enableMenu();
    }

    public void enableMenu()
    {
        active = true;
        text.SetText(settingsState.ToString());
        panel.SetActive(true);

        //scale
        var _localScale = _oldScale;
        _localScale.Scale(_scale);
        transform.localScale = _localScale;
        _localScale = _oldIconScale;
        _localScale.Scale(_scale);
        icon.transform.localScale = _localScale;

        _iconImage.material = activeIconMaterial;
        disableButtons.ForEach(m => m.disableMenu());
    }
    public void disableMenu()
    {
        panel.SetActive(false);
        if (!enabled) return;
        active = false;
        _iconImage.material = _oldMaterial;
        transform.localScale = _oldScale;
        icon.transform.localScale = _oldIconScale;
    }
}
