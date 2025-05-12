using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ArrowScript : MonoBehaviour
{
    [SerializeField] TabMenu tabMenuToEnable;
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        tabMenuToEnable.onButtonClick();
    }
}
