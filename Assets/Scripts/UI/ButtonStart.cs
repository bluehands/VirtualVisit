using UnityEngine.SceneManagement;

public class ButtonStart : ButtonBase
{
    public int nextLevelIndex;

    protected override void DoAction()
    {
        SceneManager.LoadScene(nextLevelIndex);
    }
}
