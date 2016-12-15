using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, SwitchTourListener
{
    public Visit visitPrefab;

    public Player playerPrefab;

    public Stage stagePrefab;

    public string startVisit;

    private Visit m_Visit;

    private Player m_Player;

    private Stage m_Stage;

    private VisitSettingsFactory m_VisitSettingsFactory;

    public WelcomeTextController welcomeTextController;

    private void Start()
    {
        beginGame();
    }

    private void beginGame()
    {
        m_VisitSettingsFactory = new VisitSettingsFactory();

        string visitId = ApplicationModel.SelectedVisitId;
        if(visitId.Equals(""))
        {
            visitId = startVisit;
        }

        welcomeTextController.SetTourTitle(m_VisitSettingsFactory.GetVisitSetting(visitId).title);

        m_Visit = Instantiate(visitPrefab) as Visit;
        m_Visit.Generate(visitId, m_VisitSettingsFactory);

        m_Stage = Instantiate(stagePrefab) as Stage;
        m_Stage.Generate(m_VisitSettingsFactory, this);

        m_Player = Instantiate(playerPrefab) as Player;
        m_Player.Initialize(m_Visit, m_Stage);

        m_Visit.Initialize(m_Player);
    }

    public void SwitchTour(string nextVisitId)
    {
        welcomeTextController.SetLoading();

        ApplicationModel.SelectedVisitId = nextVisitId;

        SceneManager.LoadScene(0);
    }
}
