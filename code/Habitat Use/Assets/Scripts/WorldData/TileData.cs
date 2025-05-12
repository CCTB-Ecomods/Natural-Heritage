using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileData : MonoBehaviour
{
    public float currentBiodiversity;
    public float currentProductivity;

    public TileType oldTileType;
    public Intensity oldIntensity;

    [System.Obsolete("Use TileData.type.Type")]
    public TileType tileType = TileType.FOREST;
    public GeneralTileType type;
    public Intensity intensity = Intensity.NONE;
    public IArea area = new Area();
    public TileData[] adjacentTiles;

	public List<Bonus> diversityBonus = new List<Bonus>();
	public List<Bonus> productivityBonus = new List<Bonus>();

	//Show the components of this tile's biodiversity and productivity
	public float[] biodiversityFactors = new float[4]; //intensity, area, landscape, bonuses
	public float[] productivityFactors = new float[5]; //intensity, area, landscape, ecosystem services, bonuses

    public int xCoordinate;
    public int yCoordinate;

    public Dictionary<String, ParticleSystem> ParticleSystems = new Dictionary<string, ParticleSystem>();

    private void Awake()
    {
        if (type == null)
        {
            switch (tileType)
            {
                case TileType.CITY:
                    type = new City(this);
                    break;

                case TileType.FIELD:
                    type = new Field(this);
                    break;

                case TileType.FOREST:
                    type = new Forest(this);
                    break;

                case TileType.RIVER:
                    type = new River(this);
                    break;

            }
        }

        foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
            ParticleSystems.Add(particle.gameObject.name,particle);
    }

    public override string ToString() { 
        return "TileData {TileType: " + type.Type + ", Intensity: "+ intensity + ", CurrentBiodiversity: "+ currentBiodiversity + ", CurrentProductivity: " + currentProductivity + "}";
    }

    private void OnMouseEnter()
    {
        if (LayerUI.activeLayer != OverlayLayers.NONE)
            Tooltips.ShowTileTooltip(this, 1, Input.mousePosition);
    }

    private void OnMouseExit()
    {
        Tooltips.HideTooltip();
    }

    public void updateData()
    {
        oldTileType = type.Type;
        oldIntensity = intensity;
    }
}
