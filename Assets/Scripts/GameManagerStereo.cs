using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class GameManagerStereo : MonoBehaviour
{
    public VisitStereo visitPrefab;

    public PlayerStereo playerPrefab;

    public UICamera cameraPrefabs;

    private VisitStereo visitInstance;

    private PlayerStereo playerInstance;

    private UICamera cameraInstance;

    private void Start()
    {
        BeginGame();
    }

    private void BeginGame()
    {
        string visitId = ApplicationModel.selectedVisitId;

        visitInstance = Instantiate(visitPrefab) as VisitStereo;
        visitInstance.Generate(visitId);

        playerInstance = Instantiate(playerPrefab) as PlayerStereo;
        playerInstance.Initialize(visitInstance.map);

        cameraInstance = Instantiate(cameraPrefabs) as UICamera;
        cameraInstance.Generate();

    }
}
