public class CollectState : IState
{
    public void OnEnter(Enemy enemy)
    {
        enemy.Moving();
    }

    public void OnExecute(Enemy enemy)
    {

    }

    public void OnExit(Enemy enemy)
    {

    }
}
