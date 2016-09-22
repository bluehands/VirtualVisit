using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonGo : ButtonBase
{
    private string nextVisitId;

    public void Initialize(string nextVisitId, Transform parent)
    {
        this.nextVisitId = nextVisitId;
        transform.SetParent(parent.transform, false);
    }

    protected override void DoAction()
    {
        ApplicationModel.selectedVisitId = nextVisitId;

        SceneManager.LoadScene(0);
    }
}
