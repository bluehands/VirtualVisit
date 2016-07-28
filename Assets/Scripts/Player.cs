using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public Cardboard cardboardPrefab;

    public ButtonBack backButtonPrefab;

    public ButtonMap mapButtonPrefab;

    private Cardboard cardboardInstance;

    private ButtonBack backButtonInstance;

    private ButtonMap mapButtonInstance;

    public void Initialize(Map map)
    {
        var canvasBack = GameObject.Find("Canvas Back");
        var canvasMap = GameObject.Find("Canvas Map");

        cardboardInstance = Instantiate(cardboardPrefab) as Cardboard;
        cardboardInstance.transform.parent = transform;

        CardboardHead cardboardHead = cardboardInstance.GetComponentInChildren<CardboardHead>();
        map.transform.parent = cardboardHead.transform;

        backButtonInstance = Instantiate(backButtonPrefab) as ButtonBack;
        backButtonInstance.transform.SetParent(canvasBack.transform, false);

        mapButtonInstance = Instantiate(mapButtonPrefab) as ButtonMap;
        mapButtonInstance.Initialize(map);
        mapButtonInstance.transform.SetParent(canvasMap.transform, false);
    }

}
