using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WikiUiController : MonoBehaviour
{
    public static GameObject oldGo;

    private static GameObject _wikiPanel;
    private static Image _bgImage;
    private static Button _closeBtn;

    // Start is called before the first frame update
    void Start()
    {
        _wikiPanel = transform.Find("Panel").gameObject;
        _bgImage = transform.Find("CloseButton").GetComponent<Image>();
        _closeBtn = transform.Find("CloseButton").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetActivePanel(false);
        }
    }

    public void ActivateMenu(GameObject go)
    {
        if (oldGo != null)
            oldGo.SetActive(false);
        go.SetActive(true);
        oldGo = go;
    }

    public void SetActivePanel(bool state)
    {
        _wikiPanel.SetActive(state);
        _bgImage.enabled = state;
        _closeBtn.enabled = state;
    }

    public static void ActivatePanel(bool state)
    {
        _wikiPanel.SetActive(state);
        _bgImage.enabled = state;
        _closeBtn.enabled = state;
    }
}
