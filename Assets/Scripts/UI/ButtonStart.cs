using UnityEngine.SceneManagement;

public class ButtonStart : ButtonBase
{
    public string nextVisitId;

    protected override void DoAction()
    {
        ApplicationModel.selectedVisitId = nextVisitId;

        SceneManager.LoadScene(0);
    }
}
