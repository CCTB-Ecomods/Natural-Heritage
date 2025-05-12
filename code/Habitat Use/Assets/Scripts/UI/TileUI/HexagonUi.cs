using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class HexagonUi : MonoBehaviour
{
    public GameObject hoverOutline;
    public GameObject selectedOutline;
    public GameObject changedOutline;

    public GameObject biodiversityOverlay;
    public Material biodiversityMaterial;
    public GameObject productivityOverlay;
    public Material productivityMaterial;
    private float _overlayScaleFactor = 1/50f; //biodiversity and productivity between 0 and 50
    private int _overlayDistance = 0;
    private TileData _td;

    static private WorldData worldData;

    private void Awake()
    {
        _td = GetComponent<TileData>();
        if (_td == null)
            throw new Exception("HexagonUI-Script is a component of an object wich does not have a TileData-component");

        if (worldData == null)
            worldData = GameObject.FindGameObjectWithTag("GameManager").GetComponent<IGameLogic>().GetWorld();
    }

    static public Queue<HexagonUi> editedUIs = new Queue<HexagonUi>();
    public void activateBiodiversityOverlay(bool b)
    {
        biodiversityOverlay.SetActive(b);
    }

    public void updateBiodiversityOverlay()
    {
        float currentBio = _td.currentBiodiversity;

        //height
        biodiversityOverlay.transform.localPosition = new Vector3(0, _overlayDistance + currentBio * _overlayScaleFactor, 0);
        //biodiversityOverlay.transform.localPosition = new Vector3(0, 3, 0);
        
        //brightness
        Color bioColor = biodiversityMaterial.color;
        float h;
        float s;
        float v;
        Color.RGBToHSV(bioColor, out h, out s, out v);
        v = currentBio * _overlayScaleFactor;
        bioColor = Color.HSVToRGB(h, s, v);

        //alpha
        //bioColor.a =  currentBio / 20f;
        bioColor.a =  1f;
        biodiversityOverlay.GetComponent<MeshRenderer>().material.color = bioColor;
    }
    public void activateProductivityOverlay(bool b)
    {
        productivityOverlay.SetActive(b);
    }

    public void updateProductivityOverlay()
    {
        float currentProd = _td.currentProductivity;
        productivityOverlay.transform.localPosition = new Vector3(0, _overlayDistance + currentProd * _overlayScaleFactor*2, 0);

        //brightness
        Color prodColor = productivityMaterial.color;
        float h;
        float s;
        float v;
        Color.RGBToHSV(prodColor, out h, out s, out v);
        v = currentProd * _overlayScaleFactor;
        prodColor = Color.HSVToRGB(h, s, v);

        //alpha
        //prodColor.a = currentProd / 20f;
        prodColor.a = 1f;
        productivityOverlay.GetComponent<MeshRenderer>().material.color = prodColor;
    }
    public void activateHoverOutline(bool b)
    {
        hoverOutline.SetActive(b);
    }
    public void select(bool b)
    {
        if (b)
            openTileSettings();
        selectedOutline.SetActive(b);
    }
    public void activateChangedOutline(bool b)
    {    
        changedOutline.SetActive(b);
    }
    private void OnMouseEnter()
    {
        activateHoverOutline(true);
    }
    private void OnMouseExit()
    {
        activateHoverOutline(false);
    }

    public void openTileSettings()
    {
        var canvas = GameObject.Find("GameUI").GetComponent<RectTransform>();
        Transform panel = canvas.Find("TileSettings");
        var settings = panel.GetComponent<TileSettingsScript>();

        //settings.openWithData(gameObject.GetComponent<TileData>(), this);
    }

    public void closeWithChange()
    {
        activateChangedOutline(true);
        editedUIs.Enqueue(this);
    }

    static public void onUpdate()
    {
        if (worldData == null)
            worldData = GameObject.FindGameObjectWithTag("GameManager").GetComponent<IGameLogic>().GetWorld();
        foreach (TileData TileData in worldData.tiles)
        {
            TileData.gameObject.GetComponent<HexagonUi>().updateBiodiversityOverlay();
            TileData.gameObject.GetComponent<HexagonUi>().updateProductivityOverlay();
        }
        foreach (var UI in editedUIs)
        {
            UI.disableOutline();
        }
    }
    public void disableOutline()
    {
        activateChangedOutline(false);
    }
}
