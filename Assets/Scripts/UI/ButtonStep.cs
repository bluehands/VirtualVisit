
public class ButtonStep : ButtonBase
{
    protected override void DoAction()
    {
        VisitEdge edge = GetComponentInParent<VisitEdge>();
        Visit visit = GetComponentInParent<Visit>();
        visit.MoveTo(edge);
    }
}
