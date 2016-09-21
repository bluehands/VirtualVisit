using UnityEngine;
using System.IO;

public class GameManagerStereo : MonoBehaviour
{
    public VisitStereo visitPrefab;

    public PlayerStereo playerPrefab;

    public UICamera cameraPrefabs;

    private VisitStereo visitInstance;

    private PlayerStereo playerInstance;

    private UICamera cameraInstance;

    private VisitSettingsFactory visitSettingsFactory;

    private void Start()
    {
        BeginGame();
    }

    private void BeginGame()
    {
        visitSettingsFactory = new VisitSettingsFactory();

        string visitId = ApplicationModel.selectedVisitId;

        visitInstance = Instantiate(visitPrefab) as VisitStereo;
        visitInstance.Generate(visitId, visitSettingsFactory);
        visitInstance.Initialize();

        playerInstance = Instantiate(playerPrefab) as PlayerStereo;
        playerInstance.Initialize(visitInstance.map);

        cameraInstance = Instantiate(cameraPrefabs) as UICamera;
        cameraInstance.Generate(visitSettingsFactory);

    }
}
