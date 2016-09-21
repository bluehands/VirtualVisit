using UnityEngine;

public class Button3DStereo : ButtonBase
{
    private VisitStereo visit;

    public void Initialize(VisitStereo visit, Transform parent)
    {
        this.visit = visit;
        transform.SetParent(parent.transform, false);
    }

    protected override void DoAction()
    {
        visit.getCurrentVisitNode().ToggleStereoView();
    }
}
