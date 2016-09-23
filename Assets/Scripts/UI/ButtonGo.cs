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
        ApplicationModel.SelectedVisitId = nextVisitId;

        SceneManager.LoadScene(0);
    }
}
