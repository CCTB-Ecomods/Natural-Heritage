using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TileSettingsSelectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.enabled = false;
    }
}
