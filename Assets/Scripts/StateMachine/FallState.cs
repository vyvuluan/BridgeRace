using UnityEngine;

public class FallState : IState
{
    private float timer;
    public void OnEnter(Enemy enemy)
    {
        timer = 0;
        enemy.Falling();
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer > 3f)
        {
            enemy.Respawn();
            timer = 0;
        }
    }

    public void OnExit(Enemy enemy)
    {
    }
}
