using UnityEngine;
using System.Collections;

public class PlayerStereo : MonoBehaviour {

    public Cardboard cardboardPrefab;

    public ButtonBackStereo backButtonPrefab;

    public ButtonMapStereo mapButtonPrefab;

    public UIScreen uiScreenPrefabs;

    private Cardboard cardboardInstance;

    private ButtonBackStereo backButtonInstance;

    private ButtonMapStereo mapButtonInstance;

    private UIScreen uiScreenInstance;

    public void Initialize(MapStereo map)
    {
        var canvasBack = GameObject.Find("Canvas Back");
        var canvasMap = GameObject.Find("Canvas Map");

        cardboardInstance = Instantiate(cardboardPrefab) as Cardboard;
        cardboardInstance.transform.parent = transform;

        CardboardHead cardboardHead = cardboardInstance.GetComponentInChildren<CardboardHead>();
        map.transform.parent = cardboardHead.transform;

        uiScreenInstance = Instantiate(uiScreenPrefabs) as UIScreen;
        uiScreenInstance.transform.parent = transform;

        backButtonInstance = Instantiate(backButtonPrefab) as ButtonBackStereo;
        backButtonInstance.Initialize(uiScreenInstance);
        backButtonInstance.transform.SetParent(canvasBack.transform, false);

        mapButtonInstance = Instantiate(mapButtonPrefab) as ButtonMapStereo;
        mapButtonInstance.Initialize(map);
        mapButtonInstance.transform.SetParent(canvasMap.transform, false);


    }

}
