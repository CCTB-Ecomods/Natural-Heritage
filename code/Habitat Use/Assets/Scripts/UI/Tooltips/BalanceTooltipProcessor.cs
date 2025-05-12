using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BalanceTooltipProcessor : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    private WorldData _worldData;
    private static bool _tooltipActive;

    void Start()
    {
        _worldData = GameObject.FindWithTag("GameManager").GetComponent<IGameLogic>().GetWorld();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_tooltipActive)
        {
            Tooltips.HideTooltip();
            _tooltipActive = false;
        }
        else
        {
            Tooltips.ShowBalanceTooltip(_worldData, 0);
            _tooltipActive = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_tooltipActive)
        {
            Tooltips.HideTooltip();
            _tooltipActive = false;
        }
    }
}
