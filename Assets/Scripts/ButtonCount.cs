using UnityEngine.SceneManagement;

public class ButtonCount : ButtonBase
{
    public int nextLevelIndex;

    protected override void DoAction()
    {
        SceneManager.LoadScene(nextLevelIndex);
    }
}
