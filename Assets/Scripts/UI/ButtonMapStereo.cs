using UnityEngine;

public class ButtonMapStereo : ButtonBase
{
    private MapStereo map;

    public void Initialize(MapStereo map, Transform parent)
    {
        this.map = map;
        transform.SetParent(parent, false);
    }

    protected override void DoAction()
    {
        map.ToggleVisibility();
    }
}
