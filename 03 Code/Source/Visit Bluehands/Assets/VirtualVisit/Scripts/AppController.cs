using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour, SwitchTourListener {

    public VisitController visitControllerPrefab;

    public FollowingDisplay followingDisplayPrefab;

    public FollowingMenu followingMenuPrefab;

    public string startVisit;

    private VisitController m_VisitController;

    private FollowingDisplay m_FollowingDisplay;

    private FollowingMenu m_FollowingMenu;

    private VisitSettingsFactory m_VisitSettingsFactory;

    void Start () {
        beginApp();
    }

    private void beginApp()
    {
        m_VisitSettingsFactory = new VisitSettingsFactory();

        string visitId = ApplicationModel.SelectedVisitId;
        if (visitId.Equals(""))
        {
            visitId = startVisit;
        }

        var visitSetting = m_VisitSettingsFactory.GetVisitSetting(visitId);
        var visitSettings = m_VisitSettingsFactory.GetVisitSettings();

        m_VisitController = Instantiate(visitControllerPrefab) as VisitController;
        m_VisitController.Initialize(visitSetting);

        m_FollowingDisplay = Instantiate(followingDisplayPrefab) as FollowingDisplay;
        m_FollowingDisplay.Initialize(visitSettings, this);

        m_FollowingMenu = Instantiate(followingMenuPrefab) as FollowingMenu;
        m_FollowingMenu.Initialize(m_FollowingDisplay);

        m_FollowingDisplay.addListener(m_VisitController);
        m_FollowingDisplay.addListener(m_FollowingMenu);

        m_FollowingDisplay.close();
    }

    public void SwitchTour(string nextVisitId)
    {
        ApplicationModel.SelectedVisitId = nextVisitId;

        SceneManager.LoadScene(0);
    }
}
