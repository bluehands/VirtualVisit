
public class ButtonMapStereo : ButtonBase
{
    private MapStereo map;

    public void Initialize(MapStereo map)
    {
        this.map = map;
    }

    protected override void DoAction()
    {
        map.ToggleVisibility();
    }
}
