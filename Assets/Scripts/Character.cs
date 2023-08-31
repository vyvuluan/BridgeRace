using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected BrickColor color;
    [SerializeField] protected Transform balo;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private List<Stage> stages;
    [SerializeField] protected List<Brick> bricks;
    private string currentAnimName;
    protected int currentStage = 0;
    public BrickColor Color { get => color; set => color = value; }
    private void Awake()
    {
        OnInit();
    }
    public abstract void Control();
    public void OnInit()
    {
        bricks = new List<Brick>();
    }

    public int GetLengthBrick()
    {
        return bricks.Count;
    }
    public void AddBrick(Brick brick)
    {
        Brick brickNew = SimplePool.Spawn(brick.gameObject, brick.transform.position, brick.transform.rotation).GetComponent<Brick>();
        brickNew.gameObject.layer = 0;
        brickNew.gameObject.isStatic = false;
        brickNew.transform.SetParent(balo);
        brickNew.transform.localPosition = bricks.Count == 0 ? Vector3.zero : new(0f, bricks[bricks.Count - 1].transform.localPosition.y + 0.5f, 0f);
        brickNew.transform.localRotation = Quaternion.Euler(Vector3.zero);
        //Debug.Log(brickNew.transform.position);
        bricks.Add(brickNew);

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
