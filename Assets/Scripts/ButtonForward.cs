using UnityEngine.SceneManagement;

public class ButtonForward : ButtonBase
{
    public int nextLevelIndex;

    protected override void DoAction()
    {
        SceneManager.LoadScene(nextLevelIndex);
    }
}
