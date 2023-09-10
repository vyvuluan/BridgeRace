using System.Collections.Generic;
using UnityEngine;

public class PlayerHome : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private BrickColor brickColor;

    private string currentAnimName;

    public BrickColor BrickColor { get => brickColor; }

    private void Start()
    {
        OnInit();
    }
    private void OnEnable()
    {
        OnInit();
    }
    public void OnInit()
    {
        skinnedMeshRenderer.material = GameManager.Instance.GetMaterial(brickColor);
        Dictionary<int, string> anims = new()
        {
            { 0, Constants.Dance1Anim },
            { 1, Constants.Dance2Anim },
            { 2, Constants.Dance3Anim },
            { 3, Constants.Dance4Anim },
        };
        int numberRandom = Random.Range(0, anims.Count);
        ChangeAnim(anims[numberRandom]);
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
