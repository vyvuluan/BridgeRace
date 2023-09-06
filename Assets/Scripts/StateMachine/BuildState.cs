public class BuildState : IState
{
    private int i;
    public void OnEnter(Enemy enemy)
    {
        i = 0;
    }

    public void OnExecute(Enemy enemy)
    {
        enemy.BuildBrige(ref i);
    }

    public void OnExit(Enemy enemy)
    {

    }
}
