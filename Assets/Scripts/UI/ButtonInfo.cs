using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonInfo : ButtonBase
{
    private UICamera uiCamera;
    private int infoIndex;

    public void Initialize(int infoIndex, UICamera uiCamera, Transform parent)
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
            uiCamera.hideInfo();
            uiCamera.shopMainMenu();
        } else
        {
            uiCamera.hideMainMenu();
            uiCamera.showInfo(infoIndex);
        }
    }
}
