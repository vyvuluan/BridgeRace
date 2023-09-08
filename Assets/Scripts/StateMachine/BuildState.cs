using UnityEngine;

public class BuildState : IState
{
    private int i;
    public void OnEnter(Enemy enemy)
    {
        i = 0;
    }

    public void OnExecute(Enemy enemy)
    {
        Debug.Log("build");
        switch (enemy.CheckNextStage(i))
        {
            //Not next stage
            case 0:
                enemy.ChangeState(new CollectState());
                break;
            //Next stage
            case 1:
                enemy.NextStage();
                break;
            //Build
            case 2:
                enemy.BuildBrige(ref i);
                break;
        }

    }

    public void OnExit(Enemy enemy)
    {

    }
}
