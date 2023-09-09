using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private Transform brickTransform;
    [SerializeField] private List<Vector2Int> listCheckRandom = new();
    [SerializeField] private List<Material> brickColors;
    [SerializeField] private List<Brick> bricks = new();
    [SerializeField] private List<Brige> briges;
    private Brick[,] brickTable;
    private void Start()
    {
        SpawnBrick();
        RandomColorBrick();
    }
    private void SpawnBrick()
    {
        brickTable = new Brick[size.x, size.y];
        //bricks = new();
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                //Vector3 pos = new(i * 1.5f, 0, j * -1.0f);
                Vector3 pos = new(i, 0, j);
                Brick brick = SimplePool.Spawn(brickPrefab, pos, brickPrefab.transform.rotation).GetComponent<Brick>();
                brick.Index = new(i, j);
                bricks.Add(brick);
                brickTable[i, j] = brick;
                listCheckRandom.Add(brick.Index);
                brick.transform.SetParent(brickTransform, false);
            }
        }
    }
    private void RandomColorBrick()
    {
        int quantityOfType = (size.x * size.y) / brickColors.Count;
        for (int i = 0; i < brickColors.Count; i++)
        {
            for (int j = 0; j < quantityOfType; j++)
            {
                int index = Random.Range(0, listCheckRandom.Count);
                Vector2Int indexTable = listCheckRandom[index];
                brickTable[indexTable.x, indexTable.y].SetMaterial(brickColors[i]);
                brickTable[indexTable.x, indexTable.y].Color = (BrickColor)(i + 1);
                listCheckRandom.RemoveAt(index);
                brickTable[indexTable.x, indexTable.y].gameObject.SetActive(false);
            }
        }
    }
    public Brige GetBriges(int index) => briges[index];
    public int RandomBrige() => Random.Range(0, briges.Count);
    public int RandomBrige(BrickColor color)
    {
        int result = 0;
        int max = 0;
        for (int i = 0; i < briges.Count; i++)
        {
            int temp = briges[i].BrickBriges.Count(n => n.Color == color);
            if (max < temp)
            {
                max = temp;
                result = i;
            }
        }
        return result == 0 ? RandomBrige() : result;
    }
    public void SetActiveBrick(BrickColor brickColor, bool status)
    {
        foreach (var item in bricks.Where(n => n.Color == brickColor))
        {
            item.gameObject.SetActive(status);
        }
    }
}
