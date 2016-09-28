
public class ButtonStep : ButtonBase
{
    protected override void DoButtonAction()
    {
        VisitEdge edge = GetComponentInParent<VisitEdge>();
        Visit visit = GetComponentInParent<Visit>();
        visit.MoveTo(edge);
    }
}
