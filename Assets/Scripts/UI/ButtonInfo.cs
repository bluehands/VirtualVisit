using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonInfo : ButtonBase
{
    private Stage uiCamera;
    private int infoIndex;

    public void Initialize(int infoIndex, Stage uiCamera, Transform parent)
    {
        this.infoIndex = infoIndex;
        this.uiCamera = uiCamera;
        transform.SetParent(parent.transform, false);
        setSelected(infoIndex == -1);
    }

    protected override void DoAction()
    {
        if(infoIndex == -1)
        {
            uiCamera.HideInfo();
            uiCamera.ShowMainMenu();
        } else
        {
            uiCamera.HideMainMenu();
            uiCamera.ShowInfo(infoIndex);
        }
    }
}
