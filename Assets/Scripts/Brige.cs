using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Brige : MonoBehaviour
{
    [SerializeField] private Transform brickBrige;
    [SerializeField] private Transform gateIn;
    [SerializeField] private Transform gateOut;
    [SerializeField] List<BrickBrige> brickBriges;
    private bool isLock = false;
    public List<BrickBrige> BrickBriges { get => brickBriges; }
    public bool IsLock { get => isLock; }

    private void Start()
    {
        OnInit();
    }
    public void OnInit()
    {
        FindBrickBrige();
    }
    public void FindBrickBrige()
    {
        brickBriges = new();
        int count = brickBrige.childCount;
        for (int i = 0; i < count; i++)
        {
            brickBriges.Add(brickBrige.GetChild(i).GetComponent<BrickBrige>());
        }
    }
    public BrickBrige GetBrickByIndex(int index) => brickBriges[index];
    public int GetCountBrickBrige() => brickBriges.Count;
    public bool CheckCompleteBuild(BrickColor brickColor) => brickBriges.All(n => n.Color == brickColor);
    public void NextStep(BrickColor brickColor)
    {
        gateIn.DOMoveX(gateIn.position.x - 2.8f, 2f);
        gateOut.DOMoveX(gateOut.position.x + 2.8f, 2f);
        gateIn.GetComponent<MeshRenderer>().material = GameManager.Instance.GetMaterial(brickColor);
        gateOut.GetComponent<MeshRenderer>().material = GameManager.Instance.GetMaterial(brickColor);
        isLock = true;
    }

}
