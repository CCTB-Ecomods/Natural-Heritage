using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class GeneralInfos : MonoBehaviour
{
    private static IGameLogic _gameLogic;
    private static WorldData _world;
    private static ElectionSystem _elecSys;
    private static CultureInfo _de;

    [SerializeField]
    private TextMeshProUGUI _productivityText;
    [SerializeField]
    private TextMeshProUGUI _biodiversityText;
    [SerializeField]
    private TextMeshProUGUI _TourismText;

    private void Awake()
    {
        _gameLogic = GameObject.FindWithTag("GameManager").GetComponent<IGameLogic>();
        _world = _gameLogic.GetWorld();
        _elecSys = _gameLogic.GetElectionSystem();
        _de = new CultureInfo("de-DE");
        _gameLogic.AddRoundChangeEventListener(UpdateInfos);
        UpdateInfos();
    }
    public void UpdateInfos()
    {
        var num = _world.tiles.Length;
        var landscapeVals = _world.landscapeValues;

        int prod = (int)Math.Abs((landscapeVals.IndustryValue / num) * 2f);
        int bio = (int)Math.Abs((landscapeVals.NatureValue / num) * 2f);
        int touri = (int)Math.Abs((landscapeVals.TouristValue / num) * 2f);

        _productivityText.text = prod.ToString("##,#", _de) + "%";
        _biodiversityText.text = bio.ToString("##,#", _de) + "%";
        _TourismText.text = touri.ToString("##,#", _de) + "%";
    }
}
