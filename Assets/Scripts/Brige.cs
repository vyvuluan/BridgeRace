using System.Collections.Generic;
using UnityEngine;

public class Brige : MonoBehaviour
{
    [SerializeField] private Transform brickBrige;
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

}
