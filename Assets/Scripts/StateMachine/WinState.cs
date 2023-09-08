using UnityEngine;

public class WinState : IState
{
    public void OnEnter(Enemy enemy)
    {
        Debug.Log("WIN");
    }

    public void OnExecute(Enemy enemy)
    {

    }

    public void OnExit(Enemy enemy)
    {

    }
}
