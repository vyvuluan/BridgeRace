using UnityEngine;

public class FailState : IState
{
    public void OnEnter(Enemy enemy)
    {
        Debug.Log(enemy.Color + "Fail");
        enemy.Fail();
    }

    public void OnExecute(Enemy enemy)
    {

    }

    public void OnExit(Enemy enemy)
    {

    }
}
