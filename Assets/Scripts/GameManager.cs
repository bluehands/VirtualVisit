using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Visit visitPrefab;

    public Player playerPrefab;

    public Stage cameraPrefab;

    private Visit m_Visit;

    private Player m_Player;

    private Stage m_Camera;

    private VisitSettingsFactory m_VisitSettingsFactory;

    private void Start()
    {
        beginGame();
    }

    private void beginGame()
    {
        m_VisitSettingsFactory = new VisitSettingsFactory();

        string visitId = ApplicationModel.SelectedVisitId;

        m_Visit = Instantiate(visitPrefab) as Visit;
        m_Visit.Generate(visitId, m_VisitSettingsFactory);

        m_Player = Instantiate(playerPrefab) as Player;
        m_Player.Initialize(m_Visit);

        m_Visit.Initialize(m_Player);

        m_Camera = Instantiate(cameraPrefab) as Stage;
        m_Camera.Generate(m_VisitSettingsFactory);

    }
}
