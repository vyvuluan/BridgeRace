using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected BrickColor color;
    [SerializeField] protected Transform balo;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private List<Stage> stages;
    [SerializeField] private List<Brick> bricks;
    private string currentAnimName;
    protected int currentStage = 0;
    public BrickColor Color { get => color; set => color = value; }
    public abstract void Control();
    public void OnInit()
    {

    }
    public void AddBrick(Brick brick)
    {
        Brick brickNew = SimplePool.Spawn(brick.gameObject, brick.transform.position, brick.transform.rotation).GetComponent<Brick>();
        brickNew.gameObject.layer = 0;
        brickNew.transform.SetParent(balo);
        brickNew.transform.localPosition = bricks.Count == 0 ? Vector3.zero : new(0f, bricks[bricks.Count - 1].transform.localPosition.y + 0.3f, 0f);
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
    protected void ApplyKnockback(Vector3 direction)
    {
        rb.AddForce(direction * 50f, ForceMode.Impulse);
        Debug.Log("knock");
        StartCoroutine(DisableControlDuringKnockback());
    }

    private IEnumerator DisableControlDuringKnockback()
    {
        // Vô hiệu hóa điều khiển trong thời gian knockback
        // Đảm bảo người chơi không thể điều khiển trong thời gian này

        yield return new WaitForSeconds(2f);

        // Kích hoạt lại điều khiển sau khi kết thúc knockback
    }

}
