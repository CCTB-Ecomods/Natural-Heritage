using System;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TopBarUiController : MonoBehaviour
{
    private static IGameLogic _gameLogic;
    private static WorldData _world;
    private static ElectionSystem _elecSys;

    [SerializeField]
    private GameObject _TurnObject;
    [SerializeField]
    private GameObject _ElectionObject;
    [SerializeField]
    private GameObject _BalanceCounterObject;
    [SerializeField]
    private GameObject _BalanceChangeObject;

    [SerializeField]
    private TextMeshProUGUI _LocalTorismValue;
    [SerializeField]
    private TextMeshProUGUI _LocalTorismAverage;
    [SerializeField]
    private TextMeshProUGUI _LocalBioValue;
    [SerializeField]
    private TextMeshProUGUI _LocalBioAverage;
    [SerializeField]
    private TextMeshProUGUI _LocalProdValue;
    [SerializeField]
    private TextMeshProUGUI _LocalProdAverage;

    [SerializeField]
    private static TextMeshProUGUI _TorismValue;
    [SerializeField]
    private static TextMeshProUGUI _TorismAverage;
    [SerializeField]
    private static TextMeshProUGUI _BioValue;
    [SerializeField]
    private static TextMeshProUGUI _BioAverage;
    [SerializeField]
    private static TextMeshProUGUI _ProdValue;
    [SerializeField]
    private static TextMeshProUGUI _ProdAverage;

    private static TextMeshProUGUI _gameTurnCounter;
    private static TextMeshProUGUI _electionCounter;
    private static TextMeshProUGUI _balanceCurrent;
    private static TextMeshProUGUI _balanceChange;
	private static CultureInfo _de;

    // Start is called before the first frame update
    void Awake()
    {
        _gameTurnCounter = _TurnObject.GetComponent<TextMeshProUGUI>();
        _electionCounter = _ElectionObject.GetComponent<TextMeshProUGUI>();
        _balanceCurrent = _BalanceCounterObject.GetComponent<TextMeshProUGUI>();
        _balanceChange = _BalanceChangeObject.GetComponent<TextMeshProUGUI>();
        _TorismValue = _LocalTorismValue;
        _TorismAverage = _LocalTorismAverage;
        _BioValue= _LocalBioValue;
        _BioAverage= _LocalBioAverage;
        _ProdValue= _LocalProdValue;
        _ProdAverage= _LocalProdAverage;

    _gameLogic = GameObject.FindWithTag("GameManager").GetComponent<IGameLogic>();
        _world = _gameLogic.GetWorld();
        _elecSys = _gameLogic.GetElectionSystem();

        _gameLogic.AddRoundChangeEventListener(UpdateTopBar);
		_de = new CultureInfo("de-DE");

        UpdateTopBar();
    }

    private static void UpdateTopBar()
    {
        _gameTurnCounter.text = "Round " + _elecSys._currentRound;
        _electionCounter.text = "Election in " + (GameLogic.termDuration - (_elecSys._currentRound % GameLogic.termDuration));
        var num = _world.tiles.Length;
        var landscapeVals = _world.landscapeValues;
        _TorismValue.text = Math.Abs(landscapeVals.TouristValue).ToString("##,#", _de);
        _TorismAverage.text = Math.Abs(landscapeVals.TouristValue/num).ToString("Ø ##,#", _de);
        _BioValue.text = Math.Abs(landscapeVals.NatureValue).ToString("##,#", _de);
        _BioAverage.text = Math.Abs(landscapeVals.NatureValue / num).ToString("Ø ##,#", _de);
        _ProdValue.text = Math.Abs(landscapeVals.IndustryValue).ToString("##,#", _de);
        _ProdAverage.text = Math.Abs(landscapeVals.IndustryValue / num).ToString("Ø ##,#", _de);

        UpdateBalanceChange();
    }

    public static void UpdateBalanceChange()
    {
        int change = Convert.ToInt32(Economy.CalculateBalance(_world));
        string bchange = "";
        bchange += (change >= 0 ? "+" : "-") + " ";
        bchange += Math.Abs(change).ToString("##,#", _de);

        int finances = Convert.ToInt32(_world.finances);
        string bcurrent = "";
        bcurrent += finances >= 0 ? "" : "- ";
        bcurrent += Math.Abs(finances).ToString("##,#", _de);
        _balanceChange.text = bchange;
        _balanceCurrent.text = bcurrent;
    }
}
