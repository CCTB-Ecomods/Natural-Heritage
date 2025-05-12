using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundMenuController : MonoBehaviour
{
    private selected_dictionary _selected_Dictionary;
    public void OnClickNextRoundButton()
    {
        if (_selected_Dictionary == null)
            _selected_Dictionary = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<selected_dictionary>();
        _selected_Dictionary.deselectAll();

        GameObject[] gameManagers = GameObject.FindGameObjectsWithTag("GameManager");
        foreach (GameObject gameManager in gameManagers)
        {
            gameManager.GetComponent<IGameLogic>().OnRoundChange();
        }
    }
}
