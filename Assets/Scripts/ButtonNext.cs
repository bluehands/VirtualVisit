
public class ButtonNext : ButtonBase
{
    protected override void DoAction()
    {
        VisitEdge edge = GetComponentInParent<VisitEdge>();
        Visit visit = GetComponentInParent<Visit>();
        visit.moveTo(edge);
    }
}
