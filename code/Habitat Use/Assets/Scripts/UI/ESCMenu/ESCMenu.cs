using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ESCMenu : MonoBehaviour
{
    private GameObject _panel;
    private Image _bgImage;
    private Button _closeBtn;

    void Start()
    {
        _panel = transform.Find("Panel").gameObject;
        _bgImage = transform.Find("CloseButton").GetComponent<Image>();
        _closeBtn = transform.Find("CloseButton").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var state = _panel.activeSelf;
            SetPanelActive(!state);
        }
    }

    public void SetPanelActive(bool state)
    {
        _panel.SetActive(state);
        _bgImage.enabled = state;
        _closeBtn.enabled = state;
    }

    //private void toggleMenu()
    //{
    //    _panel.SetActive(!_panel.activeSelf);
    //    _bgImage.enabled = !_bgImage.enabled;
    //    _closeBtn.enabled = !_closeBtn.enabled;
    //}

    public void quitApplication()
    {
        Application.Quit();
    }

    public void openWiki()
    {
        WikiUiController.ActivatePanel(true);
        SetPanelActive(false);
    }

    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
