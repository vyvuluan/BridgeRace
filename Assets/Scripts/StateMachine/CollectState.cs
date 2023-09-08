using UnityEngine;

public class CollectState : IState
{
    public void OnEnter(Enemy enemy)
    {
        enemy.Moving();
    }

    public void OnExecute(Enemy enemy)
    {
        Debug.Log("collect");
    }

    public void OnExit(Enemy enemy)
    {

    }
}
