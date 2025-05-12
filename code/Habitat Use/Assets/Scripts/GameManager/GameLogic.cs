using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

public class GameLogic : MonoBehaviour, IGameLogic
{
    public static readonly int termDuration = 6;
    public static readonly float minApprovalPerc = 0.5F;
	public static readonly float minBankBalance = 0;

	public Animation dayNightCycle;

    public WorldData world;
	public static Dictionary<string,Action<WorldData>> hooks;
	public UIReferences UI;
	private static System.Random rng;
	private ElectionSystem _elecSys;
	private UnityEvent roundChange = new UnityEvent();
    private bool _votedOut;
	private bool firstRound = true;
        
    public void AddRoundChangeEventListener(UnityAction call) { roundChange.AddListener(call); }
    public void SetWorld(WorldData w) { world = w; }
    public WorldData GetWorld() { 
		if (world == null)
			world = GameObject.FindGameObjectWithTag("Map").GetComponent<WorldData>();
		return world; }
    public ElectionSystem GetElectionSystem() {
		if (_elecSys == null)
			_elecSys = new ElectionSystem(world);
		return _elecSys; 
	}

	/**
	 * A utility function to give the rest of the project access to an RNG instance.
	 */
	public static int Random(int max)
	{
		if (rng == null) rng = new System.Random();
		return rng.Next(max);
	}


    private void Awake()
	{
		if (world == null)
			world = GameObject.FindGameObjectWithTag("Map").GetComponent<WorldData>();
        UI = GameObject.Find("GameUI").GetComponent<UIReferences>();
        world.populationSize = Economy.GetTotalPopulation(world.tiles);
		world.maxLandscapeValues = GetMaxLandscapeValues();

		if (_elecSys == null)
			_elecSys = new ElectionSystem(world);
        dayNightCycle = GameObject.Find("Directional Light").GetComponent<Animation>();
    }

    public void Start()
    {
		hooks = new Dictionary<string,Action<WorldData>>();
		world.populationSize = Economy.GetTotalPopulation(world.tiles);
		world.maxLandscapeValues = GetMaxLandscapeValues();
		Parliament.InitLaws();

		OnRoundChange();

		//reset finances value of map prefab
		world.finances = Economy.initBalance;
		world.init(); //initializes Past TermLists with the starting values
		TopBarUiController.UpdateBalanceChange();
	}

    public void OnRoundChange()
    {
        RunGameIteration();
        if(!firstRound)
	        dayNightCycle.Play();
    }

    private void RunGameIteration()
    {
		world.year++;

        _elecSys.NextRound();
        if (_elecSys.StillInOffice)
        {
            WorldEvents();
			Legislate();
			RunHooks();
            CalculateTileData();
            CalculateWorldData();
            UpdateTileMeshes();
            UpdateUI();
            ShowEvents();
        }
        else
        {
            Debug.Log("You lost");
			/* TODO
			 * deactivate Ui
			 * show lose screen with stats and quit button
			 * back to main menu on quit button
			 */
        }

        roundChange.Invoke();
    }

	private void WorldEvents()
	{
		if (!firstRound) {
			// city sprawl
			if (world.year % termDuration == 0) {
				Society.CityGrowth(world);
				Society.CityGrowth(world);
			}
			// climate & biodiversity events
			bool eventHappened = ClimateEvents();
			if (!eventHappened) BiodiversityEvents();
		}
		else firstRound = false;
	}

	private void Legislate()
	{
		if (world.year < termDuration) return; //no legislation in the first six turns
		int term = world.year % termDuration;
		if (term == 0) {
			UI.trippleLawUI.GetComponent<TrippleUI>().StartTrippleLaw(Parliament.GetAllResearchBudgets(),Parliament.GetAllSubsidies(), ref world);
		} else if (term == 4 || term == 2) {
			UI.lawUI.GetComponent<BillUI>().StartBill(Parliament.GetRandomBill(), ref world);
		} else if (Random(8)==1) {
			UI.questUI.GetComponent<QuestUI>().StartQuest(Parliament.GetRandomPetition(), ref world);
		}
	}

	private bool ClimateEvents()
	{
		bool success = false;
		if (GameLogic.Random(100) < Climate.GetWeatherEventProbability(world.year)*100) {
			WorldEvent weatherEvent = Climate.GetRandomWeatherEvent();
			success = weatherEvent.EventTakesPlace(world);
			if (success) {
				Debug.Log(weatherEvent.GetName());
				UI.eventUI.GetComponent<EventUI>().StartEvent(weatherEvent.GetName(), weatherEvent.GetDescriptionForPlayer());
			}
		}
		return success;
	}

	private bool BiodiversityEvents()
	{
		WorldEvent biodivEvent = Climate.GetRandomBiodiversityEvent();
		bool success = biodivEvent.EventTakesPlace(world);
		if (success) {
			Debug.Log(biodivEvent.GetName());
			UI.eventUI.GetComponent<EventUI>().StartEvent(biodivEvent.GetName(), biodivEvent.GetDescriptionForPlayer());
		}
		return success;
	}

	private void RunHooks()
	{
		// we need to create a shallow copy of the hooks to work with, because
		// some hooks remove themselves, thus triggering an InvalidOperationException
		// when we try to iterate over the "master" hooks
		Dictionary <string, Action<WorldData>> hookCopies = new Dictionary<string,Action<WorldData>>(hooks);
		foreach (string k in hookCopies.Keys) {
			hookCopies[k](world);
		}
	}

    private void CalculateTileData()
    {
		world.averageIntensity = Nature.AverageIntensity(world.tiles);
        world.areas = Nature.CalculateAreas(world.tiles);
        //We need to calculate all diversities before we can do the productivity
        foreach (TileData tileData in world.tiles) {
            TileLogic.CalculateBiodiversity(tileData, world);
        }
		foreach (TileData tileData in world.tiles) {
            TileLogic.CalculateProductivity(tileData, world);
        }
    }

    private void CalculateWorldData()
    {
		world.landscapeValues.NatureValue = Nature.NatureValue(world.tiles);
		world.landscapeValues.TouristValue = Society.TouristValue(world.tiles);
		world.landscapeValues.IndustryValue = Economy.IndustryValue(world.tiles);
		Society.UpdateApproval(world);
		world.finances += Economy.CalculateBalance(world);
		CalculateDebug(world);
    }

	private void CalculateDebug(WorldData world)
	{
		float prodSum = 0;
		float divSum = 0;
		foreach (TileData td in world.tiles) {
			divSum += td.currentBiodiversity;
			prodSum += td.currentProductivity;
			if (td.currentBiodiversity > world.maxDiv) world.maxDiv = td.currentBiodiversity;
			else if (td.currentBiodiversity < world.minDiv) world.minDiv = td.currentBiodiversity;
			if (td.currentProductivity > world.maxProd) world.maxProd = td.currentProductivity;
			else if (td.currentProductivity < world.minProd) world.minProd = td.currentProductivity;

		}
		world.meanDiv = divSum / world.tiles.Length;
		world.meanProd = prodSum / world.tiles.Length;
	}

    private void UpdateTileMeshes()
    {
        foreach (TileData tileData in world.tiles)
        {
            tileData.gameObject.GetComponent<TileMeshUpdater>().UpdateMesh();
			tileData.updateData();
        }
    }

    private void UpdateUI()
    {
        HexagonUi.onUpdate();
        ResidentApprovalUi.onUpdate();
        UI.nextPopup();
    }

	/**
	 * Calculate the maximum possible landscape values.
	 * XXX warning: magic numbers!
	 */
	private LandUseValue GetMaxLandscapeValues()
	{
		LandUseValue luv = new LandUseValue(0,0,0);
		luv.NatureValue = 50*world.tiles.Length;
		luv.TouristValue = 50*world.tiles.Length;
		luv.IndustryValue = 50*world.tiles.Length;
		return luv;
	}

	private void ShowEvents()
	{
		foreach (TileData tile in world.tiles)
		{
				foreach (ParticleSystem particle in tile.ParticleSystems.Values)
					particle.Stop();
				foreach (Bonus bonus in tile.productivityBonus)
					if(tile.ParticleSystems.ContainsKey(bonus.GetName()))
						tile.ParticleSystems[bonus.GetName()].Play();
				foreach (Bonus bonus in tile.diversityBonus)
					if(tile.ParticleSystems.ContainsKey(bonus.GetName()))
						tile.ParticleSystems[bonus.GetName()].Play();
		}
	}
}
