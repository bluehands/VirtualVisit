using UnityEngine;
using System.Collections;

public class PlayerStereo : MonoBehaviour {

    public Cardboard cardboardPrefab;

    public ButtonBackStereo backButtonPrefab;

    public ButtonMapStereo mapButtonPrefab;

    public Button3DStereo d3ButtonPrefab;

    public UIScreen uiScreenPrefabs;

    private Cardboard cardboardInstance;

    private ButtonBackStereo backButtonInstance;

    private ButtonMapStereo mapButtonInstance;

    private Button3DStereo d3ButtonInstance;

    private UIScreen uiScreenInstance;

    public void Initialize(VisitStereo visit)
    {
        var canvasBack = GameObject.Find("Canvas Back");
        var canvasMap = GameObject.Find("Canvas Map");
        var canvas3D = GameObject.Find("Canvas 3D");

        cardboardInstance = Instantiate(cardboardPrefab) as Cardboard;
        cardboardInstance.transform.parent = transform;

        CardboardHead cardboardHead = cardboardInstance.GetComponentInChildren<CardboardHead>();
        visit.map.transform.parent = cardboardHead.transform;

        uiScreenInstance = Instantiate(uiScreenPrefabs) as UIScreen;
        uiScreenInstance.transform.parent = transform;

        backButtonInstance = Instantiate(backButtonPrefab) as ButtonBackStereo;
        backButtonInstance.Initialize(uiScreenInstance, canvasBack.transform);

        mapButtonInstance = Instantiate(mapButtonPrefab) as ButtonMapStereo;
        mapButtonInstance.Initialize(visit.map, canvasMap.transform);

        d3ButtonInstance = Instantiate(d3ButtonPrefab) as Button3DStereo;
        d3ButtonInstance.Initialize(visit, canvas3D.transform);
    }

}
