using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.VirtualVisit.Scripts
{
    public class AppController : MonoBehaviour, SwitchTourListener {

        public VisitController VisitControllerPrefab;
        public FollowingDisplay FollowingDisplayPrefab;
        public FollowingMenu FollowingMenuPrefab;
        public string StartVisit;

        private VisitController _visitController;
        private FollowingDisplay _followingDisplay;
        private FollowingMenu _followingMenu;
        private VisitSettingsFactory _visitSettingsFactory;

        internal void Start() {
            //QualitySettings.antiAliasing = 2;

            BeginApp();
        }

        internal void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void BeginApp()
        {
            _visitSettingsFactory = new VisitSettingsFactory();

            var visitSettings = _visitSettingsFactory.GetVisitSettings();

            string visitId = ApplicationModel.SelectedVisitId;
            if (visitId.Equals(""))
            {
                _followingDisplay = Instantiate(FollowingDisplayPrefab);
                _followingDisplay.Initialize(visitSettings, this);

                _followingMenu = Instantiate(FollowingMenuPrefab);
                _followingMenu.Initialize(_followingDisplay);
                _followingMenu.openDisplay();
            }
            else
            {
                var visitSetting = _visitSettingsFactory.GetVisitSetting(visitId);

                _visitController = Instantiate(VisitControllerPrefab);
                _visitController.Initialize(visitSetting);

                _followingDisplay = Instantiate(FollowingDisplayPrefab);
                _followingDisplay.Initialize(visitSettings, this);

                _followingMenu = Instantiate(FollowingMenuPrefab);
                _followingMenu.Initialize(_followingDisplay);

                _followingDisplay.addListener(_visitController);
                _followingDisplay.addListener(_followingMenu);

                _followingDisplay.close();
            }
        }

        public void SwitchTour(string nextVisitId)
        {
            ApplicationModel.SelectedVisitId = nextVisitId;

            SceneManager.LoadScene(0);
        }
    }
}
