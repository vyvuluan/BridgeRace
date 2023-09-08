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
    public void OpenGate(Transform tf) => tf.DOMoveX(tf.position.x + 2f, 2f);
    public void CloseGate(Transform tf) => tf.DOMoveX(tf.position.x - 2f, 2f);
    public bool CheckCompleteBuild(BrickColor brickColor) => brickBriges.All(n => n.Color == brickColor);
    public void NextStep()
    {
        gateIn.DOMoveX(gateIn.position.x - 2.8f, 2f);
        gateOut.DOMoveX(gateOut.position.x + 2.8f, 2f);
    }

}
