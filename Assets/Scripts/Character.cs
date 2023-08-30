using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private List<Stage> stages;
    private string currentAnimName;
    protected int currentStage = 0;
    public abstract void Control();
    public void OnInit()
    {

    }
    public void AddBrick()
    {

    }
    public void RemoveBrick()
    {

    }
    public void SetStage(List<Stage> stages)
    {
        this.stages = stages;
    }
    protected Stage GetStageCurrent()
    {
        return stages[currentStage];
    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            animator.ResetTrigger(animName);
            currentAnimName = animName;
            animator.SetTrigger(currentAnimName);
        }
    }
}
