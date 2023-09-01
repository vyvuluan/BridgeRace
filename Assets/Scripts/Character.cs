using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected BrickColor color;
    [SerializeField] protected Transform balo;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Animator animator;
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
        if (brick.Color == BrickColor.Grey)
        {
            brickNew.SetMaterial(GameManager.Instance.GetMaterial(color));
            brickNew.Color = color;
        }
        brickNew.gameObject.layer = 0;
        //brickNew.gameObject.isStatic = false;
        brickNew.transform.SetParent(balo);
        brickNew.transform.SetLocalPositionAndRotation(bricks.Count == 0 ? Vector3.zero : new(0f, bricks[bricks.Count - 1].transform.localPosition.y + 0.5f, 0f), Quaternion.Euler(Vector3.zero));
        //Debug.Log(brickNew.transform.position);
        bricks.Add(brickNew);

    }
    public void RemoveAllBrick()
    {
        foreach (var brick in bricks)
        {
            Vector3 temp = brick.transform.position;
            temp.x += Random.Range(0f, 1.5f);
            temp.z += Random.Range(0f, 1.5f);
            brick.transform.DOMove(temp, 0.3f).OnComplete(() =>
            {
                temp.y = 2.5f;
                brick.transform.DOMove(temp, 0.4f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    brick.transform.SetParent(null);
                    brick.SetMaterial(GameManager.Instance.GetMaterial(BrickColor.Grey));
                    brick.Color = BrickColor.Grey;
                    brick.gameObject.layer = 6;
                    brick.gameObject.tag = Constants.BrickTag;
                });
            });
        }
        bricks.Clear();
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
