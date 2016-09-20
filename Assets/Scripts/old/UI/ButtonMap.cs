
public class ButtonMap : ButtonBase
{
    private Map map;

    public void Initialize(Map map)
    {
        this.map = map;
    }

    protected override void DoAction()
    {
        map.ToggleVisibility();
    }
}
