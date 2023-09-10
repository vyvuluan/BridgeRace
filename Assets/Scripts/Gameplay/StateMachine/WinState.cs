public class WinState : IState
{
    public void OnEnter(Enemy enemy)
    {
        GameView.Instance.GameOver();
    }

    public void OnExecute(Enemy enemy)
    {

    }

    public void OnExit(Enemy enemy)
    {

    }
}
