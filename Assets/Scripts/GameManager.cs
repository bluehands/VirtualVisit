using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Visit visitPrefab;

    public Player playerPrefab;

    private Visit visitInstance;

    private Player playerInstance;

    public VisitSettings visitSettings;

    private void Start()
    {
        BeginGame();
    }

    private void BeginGame()
    {
        visitInstance = Instantiate(visitPrefab) as Visit;

        visitInstance.Generate(visitSettings);

        playerInstance = Instantiate(playerPrefab) as Player;

        playerInstance.Initialize(visitInstance.map);
    }
}
