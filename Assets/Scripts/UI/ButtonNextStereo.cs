
public class ButtonNextStereo : ButtonBase
{
    protected override void DoAction()
    {
        VisitEdgeStereo edge = GetComponentInParent<VisitEdgeStereo>();
        VisitStereo visit = GetComponentInParent<VisitStereo>();
        visit.moveTo(edge);
    }
}
