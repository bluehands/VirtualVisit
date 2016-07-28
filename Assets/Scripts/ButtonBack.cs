using UnityEngine.SceneManagement;

public class ButtonBack : ButtonBase
{
    public int nextLevelIndex;

    protected override void DoAction()
    {
        SceneManager.LoadScene(nextLevelIndex);
    }
}
